/*
 * This file is part of libphidget22
 *
 * Copyright 2015 Phidgets Inc <patrick@phidgets.com>
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 3 of the License, or (at your option) any later version.

 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, see
 * <http://www.gnu.org/licenses/>
 */

#define _PHIDGET_NETWORKCODE

#include "phidgetbase.h"
#include "zeroconf.h"
#include "mos/mos_time.h"
#include "stats.h"

#ifdef ZEROCONF_RUNTIME_LINKING
#ifdef _WINDOWS
static HMODULE libHandle;
#else
static void *libHandle;
#endif
#else
#endif

typedef struct zcdisp {
	ZeroconfListener_t	listener;
	void				 *handle;
	void				*ctx;
	int					added;
	int					interface;
	Zeroconf_Protocol	protocol;
	char				*name;
	char				*host; /* fqdn */
	char				*type;
	char				*domain;
	MSLIST_ENTRY(zcdisp)	link;
} zcdisp_t;

MSLIST_HEAD(displisthead, zcdisp) displist = MSLIST_HEAD_INITIALIZER(displist);
static int			disprunning;
static mos_mutex_t	displock;
static mos_cond_t	dispcond;
typedef struct _wrapper {
	const char *			name;
	const char *			host;	/* fqdn */
	const char *			type;
	const char *			domain;
	int						port;
	PhidgetReturnCode		res;
	int						more;
	ZeroconfListenerHandle	handle;
	void					*arg;
	kv_t					**kv;
} wrapper_t;

static mos_mutex_t	lock;
static int			ncinitialized;
static int			zcstarted;

#define CHKINITV if (!ncinitialized) return
#define CHKINIT if (!ncinitialized) return (EPHIDGET_UNSUPPORTED)

#ifdef ZEROCONF_RUNTIME_LINKING
//function prototypes for run-time loaded library
typedef DNSServiceErrorType(DNSSD_API *DNSServiceRegisterType)(DNSServiceRef *, DNSServiceFlags, uint32_t,
  const char *, const char *, const char *, const char *, uint16_t, uint16_t, const void *,
  DNSServiceRegisterReply, void *);
typedef DNSServiceErrorType(DNSSD_API *DNSServiceProcessResultType)(DNSServiceRef);
typedef void (DNSSD_API *DNSServiceRefDeallocateType)(DNSServiceRef);
typedef DNSServiceErrorType(DNSSD_API *DNSServiceAddRecordType)(DNSServiceRef, DNSRecordRef *,
  DNSServiceFlags, uint16_t, uint16_t, const void *, uint32_t);
typedef DNSServiceErrorType(DNSSD_API *DNSServiceUpdateRecordType)(DNSServiceRef, DNSRecordRef,
  DNSServiceFlags, uint16_t, const void *, uint32_t);
typedef DNSServiceErrorType(DNSSD_API *DNSServiceRemoveRecordType)(DNSServiceRef, DNSRecordRef,
  DNSServiceFlags);
typedef DNSServiceErrorType(DNSSD_API *DNSServiceBrowseType)(DNSServiceRef *, DNSServiceFlags, uint32_t,
  const char *, const char *, DNSServiceBrowseReply, void *);
typedef DNSServiceErrorType(DNSSD_API *DNSServiceResolveType)(DNSServiceRef *, DNSServiceFlags,
  uint32_t, const char *, const char *, const char *, DNSServiceResolveReply, void *context);
typedef DNSServiceErrorType(DNSSD_API *DNSServiceQueryRecordType)(DNSServiceRef *, DNSServiceFlags,
  uint32_t, const char *, uint16_t, uint16_t, DNSServiceQueryRecordReply, void *context);
typedef int (DNSSD_API *DNSServiceConstructFullNameType)(char *, const char *, const char *, const char *);
typedef int (DNSSD_API *DNSServiceRefSockFDType)(DNSServiceRef);

typedef DNSServiceErrorType(DNSSD_API *DNSServiceGetAddrInfoType)(DNSServiceRef *, DNSServiceFlags,
  uint32_t, DNSServiceProtocol, const char *, DNSServiceGetAddrInfoReply, void *);

