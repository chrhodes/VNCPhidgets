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

#define POWER_CALLBACKS

#include "phidgetbase.h"
#include "manager.h"
#include "constants.h"
#include "plat/mac/macusb.h"
#include "util/utils.h"

// globals
static CFRunLoopRef				runLoopRef = NULL;
static IONotificationPortRef	gNotifyPort;
static io_iterator_t			gRawAddedIter;
static io_iterator_t			gRawRemovedIter;

#ifdef POWER_CALLBACKS
extern int pause_usb_traffic;
extern int usb_read_paused;
extern int usb_write_paused;

static IONotificationPortRef	notifyPortRef;
static io_connect_t				root_port; // a reference to the Root Power Domain IOService
static io_object_t				notifierObject; // notifier object, used to deregister later
#endif

#define mach_test(expr, code, msg) do { \
	kern_return_t err = (expr); \
	if (KERN_SUCCESS != err) { \
		LOG(code, "%s - %s(%x,%d)", \
		  msg, mach_error_string(err), \
		  err, err & 0xffffff); \
	} \
} while(0)

void (CCONV *fptrSleep)(void *) = 0;
void *fptrSleepptr = NULL;
void (CCONV *fptrWakeup)(void *) = 0;
void *fptrWakeupptr = NULL;

PhidgetReturnCode CCONV
Phidget_set_OnWillSleep_Handler(void (CCONV *fptr)(void *userPtr), void *userPtr) {

	fptrSleep = fptr;
	fptrSleepptr = userPtr;
	return EPHIDGET_OK;
}

PhidgetReturnCode CCONV
Phidget_set_OnWakeup_Handler(void (CCONV *fptr)(void *userPtr), void *userPtr) {

	fptrWakeup = fptr;
	fptrWakeupptr = userPtr;
	return EPHIDGET_OK;
}

void freeString(char * cstr) {

	if (cstr != NULL) {
		free(cstr);
		cstr = NULL;
	}
}

UInt16	Swap16(void *p) {

	*(UInt16 *)p = CFSwapInt16LittleToHost(*(UInt16 *)p);
	return *(UInt16 *)p;
}

UInt32	Swap32(void *p) {

	*(UInt32 *)p = CFSwapInt32LittleToHost(*(UInt32 *)p);
	return *(UInt32 *)p;
}

UInt64	Swap64(void *p) {
	*(UInt64 *)p = CFSwapInt64LittleToHost(*(UInt64 *)p);
	return *(UInt64 *)p;
}

//for getting the device descriptor
int GetDescriptor(IOUSBDeviceInterface245 **deviceIntf, UInt8 descType, UInt8 descIndex, void *buf, UInt16 len) {
	IOUSBDevRequest req;
	IOReturn err;

	req.bmRequestType = USBmakebmRequestType(kUSBIn, kUSBStandard, kUSBDevice);
	req.bRequest = kUSBRqGetDescriptor;
	req.wValue = (descType << 8) | descIndex;
	req.wIndex = 0;
	req.wLength = len;
	req.pData = buf;

	err = (*deviceIntf)->DeviceRequest(deviceIntf, &req);
	if (err != kIOReturnSuccess) {
		logwarn("GetDescriptor failed with error: 0x%08x", err);
		return -1;
	}
	return req.wLenDone;
}

//for getting the interface descriptor (HID Descriptor in this case)
int GetDescriptorFromInterface(IOUSBDeviceInterface245 **deviceIntf, UInt8 descType, UInt8 descIndex, UInt16 wIndex, void *buf, UInt16 len) {
	IOUSBDevRequest req;
	IOReturn err;

	req.bmRequestType = USBmakebmRequestType(kUSBIn, kUSBStandard, kUSBInterface);
	req.bRequest = kUSBRqGetDescriptor;
	req.wValue = (descType << 8) | descIndex;
	req.wIndex = wIndex;
	req.wLength = len;
	req.pData = buf;

	err = (*deviceIntf)->DeviceRequest(deviceIntf, &req);
	if (err != kIOReturnSuccess) {
		logwarn("GetDescriptorFromInterface failed with error: 0x%08x", err);
		return -1;
	}

	return req.wLenDone;
}

