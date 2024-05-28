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

#include <jni.h>

#include "phidgetbase.h"
#include "phidget_jni.h"
#include "com_phidget22_usb_Manager.h"
#include "com_phidget22_usb_Phidget.h"

jclass usb_phidget_class;

jmethodID com_phidget22_usb_Phidget_getSerialNumber_mid;
jmethodID com_phidget22_usb_Phidget_getUniqueName_mid;
jmethodID com_phidget22_usb_Phidget_getvID_mid;
jmethodID com_phidget22_usb_Phidget_getpID_mid;
jmethodID com_phidget22_usb_Phidget_getVersion_mid;
jmethodID com_phidget22_usb_Phidget_getInterfaceNum_mid;
jmethodID com_phidget22_usb_Phidget_getInputReportSize_mid;
jmethodID com_phidget22_usb_Phidget_getOutputReportSize_mid;
jmethodID com_phidget22_usb_Phidget_getLabel_mid;
jmethodID com_phidget22_usb_Phidget_getDescriptor_mid;
jmethodID com_phidget22_usb_Phidget_close_mid;
jmethodID com_phidget22_usb_Phidget_cancelRead_mid;
jmethodID com_phidget22_usb_Phidget_write_mid;
jmethodID com_phidget22_usb_Phidget_setLabel_mid;
jmethodID com_phidget22_usb_Phidget_read_mid;

int
com_phidget22_usb_Phidget_OnLoad(JNIEnv *env) {
	//USB Phidget
	if (!(usb_phidget_class = (*env)->FindClass(env, "com/phidget22/usb/Phidget"))) {
		usbloginfo("Running on Android without USB (Couldn't load com/phidget22/usb/Phidget).");
		(*env)->ExceptionClear(env);
		return PFALSE;
	}
	if (!(usb_phidget_class = (jclass)(*env)->NewGlobalRef(env, usb_phidget_class)))
		JNI_ABORT_STDERR("Couldn't get NewGlobalRef from usb_phidget_class");

	//Methods
	if (!(com_phidget22_usb_Phidget_getSerialNumber_mid = (*env)->GetMethodID(env, usb_phidget_class, "getSerialNumber", "()I")))
		JNI_ABORT_STDERR("Couldn't get method ID getSerialNumber from usb_phidget_class");

	if (!(com_phidget22_usb_Phidget_getUniqueName_mid = (*env)->GetMethodID(env, usb_phidget_class, "getUniqueName", "()Ljava/lang/String;")))
		JNI_ABORT_STDERR("Couldn't get method ID getUniqueName from usb_phidget_class");

	if (!(com_phidget22_usb_Phidget_getvID_mid = (*env)->GetMethodID(env, usb_phidget_class, "getvID", "()I")))
		JNI_ABORT_STDERR("Couldn't get method ID getvID from usb_phidget_class");

	if (!(com_phidget22_usb_Phidget_getpID_mid = (*env)->GetMethodID(env, usb_phidget_class, "getpID", "()I")))
		JNI_ABORT_STDERR("Couldn't get method ID getpID from usb_phidget_class");

	if (!(com_phidget22_usb_Phidget_getVersion_mid = (*env)->GetMethodID(env, usb_phidget_class, "getVersion", "()I")))
		JNI_ABORT_STDERR("Couldn't get method ID getVersion from usb_phidget_class");

	if (!(com_phidget22_usb_Phidget_getInterfaceNum_mid = (*env)->GetMethodID(env, usb_phidget_class, "getInterfaceNum", "()I")))
		JNI_ABORT_STDERR("Couldn't get method ID getInterfaceNum from usb_phidget_class");

	if (!(com_phidget22_usb_Phidget_getInputReportSize_mid = (*env)->GetMethodID(env, usb_phidget_class, "getInputReportSize", "()I")))
		JNI_ABORT_STDERR("Couldn't get method ID getInputReportSize from usb_phidget_class");

	if (!(com_phidget22_usb_Phidget_getOutputReportSize_mid = (*env)->GetMethodID(env, usb_phidget_class, "getOutputReportSize", "()I")))
		JNI_ABORT_STDERR("Couldn't get method ID getOutputReportSize from usb_phidget_class");

	if (!(com_phidget22_usb_Phidget_getLabel_mid = (*env)->GetMethodID(env, usb_phidget_class, "getLabel", "()[B")))
		JNI_ABORT_STDERR("Couldn't get method ID getLabel from usb_phidget_class");

	if (!(com_phidget22_usb_Phidget_getDescriptor_mid = (*env)->GetMethodID(env, usb_phidget_class, "getDescriptor", "(I)Ljava/lang/String;")))
		JNI_ABORT_STDERR("Couldn't get method ID getDescriptor from usb_phidget_class");

	if (!(com_phidget22_usb_Phidget_close_mid = (*env)->GetMethodID(env, usb_phidget_class, "close", "()V")))
		JNI_ABORT_STDERR("Couldn't get method ID close from usb_phidget_class");

	if (!(com_phidget22_usb_Phidget_cancelRead_mid = (*env)->GetMethodID(env, usb_phidget_class, "cancelRead", "()V")))
		JNI_ABORT_STDERR("Couldn't get method ID cancelRead from usb_phidget_class");

	if (!(com_phidget22_usb_Phidget_write_mid = (*env)->GetMethodID(env, usb_phidget_class, "write", "([B)I")))
		JNI_ABORT_STDERR("Couldn't get method ID write from usb_phidget_class");

	if (!(com_phidget22_usb_Phidget_setLabel_mid = (*env)->GetMethodID(env, usb_phidget_class, "setLabel", "([B)I")))
		JNI_ABORT_STDERR("Couldn't get method ID setLabel from usb_phidget_class");

	if (!(com_phidget22_usb_Phidget_read_mid = (*env)->GetMethodID(env, usb_phidget_class, "read", "()[B")))
		JNI_ABORT_STDERR("Couldn't get method ID read from usb_phidget_class");

	return PTRUE;
}

