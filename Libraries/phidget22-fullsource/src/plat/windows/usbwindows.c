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
#include "gpp.h"
#include "manager.h"
#include "util/phidgetlog.h"

#include <Wtypes.h>
#include <setupapi.h>
#include <wchar.h>
#include <dbt.h>
#include <Winuser.h>

#ifdef _WINDOWS
#include "ext/include/hid-windows/hidsdi.h"
#include "ext/include/hid-windows/hidpi.h"
#else
#include <ddk/hidsdi.h>
#include <ddk/hidpi.h>
#endif

#define usblogerr(...) PhidgetLog_loge(NULL, 0, NULL, "phidget22usb", PHIDGET_LOG_ERROR, __VA_ARGS__)
#define usbloginfo(...) PhidgetLog_loge(NULL, 0, NULL, "phidget22usb", PHIDGET_LOG_INFO, __VA_ARGS__)
#define usblogwarn(...) PhidgetLog_loge(NULL, 0, NULL, "phidget22usb", PHIDGET_LOG_WARNING, __VA_ARGS__)
#define usblogdebug(...) PhidgetLog_loge(NULL, 0, NULL, "phidget22usb", PHIDGET_LOG_DEBUG, __VA_ARGS__)
#define usblogverbose(...) PhidgetLog_loge(NULL, 0, NULL, "phidget22usb", PHIDGET_LOG_VERBOSE, __VA_ARGS__)

static void
logBuffer(unsigned char *data, int dataLen, const char *message) {
	Phidget_LogLevel ll;
	char str[2000];
	int i, j;

	PhidgetLog_getSourceLevel("phidget22usb", &ll);
	if (ll != PHIDGET_LOG_VERBOSE)
		return;

	str[0]='\0';
	if (dataLen > 0) {
		for (i = 0, j = 0; i < dataLen; i++, j += 6) {
			if (!(i % 8)) {
				str[j] = '\n';
				str[j + 1] = '\t';
				j += 2;
			}
			mos_snprintf(str + j, sizeof (str) - j, "0x%02x, ", data[i]);
		}
		str[j - 2] = '\0'; //delete last ','
	}

	usblogverbose("%s%s", message, str);
}

static HANDLE
openHandle(WCHAR *path) {
	HANDLE handle;
	int res;

	handle = CreateFileW(path, 0, FILE_SHARE_READ | FILE_SHARE_WRITE, NULL, OPEN_EXISTING, 0, NULL);
	if (handle == INVALID_HANDLE_VALUE) {
		res = GetLastError();
		usblogverbose("Failed to open handle: 0x%08x", res);
		return (INVALID_HANDLE_VALUE);
	}
	return (handle);
}

/*
 * Enable read sharing so that the manager in other apps can read the device info
 * but do not share write, as the device may then be opened twice.
 */
static PhidgetReturnCode
openHandle2(WCHAR *path, HANDLE *handle) {
	int res;

	*handle = CreateFileW(path, GENERIC_READ | GENERIC_WRITE, FILE_SHARE_READ, NULL, OPEN_EXISTING,
	  FILE_FLAG_OVERLAPPED, NULL);
	if (*handle == INVALID_HANDLE_VALUE) {
		res = GetLastError();
		usblogerr("Failed to open2 handle: 0x%08x", res);
		switch (res) {
		case ERROR_ACCESS_DENIED:
			usblogwarn("CreateFileW(%ls) failed with ERROR_ACCESS_DENIED", path);
			return (EPHIDGET_ACCESS);

		case ERROR_SHARING_VIOLATION:
			usblogdebug("CreateFileW(%ls) failed with ERROR_SHARING_VIOLATION: "
			  "the Phidget is most likely opened by another process", path);
			return (EPHIDGET_BUSY);

		default:
			usblogwarn("CreateFileW(%ls) failed with error code: 0x%08x", path, res);
			return (EPHIDGET_NOENT);
		}
	}

	return (EPHIDGET_OK);
}

static PhidgetReturnCode
closeHandle(HANDLE handle) {
	int res;

	if (handle == INVALID_HANDLE_VALUE)
		return (EPHIDGET_UNEXPECTED);

	res = CloseHandle(handle);
	if (res == 0) {
		res = GetLastError();
		usblogerr("Failed to close handle: 0x%08x", res);
		return (EPHIDGET_UNEXPECTED);
	}

	return (EPHIDGET_OK);
}

