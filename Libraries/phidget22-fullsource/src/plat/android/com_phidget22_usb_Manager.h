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


#ifndef _Included_com_phidget22_usb_Manager
#define _Included_com_phidget22_usb_Manager

extern jclass usb_manager_class;
extern jmethodID usb_manager_openPhidget;
extern jmethodID usb_manager_getDescriptor;

extern int AndroidUsbInitialized;
extern int AndroidUsbJarAvailable;

#define ANDROID_USB_GOOD (AndroidUsbJarAvailable && AndroidUsbInitialized)

PhidgetReturnCode PhidgetAndroidManager_scanDevices();

#endif