PhidgetReturnCode PhidgetAndroid_cancelRead(void *deviceHandle) {
	JNIEnv *env;

	if (!ANDROID_USB_GOOD)
		return EPHIDGET_UNSUPPORTED;

	if ((*ph_vm)->AttachCurrentThreadAsDaemon(ph_vm, (JNIEnvPtr)&env, NULL))
		JNI_ABORT_STDERR("Couldn't AttachCurrentThreadAsDaemon");

	if (deviceHandle) {
		usbloginfo("PhidgetAndroid_cancelRead -> stopping pending reads");
		(*env)->CallVoidMethod(env, deviceHandle, com_phidget22_usb_Phidget_cancelRead_mid);
	}

	return (EPHIDGET_OK);
}

PhidgetReturnCode PhidgetAndroid_closePhidget(void *deviceHandle) {
	JNIEnv *env;

	if (!ANDROID_USB_GOOD)
		return EPHIDGET_UNSUPPORTED;

	if ((*ph_vm)->AttachCurrentThreadAsDaemon(ph_vm, (JNIEnvPtr)&env, NULL))
		JNI_ABORT_STDERR("Couldn't AttachCurrentThreadAsDaemon");

	if (deviceHandle && *(void **)deviceHandle) {
		usbloginfo("PhidgetAndroid_closePhidget -> closing the Phidget");
		(*env)->CallVoidMethod(env, *(void **)deviceHandle, com_phidget22_usb_Phidget_close_mid);
		(*env)->DeleteGlobalRef(env, (jobject)(uintptr_t)*(void **)deviceHandle);
		*(void **)deviceHandle = NULL;
	}

	usbloginfo("PhidgetAndroid_closePhidget -> done closing.");

	return (EPHIDGET_OK);
}