PhidgetReturnCode
PhidgetUSBCloseHandle(PhidgetUSBConnectionHandle conn) {
	PhidgetReturnCode res;

	assert(conn);

	if (conn->deviceHandle == INVALID_HANDLE_VALUE) {
		usblogerr("device handle is not valid");
		return (EPHIDGET_NOTATTACHED);
	}

	//This stops any pending async reads
	SetEvent((void *)conn->closeReadEvent);

	stopUSBReadThread(conn);

	//Lock so we don't kill the handle while read is using it
	PhidgetRunLock(conn);

	usbloginfo("Closing USB Device: 0x%" PRIXPTR, (uintptr_t)conn->deviceHandle);
	res = closeHandle(conn->deviceHandle);

	if (res != EPHIDGET_OK) {
		usblogerr("Failed to close USB Connection handle");
		PhidgetRunUnlock(conn);
		return (EPHIDGET_UNEXPECTED);
	}
	conn->deviceHandle = INVALID_HANDLE_VALUE;
	PhidgetRunUnlock(conn);

	return (EPHIDGET_OK);
}

PhidgetReturnCode
PhidgetUSBSetLabel(PhidgetDeviceHandle device, char *buffer) {
	assert(device);

	//Give is a shot..
	return GPP_setLabel(device, buffer);
}

PhidgetReturnCode
PhidgetUSBGetString(PhidgetUSBConnectionHandle conn, int index, char *str) {
	wchar_t labelData[127] = { 0 }; //Max size is 126 wide chars, plus null termination
	HANDLE DeviceHandle;
	size_t length;
	int ret;

	DeviceHandle = openHandle(conn->DevicePath);
	if (DeviceHandle == INVALID_HANDLE_VALUE)
		return (EPHIDGET_UNEXPECTED);

	str[0] = '\0';

	ret = HidD_GetIndexedString(DeviceHandle, index, labelData, sizeof(labelData));
	closeHandle(DeviceHandle);
	if (ret == 0)
		return (EPHIDGET_NOENT); //String doesn't exist

	length = wcslen(labelData);
	if (length <= 0) {
		usblogdebug("HidD_GetIndexedString returned 0 length string in PhidgetUSBGetString");
		return (EPHIDGET_OK);
	}

	if (UTF16toUTF8((char *)labelData, (int)(length * 2), str) != EPHIDGET_OK) {
		usblogerr("UTF16toUTF8 failed in PhidgetUSBGetString");
		return (EPHIDGET_UNEXPECTED);
	}

	return (EPHIDGET_OK);
}

static PhidgetReturnCode
getLabel(HANDLE DeviceHandle, char *label, int serialNumber) {
	char labelData[24];
	size_t length;

	memset(labelData, 0, sizeof(labelData));

	if (HidD_GetIndexedString(DeviceHandle, 4, &labelData[2], 22) == 0) {
		label[0] = '\0';
		return (EPHIDGET_OK);
	}

	length = wcslen((wchar_t *)&labelData[2]);
	if (length <= 0) {
		label[0] = '\0';
		return (EPHIDGET_OK);
	}

	labelData[0] = (char)(length * 2 + 2);
	labelData[1] = 0x03;
	if (decodeLabelString(labelData, label, serialNumber) != EPHIDGET_OK)
		usblogerr("Problem decoding label string.");

	return (EPHIDGET_OK);
}

PhidgetReturnCode
PhidgetUSBRefreshLabelString(PhidgetDeviceHandle device) {
	PhidgetUSBConnectionHandle conn;
	PhidgetReturnCode ret;
	HANDLE DeviceHandle;

	TESTPTR(device);
	conn = PhidgetUSBConnectionCast(device->conn);
	TESTPTR(conn);

	DeviceHandle = openHandle(conn->DevicePath);
	if (DeviceHandle == INVALID_HANDLE_VALUE) {
		usblogerr("Failed to open device handle");
		return (EPHIDGET_UNEXPECTED);
	}

	ret = getLabel(DeviceHandle, device->deviceInfo.label, device->deviceInfo.serialNumber);
	closeHandle(DeviceHandle);

	return (ret);
}

