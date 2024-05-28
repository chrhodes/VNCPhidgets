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


#ifndef _Included_com_phidget22_usb_Phidget
#define _Included_com_phidget22_usb_Phidget

extern jclass usb_phidget_class;

extern jmethodID com_phidget22_usb_Phidget_getSerialNumber_mid;
extern jmethodID com_phidget22_usb_Phidget_getUniqueName_mid;
extern jmethodID com_phidget22_usb_Phidget_getvID_mid;
extern jmethodID com_phidget22_usb_Phidget_getpID_mid;
extern jmethodID com_phidget22_usb_Phidget_getVersion_mid;
extern jmethodID com_phidget22_usb_Phidget_getInterfaceNum_mid;
extern jmethodID com_phidget22_usb_Phidget_getInputReportSize_mid;
extern jmethodID com_phidget22_usb_Phidget_getOutputReportSize_mid;
extern jmethodID com_phidget22_usb_Phidget_getLabel_mid;

PhidgetReturnCode PhidgetAndroid_cancelRead(void *deviceHandle);
PhidgetReturnCode PhidgetAndroid_closePhidget(void *deviceHandle);
PhidgetReturnCode PhidgetAndroid_write(void *deviceHandle, const unsigned char *buffer, size_t bufferLen);
PhidgetReturnCode PhidgetAndroid_setLabel(void *deviceHandle, const char *buffer);
PhidgetReturnCode PhidgetAndroid_read(void *deviceHandle, unsigned char *buffer);
PhidgetReturnCode PhidgetAndroid_refreshLabel(void *deviceHandle, char *label, int serialNumber);
PhidgetReturnCode PhidgetAndroid_openPhidget(char *dev, int interface, void *deviceHandle, uint16_t *inputReportByteLength, uint16_t *outputReportByteLength);
PhidgetReturnCode PhidgetAndroid_getStringDescriptor(char *dev, int interface, void *deviceHandle, int strIdx, char *str, int strLen);

#endif