static DNSServiceRegisterType DNSServiceRegisterPtr;
static DNSServiceProcessResultType DNSServiceProcessResultPtr;
static DNSServiceRefDeallocateType DNSServiceRefDeallocatePtr;
static DNSServiceAddRecordType DNSServiceAddRecordPtr;
static DNSServiceUpdateRecordType DNSServiceUpdateRecordPtr;
static DNSServiceRemoveRecordType DNSServiceRemoveRecordPtr;
static DNSServiceBrowseType DNSServiceBrowsePtr;
static DNSServiceResolveType DNSServiceResolvePtr;
static DNSServiceQueryRecordType DNSServiceQueryRecordPtr;
static DNSServiceGetAddrInfoType DNSServiceGetAddrInfoPtr;
static DNSServiceConstructFullNameType DNSServiceConstructFullNamePtr;
static DNSServiceRefSockFDType DNSServiceRefSockFDPtr;

#else

#define DNSServiceRegisterPtr DNSServiceRegister
#define DNSServiceProcessResultPtr DNSServiceProcessResult
#define DNSServiceRefDeallocatePtr DNSServiceRefDeallocate
#define DNSServiceAddRecordPtr DNSServiceAddRecord
#define DNSServiceUpdateRecordPtr DNSServiceUpdateRecord
#define DNSServiceRemoveRecordPtr DNSServiceRemoveRecord
#define DNSServiceBrowsePtr DNSServiceBrowse
#define DNSServiceResolvePtr DNSServiceResolve
#define DNSServiceQueryRecordPtr DNSServiceQueryRecord
#define DNSServiceConstructFullNamePtr DNSServiceConstructFullName
#define DNSServiceRefSockFDPtr DNSServiceRefSockFD
#define DNSServiceGetAddrInfoPtr DNSServiceGetAddrInfo

#endif

static PhidgetReturnCode ZeroconfLoad(void);

static void
dispatch(ZeroconfListener_t listener, void *handle, void *ctx, int added, int interface,
  Zeroconf_Protocol proto, const char *name, const char *host, const char *type, const char *domain) {
	zcdisp_t *dp;

	dp = mos_malloc(sizeof(*dp));
	dp->listener = listener;
	dp->handle = handle;
	dp->ctx = ctx;
	dp->added = added;
	dp->interface = interface;
	dp->protocol = proto;
	dp->name = mos_strdup(name, NULL);
	dp->host = mos_strdup(host, NULL);
	dp->type = mos_strdup(type, NULL);
	dp->domain = mos_strdup(domain, NULL);

	mos_mutex_lock(&displock);
	MSLIST_INSERT_HEAD(&displist, dp, link);
	mos_cond_broadcast(&dispcond);
	mos_mutex_unlock(&displock);
}

static MOS_TASK_RESULT
listener_dispatch_thread(void *arg) {
	zcdisp_t *dp;

	mos_mutex_lock(&displock);
	while (disprunning) {
		if (MSLIST_EMPTY(&displist)) {
			mos_cond_timedwait(&dispcond, &displock, MOS_SEC /* nsec */);
			continue;
		}

		dp = MSLIST_FIRST(&displist);
		MSLIST_REMOVE(&displist, dp, zcdisp, link);
		mos_mutex_unlock(&displock);
		dp->listener(dp->handle, dp->ctx, dp->added, dp->interface, dp->protocol, dp->name, dp->host, dp->type,
		  dp->domain);
		mos_free(dp->name, MOSM_FSTR);
		mos_free(dp->host, MOSM_FSTR);
		mos_free(dp->type, MOSM_FSTR);
		mos_free(dp->domain, MOSM_FSTR);
		mos_free(dp, sizeof(*dp));
		mos_mutex_lock(&displock);
	}
	disprunning = -1;
	mos_cond_broadcast(&dispcond);
	mos_mutex_unlock(&displock);

	decPhidgetStat("discovery.dispatchers");
	MOS_TASK_EXIT(0);
}

void
ZeroconfInit() {

	mos_glock((void *)1);
	if (ncinitialized) {
		mos_gunlock((void *)1);
		return;
	}
	mos_gunlock((void *)1);

	mos_mutex_init(&lock);
	mos_mutex_init(&displock);
	mos_cond_init(&dispcond);

	if (ZeroconfLoad() != 0)
		disprunning = 0;
	mos_mutex_destroy(&lock);
}