static PhidgetReturnCode
getUSBDeviceInfo(GUID HidGuid, HANDLE hDevInfo, DWORD MemberIndex, SP_DEVICE_INTERFACE_DATA *devInfoData,
  PSP_DEVICE_INTERFACE_DETAIL_DATA_W *detailData) {
	DWORD errorcode;
	DWORD Length;

	devInfoData->cbSize = sizeof(*devInfoData);
	Length = 0;

	if (!SetupDiEnumDeviceInterfaces(hDevInfo, 0, &HidGuid, MemberIndex, devInfoData)) {
		errorcode = GetLastError();
		switch (errorcode) {
		case ERROR_NO_MORE_ITEMS:
			break;
		default:
			usblogerr("SetupDiEnumDeviceInterfaces failed with error code: 0x%08x", errorcode);
			return (EPHIDGET_UNEXPECTED);
		}
		return (EPHIDGET_NOENT);
	}

	// This gets the required length for detailData
	if (!SetupDiGetDeviceInterfaceDetailW(hDevInfo, devInfoData, NULL, 0, &Length, NULL)) {
		errorcode = GetLastError();
		switch (errorcode) {
		//this is expected
		case ERROR_INSUFFICIENT_BUFFER:
			break;
		default:
			usblogerr("SetupDiGetDeviceInterfaceDetailW failed with error code: 0x%08x",
			  errorcode);
			return (EPHIDGET_UNEXPECTED);
		}
	}

	// This actually fills in detailData using the length
	*detailData = (PSP_DEVICE_INTERFACE_DETAIL_DATA_W)malloc(Length);
	if (*detailData == NULL)
		return (EPHIDGET_NOMEMORY);

	(*detailData)->cbSize = sizeof(SP_DEVICE_INTERFACE_DETAIL_DATA);
	if (!SetupDiGetDeviceInterfaceDetailW(hDevInfo, devInfoData, *detailData, Length, NULL, NULL)) {
		errorcode = GetLastError();
		usblogerr("SetupDiGetDeviceInterfaceDetailW failed with error code: 0x%08x",
		  errorcode);
		free(*detailData);
		*detailData = NULL;
		return (EPHIDGET_UNEXPECTED);
	}

	return (EPHIDGET_OK);
}

static PhidgetReturnCode
AttachUSBDevice(PSP_DEVICE_INTERFACE_DETAIL_DATA_W detailData) {
	char label[MAX_LABEL_STORAGE];
	PhidgetDeviceHandle device;
	HIDD_ATTRIBUTES Attributes;
	wchar_t productStringW[64];
	wchar_t SerialNumber[64];
	char productString[64];
	PhidgetReturnCode res;
	WCHAR *interface_spot;
	HANDLE DeviceHandle;
	int interface_num;
	int serialNumber;
	DWORD errorcode;
	int version;
	int devOff;

	//usbloginfo("Device: %S", detailData->DevicePath);

	interface_spot = wcsstr(detailData->DevicePath, L"mi_");
	if (interface_spot != 0)
		interface_num = (interface_spot)[4] - 48;
	else
		interface_num = 0;

	// Don't request any read/write permissions, but make sure open is not blocked (share read/write)
	DeviceHandle = openHandle(detailData->DevicePath);
	if (DeviceHandle == INVALID_HANDLE_VALUE) {
		usblogverbose("Failed to open USB device handle");
		return (EPHIDGET_UNEXPECTED);
	}

	Attributes.Size = sizeof(Attributes);
	if (!HidD_GetAttributes(DeviceHandle, &Attributes)) {
		errorcode = GetLastError();
		usblogerr("HidD_GetAttributes failed with error code: 0x%08x", errorcode);
		closeHandle(DeviceHandle);
		return (EPHIDGET_UNEXPECTED);
	}

	if (Attributes.VersionNumber < 0x100)
		version = Attributes.VersionNumber * 100;
	else
		version = ((Attributes.VersionNumber >> 8) * 100) + ((Attributes.VersionNumber & 0xff));

	res = matchUniqueDevice(PHIDTYPE_USB, Attributes.VendorID, Attributes.ProductID, interface_num, version,
	  &devOff);
	if (res != EPHIDGET_OK) {
	/*
	 * Might be a Phidget, but not one known by this version of the library: log if that is the case.
	 */
		if (res == EPHIDGET_NOENT)
			usblogwarn("A USB Phidget (PID: 0x%04x Version: %d) was found which is not "
			  "supported by the library. A library upgrade is probably required to work with this Phidget",
			  Attributes.ProductID, version);
		closeHandle(DeviceHandle);
		return (EPHIDGET_UNSUPPORTED);
	}

	usbloginfo("Attaching Phidget: %S", detailData->DevicePath);

	if (!HidD_GetSerialNumberString(DeviceHandle, SerialNumber, 64 * sizeof(wchar_t))) {
		errorcode = GetLastError();
		usblogerr("HidD_GetSerialNumberString failed: 0x%08x", errorcode);
		closeHandle(DeviceHandle);
		return (EPHIDGET_UNEXPECTED);
	}
	serialNumber = wcstol(SerialNumber, NULL, 10);

	if (!HidD_GetProductString(DeviceHandle, productStringW, 64 * sizeof(wchar_t))) {
		errorcode = GetLastError();
		usblogerr("HidD_GetProductString failed: 0x%08x", errorcode);
		closeHandle(DeviceHandle);
		return (EPHIDGET_UNEXPECTED);
	}

	getLabel(DeviceHandle, label, serialNumber);
	UTF16toUTF8((char *)productStringW, (int)(wcslen(productStringW) * 2), productString);
	closeHandle(DeviceHandle);

	res = createPhidgetUSBDevice(&Phidget_Unique_Device_Def[devOff], version, label, serialNumber,
	  detailData->DevicePath, productString, &device);
	if (res != EPHIDGET_OK) {
		usblogerr("createPhidgetUSBDevice() failed.");
		return (res);
	}

	PhidgetSetFlags(device, PHIDGET_SCANNED_FLAG);
	deviceAttach(device, 1);

	chlog("%"PRIphid"", device);
	PhidgetRelease(&device);	/* release our reference */

	return (res);
}

