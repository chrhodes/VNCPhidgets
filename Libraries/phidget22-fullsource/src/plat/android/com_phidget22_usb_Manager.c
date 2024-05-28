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
#include "manager.h"
#include "phidget_jni.h"
#include "com_phidget22_usb_Manager.h"
#include "com_phidget22_usb_Phidget.h"

jclass usb_manager_class;

static jmethodID usb_manager_getPhidgetList;
static jmethodID usb_manager_doneWithPhidgetList;
jmethodID usb_manager_openPhidget;
jmethodID usb_manager_getDescriptor;

int
com_phidget22_usb_Manager_OnLoad(JNIEnv *env) {
	//USB Phidget
	if (!(usb_manager_class = (*env)->FindClass(env, "com/phidget22/usb/Manager"))) {
		usbloginfo("Running on Android without USB (Couldn't load com/phidget22/usb/Manager).");
		(*env)->ExceptionClear(env);
		return PFALSE;
	}
	if (!(usb_manager_class = (jclass)(*env)->NewGlobalRef(env, usb_manager_class)))
		JNI_ABORT_STDERR("Couldn't get NewGlobalRef from usb_manager_class");

	//Methods
	if (!(usb_manager_getPhidgetList = (*env)->GetStaticMethodID(env, usb_manager_class, "getPhidgetList", "()[Lcom/phidget22/usb/Phidget;")))
		JNI_ABORT_STDERR("Couldn't get method ID getPhidgetList from usb_manager_class");

	if (!(usb_manager_doneWithPhidgetList = (*env)->GetStaticMethodID(env, usb_manager_class, "doneWithPhidgetList", "()V")))
		JNI_ABORT_STDERR("Couldn't get method ID doneWithPhidgetList from usb_manager_class");

	if (!(usb_manager_openPhidget = (*env)->GetStaticMethodID(env, usb_manager_class, "openPhidget", "(Ljava/lang/String;I)Lcom/phidget22/usb/Phidget;")))
		JNI_ABORT_STDERR("Couldn't get method ID openPhidget from usb_manager_class");

	if (!(usb_manager_getDescriptor = (*env)->GetStaticMethodID(env, usb_manager_class, "getDescriptor", "(Ljava/lang/String;II)Ljava/lang/String;")))
		JNI_ABORT_STDERR("Couldn't get method ID openPhidget from usb_manager_class");

	return PTRUE;
}

JNIEXPORT jboolean JNICALL
Java_com_phidget22_usb_Manager_getInitialized(JNIEnv *env, jclass cls) {
	return ANDROID_USB_GOOD;
}

JNIEXPORT void JNICALL
Java_com_phidget22_usb_Manager_setInitialized(JNIEnv *env, jclass cls, jboolean status) {
	AndroidUsbInitialized = status;
}