PhidgetReturnCode PhidgetAndroid_write(void *deviceHandle, const unsigned char *buffer, size_t bufferLen) {
	PhidgetReturnCode res = EPHIDGET_OK;
	jbyteArray jb;
	int bytesSent;
	JNIEnv *env;

	if (!ANDROID_USB_GOOD)
		return EPHIDGET_UNSUPPORTED;

	if ((*ph_vm)->AttachCurrentThreadAsDaemon(ph_vm, (JNIEnvPtr)&env, NULL))
		JNI_ABORT_STDERR("Couldn't AttachCurrentThreadAsDaemon");

	jb = (*env)->NewByteArray(env, (jsize)bufferLen);
	if (jb == NULL)
		return (EPHIDGET_UNEXPECTED);
	(*env)->SetByteArrayRegion(env, jb, 0, bufferLen, (jbyte *)buffer);

	bytesSent = (*env)->CallIntMethod(env, deviceHandle, com_phidget22_usb_Phidget_write_mid, jb);
	if (bytesSent < 0) {
		usblogerr("com_phidget22_usb_Phidget_write_mid returned: %d", bytesSent);
		res = EPHIDGET_UNEXPECTED;
	}

	(*env)->DeleteLocalRef(env, jb);

	return res;
}

PhidgetReturnCode PhidgetAndroid_setLabel(void *deviceHandle, const char *buffer) {
	PhidgetReturnCode res = EPHIDGET_OK;
	jbyteArray jb;
	int bytesSent;
	JNIEnv *env;

	if (!ANDROID_USB_GOOD)
		return EPHIDGET_UNSUPPORTED;

	if ((*ph_vm)->AttachCurrentThreadAsDaemon(ph_vm, (JNIEnvPtr)&env, NULL))
		JNI_ABORT_STDERR("Couldn't AttachCurrentThreadAsDaemon");

	jb = (*env)->NewByteArray(env, buffer[0]);
	if (jb == NULL)
		return (EPHIDGET_UNEXPECTED);
	(*env)->SetByteArrayRegion(env, jb, 0, buffer[0], (jbyte *)buffer);

	bytesSent = (*env)->CallIntMethod(env, deviceHandle, com_phidget22_usb_Phidget_setLabel_mid, jb);
	if (bytesSent < 0)
		res = EPHIDGET_UNEXPECTED;

	(*env)->DeleteLocalRef(env, jb);

	return res;
}

PhidgetReturnCode PhidgetAndroid_read(void *deviceHandle, unsigned char *buffer) {
	PhidgetReturnCode res = EPHIDGET_OK;
	jbyteArray readArray;
	JNIEnv *env;
	jsize len;

	if (!ANDROID_USB_GOOD)
		return EPHIDGET_UNSUPPORTED;

	if ((*ph_vm)->AttachCurrentThreadAsDaemon(ph_vm, (JNIEnvPtr)&env, NULL))
		JNI_ABORT_STDERR("Couldn't AttachCurrentThreadAsDaemon");

	//Read Data - this will probably block?
	readArray = (*env)->CallObjectMethod(env, deviceHandle, com_phidget22_usb_Phidget_read_mid);
	if (readArray == NULL)
		return (EPHIDGET_UNEXPECTED);

	len = (*env)->GetArrayLength(env, readArray);
	if (len == 0)
		res = EPHIDGET_TIMEOUT;
	else
		(*env)->GetByteArrayRegion(env, readArray, 0, len, (jbyte *)buffer);

	(*env)->DeleteLocalRef(env, readArray);
	return res;
}

PhidgetReturnCode PhidgetAndroid_getStringDescriptor(char *dev, int interface, void *deviceHandle, int strIdx, char *str, int strLen) {
	const char *textString;
	jstring devstr;
	jstring jstr;
	JNIEnv *env;

	if (!ANDROID_USB_GOOD)
		return EPHIDGET_UNSUPPORTED;

	if ((*ph_vm)->AttachCurrentThreadAsDaemon(ph_vm, (JNIEnvPtr)&env, NULL))
		JNI_ABORT_STDERR("Couldn't AttachCurrentThreadAsDaemon");

	if (deviceHandle) {
		jstr = (*env)->CallObjectMethod(env, deviceHandle, com_phidget22_usb_Phidget_getDescriptor_mid, strIdx);
	} else {
		devstr = (*env)->NewStringUTF(env, dev);
		if (devstr == NULL) {
			usblogerr("PhidgetAndroid_getStringDescriptor -> NewStringUTF returned null!");
			return (EPHIDGET_UNEXPECTED);
		}
		jstr = (*env)->CallStaticObjectMethod(env, usb_manager_class, usb_manager_getDescriptor, devstr, interface, strIdx);
		(*env)->DeleteLocalRef(env, devstr);
	}

	if (jstr == NULL)
		return (EPHIDGET_UNEXPECTED);

	textString = (*env)->GetStringUTFChars(env, jstr, NULL);
	mos_strncpy(str, textString, strLen);
	(*env)->ReleaseStringUTFChars(env, jstr, textString);
	(*env)->DeleteLocalRef(env, jstr);

	return (EPHIDGET_OK);
}

