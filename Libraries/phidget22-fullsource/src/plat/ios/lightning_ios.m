/*
 * This file is part of libphidget22
 *
 * Copyright 2015 Phidgets Inc <patrick@phidgets.com>
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 3 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, see
 * <http://www.gnu.org/licenses/>
 */

#ifdef LIGHTNING_SUPPORT

#include "phidgetbase.h"

#include "phidget.h"
#include "gpp.h"
#include "lightning.h"
#include "manager.h"
#include "device/hubdevice.h"
#include "util/utils.h"

#import <Foundation/Foundation.h>
#import <ExternalAccessory/ExternalAccessory.h>

static void handleLightningPacket(PhidgetDeviceHandle device, uint8_t packetType, uint8_t *buf, int len);

@interface EAController : NSObject <EAAccessoryDelegate, NSStreamDelegate> {
	EAAccessory *_accessory;
	EASession *_session;
	PhidgetDeviceHandle phid;
	NSMutableData *_writeData;

	mos_tlock_t *writeLock;
	BOOL writeOperationOngoing;
	BOOL stopOpenSession;
}
- (void) _accessoryConnected:(NSNotification *)notification;
- (void) _accessoryDisconnected:(NSNotification*)notification;

- (void)writeData:(NSData *)data;
//- (PhidgetReturnCode) writeData:(uint8_t *)buf ofLength:(int)len;
@end

NSString *EADSessionDataReceivedNotification = @"EADSessionDataReceivedNotification";

@implementation EAController

// low level write method - write data to the accessory while there is space available and data to write
- (void)_writeData {
	if(writeOperationOngoing)
		return;
	mos_tlock_lock(writeLock);
	writeOperationOngoing = TRUE;
	while (([[_session outputStream] hasSpaceAvailable]) && ([_writeData length] > 0)) {
		NSInteger bytesWritten = [[_session outputStream] write:[_writeData bytes] maxLength:[_writeData length]];
		if (bytesWritten == -1) {
			NSLog(@"write error");
			break;
		} else if (bytesWritten > 0) {
			logBuffer(PHIDGET_LOG_VERBOSE, (uint8_t*)[_writeData bytes], (int)bytesWritten, "Sent Lightning OUT data");
			 [_writeData replaceBytesInRange:NSMakeRange(0, bytesWritten) withBytes:NULL length:0];
		}
	}
	writeOperationOngoing = FALSE;
	mos_tlock_unlock(writeLock);
}

// low level read method - read data while there is data and space available in the input buffer
- (void)_readData {
	unsigned char buffer[MAX_LIGHTNING_IN_PACKET_SIZE];
	unsigned char *bufPtr;
	NSInteger bytesRead = [[_session inputStream] read:buffer maxLength:MAX_LIGHTNING_IN_PACKET_SIZE];
	int len;
	writeOperationOngoing = FALSE;

	//logBuffer(PHIDGET_LOG_VERBOSE, buffer, (int)bytesRead, "Got Lightning IN data");

	if(bytesRead < 1) {
		logwarn("Lighting bytes read was: %d", bytesRead);
		return;
	}

	bufPtr = buffer;
	do {
		len = ((bufPtr[0] << 8) & 0x3F) + bufPtr[1];

		if(len < 2 || len > bytesRead) {
			logerr("Invalid lightning packet: %d, %d", len, bytesRead);
			return;
		}

		//logBuffer(PHIDGET_LOG_VERBOSE, bufPtr, len, "Got Lightning Packet");

		handleLightningPacket(phid, (bufPtr[0] & 0xC0), bufPtr+2, len-2);
		bytesRead-=len;
		bufPtr+=len;
	} while(bytesRead>0);
}

- (void) dealloc {
	mos_tlock_destroy(&writeLock);

	// If you don't remove yourself as an observer, the Notification Center
	// will continue to try and send notification objects to the deallocated
	// object.
	[[NSNotificationCenter defaultCenter] removeObserver:self];
	[super dealloc];
}

