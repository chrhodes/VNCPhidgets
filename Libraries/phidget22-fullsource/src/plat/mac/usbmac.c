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


#include "phidgetbase.h"
#include "usb.h"
#include "manager.h"
#include "gpp.h"
#include "plat/mac/macusb.h"

/* CUSB is meant to be platform dependent */
PhidgetReturnCode PhidgetUSBCloseHandle(PhidgetUSBConnectionHandle conn) {
	IOUSBInterfaceInterface **intf;

	assert(conn);
	intf = conn->intf;

	if (((*intf)->USBInterfaceClose(intf)) == kIOReturnNoDevice)
		return (EPHIDGET_NOTATTACHED);

	stopUSBReadThread(conn);

	return (EPHIDGET_OK);
}

PhidgetReturnCode PhidgetUSBSendPacket(PhidgetUSBConnectionHandle conn, const unsigned char *buffer, size_t bufferLen) {
	int stallCount = 0;
	unsigned long BytesWritten = 0;
	IOReturn			kr;
	IOUSBDevRequest 		request;
	IOUSBInterfaceInterface **intf = conn->intf;
	uint8_t outbuffer[MAX_USB_OUT_PACKET_SIZE] = { 0 };

	assert(conn);
	assert(buffer);
	assert(bufferLen <= conn->outputReportByteLength);
	memcpy(outbuffer, buffer, bufferLen);

	if (!conn->interruptOutEndpoint) {
		request.bmRequestType = USBmakebmRequestType(kUSBOut, kUSBClass, kUSBInterface);
		request.bRequest = kUSBRqSetConfig;
		request.wValue = 0x0200;
		request.wIndex = conn->interfaceNum;
		request.wLength = conn->outputReportByteLength;
		request.pData = (void *)outbuffer;
	}

tryagain:

	if (conn->interruptOutEndpoint) {
		//Why 2? I don't know either...
		kr = (*intf)->WritePipe(intf, 2, (void *)buffer, conn->outputReportByteLength);
	} else {
		kr = (*intf)->ControlRequest(intf, 0, &request);
	}

	switch (kr) {
	case kIOReturnSuccess:
		break;

	case kIOReturnNoDevice:
		logerr("USB Write returned kIOReturnNoDevice: there is no connection to an IOService.");
		return EPHIDGET_UNEXPECTED;
	case kIOReturnNotOpen:
		logerr("USB Write returned kIOReturnNotOpen: the interface is not open for exclusive access.");
		return EPHIDGET_UNEXPECTED;
	case kIOUSBTransactionTimeout:
		logerr("USB Write returned kIOUSBTransactionTimeout: A timeout occured.");
		return EPHIDGET_UNEXPECTED;
	case kIOUSBPipeStalled:
		stallCount++;
		if (stallCount <= 1) {
			loginfo("USB Write returned kIOUSBPipeStalled: We'll try to clear it.");
			//Try to clear the stall
			kr = (*intf)->ClearPipeStall(intf, 0);
			switch (kr) {
			case kIOReturnSuccess:
				loginfo("ClearPipeStall Succeeded, trying to read again.");
				goto tryagain;
			default:
				logerr("ClearPipeStall returned an unexpected value: %x", kr);
				return EPHIDGET_UNEXPECTED;
			}
		} else {
			logerr("USB Write returned kIOUSBPipeStalled too many times in a row. Will try a device reset.");
		}

		return EPHIDGET_UNEXPECTED;
	case kIOReturnNotResponding:
		logwarn("USB Write returned kIOReturnNotResponding. Device was probably unplugged.");
		return EPHIDGET_NOTATTACHED;
	case kIOUSBEndpointNotFound:
		logerr("USB Write returned kIOUSBEndpointNotFound.");
		return EPHIDGET_UNEXPECTED;

	default:
		/* Sometimes there is an unexpected return value - why??? */
		logerr("USB Write returned an unexpected value: %x", kr);
		return EPHIDGET_UNEXPECTED;
	}

	if (!conn->interruptOutEndpoint)
		BytesWritten = request.wLenDone;

	if (!kr && (conn->interruptOutEndpoint ? 1 : BytesWritten == conn->outputReportByteLength))
		return EPHIDGET_OK;
	else {
		logerr("Wrong number of Bytes written in CUSBSendPacket: %d", BytesWritten);
		return EPHIDGET_UNEXPECTED;
	}
}