//for getting String descriptors, such as serial number, manufacturer, etc.
static int GetStringDescriptorMaster(void *deviceIntf, UInt8 descIndex, void *buf, UInt16 len, UInt16 lang, int isIOUSBDeviceInterface) {
	IOUSBDevRequest req;
	UInt8 		desc[256] = { 0 }; // Max possible descriptor length
	int stringLen;
	IOReturn err;
	if (lang == 0) // set default langID
		lang=0x0409;

	//this gets the descriptor length
	req.bmRequestType = USBmakebmRequestType(kUSBIn, kUSBStandard, kUSBDevice);
	req.bRequest = kUSBRqGetDescriptor;
	req.wValue = (kUSBStringDesc << 8) | descIndex;
	req.wIndex = lang;	// English
	req.wLength = 2;
	req.pData = &desc;

	if (isIOUSBDeviceInterface)
		err = (*(IOUSBDeviceInterface245 **)deviceIntf)->DeviceRequest((IOUSBDeviceInterface245 **)deviceIntf, &req);
	else
		err = (*(IOUSBInterfaceInterface **)deviceIntf)->ControlRequest((IOUSBInterfaceInterface **)deviceIntf, 0, &req);
	switch (err) {
	case kIOReturnSuccess:
	case kIOReturnOverrun:
		//Acceptable
		break;
	case kIOUSBPipeStalled:
		//Descriptor doesn't exist
		return -2;
	default:
		//actual error
		logwarn("GetStringDescriptor failed getting length with error: 0x%08x", err);
		return -1;
	}

	// If the string is 0 (it happens), then just return 0 as the length
	//
	stringLen = desc[0];
	if (stringLen == 0) {
		return 0;
	}

	// OK, now that we have the string length, make a request for the full length
	//
	req.bmRequestType = USBmakebmRequestType(kUSBIn, kUSBStandard, kUSBDevice);
	req.bRequest = kUSBRqGetDescriptor;
	req.wValue = (kUSBStringDesc << 8) | descIndex;
	req.wIndex = lang;	// English
	req.wLength = stringLen;
	req.pData = buf;

	if (isIOUSBDeviceInterface)
		err = (*(IOUSBDeviceInterface245 **)deviceIntf)->DeviceRequest((IOUSBDeviceInterface245 **)deviceIntf, &req);
	else
		err = (*(IOUSBInterfaceInterface **)deviceIntf)->ControlRequest((IOUSBInterfaceInterface **)deviceIntf, 0, &req);

	if ((err != kIOReturnSuccess) && (err != kIOReturnOverrun)) {
		logwarn("GetStringDescriptor failed getting string with error: 0x%08x", err);
		return -1;
	}
	return req.wLenDone;
}

int GetStringDescriptor_Device(IOUSBDeviceInterface245 **deviceIntf, UInt8 descIndex, void *buf, UInt16 len, UInt16 lang) {
	return GetStringDescriptorMaster(deviceIntf, descIndex, buf, len, lang, TRUE);
}
int GetStringDescriptor_Interface(IOUSBInterfaceInterface **deviceIntf, UInt8 descIndex, void *buf, UInt16 len, UInt16 lang) {
	return GetStringDescriptorMaster(deviceIntf, descIndex, buf, len, lang, FALSE);
}

//formats getStringDescriptor messages to get string descriptors based on their index value
PhidgetReturnCode getStringProperty(UInt8 strIndex, IOUSBDeviceInterface245 **deviceIntf, char *str2, int str2size) {
	Byte buf[256];
	int trycount;

	if (strIndex > 0) {
		int len;
		buf[0] = 0;

		trycount = 0;
		while ((len = GetStringDescriptor_Device(deviceIntf, strIndex, buf, sizeof(buf), 0)) == -1 || len == 0 || len != buf[0]) {
			trycount++;
			if (trycount >= 10) {
				logerr("getStringProperty failed after multiple attempts");
				return EPHIDGET_UNEXPECTED;
			}
			usleep(25000);
		}

		if (len > 2) {
			Byte *p;
			CFStringRef str;
			/* This converts to machine endianness from little endian - so does nothing on Intel */
			for (p = buf + 2; p < buf + len; p += 2) {
				Swap16(p);
			}
			str = CFStringCreateWithCharacters(NULL, (const UniChar *)(buf + 2), (len - 2) / 2);
			CFStringGetCString(str, str2, str2size, kCFStringEncodingUTF8);
			CFRelease(str);
		} else {
			str2[0] = '\0';
		}

		//loginfo("Label len: %d, string: \"%s\"",len,str2);
	} else {
		str2[0] = '\0';
	}

	return EPHIDGET_OK;
}