- (id) init {
	self = [super init];
	if (!self) return nil;

	mos_tlock_init(writeLock, P22LOCK_PHIDOUTPUTLOCK, P22LOCK_FLAGS);

	[[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(_accessoryConnected:) name:EAAccessoryDidConnectNotification object:nil];
	[[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(_accessoryDisconnected:) name:EAAccessoryDidDisconnectNotification object:nil];
	[[EAAccessoryManager sharedAccessoryManager] registerForLocalNotifications];

	loginfo("Set up EA Accessory notifications");

	return self;
}

- (void) _accessoryConnected:(NSNotification *)notification {

	EAAccessory *connectedAccessory = [[notification userInfo] objectForKey:EAAccessoryKey];
	const PhidgetUniqueDeviceDef *pdd = Phidget_Unique_Device_Def;
	int res;
	uint8_t buf[2];

	loginfo("Accessory connected");

	while(((int)pdd->type) != END_OF_LIST) {
		if(pdd->type == PHIDTYPE_LIGHTNING) {
			PhidgetDeviceHandle device = NULL;

			res = createPhidgetDevice(PHIDCONN_LIGHTNING, pdd, 100, "", 9999999, &device);

			device->status = PHIDGET_MANAGER_FLAG | PHIDGET_ATTACHED_FLAG | PHIDGET_SCANNED_FLAG;
			device->pd_lightningConnection = mallocPhidgetLightningConnection();

			device->pd_lightningConnection->accessory = connectedAccessory;
			[connectedAccessory retain];

			[_accessory release];
			_accessory = [connectedAccessory retain];
			phid = device;

			[self openSession];

			//Start openSession on a new thread so that the Stream I/O doesn't happen on the main thead
			//stopOpenSession = FALSE;
			//[self performSelectorInBackground:@selector(openSession) withObject:nil];

			loginfo("Found lightning device to connect...");

			//Send a get devices msg
			buf[0] = VINT_LIGHTING_OUT_PACKET_GETDEVICESINFO;
			buf[1] = 2; //length
			[self writeData:[NSData dataWithBytes:buf length:2]];

			loginfo("Calling deviceAttach()..");

			//success! run attach events, etc.
			if(deviceAttach(device) != EPHIDGET_OK) {
				logerr("deviceAttach on a new device. Probably duplicate serial numbers.");
				PhidgetDevice_free(device);
			}
			break;
		}
		pdd++;
	}
}

- (void) _accessoryDisconnected:(NSNotification*)notification {

	EAAccessory *disconnectedAccessory = [[notification userInfo] objectForKey:EAAccessoryKey];
	EAAccessory *accessory;

	PhidgetDeviceHandle devicetmp;
	PhidgetDeviceHandle device;

	loginfo("Accessory disconnected");

	mos_tlock_lock(attachedDevicesLock);

again:
	FOREACH_ATTACHEDDEVICE_SAFE(device, devicetmp) {
		if (device->connType != PHIDCONN_LIGHTNING)
			continue;

		loginfo("Found Lightning device in _accessoryDisconnected");

		accessory = (EAAccessory *)device->pd_lightningConnection->accessory;
		if([accessory connectionID] == [disconnectedAccessory connectionID]) {
			[self closeSession];
			[accessory release];
			loginfo("Detaching Lightning device in _accessoryDisconnected...");
			deviceDetach(device);
			stopOpenSession = TRUE;

			goto again;
		}
	}

	mos_tlock_unlock(attachedDevicesLock);
}


// open a session with the accessory and set up the input and output stream on the default run loop
- (void)openSession {
	//NSRunLoop* myRunLoop = [NSRunLoop currentRunLoop];

	[_accessory setDelegate:self];
	_session = [[EASession alloc] initWithAccessory:_accessory forProtocol:@"com.phidgets.lightning"];

	if (_session) {
		[[_session inputStream] setDelegate:self];
		[[_session inputStream] scheduleInRunLoop:[NSRunLoop currentRunLoop] forMode:NSDefaultRunLoopMode];
		[[_session inputStream] open];

		[[_session outputStream] setDelegate:self];
		[[_session outputStream] scheduleInRunLoop:[NSRunLoop currentRunLoop] forMode:NSDefaultRunLoopMode];
		[[_session outputStream] open];
	}
	else
	{
		NSLog(@"creating session failed");
	}

//	do {
//		// Run the run loop 10 times to let the timer fire.
//		[myRunLoop runUntilDate:[NSDate dateWithTimeIntervalSinceNow:1]];
//	}
//	while (!stopOpenSession);
}

// close the session with the accessory.
- (void)closeSession {
	[[_session inputStream] close];
	[[_session inputStream] removeFromRunLoop:[NSRunLoop currentRunLoop] forMode:NSDefaultRunLoopMode];
	[[_session inputStream] setDelegate:nil];
	[[_session outputStream] close];
	[[_session outputStream] removeFromRunLoop:[NSRunLoop currentRunLoop] forMode:NSDefaultRunLoopMode];
	[[_session outputStream] setDelegate:nil];

	mos_tlock_lock(writeLock);
	[_writeData release];
	_writeData = nil;
	mos_tlock_unlock(writeLock);
	[_session release];
	_session = nil;
}

// high level write data method
- (void)writeData:(NSData *)data {
	mos_tlock_lock(writeLock);
	loginfo("Queueing lightning data to send..");
	if (_writeData == nil) {
		_writeData = [[NSMutableData alloc] init];
	}

	[_writeData appendData:data];
	mos_tlock_unlock(writeLock);
	[self _writeData];
}

#pragma mark EAAccessoryDelegate
- (void)accessoryDidDisconnect:(EAAccessory *)accessory {
	// do something ...
}

#pragma mark NSStreamDelegateEventExtensions

// asynchronous NSStream handleEvent method
- (void)stream:(NSStream *)aStream handleEvent:(NSStreamEvent)eventCode {
	switch (eventCode) {
		case NSStreamEventNone:
			break;
		case NSStreamEventOpenCompleted:
			break;
		case NSStreamEventHasBytesAvailable:
			loginfo("Got to NSStreamEventHasBytesAvailable");
			[self _readData];
			break;
		case NSStreamEventHasSpaceAvailable:
			loginfo("Got to NSStreamEventHasSpaceAvailable");
			[self _writeData];
			break;
		case NSStreamEventErrorOccurred:
			break;
		case NSStreamEventEndEncountered:
			break;
		default:
			break;
	}
}
@end

EAController * eaController;

static void handleLightningPacket(PhidgetDeviceHandle device, uint8_t packetType, uint8_t *buf, int len) {
	PhidgetHubDeviceHandle hub = (PhidgetHubDeviceHandle)device;
	int i;
	int idx, bufidx;
	switch (packetType) {
	case VINT_LIGHTING_IN_PACKET_DATA:
		mos_tlock_lock(device->inputLock);
		if (buf[0] & PHID_GENERAL_PACKET_FLAG && deviceSupportsGeneralPacketProtocol(device)) {
			PhidgetGPP_dataInput(device, buf, len);
		} else {
			if(Phidget_status((PhidgetHandle)device, PHIDGET_OPENED_FLAG)) {
				device->fptrData(device, buf, len);
				PROPSLOCK((PhidgetHandle)device);
				device->dataCounter++;
				PROPSUNLOCK((PhidgetHandle)device);
			}
		}
		mos_tlock_unlock(device->inputLock);
		break;

	case VINT_LIGHTING_IN_PACKET_DEVICESINFO:
		loginfo("Got Lighting devicesinfo msg.");
		if(len != (device->dev_hub.numVintPorts + device->dev_hub.numVintPortModes) * 4) {
			logerr("VINT_LIGHTING_IN_PACKET_GETDEVICESINFO length is wrong (%d)", len);
			return;
		}
		mos_tlock_lock(attachedDevicesLock);
		idx = 0;
		bufidx = 0;
		for(i=0;i<device->dev_hub.numVintPorts + device->dev_hub.numVintPortModes;i++) {
			hub->portDescString[idx++] = (buf[bufidx++] & 0x0F) + '0';
			hub->portDescString[idx++] = ((buf[bufidx] >> 4) & 0x0F) + '0';
			hub->portDescString[idx++] = (buf[bufidx++] & 0x0F) + '0';
			hub->portDescString[idx++] = (buf[bufidx++] & 0x0F) + '0';
			hub->portDescString[idx++] = ((buf[bufidx] >> 4) & 0x0F) + '0';
			hub->portDescString[idx++] = (buf[bufidx++] & 0x0F) + '0';
		}
		hub->portDescString[idx] = '\0';
		mos_tlock_unlock(attachedDevicesLock);
		break;

	default:
		logerr("Got unexpected Lightning packet type: 0x%02x", packetType);
	}
}

PhidgetReturnCode PhidgetLightning_setup(void) {
	loginfo("Setting up lightning notifications");
	eaController = [[EAController alloc] init];

	return EPHIDGET_OK;
}

PhidgetReturnCode PhidgetLightning_teardown(void) {
	loginfo("Tearing down lightning notifications");
	[eaController release];
	eaController = NULL;

	return EPHIDGET_OK;
}

PhidgetReturnCode PhidgetLightningSendPacket(PhidgetLightningConnectionHandle conn, const unsigned char *buffer, size_t len) {
	uint8_t newBuf[MAX_LIGHTNING_OUT_PACKET_SIZE+2];

	assert(eaController);
	assert(len <= MAX_LIGHTNING_OUT_PACKET_SIZE);

	newBuf[0] = VINT_LIGHTING_OUT_PACKET_DATA | (((len+2) >> 8) & 0x3F);
	newBuf[1] = (len+2) & 0xFF;
	memcpy(newBuf+2, buffer, len);
	[eaController writeData:[NSData dataWithBytes:(uint8_t *)newBuf length:(int)len+2]];
	return EPHIDGET_OK;
}

#endif