PhidgetReturnCode
PhidgetUSBScanDevices() {
	DWORD MemberIndex;
	DWORD errorcode;

	PhidgetUSBConnectionHandle conn;
	PhidgetDeviceHandle device;
	PhidgetReturnCode res;

	PSP_DEVICE_INTERFACE_DETAIL_DATA_W detailData;
	SP_DEVICE_INTERFACE_DATA devInfoData;
	HANDLE hDevInfo;
	GUID HidGuid;

	HidD_GetHidGuid(&HidGuid);
	hDevInfo = SetupDiGetClassDevs(&HidGuid, NULL, NULL, DIGCF_PRESENT | DIGCF_DEVICEINTERFACE);
	if (hDevInfo == INVALID_HANDLE_VALUE) {
		errorcode = GetLastError();
		usblogerr("SetupDiGetClassDevs failed with error code: 0x%08x", errorcode);
		return (EPHIDGET_UNEXPECTED);
	}

	for (MemberIndex = 0;; MemberIndex++) {
		res = getUSBDeviceInfo(HidGuid, hDevInfo, MemberIndex, &devInfoData, &detailData);
		switch (res) {
		case EPHIDGET_OK:
			break;
		case EPHIDGET_NOENT:	/* out of devices */
			res = EPHIDGET_OK;
			goto done;
		default:
			continue;
		}

		/*
		 * If the device is already in the device list, flag it and move onto the next.
		 */
		PhidgetReadLockDevices();
		FOREACH_DEVICE(device) {
			if (device->connType != PHIDCONN_USB)
				continue;

			conn = PhidgetUSBConnectionCast(device->conn);
			assert(conn);

			if (!wcsncmp(conn->DevicePath, detailData->DevicePath, 128)) {
				PhidgetSetFlags(device, PHIDGET_SCANNED_FLAG);
				PhidgetUnlockDevices();
				goto next;
			}
		}
		PhidgetUnlockDevices();

		/*
		 * The device wasn't found in the device list.  Check if it is a known phidget and attach it if so.
		 */
		res = AttachUSBDevice(detailData);

	next:
		if (detailData) {
			free(detailData);
			detailData = NULL;
		}
	}

done:
	if (!SetupDiDestroyDeviceInfoList(hDevInfo)) {
		errorcode = GetLastError();
		usblogerr("SetupDiDestroyDeviceInfoList failed with error code: 0x%08x", errorcode);
	}

	return (res);
}

/*
 * PhidgetUSBOpenHandle takes a PhidgetInfo structure, with
 * device file path filled in.
 *
 * PhidgetUSBOpenHandle should reserve the file handles - in other words,
 * a future call to CHIDOpenHandle should fail.
 */