PhidgetReturnCode getUSBString(PhidgetUSBConnectionHandle conn, char *str2, int index) {
	Byte buf[256] = { 0 };
	int trycount;

	IOUSBInterfaceInterface **intf = (IOUSBInterfaceInterface**)conn->intf;

	int len;
	buf[0] = 0;

	trycount = 0;
	while ((len = GetStringDescriptor_Interface(intf, index, buf, sizeof(buf), 0)) == -1 || len == 0 || len != buf[0]) {
		if (len == -2) {
			return EPHIDGET_UNEXPECTED;
		}

		trycount++;
		if (trycount >= 10) {
			logerr("getString failed after multiple attempts");
			return EPHIDGET_UNEXPECTED;
		}
		usleep(25000);
	}

	str2[0] = '\0';
	if (len > 2) {
		return UTF16toUTF8((char *)&buf[2], buf[0] - 2, str2);
	} else {
		return EPHIDGET_OK;
	}
}

PhidgetReturnCode getLabelString(void *deviceIntf, char *str2, int serialNumber, int isIOUSBDeviceInterface) {
	Byte buf[256];
	int trycount;
	int ret = EPHIDGET_OK;

	int len;
	buf[0] = 0;

	trycount = 0;
	while ((len = GetStringDescriptorMaster(deviceIntf, 4, buf, sizeof(buf), 0, isIOUSBDeviceInterface)) == -1 || len == 0 || len != buf[0]) {
		if (len == -2) {
			str2[0] = '\0';
			loginfo("No label string - probably an older Phidget.");
			return EPHIDGET_OK;
		}

		trycount++;
		if (trycount >= 10) {
			logerr("getLabelString failed after multiple attempts");
			return EPHIDGET_UNEXPECTED;
		}
		usleep(25000);
	}

	if (len > 2) {
		ret = decodeLabelString((char *)buf, str2, serialNumber);
	} else {
		str2[0] = '\0';
	}

	//loginfo("Label len: %d, string: \"%s\"",len,str2);

	return ret;
}


PhidgetReturnCode reenumerateDevice(PhidgetDeviceHandle device) {
	IOCFPlugInInterface **plugInInterface=NULL;
	PhidgetUSBConnectionHandle conn;
	IOUSBInterfaceInterface **intf;
	IOUSBDeviceInterface245 **dev;
	io_service_t usbDevice2;
	kern_return_t kr;
	SInt32 score;
	HRESULT res;

	assert(device);
	conn = PhidgetUSBConnectionCast(device->conn);
	assert(conn);

	intf = (IOUSBInterfaceInterface**)conn->intf;

	logdebug("Trying to reset device...");

	kr = (*intf)->GetDevice(intf, &usbDevice2);
	if (kIOReturnSuccess != kr) {
		logerr("Failed to get device (%08x)", kr);
		return EPHIDGET_UNEXPECTED;
	}

	kr = IOCreatePlugInInterfaceForService(usbDevice2, kIOUSBDeviceUserClientTypeID, kIOCFPlugInInterfaceID, &plugInInterface, &score);
	if ((kIOReturnSuccess != kr) || !plugInInterface) {
		logerr("unable to create a plugin (%08x)\n", kr);
		return EPHIDGET_UNEXPECTED;
	}

	kr = IOObjectRelease(usbDevice2);
	if (kIOReturnSuccess != kr) {
		logerr("Failed release usbDevice (%08x)", kr);
		return EPHIDGET_UNEXPECTED;
	}

	// I have the device plugin, I need the device interface
	res = (*plugInInterface)->QueryInterface(plugInInterface, CFUUIDGetUUIDBytes(kIOUSBDeviceInterfaceID), (LPVOID*)&dev);
	IODestroyPlugInInterface(plugInInterface);
	if (res || !dev) {
		logerr("couldn't create a device interface (%08x)\n", (int)res);
		return EPHIDGET_UNEXPECTED;
	}

	//need to open it first
	if (kIOReturnSuccess != (*dev)->USBDeviceOpen(dev)) {
		(void)(*dev)->Release(dev);
		logerr("Failed to open device (%08x)", kr);
		return EPHIDGET_UNEXPECTED;
	}

	kr = (*dev)->USBDeviceReEnumerate(dev, 0);
	if (kIOReturnSuccess != kr) {
		(void)(*dev)->Release(dev);
		logerr("Failed to reset device (%08x)", kr);
		return EPHIDGET_UNEXPECTED;
	}

	return EPHIDGET_OK;
}