void
ZeroconfFini() {

	ZeroconfStop();

	mos_glock((void *)1);
	if (!ncinitialized) {
		mos_gunlock((void *)1);
		return;
	}

	ncinitialized = 0;
	mos_gunlock((void *)1);

#ifdef ZEROCONF_RUNTIME_LINKING
#ifdef _WINDOWS
	FreeLibrary(libHandle);
#else
	dlclose(libHandle);
#endif

	DNSServiceRegisterPtr = NULL;
	DNSServiceProcessResultPtr = NULL;
	DNSServiceRefDeallocatePtr = NULL;
	DNSServiceAddRecordPtr = NULL;
	DNSServiceUpdateRecordPtr = NULL;
	DNSServiceRemoveRecordPtr = NULL;
	DNSServiceBrowsePtr = NULL;
	DNSServiceResolvePtr =  NULL;
	DNSServiceQueryRecordPtr = NULL;
	DNSServiceConstructFullNamePtr = NULL;
	DNSServiceRefSockFDPtr = NULL;
	DNSServiceGetAddrInfoPtr = NULL;
#endif

	mos_mutex_destroy(&displock);
	mos_cond_destroy(&dispcond);
}

void
ZeroconfStart() {
	mos_task_t task;

	mos_glock((void *)1);
	if (!ncinitialized || zcstarted) {
		mos_gunlock((void *)1);
		return;
	}
	zcstarted = 1;
	mos_gunlock((void *)1);

	mos_mutex_lock(&displock);
	if (mos_task_create(&task, listener_dispatch_thread, NULL) != 0) {
		netlogerr("Failed to create dns listener dispatch thread");
		mos_mutex_unlock(&displock);
		return;
	}
	incPhidgetStat("discovery.dispatchers");
	disprunning = 1;
	mos_mutex_unlock(&displock);
}

void
ZeroconfStop() {

	mos_glock((void *)1);
	if (!ncinitialized || !zcstarted) {
		mos_gunlock((void *)1);
		return;
	}
	zcstarted = 0;
	mos_gunlock((void *)1);

	mos_mutex_lock(&displock);
	disprunning = 0;
	mos_cond_broadcast(&dispcond);
	while (disprunning == 0)
		mos_cond_wait(&dispcond, &displock);
	mos_mutex_unlock(&displock);
}