PhidgetReturnCode PhidgetUSBSetLabel(PhidgetDeviceHandle device, char *buffer) {
	PhidgetUSBConnectionHandle conn;
	IOUSBInterfaceInterface **intf;
	unsigned long BytesWritten;
	IOUSBDevRequest request;
	IOReturn kr;

	assert(device);
	assert(buffer);

	conn = PhidgetUSBConnectionCast(device->conn);
	assert(conn);

	if (deviceSupportsGeneralPacketProtocol(device))
		return GPP_setLabel(device, buffer);

	intf = conn->intf;

	request.bmRequestType = USBmakebmRequestType(kUSBOut, kUSBStandard, kUSBDevice);
	request.bRequest = kUSBRqSetDescriptor;
	request.wValue = 0x0304;
	request.wIndex = 0x0409;
	request.wLength = buffer[0];
	request.pData = buffer;

	kr = (*intf)->ControlRequest(intf, 0, &request);
	if (kr)
		return EPHIDGET_UNSUPPORTED;

	BytesWritten = request.wLenDone;

	if (kr || BytesWritten != buffer[0])
		return EPHIDGET_UNEXPECTED;

	return EPHIDGET_OK;
}

/* Buffer should be at least 8 bytes long */
PhidgetReturnCode PhidgetUSBReadPacket(PhidgetUSBConnectionHandle phid, unsigned char *buffer) {
	int stallCount = 0;
	IOReturn kr;
	IOUSBInterfaceInterface **intf = phid->intf;
	UInt32 BytesRead = phid->inputReportByteLength;

tryagain:
	kr = (*intf)->ReadPipe(intf, 1, buffer, &BytesRead);
	switch (kr) {
	case kIOReturnSuccess:
		break;
	case kIOReturnNoDevice:
		logerr("ReadPipe returned kIOReturnNoDevice: there is no connection to an IOService.");
		return EPHIDGET_UNEXPECTED;
	case kIOReturnNotOpen:
		logerr("ReadPipe returned kIOReturnNotOpen: the interface is not open for exclusive access.");
		return EPHIDGET_UNEXPECTED;
	case kIOReturnNotResponding:
		logwarn("ReadPipe returned kIOReturnNotResponding: Device was probably unplugged.");
		return EPHIDGET_NOTATTACHED;
	case kIOReturnAborted:
		loginfo("ReadPipe returned kIOReturnAborted: This probably means that close was called.");
		return EPHIDGET_INTERRUPTED;
	case kIOUSBPipeStalled:
		stallCount++;
		if (stallCount <= 1) {
			loginfo("ReadPipe returned kIOUSBPipeStalled: We'll try to clear it.");
			//Try to clear the stall
			kr = (*intf)->ClearPipeStall(intf, 1);
			switch (kr) {
			case kIOReturnSuccess:
				loginfo("ClearPipeStall Succeeded, trying to read again.");
				goto tryagain;
			default:
				logerr("ClearPipeStall returned an unexpected value: %x", kr);
				return EPHIDGET_UNEXPECTED;
			}
		} else {
			logerr("ReadPipe returned kIOUSBPipeStalled too many times in a row. Will try a device reset.");
		}

		return EPHIDGET_UNEXPECTED;
	default:
		/* Sometimes there is an unexpected return value - why?? */
		logerr("ReadPipe returned an unexpected value: %x", kr);
		return EPHIDGET_UNEXPECTED;
	}

	if (phid->inputReportByteLength != BytesRead) {
		logerr("Wrong number of Bytes read: %d", BytesRead);
		return EPHIDGET_UNEXPECTED;
	}

	return EPHIDGET_OK;
}

PhidgetReturnCode
PhidgetUSBGetString(PhidgetUSBConnectionHandle conn, int index, char *str) {

	TESTPTR(conn);
	return getUSBString(conn, str, index);
}

PhidgetReturnCode
PhidgetUSBRefreshLabelString(PhidgetDeviceHandle device) {

	assert(device);
	assert(device->conn);

	return refreshLabelString(device);
}