PhidgetReturnCode refreshLabelString(PhidgetDeviceHandle device) {
	char label[MAX_LABEL_STORAGE] = { 0 };
	PhidgetUSBConnectionHandle conn;

	assert(device);
	conn = PhidgetUSBConnectionCast(device->conn);
	assert(conn);

	//get the label string
	if (getLabelString(conn->intf, label, device->deviceInfo.serialNumber, FALSE)) {
		logerr("Failed to get label string");
		return (EPHIDGET_UNEXPECTED);
	}

	memcpy(device->deviceInfo.label, label, MAX_LABEL_STORAGE);

	return (EPHIDGET_OK);
}

/*
This is a notification function called when a Phidget has matched the kernel driver
It will instantiate and verify the Phidget and add it to the Phidget list for Phidget Manager
*/
void RawDeviceAdded(void *refCon, io_iterator_t iterator) {
	kern_return_t		kr;
	HRESULT 			res;
	io_service_t	usbDevice;
	PhidgetUSBConnectionHandle conn;
	logverbose("Running RawDeviceAdded()");

	//iterate devices
	while ((usbDevice = IOIteratorNext(iterator))) {
		IOUSBDeviceInterface245 **dev = NULL;
		SInt32 					score;
		IOCFPlugInInterface 	**plugInInterface=NULL;

		loginfo("Attach: 0x%08lx\n", (long)usbDevice);

		kr = IOCreatePlugInInterfaceForService(usbDevice, kIOUSBDeviceUserClientTypeID, kIOCFPlugInInterfaceID, &plugInInterface, &score);

		if ((kIOReturnSuccess != kr) || !plugInInterface) {
			logerr("unable to create a plugin (%08x)", kr);
			continue;
		}

		// I have the device plugin, I need the device interface
		res = (*plugInInterface)->QueryInterface(plugInInterface, CFUUIDGetUUIDBytes(kIOUSBDeviceInterfaceID), (LPVOID*)&dev);
		(*plugInInterface)->Stop(plugInInterface);
		IODestroyPlugInInterface(plugInInterface);

		if (res || !dev) {
			logerr("couldn't create a device interface (%08x)\n", (int)res);
			continue;
		}

		//read properties
		UInt16			vendor;
		UInt16			product;
		UInt16			release;
		int 			version;

		if ((kr = (*dev)->GetDeviceVendor(dev, &vendor)) != kIOReturnSuccess)
			goto error_device;
		if ((kr = (*dev)->GetDeviceProduct(dev, &product)) != kIOReturnSuccess)
			goto error_device;
		if ((kr = (*dev)->GetDeviceReleaseNumber(dev, &release)) != kIOReturnSuccess)
			goto error_device;

		//decode Phidgets version from release number
		if (release < 0x100)
			version = release * 100;
		else
			version = ((release >> 8) * 100) + ((release & 0xff));

		//iterate interfaces
		IOUSBFindInterfaceRequest	request =
		{
			.bInterfaceClass = kIOUSBFindInterfaceDontCare,
			.bInterfaceSubClass = kIOUSBFindInterfaceDontCare,
			.bInterfaceProtocol = kIOUSBFindInterfaceDontCare,
			.bAlternateSetting = kIOUSBFindInterfaceDontCare,
		};
		io_iterator_t				interfaceIterator;
		io_service_t				usbInterface;

		(*dev)->CreateInterfaceIterator(dev, &request, &interfaceIterator);
		while ((usbInterface = IOIteratorNext(interfaceIterator)) != 0) {
			IOUSBInterfaceInterface 	**intf = NULL;

			IOCreatePlugInInterfaceForService(usbInterface, kIOUSBInterfaceUserClientTypeID, kIOCFPlugInInterfaceID, &plugInInterface, &score);
			kr = IOObjectRelease(usbInterface); // done with the usbInterface object now that I have the plugin
			if ((kIOReturnSuccess != kr) || !plugInInterface) {
				logerr("unable to create a plugin (%08x)", kr);
				continue;
			}

			// I have the interface plugin. I need the interface interface
			res = (*plugInInterface)->QueryInterface(plugInInterface, CFUUIDGetUUIDBytes(kIOUSBInterfaceInterfaceID), (LPVOID*)&intf);
			(*plugInInterface)->Stop(plugInInterface);
			IODestroyPlugInInterface(plugInInterface);

			if (res || !intf) {
				logerr("couldn't create a interface interface (%08x)\n", (int)res);
				continue;
			}

			//read properties
			UInt8	interfaceNumber;

			if ((kr = (*intf)->GetInterfaceNumber(intf, &interfaceNumber)) != kIOReturnSuccess)
				goto error_interface;

			//Next match this device against a specific phidget
			const PhidgetUniqueDeviceDef *pdd = Phidget_Unique_Device_Def;
			while (((int)pdd->type) != END_OF_LIST) {
				if (pdd->type == PHIDTYPE_USB
					&& vendor == pdd->vendorID
					&& product == pdd->productID
					&& interfaceNumber == pdd->interfaceNum
					&& version >= pdd->versionLow
					&& version < pdd->versionHigh) {
					//Found a match! 1st, read out all the properties
					UInt8 serialStringIndex, productStringIndex;
					char serialString[50];
					int serial;
					char label[MAX_LABEL_STORAGE];

					if ((kr = (*dev)->USBGetSerialNumberStringIndex(dev, &serialStringIndex)) != kIOReturnSuccess)
						goto error_interface;
					if ((kr = (*dev)->USBGetProductStringIndex(dev, &productStringIndex)) != kIOReturnSuccess)
						goto error_interface;

					if (getStringProperty(serialStringIndex, dev, serialString, 50)) {
						logerr("Failed to get serial number string");
						goto error_interface;
					}
					serial = atoi(serialString);

					if (getLabelString(dev, label, serial, TRUE)) {
						loginfo("Failed to get label string - probably an older Phidget.");
						label[0] = '\0';
					}

					char productString[64];
					if (getStringProperty(productStringIndex, dev, productString, 64)) {
						logerr("Failed to get Product String");
						goto error_interface;
					}

					//then, instantiate and attach this phidget.
					PhidgetDeviceHandle device = NULL;

					createPhidgetUSBDevice(pdd, version, label, serial, NULL, productString, &device);

					PhidgetSetFlags(device, PHIDGET_SCANNED_FLAG);

					conn = PhidgetUSBConnectionCast(device->conn);

					conn->usbDevice = usbDevice;
					conn->intf = intf;

					//success! run attach events, etc.
					deviceAttach(device, 1);

					PhidgetRelease(&device); /* release our reference */

					goto next_interface;
				}
				pdd++;
			}

			//not found
			//If this is a valid Phidgets owned PID, then inform the log
			if (vendor == USBVID_PHIDGETS && product >= USBPID_PHIDGETS_MIN && product <= USBPID_PHIDGETS_MAX) {
				logwarn("A USB Phidget (PID: 0x%04x Version: %d) was found which is not supported by the library. A library upgrade is probably required to work with this Phidget", product, version);
			}

		error_interface:
			(void)(*intf)->Release(intf);
		next_interface:
			continue;
		}

	error_device:
		continue;
	}
}