PhidgetReturnCode PhidgetAndroidManager_scanDevices() {
	PhidgetReturnCode res = EPHIDGET_OK;
	const PhidgetUniqueDeviceDef *pdd;
	PhidgetUSBConnectionHandle conn;
	char label[MAX_LABEL_STORAGE];
	uint8_t productString[64];
	PhidgetDeviceHandle phid;
	jobjectArray phidgetList;
	jsize phidgetListLength;
	const char *textString;
	char uniqueName[1024];
	jbyteArray labelArray;
	int pID, vID, iID;
	int serialNumber;
	jobject phidget;
	jbyte *datab;
	jstring str;
	JNIEnv *env;
	int version;
	jsize len;
	int i;

	if (!ANDROID_USB_GOOD)
		return EPHIDGET_UNSUPPORTED;

	if ((*ph_vm)->AttachCurrentThreadAsDaemon(ph_vm, (JNIEnvPtr)&env, NULL))
		JNI_ABORT_STDERR("Couldn't AttachCurrentThreadAsDaemon");

	phidgetList = (*env)->CallStaticObjectMethod(env, usb_manager_class, usb_manager_getPhidgetList);
	if (phidgetList == NULL)
		return (EPHIDGET_UNEXPECTED);

	phidgetListLength = (*env)->GetArrayLength(env, phidgetList);

	for (i = 0; i < phidgetListLength; i++) {
		phidget = (*env)->GetObjectArrayElement(env, phidgetList, i);
		if (phidget == NULL) {

			(*env)->DeleteLocalRef(env, phidgetList);			res = EPHIDGET_UNEXPECTED;
			break;
		}

		//Unique name - USBFS path
		str = (*env)->CallObjectMethod(env, phidget, com_phidget22_usb_Phidget_getUniqueName_mid);
		if (str == NULL) {
			(*env)->DeleteLocalRef(env, phidget);
			res = EPHIDGET_UNEXPECTED;
			break;
		}

		textString = (*env)->GetStringUTFChars(env, str, NULL);
		strncpy(uniqueName, textString, 1024);
		(*env)->ReleaseStringUTFChars(env, str, textString);
		(*env)->DeleteLocalRef(env, str);
		/*
		 * If the device is already in the attached list, flag it and move onto the next.
		 */
		PhidgetReadLockDevices();
		FOREACH_DEVICE(phid) {
			if (phid->connType != PHIDCONN_USB)
				continue;

			conn = PhidgetUSBConnectionCast(phid->conn);
			assert(conn);

			if (!strcmp(conn->dev, uniqueName)) {
				PhidgetSetFlags(phid, PHIDGET_SCANNED_FLAG);
				PhidgetUnlockDevices();
				goto next;
			}
		}
		PhidgetUnlockDevices();

		usbloginfo("Found device not in AttachedDevices: %s", uniqueName);

		//New, add it
		pID = (*env)->CallIntMethod(env, phidget, com_phidget22_usb_Phidget_getpID_mid);
		vID = (*env)->CallIntMethod(env, phidget, com_phidget22_usb_Phidget_getvID_mid);
		iID = (*env)->CallIntMethod(env, phidget, com_phidget22_usb_Phidget_getInterfaceNum_mid);
		version = (*env)->CallIntMethod(env, phidget, com_phidget22_usb_Phidget_getVersion_mid);
		serialNumber = (*env)->CallIntMethod(env, phidget, com_phidget22_usb_Phidget_getSerialNumber_mid);
		labelArray = (*env)->CallObjectMethod(env, phidget, com_phidget22_usb_Phidget_getLabel_mid);
		if (labelArray == NULL) {
			(*env)->DeleteLocalRef(env, phidget);
			res = EPHIDGET_UNEXPECTED;
			break;
		}

		len = (*env)->GetArrayLength(env, labelArray);
		datab = (*env)->GetByteArrayElements(env, labelArray, 0);
		if (datab) {
			res = decodeLabelString((char *)datab, label, serialNumber);
			if (res != EPHIDGET_OK)
				label[0]='\0';
			(*env)->ReleaseByteArrayElements(env, labelArray, datab, 0);
		} else {
			label[0]='\0';
		}
		(*env)->DeleteLocalRef(env, labelArray);

		// Find it
		for (pdd = Phidget_Unique_Device_Def; ((int)pdd->type) != END_OF_LIST; pdd++) {
			if (!(pdd->type == PHIDTYPE_USB
				&& vID == pdd->vendorID && pID == pdd->productID
				&& version >= pdd->versionLow && version < pdd->versionHigh))
				continue;

			res = createPhidgetUSBDevice(pdd, version, label, serialNumber, uniqueName, (char *)productString, &phid);
			if (res == EPHIDGET_OK) {
				PhidgetSetFlags(phid, PHIDGET_SCANNED_FLAG);
				res = deviceAttach(phid, 1);
				PhidgetRelease((void **)&phid); /* release our reference */
			}
		}

	next:
		(*env)->DeleteLocalRef(env, phidget);
		continue;
	}


	(*env)->DeleteLocalRef(env, phidgetList);	//Always relese our lock on the phidget list when we're done with it
	(*env)->CallStaticVoidMethod(env, usb_manager_class, usb_manager_doneWithPhidgetList);
	return res;
}