PhidgetReturnCode
PhidgetUSBResetDevice(PhidgetDeviceHandle device) {

	assert(device);
	assert(device->conn);

	return reenumerateDevice(device);
}

PhidgetReturnCode
PhidgetUSBOpenHandle(PhidgetDeviceHandle device) {
	IOCFPlugInInterface **plugInInterface;
	PhidgetUSBConnectionHandle conn;
	IOUSBInterfaceInterface **intf;
	IOUSBDeviceInterface245 **dev;
	UInt8 numEndpoints;
	kern_return_t kr;
	HRESULT result;
	Byte buf1[256];
	SInt32 score;
	int trycount;
	int len;
	int i;

	assert(device);

	conn = PhidgetUSBConnectionCast(device->conn);
	assert(conn);

	intf = conn->intf;
	assert(intf);

	kr = (*intf)->USBInterfaceOpen(intf);
	if (kr != kIOReturnSuccess) {
		logerr("Error opening interface: (%08x)\n", kr);
		return EPHIDGET_UNEXPECTED;
	}

	//ok, we have an opened interface.. read in the rest of our properties.
	kr = (*intf)->GetNumEndpoints(intf, &numEndpoints);
	if (kr != kIOReturnSuccess)
		goto error_exit;

	if (numEndpoints == 2) {
		loginfo("Using Interrupt OUT Endpoint for Host->Device communication.");
		conn->interruptOutEndpoint = PTRUE;
	} else {
		loginfo("Using Control Endpoint for Host->Device communication.");
		conn->interruptOutEndpoint = PFALSE;
	}

	loginfo("Attach: 0x%08lx\n", (long)conn->usbDevice);

	plugInInterface = NULL;
	kr = IOCreatePlugInInterfaceForService(conn->usbDevice, kIOUSBDeviceUserClientTypeID, kIOCFPlugInInterfaceID, &plugInInterface, &score);

	if ((kr != kIOReturnSuccess) || !plugInInterface) {
		logerr("unable to create a plugin (%08x)", kr);
		goto error_exit;
	}

	// I have the device plugin, I need the device interface
	dev = NULL;
	result = (*plugInInterface)->QueryInterface(plugInInterface, CFUUIDGetUUIDBytes(kIOUSBDeviceInterfaceID), (LPVOID*)&dev);
	(*plugInInterface)->Stop(plugInInterface);
	IODestroyPlugInInterface(plugInInterface);

	if (result || !dev) {
		logerr("couldn't create a device interface (%08x)\n", (int)result);
		goto error_exit;
	}

	//find output report size from HID descriptor
	trycount = 0;
	while ((len = GetDescriptorFromInterface(dev, kUSBReportDesc, 0, device->deviceInfo.UDD->interfaceNum, &buf1, sizeof(buf1))) == -1) {
		trycount++;
		if (trycount > 5) {
			logerr("Failed to get HID descriptor after several tries");
			goto error_exit;
		}
	}

	if (len >= 10) {
		for (i=10; i < len; i++) {
			if (buf1[i] == 0x81 && buf1[i - 2] == 0x95)
				conn->inputReportByteLength=buf1[i - 1];
			else if (buf1[i] == 0x81 && buf1[i - 4] == 0x95)
				conn->inputReportByteLength=buf1[i - 3];
			if (buf1[i] == 0x91 && buf1[i - 2] == 0x95)
				conn->outputReportByteLength=buf1[i - 1];
			else if (buf1[i] == 0x91 && buf1[i - 4] == 0x95)
				conn->outputReportByteLength=buf1[i - 3];
		}
	} else {
		logerr("Couldn't get report lengths in CUSBGetDeviceCapabilities");
		goto error_exit;
	}

	conn->intf = intf;
	conn->interfaceNum = device->deviceInfo.UDD->interfaceNum;

	return EPHIDGET_OK;

error_exit:
	(*intf)->USBInterfaceClose(intf);
	return EPHIDGET_UNEXPECTED;
}

PhidgetReturnCode
PhidgetUSBScanDevices() {
	return EPHIDGET_UNEXPECTED;
}