PhidgetReturnCode PhidgetAndroid_refreshLabel(void *deviceHandle, char *label, int serialNumber) {
	PhidgetReturnCode res = EPHIDGET_OK;
	jbyteArray labelArray;
	jbyte *datab;
	JNIEnv *env;
	jsize len;

	if (!ANDROID_USB_GOOD)
		return EPHIDGET_UNSUPPORTED;

	if ((*ph_vm)->AttachCurrentThreadAsDaemon(ph_vm, (JNIEnvPtr)&env, NULL))
		JNI_ABORT_STDERR("Couldn't AttachCurrentThreadAsDaemon");

	usbloginfo("Refreshing label in CPhidgetAndroid_refreshLabel...");
	labelArray = (*env)->CallObjectMethod(env, deviceHandle, com_phidget22_usb_Phidget_getLabel_mid);
	if (labelArray == NULL)
		return (EPHIDGET_UNEXPECTED);

	len = (*env)->GetArrayLength(env, labelArray);
	datab = (*env)->GetByteArrayElements(env, labelArray, 0);
	if (datab == NULL)
		return (EPHIDGET_UNEXPECTED);
	res = decodeLabelString((char *)datab, label, serialNumber);
	(*env)->ReleaseByteArrayElements(env, labelArray, datab, 0);

	(*env)->DeleteLocalRef(env, labelArray);
	return res;

}

PhidgetReturnCode PhidgetAndroid_openPhidget(char *dev, int interface, void *deviceHandle, uint16_t *inputReportByteLength, uint16_t *outputReportByteLength) {
	jobject jphid;
	JNIEnv *env;
	jstring str;

	if (!ANDROID_USB_GOOD)
		return EPHIDGET_UNSUPPORTED;

	if ((*ph_vm)->AttachCurrentThreadAsDaemon(ph_vm, (JNIEnvPtr)&env, NULL))
		JNI_ABORT_STDERR("Couldn't AttachCurrentThreadAsDaemon");

	//Ready to call, etc.
	str = (*env)->NewStringUTF(env, dev);
	if (str == NULL) {
		usblogerr("PhidgetAndroid_openPhidget -> NewStringUTF returned null!");
		return (EPHIDGET_UNEXPECTED);
	}

	usbloginfo("PhidgetAndroid_openPhidget -> Opening: %s Interface: %d", dev, interface);
	jphid = (*env)->CallStaticObjectMethod(env, usb_manager_class, usb_manager_openPhidget, str, interface);
	(*env)->DeleteLocalRef(env, str);
	if (jphid == NULL) {
		usblogerr("PhidgetAndroid_openPhidget -> usb_manager_openPhidget returned null!");
		return (EPHIDGET_UNEXPECTED);
	}

	jphid = (jobject)(*env)->NewGlobalRef(env, jphid);
	if (jphid == NULL) {
		usblogerr("PhidgetAndroid_openPhidget -> Unable to create global ref of phidget");
		return (EPHIDGET_UNEXPECTED);
	}

	usbloginfo("PhidgetAndroid_openPhidget -> Got a successful opened device!");
	*(void **)deviceHandle = (uintptr_t *)jphid;
	*inputReportByteLength = (*env)->CallIntMethod(env, jphid, com_phidget22_usb_Phidget_getInputReportSize_mid);
	*outputReportByteLength = (*env)->CallIntMethod(env, jphid, com_phidget22_usb_Phidget_getOutputReportSize_mid);

	return (EPHIDGET_OK);
}