PhidgetReturnCode
PhidgetUSBOpenHandle(PhidgetDeviceHandle device) {
	PHIDP_PREPARSED_DATA PreparsedData;
	PhidgetUSBConnectionHandle conn;
	HIDP_CAPS Capabilities;
	PhidgetReturnCode res;
	HANDLE deviceHandle;
	NTSTATUS result;

	TESTPTR(device);
	conn = PhidgetUSBConnectionCast(device->conn);
	TESTPTR(conn);

	ResetEvent((void *)conn->closeReadEvent);

	res = openHandle2(conn->DevicePath, &deviceHandle);
	if (res != EPHIDGET_OK)
		return (res);


	if (!HidD_GetPreparsedData(deviceHandle, &PreparsedData)) {
		usblogerr("HidD_GetPreparsedData() failed");
		closeHandle(deviceHandle);
		return (EPHIDGET_UNEXPECTED);
	}

	result = HidP_GetCaps(PreparsedData, &Capabilities);
	switch (result) {
	case HIDP_STATUS_SUCCESS:
		conn->inputReportByteLength = Capabilities.InputReportByteLength - 1;
		if (Capabilities.InputReportByteLength == 0)
			conn->inputReportByteLength = 0;
		conn->outputReportByteLength = Capabilities.OutputReportByteLength - 1;
		if (Capabilities.OutputReportByteLength == 0)
			conn->outputReportByteLength = 0;
		HidD_FreePreparsedData(PreparsedData);

		//Increase HID driver buffer size so we're less likely to miss packets
		HidD_SetNumInputBuffers(deviceHandle, 200);

		conn->deviceHandle = deviceHandle;

		assert(conn->outputReportByteLength <= MAX_USB_OUT_PACKET_SIZE);
		assert(conn->inputReportByteLength <= MAX_USB_IN_PACKET_SIZE);

		usbloginfo("Opened USB Device: 0x%" PRIXPTR, (uintptr_t)deviceHandle);

		return (EPHIDGET_OK);
	case HIDP_STATUS_INVALID_PREPARSED_DATA:
		usblogerr("HidP_GetCaps() failed with HIDP_STATUS_INVALID_PREPARSED_DATA");
		HidD_FreePreparsedData(PreparsedData);
		break;
	default:
		usblogerr("HidP_GetCaps() failed with 0x%08x", result);
		HidD_FreePreparsedData(PreparsedData);
		break;
	}

	closeHandle(deviceHandle);
	return (EPHIDGET_UNEXPECTED);
}

/* CHIDSendPacket compensates for an oddity in the Windows HID driver.
Windows HID expects the first byte of the packet to be a zero byte,
thus offsetting all the bytes by 1.  None of the other operating systems
have this behaviour, as far as I know.  Rather than offsetting all bytes
in the functions that format the packets, I deal with it here, for maximum
code reuse on Linux/MAC. */

