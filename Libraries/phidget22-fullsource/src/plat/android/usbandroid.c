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

#include "phidget.h"
#include "gpp.h"
#include "usb.h"
#include "manager.h"
#include "util/utils.h"

#include <jni.h>

#include "plat/android/com_phidget22_usb_Manager.h"
#include "plat/android/com_phidget22_usb_Phidget.h"
#include "plat/android/phidget_jni.h"

int AndroidUsbJarAvailable = PFALSE;
int AndroidUsbInitialized = PFALSE;

PhidgetReturnCode
PhidgetUSBCloseHandle(PhidgetUSBConnectionHandle conn) {
	PhidgetReturnCode res;

	assert(conn);

	if (!ANDROID_USB_GOOD)
		return EPHIDGET_UNSUPPORTED;

	if (conn->deviceHandle == NULL)
		return EPHIDGET_NOTATTACHED;

	stopUSBReadThread(conn);

	usbloginfo("Closing USB Device: 0x%" PRIXPTR, (uintptr_t)conn->deviceHandle);
	res = PhidgetAndroid_cancelRead(conn->deviceHandle);

	//Lock so we don't kill the handle while read is using it
	PhidgetRunLock(conn);

	res = PhidgetAndroid_closePhidget(&conn->deviceHandle);

	if (res != EPHIDGET_OK) {
		usblogerr("Failed to close USB Connection handle");
		PhidgetRunUnlock(conn);
		return (EPHIDGET_UNEXPECTED);
	}
	PhidgetRunUnlock(conn);

	return (EPHIDGET_OK);
}

PhidgetReturnCode
PhidgetUSBSendPacket(PhidgetUSBConnectionHandle conn, const unsigned char *buffer, size_t bufferLen) {
	uint8_t buf[MAX_OUT_PACKET_SIZE] = { 0 };
	PhidgetReturnCode res;

	assert(conn);
	assert(buffer);
	assert(bufferLen <= conn->outputReportByteLength);

	if (!ANDROID_USB_GOOD)
		return EPHIDGET_UNSUPPORTED;

	mos_mutex_lock(&conn->usbwritelock);

	if (conn->deviceHandle == NULL) {
		usblogwarn("Handle for writing is not valid");
		mos_mutex_unlock(&conn->usbwritelock);
		return EPHIDGET_UNEXPECTED;
	}

	memcpy(buf, buffer, bufferLen);

	res = PhidgetAndroid_write(conn->deviceHandle, buf, conn->outputReportByteLength);

	mos_mutex_unlock(&conn->usbwritelock);
	return (res);
}

PhidgetReturnCode
PhidgetUSBSetLabel(PhidgetDeviceHandle device, char *buffer) {
	PhidgetUSBConnectionHandle conn;
	int size;

	if (!ANDROID_USB_GOOD)
		return EPHIDGET_UNSUPPORTED;

	assert(device != NULL);

	if (deviceSupportsGeneralPacketProtocol(device))
		return (GPP_setLabel(device, buffer));

	conn = PhidgetUSBConnectionCast(device->conn);
	assert(conn);

	size = buffer[0];
	if (size > 22)
		return (EPHIDGET_INVALID);

	if (conn->deviceHandle == NULL) {
		usblogwarn("USB Connection device handle is NULL");
		return EPHIDGET_UNEXPECTED;
	}

	return PhidgetAndroid_setLabel(conn->deviceHandle, buffer);
}

PhidgetReturnCode
PhidgetUSBGetString(PhidgetUSBConnectionHandle conn, int index, char *str) {

	if (!ANDROID_USB_GOOD)
		return EPHIDGET_UNSUPPORTED;

	assert(conn);

	return PhidgetAndroid_getStringDescriptor(conn->dev, 0, conn->deviceHandle, index, str, 256);
}

PhidgetReturnCode
PhidgetUSBReadPacket(PhidgetUSBConnectionHandle conn, unsigned char *buffer) {
	PhidgetReturnCode res;

	if (!ANDROID_USB_GOOD)
		return EPHIDGET_UNSUPPORTED;

	assert(conn);

	PhidgetRunLock(conn);

	if (conn->deviceHandle == NULL) {
		usblogwarn("Handle for writing is not valid");
		PhidgetRunUnlock(conn);
		return EPHIDGET_UNEXPECTED;
	}

	res = PhidgetAndroid_read(conn->deviceHandle, buffer);

	PhidgetRunUnlock(conn);
	return (res);
}

PhidgetReturnCode
PhidgetUSBRefreshLabelString(PhidgetDeviceHandle device) {
	PhidgetUSBConnectionHandle conn;

	if (!ANDROID_USB_GOOD)
		return EPHIDGET_UNSUPPORTED;

	assert(device);
	conn = PhidgetUSBConnectionCast(device->conn);
	assert(conn);

	return PhidgetAndroid_refreshLabel(conn->deviceHandle, device->deviceInfo.label, device->deviceInfo.serialNumber);
}

PhidgetReturnCode
PhidgetUSBScanDevices(void) {

	if (!ANDROID_USB_GOOD)
		return EPHIDGET_UNSUPPORTED;

	return (PhidgetAndroidManager_scanDevices());
}

PhidgetReturnCode
PhidgetUSBOpenHandle(PhidgetDeviceHandle device) {
	PhidgetUSBConnectionHandle conn;

	if (!ANDROID_USB_GOOD)
		return EPHIDGET_UNSUPPORTED;

	assert(device);
	conn = PhidgetUSBConnectionCast(device->conn);
	assert(conn);

	return PhidgetAndroid_openPhidget(conn->dev, device->deviceInfo.UDD->interfaceNum, &conn->deviceHandle, &conn->inputReportByteLength, &conn->outputReportByteLength);
}
