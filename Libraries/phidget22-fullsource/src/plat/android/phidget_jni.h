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

#ifndef PHIDGET_JNI_H
#define PHIDGET_JNI_H

#include <jni.h>

extern const char * CCONV Phidget_strerror(PhidgetReturnCode error);

#ifdef _ANDROID
#define JNIEnvPtr const struct JNINativeInterface ***
#else
#define JNIEnvPtr void **
#endif

extern JavaVM *ph_vm;

jlong updateGlobalRef(JNIEnv *env, jobject obj, jfieldID fid, jboolean b);

#define JNI_ABORT_STDERR(...) \
do { \
	fprintf(stderr, __VA_ARGS__); \
	(*env)->ExceptionDescribe(env); \
	(*env)->ExceptionClear(env); \
	abort(); \
} while(0)

#define PH_THROW(errno) { \
	jobject eobj; \
	jstring edesc; \
 \
	if (!(edesc = (*env)->NewStringUTF(env, Phidget_strerror(errno)))) \
		JNI_ABORT_STDERR("Couldn't get NewStringUTF"); \
	if (!(eobj = (*env)->NewObject(env, ph_exception_class, ph_exception_cons, errno, edesc))) \
		JNI_ABORT_STDERR("Couldn't get NewObject ph_exception_class"); \
	(*env)->DeleteLocalRef (env, edesc); \
	(*env)->Throw(env, (jthrowable)eobj); \
}

#define usblogerr(...) PhidgetLog_loge(NULL, 0, NULL, "phidget22usb", PHIDGET_LOG_ERROR, __VA_ARGS__)
#define usbloginfo(...) PhidgetLog_loge(NULL, 0, NULL, "phidget22usb", PHIDGET_LOG_INFO, __VA_ARGS__)
#define usblogwarn(...) PhidgetLog_loge(NULL, 0, NULL, "phidget22usb", PHIDGET_LOG_WARNING, __VA_ARGS__)
#define usblogdebug(...) PhidgetLog_loge(NULL, 0, NULL, "phidget22usb", PHIDGET_LOG_DEBUG, __VA_ARGS__)
#define usblogverbose(...) PhidgetLog_loge(NULL, 0, NULL, "phidget22usb", PHIDGET_LOG_VERBOSE, __VA_ARGS__)

#endif