//#define SIMULATE_USB_WRITE_ERROR
PhidgetReturnCode
PhidgetUSBSendPacket(PhidgetUSBConnectionHandle conn, const unsigned char *buffer, size_t bufferLen) {
	unsigned char outbuffer[100] = { 0 };
	unsigned long BytesWritten;
	PhidgetReturnCode ret;
	DWORD wait_return;
	DWORD errorcode;

#ifdef SIMULATE_USB_WRITE_ERROR
	static int cnt = 0;
#endif

	TESTPTR(conn);
	TESTPTR(buffer);
	assert(bufferLen <= conn->outputReportByteLength);

	mos_mutex_lock(&conn->usbwritelock);

	if (conn->deviceHandle == INVALID_HANDLE_VALUE) {
		mos_mutex_unlock(&conn->usbwritelock);
		return (EPHIDGET_UNEXPECTED);
	}

	memcpy(outbuffer + 1, buffer, bufferLen);

	conn->asyncWrite.Offset = 0;
	conn->asyncWrite.OffsetHigh = 0;

	logBuffer(outbuffer + 1, conn->outputReportByteLength, "Sending USB Packet: ");

	if (WriteFile((void *)conn->deviceHandle, outbuffer, conn->outputReportByteLength + 1, NULL, &conn->asyncWrite))
		goto write_completed;

	errorcode = GetLastError();
	switch (errorcode) {
	// have to wait for the async return from write
	case ERROR_IO_PENDING:
		goto wait_for_write_complete;
	// can get this is the device is unplugged
	case ERROR_DEVICE_NOT_CONNECTED:
		usblogerr("WriteFile() failed with error: ERROR_DEVICE_NOT_CONNECTED");
		mos_mutex_unlock(&conn->usbwritelock);
		return (EPHIDGET_NOTATTACHED);
	case ERROR_IO_INCOMPLETE:
		usblogerr("WriteFile() failed with error: ERROR_IO_INCOMPLETE");
		mos_mutex_unlock(&conn->usbwritelock);
		return (EPHIDGET_UNEXPECTED);
	case ERROR_INVALID_USER_BUFFER:
	case ERROR_WORKING_SET_QUOTA:
	case ERROR_NOT_ENOUGH_QUOTA:
	case ERROR_NOT_ENOUGH_MEMORY:
		usblogerr("WriteFile() failed with error: 0x%08x - "
		  "Probably too many outstanding asynchronous I/O requests.", errorcode);
		mos_mutex_unlock(&conn->usbwritelock);
		return (EPHIDGET_UNEXPECTED);
	default:
		usblogerr("WriteFile() failed with error code: 0x%08x", errorcode);
		mos_mutex_unlock(&conn->usbwritelock);
		return (EPHIDGET_UNEXPECTED);
	}

	// have to wait for WriteFile() async return
wait_for_write_complete:

	wait_return = WaitForSingleObject((HANDLE)conn->asyncWrite.hEvent, 1000);
	switch (wait_return) {
	case WAIT_OBJECT_0: //async write returned
		goto write_completed;
	case WAIT_TIMEOUT: //we don't recover from this like in read
		usblogerr("Wait on asyncWrite in PhidgetUSBSendPacket timed out on 0x%" PRIXPTR, (uintptr_t)conn->deviceHandle);
		ret = EPHIDGET_TIMEOUT;
		goto cancel_write;
	case WAIT_FAILED:
	case WAIT_ABANDONED:
	default:
		errorcode = GetLastError();
		usblogerr("Wait on asyncWrite in PhidgetUSBSendPacket failed with error code: 0x%08x", errorcode);
		ret = EPHIDGET_UNEXPECTED;
		goto cancel_write;
	}

cancel_write:

	//cancel the IO
	if (!CancelIo(conn->deviceHandle)) {
		errorcode = GetLastError();
		usblogerr("CancelIo in PhidgetUSBSendPacket failed with error code: 0x%08x", errorcode);
	} else {
	//make sure it got cancelled
		if (!(GetOverlappedResult((void *)conn->deviceHandle, &conn->asyncWrite, &BytesWritten, TRUE))) {
			ResetEvent((void *)conn->asyncWrite.hEvent);
			errorcode = GetLastError();
			switch (errorcode) {
			case ERROR_IO_INCOMPLETE:
			case ERROR_OPERATION_ABORTED:
				usblogdebug("write successfully cancelled");
				break;
			default:
				usblogerr("GetOverlappedResult unexpectedly returned: 0x%08x, while trying to cancel write", errorcode);
				break;
			}
		}
	}

	mos_mutex_unlock(&conn->usbwritelock);
	return (ret);

write_completed:

	// we want this to return non-zero
	if (!GetOverlappedResult((void *)conn->deviceHandle, &conn->asyncWrite, &BytesWritten, TRUE)) {
		errorcode = GetLastError();
		switch (errorcode) {
		case ERROR_IO_INCOMPLETE:
			usblogerr("GetOverlappedResult() failed with error: ERROR_IO_INCOMPLETE");
			mos_mutex_unlock(&conn->usbwritelock);
			return (EPHIDGET_UNEXPECTED);
		case ERROR_DEVICE_NOT_CONNECTED:
			usblogwarn("GetOverlappedResult() failed with error: ERROR_DEVICE_NOT_CONNECTED");
			mos_mutex_unlock(&conn->usbwritelock);
			return (EPHIDGET_NOTATTACHED);
		case ERROR_GEN_FAILURE:
			//ESD event, could also be a detach during write
			usblogerr("GetOverlappedResult() failed with error: ERROR_GEN_FAILURE "
			  "(A device attached to the system is not functioning)");
			mos_mutex_unlock(&conn->usbwritelock);
			return (EPHIDGET_UNEXPECTED);
		default:
			usblogerr("GetOverlappedResult() failed with error code: 0x%08x", errorcode);
			mos_mutex_unlock(&conn->usbwritelock);
			return (EPHIDGET_UNEXPECTED);
		}
	}
	ResetEvent((void *)conn->asyncWrite.hEvent);

	if (BytesWritten != (unsigned)conn->outputReportByteLength + 1) {
		usblogerr("Failure in CHIDSendPacket - Report Length %d, Bytes Written: %d",
			(int)conn->outputReportByteLength, (int)BytesWritten - 1);
		mos_mutex_unlock(&conn->usbwritelock);
		return (EPHIDGET_UNEXPECTED);
	}

	mos_mutex_unlock(&conn->usbwritelock);

#ifdef SIMULATE_USB_WRITE_ERROR
	if (++cnt % 100 == 0) {
		usblogdebug("Simulating USB Send Timeout");
		return (EPHIDGET_TIMEOUT);
	}
#endif

	return (EPHIDGET_OK);
}


