/*
 *  no usb support
 *
 *  Created by Patrick McNeil on 06/06/05.
 *  Copyright 2005 Phidgets Inc. All rights reserved.
 *
 */
 
#include "phidgetbase.h"
#include "usb.h"

#ifdef _MACOSX
PhidgetReturnCode PhidgetUSBSetupNotifications(CFRunLoopRef runloop)
{
	return EPHIDGET_UNSUPPORTED;
}

PhidgetReturnCode PhidgetUSBTeardownNotifications()
{
	return EPHIDGET_UNSUPPORTED;
}

PhidgetReturnCode PhidgetUSBResetDevice(PhidgetDeviceHandle phid)
{
	return EPHIDGET_UNSUPPORTED;
}
#endif

PhidgetReturnCode PhidgetUSBScanDevices(void)
{
	return EPHIDGET_UNSUPPORTED;
}
PhidgetReturnCode PhidgetUSBOpenHandle(PhidgetDeviceHandle device)
{
	return EPHIDGET_UNSUPPORTED;
}
PhidgetReturnCode PhidgetUSBCloseHandle(PhidgetUSBConnectionHandle conn)
{
	return EPHIDGET_UNSUPPORTED;
}
PhidgetReturnCode PhidgetUSBSetLabel(PhidgetDeviceHandle device, char *buffer)
{
	return EPHIDGET_UNSUPPORTED;
}
void PhidgetUSBCleanup()
{
	return;
}
PhidgetReturnCode PhidgetUSBRefreshLabelString(PhidgetDeviceHandle device)
{
	return EPHIDGET_UNSUPPORTED;
}
PhidgetReturnCode PhidgetUSBGetString(PhidgetUSBConnectionHandle conn, int index, char *str)
{
	return EPHIDGET_UNSUPPORTED;
}
PhidgetReturnCode PhidgetUSBReadPacket(PhidgetUSBConnectionHandle conn, unsigned char *buffer)
{
	return EPHIDGET_UNSUPPORTED;
}
PhidgetReturnCode PhidgetUSBSendPacket(PhidgetUSBConnectionHandle conn, const unsigned char *buffer, size_t bufferLen)
{
	return EPHIDGET_UNSUPPORTED;
}