/*
This is a notification function that gets called when a Phidget is unplugged
It will remove that Phidget from the Phidget list
*/
void RawDeviceRemoved(void *refCon, io_iterator_t iterator) {
	io_service_t	usbDevice;
	PhidgetDeviceHandle device;
	PhidgetUSBConnectionHandle conn;
	logverbose("Running RawDeviceRemoved()");

	PhidgetWriteLockDevices();
	while ((usbDevice = IOIteratorNext(iterator))) {
		loginfo("Detach: 0x%08lx\n", (long)usbDevice);
		FOREACH_DEVICE(device) {
			if (device->connType == PHIDCONN_USB) {
				conn = PhidgetUSBConnectionCast(device->conn);
				assert(conn);
				if (IOObjectIsEqualTo(usbDevice, conn->usbDevice))
					deviceDetach(device, 0);
			}
		}
	next:
		IOObjectRelease(usbDevice);
	}
	PhidgetUnlockDevices();
}

void SignalHandler(int sigraised) {
	PhidgetUSBTeardownNotifications();

	// exit(0) should not be called from a signal handler.  Use _exit(0) instead
	_exit(0);
}

#ifdef POWER_CALLBACKS

void
MySleepCallBack(void * refCon, io_service_t service, natural_t messageType, void * messageArgument) {

	logverbose("Running MySleepCallBack()");
	switch (messageType) {

	case kIOMessageCanSystemSleep:
	case kIOMessageSystemWillSleep:
	{
		PhidgetChannelHandle channel;
		int waitTime = 3000;
		int attachedDevices = PFALSE;

		//Lets the user access Phidgets before sleep
		if (fptrSleep)
			fptrSleep(fptrSleepptr);

		//Pause USB traffic
		loginfo("Pausing USB traffic in preparation for system sleep...");

		pause_usb_traffic = PTRUE;

		FOREACH_CHANNEL(channel) {
			if (PhidgetCKFlags(channel, PHIDGET_ATTACHED_FLAG)) {
				attachedDevices = PTRUE;
				break;
			}
		}

		// wait for reads/writes to stop
		if (attachedDevices)
			while ((!usb_read_paused || !usb_write_paused) && waitTime > 0) {
				mos_usleep(10000);
				waitTime-=10;
			}

		loginfo("USB traffic paused, now sleeping...");

		//resume sleeping
		IOAllowPowerChange(root_port, (long)messageArgument);
	}
	break;

	case kIOMessageSystemWillPowerOn:
		//System has started the wake up process...
		break;

	case kIOMessageSystemHasPoweredOn:
		//Resume USB traffic
		loginfo("Resuming USB traffic on system wake...");

		pause_usb_traffic = PFALSE;

		//Lets the user access Phidgets after wakeup
		if (fptrWakeup)
			fptrWakeup(fptrWakeupptr);

		break;

	default:
		break;
	}
}
#endif

