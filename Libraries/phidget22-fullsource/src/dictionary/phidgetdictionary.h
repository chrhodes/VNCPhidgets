#ifndef EXTERNALPROTO
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
#endif

#ifndef _PHIDGET_DICTIONARY_H_
#define _PHIDGET_DICTIONARY_H_

#include "network/network.h"

typedef struct PhidgetDictionary *PhidgetDictionaryHandle;
//
//typedef enum _PhidgetDictionary_Action {
//	PHIDGETDICTIONARY_ADD		= 1,
//	PHIDGETDICTIONARY_UPDATE,
//	PHIDGETDICTIONARY_REMOVE
//} PhidgetDictionary_Action;

API_PRETURN_HDR PhidgetDictionary_create(PhidgetDictionaryHandle *dict);
API_PRETURN_HDR PhidgetDictionary_delete(PhidgetDictionaryHandle *dict);

API_PRETURN_HDR PhidgetDictionary_openRemote(PhidgetDictionaryHandle dict, const char *addr, int port, const char *passwd);
API_PRETURN_HDR PhidgetDictionary_close(PhidgetDictionaryHandle dict);

API_PRETURN_HDR PhidgetDictionary_add(PhidgetDictionaryHandle dict, const char *key, const char *val, int persist);
API_PRETURN_HDR PhidgetDictionary_remove(PhidgetDictionaryHandle dict, const char *key);
API_PRETURN_HDR PhidgetDictionary_update(PhidgetDictionaryHandle dict, const char *key, const char *val, int persist, int add);

API_PRETURN_HDR PhidgetDictionary_get(PhidgetDictionaryHandle dict, const char *key, char *val, size_t valLen);

API_PRETURN_HDR PhidgetDictionary_getLastError(PhidgetDictionaryHandle dict, char *errstr, size_t errstrLen);

API_PRETURN_HDR PhidgetDictionary_getNamespace(PhidgetDictionaryHandle dict, char *nameSpace, size_t nameSpaceLen);
API_PRETURN_HDR PhidgetDictionary_setNamespace(PhidgetDictionaryHandle dict, const char *nameSpace, int create);

typedef void(CCONV *PhidgetDictionary_OnChangeCallback)(PhidgetDictionaryHandle dict, void *ctx, const char *nameSpace, 
  const char *regex, const char *key, const char *val, PhidgetDictionary_Action action);
API_PRETURN_HDR PhidgetDictionary_setOnFilterHandler(PhidgetDictionaryHandle dict,
  PhidgetDictionary_OnChangeCallback fptr, void *ctx, const char *regex);
API_PRETURN_HDR PhidgetDictionary_removeFilterHandler(PhidgetDictionaryHandle dict, const char *regex);

#ifndef EXTERNALPROTO

#ifdef _WINDOWS
#include "ext/regex/posix/regex.h"
#else
#include <regex.h>
#endif

#include "dictionary/dictionaryimpl.h"

#define DICTSRV_NAME		"Phidget Dictionary"
#define DICTSRV_PROTO		"dict22"
#define DICTSRV_PROTO_MAJOR	1
#define DICTSRV_PROTO_MINOR	1

#define DICTIONARY_MAGIC	0x437ff734

struct changeFilter {
#define cf_pattern cf_key.key
	tkey_t								cf_key;			/* the pattern */
	regex_t								cf_regex;		/* compiled regex - server only */
	PhidgetDictionary_OnChangeCallback	cf_handler;		/* client callback - client only */
	void								*cf_ctx;		/* client context - client only */
	RB_ENTRY(changeFilter)				cf_link;
};

typedef RB_HEAD(filters, changeFilter)	changefiltertree_t;
RB_PROTOTYPE(filters, changeFilter, cf_link, changeFiltercompare)

typedef struct PhidgetDictionary {
	uint32_t							magic;
	int									open;
	PhidgetNetConnHandle				nc;
	PhidgetDictionary_OnChangeCallback	onChange;
	char								*namespace;
	changefiltertree_t					filters;
	mos_mutex_t							filterslock;
	mos_mutex_t							execlock;	/* lock around calls */
	mosiop_t							iop;
} PhidgetDictionary;

#endif /* EXTERNALPROTO */

#endif /* _PHIDGET_DICTIONARY_H_ */