static PhidgetReturnCode
ZeroconfLoad() {
#if defined(_WINDOWS) && defined(ZEROCONF_RUNTIME_LINKING)
	DWORD error;
#endif

	mos_mutex_lock(&lock);
	if (ncinitialized) {
		mos_mutex_unlock(&lock);
		return (EPHIDGET_OK);
	}

#ifdef ZEROCONF_RUNTIME_LINKING
#ifdef WINCE
#define STRTYPE	L
#else
#define STRTYPE
#endif

#ifdef _WINDOWS
#define SYM	GetProcAddress
#define CK(stmt) do {										\
	if ((stmt) == NULL) {									\
		fprintf(stderr, "'%s' failed", #stmt);				\
		mos_mutex_unlock(&lock);							\
		return (EPHIDGET_UNEXPECTED);						\
	}														\
} while (0)

	libHandle = LoadLibrary(L"dnssd.dll");
	if (libHandle == NULL) {
		error = GetLastError();
		switch (error) {
		case ERROR_MOD_NOT_FOUND:
			break;
		default:
			fprintf(stderr, "LoadLibrary(dnssd.dll) failed with error code: 0x%08x", error);
		}
		netloginfo("Zeroconf is not supported");
		mos_mutex_unlock(&lock);
		return (EPHIDGET_UNSUPPORTED);
	}
#else
#define SYM	dlsym
#define CK(stmt) do {										\
	if ((stmt) == NULL) {									\
		fprintf(stderr, "'%s' failed", #stmt);				\
		fprintf(stderr, "'%s' failed:%s", #stmt, dlerror());\
		mos_mutex_unlock(&lock);							\
		return (EPHIDGET_UNEXPECTED);						\
	}														\
} while (0)

	libHandle = dlopen("libdns_sd.so", RTLD_LAZY);
	if (!libHandle) {
		libHandle = dlopen("libdns_sd.so.1", RTLD_LAZY);
		if (!libHandle) {
			mos_mutex_unlock(&lock);
			return (EPHIDGET_UNSUPPORTED);
		}
	}
#endif /* _MACOS */

	CK(DNSServiceRegisterPtr = (DNSServiceRegisterType)SYM(libHandle, "DNSServiceRegister"));
	CK(DNSServiceProcessResultPtr = (DNSServiceProcessResultType)SYM(libHandle, "DNSServiceProcessResult"));
	CK(DNSServiceRefDeallocatePtr = (DNSServiceRefDeallocateType)SYM(libHandle, "DNSServiceRefDeallocate"));
	CK(DNSServiceAddRecordPtr = (DNSServiceAddRecordType)SYM(libHandle, "DNSServiceAddRecord"));
	CK(DNSServiceUpdateRecordPtr = (DNSServiceUpdateRecordType)SYM(libHandle, "DNSServiceUpdateRecord"));
	CK(DNSServiceRemoveRecordPtr = (DNSServiceRemoveRecordType)SYM(libHandle, "DNSServiceRemoveRecord"));
	CK(DNSServiceBrowsePtr = (DNSServiceBrowseType)SYM(libHandle, "DNSServiceBrowse"));
	CK(DNSServiceResolvePtr = (DNSServiceResolveType)SYM(libHandle, "DNSServiceResolve"));
	CK(DNSServiceQueryRecordPtr = (DNSServiceQueryRecordType)SYM(libHandle, "DNSServiceQueryRecord"));
	CK(DNSServiceConstructFullNamePtr = (DNSServiceConstructFullNameType)SYM(libHandle, "DNSServiceConstructFullName"));
	CK(DNSServiceRefSockFDPtr = (DNSServiceRefSockFDType)SYM(libHandle, "DNSServiceRefSockFD"));
	CK(DNSServiceGetAddrInfoPtr = (DNSServiceGetAddrInfoType)SYM(libHandle, "DNSServiceGetAddrInfo"));

#endif /* ZEROCONF_RUNTIME_LINKING */

	ncinitialized = 1;
	mos_mutex_unlock(&lock);

	return (EPHIDGET_OK);
}

/*
 * src and dst may overlap; that is, they may be the same array.
 */
static const char *
unescape(const char *src, char *dst, size_t dstlen) {
	const char *end;
	const char *ret;
	int v, v0, v1, v2;
	int c;

	ret = dst;

	end = dst + dstlen;

	for (end = dst + dstlen; *src && dst < end; dst++) {
		c = *src++;
		if (c == '\\') {
			if (src[0] == '\\' || src[0] == '.') {
				*dst = *src++;
				continue;
			}
			if ((src[0] != '\0' && src[1] != '\0' && src[2] != '\0') &&
			  (mos_isdigit(src[0]) && mos_isdigit(src[1]) && mos_isdigit(src[2]))) {
				v0 = src[0] - '0';
				v1 = src[1] - '0';
				v2 = src[2] - '0';
				v = v0 * 100 + v1 * 10 + v2;
				if (v < 255) {
					c = (char)v;
					src += 3;
				}
			}
		}
		*dst = c;
	}
	*dst = '\0';

	return (ret);
}

static PhidgetReturnCode
parseTXTRecord(const char *txtRecord, uint16_t txtLen, kv_t **kv) {
	char key[255], val[255];
	char *p, *e;
	char *record;
	char *eq;

	if (txtRecord == NULL || txtLen == 0) {
		*kv = NULL;
		return (EPHIDGET_OK);
	}

	if (newkv(kv))
		return (EPHIDGET_UNEXPECTED);

	record = mos_malloc(txtLen);
	memcpy(record, txtRecord, txtLen);

	p = record;
	e = p + txtLen;

	while (p < e) {
		if (p + p[0] > e) {
			mos_free(record, txtLen);
			return (EPHIDGET_UNEXPECTED);
		}

		mos_strlcpy(key, p + 1, p[0] + 1);
		eq = mos_strchr(key, '=');
		if (eq) {
			*eq = '\0';
			mos_strlcpy(val, eq + 1, sizeof(val));
			kvset(*kv, MOS_IOP_IGNORE, key, val);
		} else {
			kvset(*kv, MOS_IOP_IGNORE, key, "");
		}
		p += p[0] + 1;
	}

	mos_free(record, txtLen);
	return (EPHIDGET_OK);
}

static PhidgetReturnCode
mkTXTRecord(kv_t *kv, uint8_t **txtRecord, uint16_t *txtLen) {
	size_t klen;
	size_t vlen;
	size_t len;
	uint8_t *p;
	kvent_t *e;

	if (kv == NULL) {
		*txtLen = 0;
		*txtRecord = NULL;
		return (EPHIDGET_OK);
	}

	len = 0;
	MTAILQ_FOREACH(e, &kv->list, link) {
		len++;	/* for the len */
		klen = mos_strlen(e->key);
		if (klen > 254)
			return (EPHIDGET_INVALID);

		vlen = mos_strlen(e->val);
		if (vlen > 255 - klen + 1)
			return (EPHIDGET_INVALID);

		if (vlen > 0)
			len++;	/* for the '=' */

		len += klen;
		len += vlen;
	}

	if (len > ZEROCONF_MAX_TXTRECORD)
		return (EPHIDGET_INVALID);

	p = mos_malloc(len);
	*txtLen = (uint16_t)len;
	*txtRecord = p;

	MTAILQ_FOREACH(e, &kv->list, link) {
		klen = mos_strlen(e->key);
		vlen = mos_strlen(e->val);
		if (vlen > 0) {
			MOS_ASSERT(klen + vlen + 1 < 256);
			*p = (uint8_t)(klen + vlen + 1), p++;
			memcpy(p, e->key, klen), p += klen;
			*p = '=', p++;
			memcpy(p, e->val, vlen), p += vlen;
		} else {
			MOS_ASSERT(klen < 256);
			*p = (uint8_t)klen;
			memcpy(p, e->key, klen), p += klen;
		}
	}

	return (EPHIDGET_OK);
}

static void
ZeroconfListenerHandle_free(ZeroconfListenerHandle *_handle) {
	ZeroconfListenerHandle handle;

	CHKINITV;

	handle = *_handle;

	mos_mutex_lock(&handle->lock);
	handle->flags &= ~ZCL_RUN;

	/*
	 * Remove any server registrations to the handle.
	 */
	releaseMDNSControlEntries(handle);

	while (!(handle->flags & ZCL_STOPPED))
		mos_cond_wait(&handle->cond, &handle->lock);

	mos_mutex_unlock(&handle->lock);
	mos_mutex_destroy(&handle->lock);
	mos_cond_destroy(&handle->cond);
	DNSServiceRefDeallocatePtr(handle->ref);
	mos_free(handle->type, MOSM_FSTR);
	mos_free(handle, sizeof(*handle));
	*_handle = NULL;
}

static void
getsrvname(const char *fullname, char *srvname, size_t srvnamelen, char *domain, size_t domainlen) {
	const char *p;

	for (p = fullname; *p; p++) {
		if (*p == '.') {
			mos_strlcpy(srvname, fullname, MOS_MIN(p - fullname + 1, (signed)srvnamelen));
			if (domain)
				mos_strlcpy(domain, p + 1, domainlen);
			goto found;
		}
		if (*p == '\\')
			p++;
	}

	mos_strlcpy(srvname, fullname, srvnamelen);

found:

	unescape(srvname, srvname, srvnamelen);
}

static void DNSSD_API
DNSServiceResolve_Callback(DNSServiceRef sdRef, DNSServiceFlags flags, uint32_t interface, DNSServiceErrorType errorCode,
  const char *fullname, const char *host, uint16_t port, uint16_t txtLen, const unsigned char *txtRecord, void *context) {
	wrapper_t *wrapper;

	wrapper = context;

	if ((flags & kDNSServiceFlagsMoreComing) == 0)
		wrapper->more = 0;

	if (errorCode != kDNSServiceErr_NoError) {
		netlogerr("DNSServiceResolve() failed 0x%x", errorCode);
		return;
	}

	if (wrapper->handle->listener)
		dispatch(wrapper->handle->listener, wrapper->handle, wrapper->handle->listenerctx, 1, interface, ZCP_UNSPEC,
		  wrapper->name, host, wrapper->type, wrapper->domain);
}

static void DNSSD_API
DNSServiceBrowse_Callback(DNSServiceRef sdRef, DNSServiceFlags flags, uint32_t interface,
  DNSServiceErrorType errorCode, const char *name, const char *type, const char *domain, void *context) {
	ZeroconfListenerHandle handle;
	DNSServiceErrorType err;
	DNSServiceRef ref;
	wrapper_t wrapper;

	if (errorCode != kDNSServiceErr_NoError) {
		netlogerr("DNSServiceBrowse(%s) failed 0x%x", type, errorCode);
		return;
	}

	/*
	 * Service Removed.
	 */
	if ((flags & kDNSServiceFlagsAdd) == 0) {
		handle = context;
		if (handle->listener)
			dispatch(handle->listener, handle, handle->listenerctx, 0, interface, ZCP_UNSPEC, name, name, type, domain);
		return;
	}

	/*
	 * Service Added.
	 */

	wrapper.more = 1;
	wrapper.handle = context;
	wrapper.name = name;
	wrapper.type = type;
	wrapper.domain = domain;

	err = DNSServiceResolvePtr(&ref, kDNSServiceFlagsForceMulticast, interface, name, type, domain,
	  DNSServiceResolve_Callback, &wrapper);
	if (err != kDNSServiceErr_NoError)
		netlogerr("DNSServiceResolve returned error %x", err);

	while (wrapper.more)
		DNSServiceProcessResultPtr(ref);
	DNSServiceRefDeallocatePtr(ref);
}

/*
 * The thread never frees the handle.
 */
static MOS_TASK_RESULT
runZeroconfListener(void *arg) {
	ZeroconfListenerHandle handle;
	DNSServiceErrorType res;
	int wasconnected;
	mos_socket_t fd;
	int errcnt;
	int err;

	wasconnected = 0;
	errcnt = 0;

	handle = (ZeroconfListenerHandle)arg;

again:

	err = DNSServiceBrowsePtr(&handle->ref, 0, 0, handle->type, "", DNSServiceBrowse_Callback, handle);
	if (err != kDNSServiceErr_NoError) {
		if (wasconnected) {
			errcnt++;
			mos_usleep(1000000); /* wait 1 second and try again... forever */
			goto again;
		}

		if (errcnt > 3) {
			netlogerr("DNSServiceBrowse() failed: missing service?");
			err = EPHIDGET_UNEXPECTED;
			goto bad;
		}
		errcnt++;
		mos_usleep(1000000);	/* wait 1 second and try again */
		goto again;
	}

	fd = DNSServiceRefSockFDPtr(handle->ref);
	if ((int)fd < 0) {
		netlogerr("Failed to get a valid service socket descriptor");
		err = EPHIDGET_UNEXPECTED;
		errcnt++;
		goto bad;
	}

	wasconnected = 1;

	mos_mutex_lock(&handle->lock);
	handle->flags |= ZCL_RUNNING;
	mos_cond_broadcast(&handle->cond);

	while (handle->flags & ZCL_RUN) {
		mos_mutex_unlock(&handle->lock);
		/* Do our own poll so that we can stop the listener */
		err = mos_netop_tcp_rpoll(MOS_IOP_IGNORE, &fd, 1000);
		mos_mutex_lock(&handle->lock);
		if (err == EPHIDGET_TIMEOUT)
			continue;
		if (err != EPHIDGET_OK) {
			mos_mutex_unlock(&handle->lock);
			netlogerr("mos_netop_tcp_rpoll() failed for service browser '%s'", handle->type);
			goto bad;
		}

		mos_mutex_unlock(&handle->lock);
		res = DNSServiceProcessResultPtr(handle->ref);
		mos_mutex_lock(&handle->lock);
		if (res != kDNSServiceErr_NoError) {
			netlogerr("failed to process result for service browser '%s'", handle->type);
			if (errcnt > 3) {
				mos_mutex_unlock(&handle->lock);
				netlogerr("too many errors processing service results: reconnecting");
				errcnt = 0;
				goto bad;
			}
			errcnt++;
			continue;
		}
		errcnt = 0;
	}

	if ((handle->flags & ZCL_RUN) == 0) {
		handle->flags = ZCL_STOPPED;
		mos_cond_broadcast(&handle->cond);
		mos_mutex_unlock(&handle->lock);

		decPhidgetStat("discovery.listeners");
		MOS_TASK_EXIT(0);
	}

bad:

	if (errcnt < 3) {
		DNSServiceRefDeallocatePtr(handle->ref);
		goto again;
	}

	mos_mutex_lock(&handle->lock);
	handle->flags = ZCL_STOPPED | ZCL_ERROR;
	mos_cond_broadcast(&handle->cond);
	mos_mutex_unlock(&handle->lock);

	decPhidgetStat("discovery.listeners");
	MOS_TASK_EXIT(err);
}

PhidgetReturnCode
Zeroconf_listen(ZeroconfListenerHandle *_handle, const char *type, ZeroconfListener_t fptr, void *ctx) {
	ZeroconfListenerHandle handle;
	int res;

	CHKINIT;

	*_handle = NULL;

	handle = mos_zalloc(sizeof(*handle));
	handle->listener = fptr;
	handle->listenerctx = ctx;
	handle->type = mos_strdup(type, NULL);
	mos_mutex_init(&handle->lock);
	mos_cond_init(&handle->cond);

	mos_mutex_lock(&handle->lock);

	handle->flags = ZCL_RUN;
	res = mos_task_create(&handle->task, runZeroconfListener, handle);
	if (res != 0) {
		ZeroconfListenerHandle_free(&handle);
		return (EPHIDGET_UNEXPECTED);
	}

	while ((handle->flags & ZCL_RUNNING) == 0) {
		mos_cond_timedwait(&handle->cond, &handle->lock, 1000000000);
		if (handle->flags & ZCL_ERROR) {
			mos_mutex_unlock(&handle->lock);
			ZeroconfListenerHandle_free(&handle);
			return (EPHIDGET_UNEXPECTED);
		}
	}
	mos_mutex_unlock(&handle->lock);

	incPhidgetStat("discovery.listeners");

	*_handle = handle;

	return (EPHIDGET_OK);
}

void
Zeroconf_listenclose(ZeroconfListenerHandle *_handle) {

	CHKINITV;

	ZeroconfListenerHandle_free(_handle);
	*_handle = NULL;
}

/*
 * Called from Zeroconf_lookup() to determine the txtRecords and the port.
 */
static void DNSSD_API
LookupResolve_Callback(DNSServiceRef sdRef, DNSServiceFlags flags, uint32_t interface, DNSServiceErrorType errorCode,
  const char *fullname, const char *host, uint16_t port, uint16_t txtLen, const unsigned char *txtRecord, void *context) {
	wrapper_t *wrapper;

	wrapper = context;
	wrapper->more = 0;	/* we only get the first */

	if (errorCode != kDNSServiceErr_NoError) {
		netlogerr("DNSServiceResolve() failed 0x%x", errorCode);
		wrapper->res = EPHIDGET_UNEXPECTED;
		return;
	}
	wrapper->port = port;
	if (wrapper->kv) {
		wrapper->res = parseTXTRecord((const char *)txtRecord, txtLen, wrapper->kv);
		if (wrapper->res != 0)
			netlogerr("parseTXTRecord() failed");
	}
}

static void
GetAddrInfoReply(DNSServiceRef ref, DNSServiceFlags flags, uint32_t interface, DNSServiceErrorType errorCode,
  const char *hostname, const struct sockaddr *addr, uint32_t ttle, void *context) {
	mos_sockaddr_t *maddr;
	wrapper_t *wrapper;

	wrapper = context;
	wrapper->more = 0;
	maddr = wrapper->arg;

	if (errorCode != kDNSServiceErr_NoError) {
		wrapper->res = EPHIDGET_UNEXPECTED;
		return;
	}

	if ((flags & kDNSServiceFlagsAdd) == 0) {
		wrapper->res = EPHIDGET_NOENT;
		return;
	}

	if (((const struct sockaddr_in *)addr)->sin_family != AF_INET) {
		wrapper->res = EPHIDGET_UNEXPECTED;
		return;
	}

	maddr->sa = *addr; /* missing port */
	wrapper->res = EPHIDGET_OK;
}

PhidgetReturnCode
Zeroconf_lookup(ZeroconfListenerHandle handle, int interface, Zeroconf_Protocol proto, const char *name, const char *host,
  const char *type, const char *domain, Zeroconf_Protocol reqproto, mos_sockaddr_t *addr, kv_t **txtRecords) {
	DNSServiceErrorType err;
	PhidgetReturnCode res;
	DNSServiceRef ref;
	wrapper_t wrapper;
	mos_socket_t fd;

	CHKINIT;

	TESTPTR(host);
	TESTPTR(addr);

	if (addr == NULL && txtRecords == NULL)
		return (EPHIDGET_INVALIDARG);

	wrapper.more = 1;
	wrapper.handle = handle;
	wrapper.arg = addr;
	wrapper.host = host;

	/*
	 * Resolve the Address first
	 */
	if (addr == NULL)
		goto txtrecords;

	err = DNSServiceGetAddrInfoPtr(&ref, 0, kDNSServiceInterfaceIndexAny, reqproto, host,
	  GetAddrInfoReply, &wrapper);
	if (err != kDNSServiceErr_NoError)
		return (EPHIDGET_UNEXPECTED);

	fd = DNSServiceRefSockFDPtr(ref);
	res = mos_netop_tcp_rpoll(MOS_IOP_IGNORE, &fd, 10000);
	if (res != EPHIDGET_OK) {
		DNSServiceRefDeallocatePtr(ref);
		return (res);
	}

	while (wrapper.more)
		DNSServiceProcessResultPtr(ref);
	DNSServiceRefDeallocatePtr(ref);

	if (wrapper.res != EPHIDGET_OK)
		return (wrapper.res);

	/*
	 * Now, get the txt records
	 */
txtrecords:

	wrapper.more = 1;
	wrapper.kv = txtRecords;

	err = DNSServiceResolvePtr(&ref, kDNSServiceFlagsForceMulticast, interface, name, type, domain,
	  LookupResolve_Callback, &wrapper);
	if (err != kDNSServiceErr_NoError)
		netlogerr("DNSServiceResolve returned error %x", err);

	fd = DNSServiceRefSockFDPtr(ref);
	res = mos_netop_tcp_rpoll(MOS_IOP_IGNORE, &fd, 10000);
	if (res != EPHIDGET_OK) {
		DNSServiceRefDeallocatePtr(ref);
		return (res);
	}
	while (wrapper.more)
		DNSServiceProcessResultPtr(ref);
	DNSServiceRefDeallocatePtr(ref);

	if (wrapper.res != 0 && txtRecords != NULL) {
		*txtRecords = NULL;
		return (wrapper.res);
	}

	if (addr)
		addr->s4.sin_port = ntohs(wrapper.port);

	return (EPHIDGET_OK);
}

/**************************************************************************************************************************
 * PUBLISH
 */

static void
ZeroconfPublishHandle_free(ZeroconfPublishHandle *handle) {

	CHKINITV;

	if ((*handle)->host)
		mos_free((*handle)->host, MOSM_FSTR);
	if ((*handle)->type)
		mos_free((*handle)->type, MOSM_FSTR);
	mos_free(*handle, sizeof(ZeroconfPublish));
	*handle = NULL;
}

static void DNSSD_API
DNSServiceRegister_Callback(DNSServiceRef sdRef, DNSServiceFlags flags, DNSServiceErrorType errorCode,
  const char *name, const char *type, const char *domain, void *context) {

	netloginfo("service register: %s %s %s", name, type, domain);
}

PhidgetReturnCode
Zeroconf_publish(ZeroconfPublishHandle *_handle, const char *name, const char *host, const char *type,
  int _port, kv_t *txtRecords) {
	ZeroconfPublishHandle handle;
	DNSServiceErrorType err;
	PhidgetReturnCode res;
	uint8_t *TXTRecords;
	uint16_t TXTLen;
	int port;

	CHKINIT;

	TESTPTR(_handle);
	TESTPTR(type);

	port = htons(_port);

	res = mkTXTRecord(txtRecords, &TXTRecords, &TXTLen);
	if (res != 0)
		return (res);

	handle = mos_zalloc(sizeof(*handle));

	err = DNSServiceRegisterPtr(&handle->ref, 0, 0, name, type, NULL, host, port, TXTLen, TXTRecords,
	  DNSServiceRegister_Callback, handle);
	if (TXTRecords)
		mos_free(TXTRecords, TXTLen);
	if (err != kDNSServiceErr_NoError) {
		ZeroconfPublishHandle_free(&handle);
		mos_free(handle, sizeof(*handle));
		return (EPHIDGET_NETUNAVAIL);
	}

	if (host)
		handle->host = mos_strdup(host, NULL);
	handle->type = mos_strdup(type, NULL);

	*_handle = handle;

	return (EPHIDGET_OK);
}

PhidgetReturnCode
Zeroconf_unpublish(ZeroconfPublishHandle *handle) {

	CHKINIT;

	TESTPTR(handle);
	DNSServiceRefDeallocatePtr((*handle)->ref);
	ZeroconfPublishHandle_free(handle);

	return (EPHIDGET_OK);
}