PhidgetReturnCode CCONV
PhidgetUSBTeardownNotifications() {
	
	logverbose("Running PhidgetUSBTeardownNotifications()");
	
	if (!runLoopRef)
		return (EPHIDGET_OK);
#ifdef POWER_CALLBACKS
	// remove the sleep notification port from the application runloop
	CFRunLoopRemoveSource(runLoopRef,
						  IONotificationPortGetRunLoopSource(notifyPortRef),
						  kCFRunLoopCommonModes);

	// deregister for system sleep notifications
	IODeregisterForSystemPower(&notifierObject);

	// IORegisterForSystemPower implicitly opens the Root Power Domain IOService
	// so we close it here
	IOServiceClose(root_port);

	IONotificationPortDestroy(notifyPortRef);
#endif

	//deregister phidgets
	CFRunLoopRemoveSource(runLoopRef,
						  IONotificationPortGetRunLoopSource(gNotifyPort),
						  kCFRunLoopDefaultMode);

	// Clean up here
	IONotificationPortDestroy(gNotifyPort);

	if (gRawAddedIter) {
		IOObjectRelease(gRawAddedIter);
		gRawAddedIter = 0;
	}

	if (gRawRemovedIter) {
		IOObjectRelease(gRawRemovedIter);
		gRawRemovedIter = 0;
	}

	runLoopRef = NULL;

	return (EPHIDGET_OK);
}