/*
 * Compensates for an oddity in the Windows HID driver.
 * Windows HID returns an extra byte on the beginning of the packet, always
 * set to zero.  None of the other operating systems have this behaviour,
 * as far as I know.  Rather than offsetting all bytes in the functions that
 * parse the packets, I deal with it here, for maximum code reuse on Linux/MAC.
 */

/* Buffer should be at least 8 bytes long */
//#define SIMULATE_USB_READ_ERROR
PhidgetReturnCode
PhidgetUSBReadPacket(PhidgetUSBConnectionHandle conn, unsigned char *buffer) {
	DWORD wait_return = 0, errorcode=0;
	HANDLE waitEvents[2];

	unsigned long BytesRead = 0;

#ifdef SIMULATE_USB_READ_ERROR
	static int cnt = 0;
#endif

	TESTPTR(conn);
	TESTPTR(buffer);

	PhidgetRunLock(conn);
	if (conn->deviceHandle == INVALID_HANDLE_VALUE) {
		PhidgetRunUnlock(conn);
		return (EPHIDGET_UNEXPECTED);
	}

	if (conn->readPending)
		goto wait_for_read_completed;

	conn->asyncRead.Offset = 0;
	conn->asyncRead.OffsetHigh = 0;

	//read a packet from the device
	if (ReadFile((void *)conn->deviceHandle, conn->inbuf, conn->inputReportByteLength + 1, NULL, &conn->asyncRead))
		goto read_completed;

	errorcode = GetLastError();
	switch (errorcode) {
	//have to wait for the async return from read
	case ERROR_IO_PENDING:
		conn->readPending = TRUE;
		goto wait_for_read_completed;
	//can get this is teh device is unplugged
	case ERROR_DEVICE_NOT_CONNECTED:
		usblogerr("ReadFile failed with error: ERROR_DEVICE_NOT_CONNECTED");
		PhidgetRunUnlock(conn);
		return (EPHIDGET_NOTATTACHED);
	case ERROR_IO_INCOMPLETE:
		usblogerr("ReadFile failed with error: ERROR_IO_INCOMPLETE");
		PhidgetRunUnlock(conn);
		return (EPHIDGET_UNEXPECTED);
	case ERROR_INVALID_USER_BUFFER: //this happens when we try to read a device with size 0 read buffer
		usblogerr("ReadFile failed with error: ERROR_INVALID_USER_BUFFER");
		PhidgetRunUnlock(conn);
		return (EPHIDGET_UNEXPECTED);
	case ERROR_WORKING_SET_QUOTA:
	case ERROR_NOT_ENOUGH_QUOTA:
	case ERROR_NOT_ENOUGH_MEMORY:
		usblogerr("ReadFile failed with error: 0x%08x - Probably too many outstanding asynchronous I/O requests.", errorcode);
		PhidgetRunUnlock(conn);
		return (EPHIDGET_UNEXPECTED);
	default:
		usblogerr("ReadFile failed with error code: 0x%08x", errorcode);
		PhidgetRunUnlock(conn);
		return (EPHIDGET_UNEXPECTED);
	}

wait_for_read_completed:
	waitEvents[0] = (HANDLE)conn->asyncRead.hEvent;
	waitEvents[1] = (HANDLE)conn->closeReadEvent;
	wait_return = WaitForMultipleObjects(2, waitEvents, FALSE, 500);
	switch (wait_return) {
	case WAIT_OBJECT_0: //async read returned
		goto read_completed;
	case WAIT_OBJECT_0 + 1: //PhidgetUSBCloseHandle signalled closeReadEvent
		usblogdebug("closeReadEvent signalled - cancelling outstanding read...");
		ResetEvent((void *)conn->closeReadEvent);
		goto cancel_read;
	case WAIT_TIMEOUT:
		//verbose because it could happen a LOT
		usblogdebug("Wait on asyncRead/closeReadEvent timed out.");
		PhidgetRunUnlock(conn);
		return (EPHIDGET_TIMEOUT); //this might be ok
	case WAIT_FAILED:
	default:
		errorcode = GetLastError();
		usblogerr("Wait on asyncRead/closeReadEvent failed with error code: 0x%08x", errorcode);
		goto cancel_read;
	}

cancel_read:
	//cancel the IO
	if (!CancelIo(conn->deviceHandle)) {
		errorcode = GetLastError();
		usblogerr("CancelIo in PhidgetUSBReadPacket failed with error code: 0x%08x", errorcode);
		PhidgetRunUnlock(conn);
		return (EPHIDGET_UNEXPECTED);
	}
	//make sure it got cancelled
	if (!(GetOverlappedResult((void *)conn->deviceHandle, &conn->asyncRead, &BytesRead, TRUE))) {
		errorcode = GetLastError();
		switch (errorcode) {
		case ERROR_IO_INCOMPLETE:
		case ERROR_OPERATION_ABORTED:
			usblogdebug("read successfully cancelled");
			break;
		default:
			usblogerr("GetOverlappedResult unexpectedly returned: 0x%08x, while trying to cancel read",
			  errorcode);
			PhidgetRunUnlock(conn);
			return (EPHIDGET_UNEXPECTED);
		}
	}
	PhidgetRunUnlock(conn);
	return (EPHIDGET_INTERRUPTED); //don't try to act on any returned data

read_completed:
	conn->readPending = FALSE;
	//we want this to return non-zero
	if (!GetOverlappedResult((void *)conn->deviceHandle, &conn->asyncRead, &BytesRead, TRUE)) {
		errorcode = GetLastError();
		switch (errorcode) {
		case ERROR_IO_INCOMPLETE:
			usblogerr("GetOverlappedResult() failed with error: ERROR_IO_INCOMPLETE");
			PhidgetRunUnlock(conn);
			return (EPHIDGET_UNEXPECTED);
		case ERROR_DEVICE_NOT_CONNECTED:
			usblogwarn("GetOverlappedResult() failed with error: ERROR_DEVICE_NOT_CONNECTED");
			PhidgetRunUnlock(conn);
			return (EPHIDGET_NOTATTACHED);
		case ERROR_GEN_FAILURE:
			usblogerr("GetOverlappedResult() failed with error: "
				"ERROR_GEN_FAILURE (A device attached to the system is not functioning)");
			PhidgetRunUnlock(conn);
			return (EPHIDGET_UNEXPECTED);
		default:
			usblogerr("GetOverlappedResult() in PhidgetUSBReadPacket failed with error code: %d", errorcode);
			PhidgetRunUnlock(conn);
			return (EPHIDGET_UNEXPECTED);
		}
	}
	ResetEvent((void *)conn->asyncRead.hEvent);

	if ((unsigned)conn->inputReportByteLength + 1 != BytesRead) {
		usblogerr("Report Length: %d, Bytes Read: %d", (int)conn->inputReportByteLength, (int)BytesRead - 1);
		PhidgetRunUnlock(conn);
#if 0
		// XXX - testing - see what happens if we just try to read again..
		if (BytesRead == 0) {
			usblogdebug("0 bytes read, try again.");
			logBuffer(buffer, conn->inputReportByteLength, "Received USB Packet (0): ");
			return (EPHIDGET_AGAIN);
		}
#endif
		return (EPHIDGET_UNEXPECTED);
	}

	memcpy(buffer, conn->inbuf + 1, conn->inputReportByteLength);

	logBuffer(buffer, conn->inputReportByteLength, "Received USB Packet: ");

	PhidgetRunUnlock(conn);

#ifdef SIMULATE_USB_READ_ERROR
	if (++cnt % 100 == 0) {
		usblogdebug("Simulating USB Send Timeout");
		return (EPHIDGET_TIMEOUT);
	}
#endif

	return (EPHIDGET_OK);
}