/*
Sets up notifications for adding and removing Phidgets.

Right now: on Phidget add, RawDeviceAdded is called, on remove, RawDeviceRemoved is called
Calling this function will block! as the notifications happen within the run loop.
*/
PhidgetReturnCode CCONV
PhidgetUSBSetupNotifications(CFRunLoopRef runloop) {
	mach_port_t				masterPort;
	kern_return_t			kr;
	sig_t					oldHandler;

	logverbose("Running PhidgetUSBSetupNotifications()");

	runLoopRef = runloop;

	CFMutableDictionaryRef devicesDict = CFDictionaryCreateMutable(NULL, 0, &kCFTypeDictionaryKeyCallBacks, &kCFTypeDictionaryValueCallBacks);
	if (!devicesDict) {
		logerr("Can't create a USB matching dictionary");
		return (EPHIDGET_UNEXPECTED);
	}

	// Set up a signal handler so we can clean up when we're interrupted
	// Otherwise we stay in our run loop forever.
	oldHandler = signal(SIGINT, SignalHandler);
	if (oldHandler == SIG_ERR)
		logerr("Could not establish new signal handler");

	// first create a master_port for my task
	kr = IOMasterPort(MACH_PORT_NULL, &masterPort);
	if (kr || !masterPort) {
		logerr("ERR: Couldn't create a master IOKit Port(%08x)", kr);
		return (EPHIDGET_UNEXPECTED);
	}

#ifdef POWER_CALLBACKS
	//register for power notifications
	root_port = IORegisterForSystemPower(NULL, &notifyPortRef, MySleepCallBack, &notifierObject);
	if (root_port == 0) {
		logerr("IORegisterForSystemPower failed");
		return (EPHIDGET_UNEXPECTED);
	}

	// add the notification port to the application runloop
	CFRunLoopAddSource(runloop, IONotificationPortGetRunLoopSource(notifyPortRef), kCFRunLoopDefaultMode);
#endif

	// Create a notification port and add its run loop event source to our run loop
	// This is how async notifications get set up.
	gNotifyPort = IONotificationPortCreate(masterPort);
	CFRunLoopAddSource(runloop, IONotificationPortGetRunLoopSource(gNotifyPort), kCFRunLoopDefaultMode);

	const PhidgetUniqueDeviceDef *pdd = Phidget_Unique_Device_Def;
	while (((int)pdd->type) != END_OF_LIST) {
		/* Ignore non-usb devices */
		if (pdd->type != PHIDTYPE_USB) {
			pdd++;
			continue;
		}

		int vp = (pdd->vendorID << 16) | pdd->productID;
		logverbose("Adding match for %08x", vp);
		CFNumberRef vendorpid = CFNumberCreate(kCFAllocatorDefault, kCFNumberSInt32Type, &vp);
		if (CFDictionaryContainsKey(devicesDict, vendorpid)) {
			CFRelease(vendorpid);
			pdd++;
			logverbose("continue - already saw this vendorpid");
			continue;
		}

		CFDictionaryAddValue(devicesDict, vendorpid, CFSTR("true"));
		CFRelease(vendorpid);

		CFMutableDictionaryRef matchingDict = IOServiceMatching(kIOUSBDeviceClassName);	// Interested in instances of class IOUSBInterface and its subclasses
		if (!matchingDict) {
			logerr("Can't create a USB matching dictionary");
			mach_port_deallocate(mach_task_self(), masterPort);
			return (EPHIDGET_UNEXPECTED);
		}

		// Retain additional references because we use this same dictionary with 2 calls to
		// IOServiceAddMatchingNotification, each of which consumes one reference.
		matchingDict = (CFMutableDictionaryRef)CFRetain(matchingDict);

		// Add our vendor ID to the matching criteria
		CFNumberRef vendor = CFNumberCreate(kCFAllocatorDefault, kCFNumberSInt32Type, &pdd->vendorID);
		CFDictionarySetValue(matchingDict, CFSTR(kUSBVendorID), vendor);
		CFRelease(vendor);

		CFNumberRef product = CFNumberCreate(kCFAllocatorDefault, kCFNumberSInt32Type, &pdd->productID);
		CFDictionarySetValue(matchingDict, CFSTR(kUSBProductID), product);
		CFRelease(product);

		// Now set up two notifications, one to be called when a raw device is first matched by I/O Kit, and the other to be
		// called when the device is terminated.
		IOServiceAddMatchingNotification(gNotifyPort,
		  kIOFirstMatchNotification,
		  matchingDict,
		  RawDeviceAdded,
		  NULL,
		  &gRawAddedIter);
		RawDeviceAdded(NULL, gRawAddedIter);	// Iterate once to get already-present devices and
												// arm the notification

		IOServiceAddMatchingNotification(gNotifyPort,
		  kIOTerminatedNotification,
		  matchingDict,
		  RawDeviceRemoved,
		  NULL,
		  &gRawRemovedIter);
		RawDeviceRemoved(NULL, gRawRemovedIter);	// Iterate once to arm the notification

		pdd++;
	}

	CFRelease(devicesDict);

	// Now done with the master_port
	mach_port_deallocate(mach_task_self(), masterPort);
	masterPort = 0;

	return (EPHIDGET_OK);
}
