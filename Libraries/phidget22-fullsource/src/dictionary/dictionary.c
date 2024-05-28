#define _PHIDGET_NETWORKCODE

#include "phidgetbase.h"
#include "phidgetdictionary.h"
#include "dictionaryimpl.h"
#include "mos/mos_iop.h"
#include "mos/mos_assert.h"

static int
changeFilterCompare(struct changeFilter *a, struct changeFilter *b) {

	return (mos_strcmp(a->cf_pattern, b->cf_pattern));
}

RB_GENERATE(filters, changeFilter, cf_link, changeFilterCompare)

static PhidgetReturnCode
handleDictionaryRequest(mosiop_t iop, PhidgetNetConnHandle nc, netreq_t *req, int *stop) {
	char ns[DICTIONARY_NAMESPACEMAX];
	char path[DICTIONARY_PATHMAX];
	char val[DICTIONARY_VALMAX];
	char act[DICTIONARY_ACTMAX];
	PhidgetDictionary_Action action;
	PhidgetDictionaryHandle dict;
	struct changeFilter skey;
	struct changeFilter *cf;
	PhidgetReturnCode res;

	dict = nc->private;
	MOS_ASSERT(dict != NULL);
	MOS_ASSERT(dict->magic == DICTIONARY_MAGIC);

	if (req->nr_type != MSG_DICTIONARY)
		return (MOS_ERROR(iop, EPHIDGET_UNEXPECTED, "unexpected msgtype:%d/%d", req->nr_type, req->nr_stype));

	switch (req->nr_stype) {
	case SMSG_DICTADD:
		action = PHIDGETDICTIONARY_ADD;
		break;
	case SMSG_DICTUPDATE:
		action = PHIDGETDICTIONARY_UPDATE;
		break;
	case SMSG_DICTDEL:
		action = PHIDGETDICTIONARY_REMOVE;
		break;
	default:
		return (MOS_ERROR(iop, EPHIDGET_UNEXPECTED, "unexpected msgtype:%d/%d", req->nr_type, req->nr_stype));
	}

	if ((req->nr_flags & NRF_EVENT) == 0)
		return (MOS_ERROR(iop, EPHIDGET_UNEXPECTED, "non-event request received by dictionary client"));

	res = getKeyValue(iop, req, ns, sizeof(ns), path, sizeof(path), val, sizeof(val), act, sizeof(act));
	if (res != EPHIDGET_OK)
		return (MOS_ERROR(iop, res, "failed to get values from event"));

	skey.cf_key.ckey = act;
	mos_mutex_lock(&dict->filterslock);
	cf = RB_FIND(filters, &dict->filters, &skey);
	if (cf && cf->cf_handler) {
		if (action == PHIDGETDICTIONARY_ADD || action == PHIDGETDICTIONARY_UPDATE)
			cf->cf_handler(dict, cf->cf_ctx, ns, act, path, val, action);
		else
			cf->cf_handler(dict, cf->cf_ctx, ns, act, path, NULL, action);
	}
	mos_mutex_unlock(&dict->filterslock);

	return (0);
}

static PhidgetReturnCode
sendKV(PhidgetDictionaryHandle dict, mosiop_t iop, int flags, msgsubtype_t stype, const char *ns,
  const char *key, const char *val, const char *act) {
	char buf[DICTIONARY_BUFSZ];
	PhidgetReturnCode rres, res;
	WaitForReply *wfr;
	size_t bufsz;

	TESTPTR(dict);
	TESTPTR(key);
	TESTPTR(val);
	TESTPTR(act);

	wfr = NULL;

	bufsz = sizeof(buf);
	res = setKeyValue(iop, buf, &bufsz, ns, key, val, act);
	if (res != 0)
		return (MOS_ERROR(iop, res, "failed to create key value packet"));

	res = writeRequest(iop, dict->nc, flags, MSG_DICTIONARY, stype, buf, (uint32_t)bufsz, &wfr);
	if (res != 0)
		return (MOS_ERROR(iop, res, "failed to write request"));

	res = simpleWaitForReply(&wfr, &rres);
	if (res != EPHIDGET_OK)
		return (MOS_ERROR(iop, res, "failed to receive server reply"));
	if (rres != EPHIDGET_OK)
		return (MOS_ERROR(iop, rres, "server indicated an error"));

	return (0);
}

static PhidgetReturnCode
removeFilterHandler(mosiop_t iop, PhidgetDictionaryHandle dict, const char *pattern) {
	struct changeFilter skey;
	struct changeFilter *cf;

	skey.cf_key.ckey = pattern;

	cf = RB_FIND(filters, &dict->filters, &skey);
	if (cf == NULL)
		return (MOS_ERROR(iop, EPHIDGET_NOENT, "no such filter '%s'", pattern));
	RB_REMOVE(filters, &dict->filters, cf);
	mos_free(cf->cf_pattern, mos_strlen(cf->cf_pattern) + 1);
	mos_free(cf, sizeof(*cf));

	return (EPHIDGET_OK);
}

/* WRAPPED FUNCTIONS */

static PhidgetReturnCode
PhidgetDictionary__addFilter(PhidgetDictionaryHandle dict, PhidgetDictionary_OnChangeCallback handler,
  void *ctx, const char *pattern) {
	struct changeFilter *cf;
	PhidgetReturnCode res;

	mos_mutex_lock(&dict->filterslock);
	removeFilterHandler(MOS_IOP_IGNORE, dict, pattern); // ignore failure

	cf = mos_malloc(sizeof(*cf));
	cf->cf_ctx = ctx;
	cf->cf_pattern = mos_strdup(pattern, NULL);
	cf->cf_handler = handler;

	RB_INSERT(filters, &dict->filters, cf);
	mos_mutex_unlock(&dict->filterslock);

	if (dict->iop)
		mos_iop_release(&dict->iop);
	dict->iop = mos_iop_alloc();

	if (dict->open == 0)
		return (MOS_ERROR(dict->iop, EPHIDGET_CLOSED, "handle is not open"));

	res = sendKV(dict, dict->iop, 0, SMSG_DICTADDFILTER, "", "", "", pattern);
	if (res != 0) {
		logerr("failed to add filter to server\n%N", dict->iop);
		mos_mutex_lock(&dict->filterslock);
		removeFilterHandler(MOS_IOP_IGNORE, dict, pattern);
		mos_mutex_unlock(&dict->filterslock);
		return (MOS_ERROR(dict->iop, res, "failed to add filter '%s' to server'", pattern));
	}

	mos_iop_release(&dict->iop);

	return (res);
}

static PhidgetReturnCode
PhidgetDictionary__removeFilterHandler(PhidgetDictionaryHandle dict, const char *pattern) {
	PhidgetReturnCode res;

	if (dict->iop)
		mos_iop_release(&dict->iop);
	dict->iop = mos_iop_alloc();

	if (dict->open == 0)
		return (MOS_ERROR(dict->iop, EPHIDGET_CLOSED, "handle is not open"));

	mos_mutex_lock(&dict->filterslock);
	res = removeFilterHandler(dict->iop, dict, pattern);
	mos_mutex_unlock(&dict->filterslock);
	if (res != 0)
		return (MOS_ERROR(dict->iop, res, "failed to remove local filter '%s'", pattern));

	res = sendKV(dict, dict->iop, 0, SMSG_DICTDELFILTER, "", "", "", pattern);
	if (res != 0)
		return (MOS_ERROR(dict->iop, res, "failed to remove filter '%s' from server", pattern));

	mos_iop_release(&dict->iop);
	return (EPHIDGET_OK);
}

static PhidgetReturnCode
PhidgetDictionary__add(PhidgetDictionaryHandle dict, const char *key, const char *val, int flags) {
	PhidgetReturnCode res;

	if (dict->iop)
		mos_iop_release(&dict->iop);
	dict->iop = mos_iop_alloc();

	if (dict->open == 0)
		return (MOS_ERROR(dict->iop, EPHIDGET_CLOSED, "handle is not open"));

	if (flags & DICTIONARY_PERSIST)
		flags = NRFDICT_PERSIST;
	else
		flags = 0;

	res = sendKV(dict, dict->iop, flags, SMSG_DICTADD, "", key, val, "");
	if (res != 0) {
		logerr("failed to add key value\n%N", dict->iop);
		return (MOS_ERROR(dict->iop, res, "failed to add key '%s'", key));
	}

	mos_iop_release(&dict->iop);
	return (res);
}

static PhidgetReturnCode
PhidgetDictionary__update(PhidgetDictionaryHandle dict, const char *key, const char *val, int flags) {
	PhidgetReturnCode res;

	if (dict->iop)
		mos_iop_release(&dict->iop);
	dict->iop = mos_iop_alloc();

	if (dict->open == 0)
		return (MOS_ERROR(dict->iop, EPHIDGET_CLOSED, "handle is not open"));

	if (flags & DICTIONARY_PERSIST)
		flags = NRFDICT_PERSIST;
	else
		flags = 0;

	res = sendKV(dict, dict->iop, flags, SMSG_DICTUPDATE, "", key, val, "");
	if (res != 0) {
		logerr("failed to update key value\n%N", dict->iop);
		return (MOS_ERROR(dict->iop, res, "failed to update key '%s'", key));
	}

	mos_iop_release(&dict->iop);
	return (res);
}

static PhidgetReturnCode
PhidgetDictionary__remove(PhidgetDictionaryHandle dict, const char *key) {
	PhidgetReturnCode res;

	TESTPTR(dict);
	TESTPTR(key);

	if (dict->iop)
		mos_iop_release(&dict->iop);
	dict->iop = mos_iop_alloc();

	if (dict->open == 0)
		return (MOS_ERROR(dict->iop, EPHIDGET_CLOSED, "handle is not open"));

	res = sendKV(dict, dict->iop, 0, SMSG_DICTDEL, "", key, "", "");
	if (res != 0) {
		logerr("failed to remove key '%s'\n%N", key, dict->iop);
		return (MOS_ERROR(dict->iop, res, "failed to remove key '%s'", key));
	}

	mos_iop_release(&dict->iop);
	return (res);
}

static PhidgetReturnCode
PhidgetDictionary__get(PhidgetDictionaryHandle dict, const char *key, char *val, size_t valsz) {
	char buf[DICTIONARY_BUFSZ];
	char act[1];
	char ns[1];
	PhidgetReturnCode rres, res;
	WaitForReply *wfr;
	size_t bufsz;

	TESTPTR(dict);
	TESTPTR(key);
	TESTPTR(val);

	if (dict->iop)
		mos_iop_release(&dict->iop);
	dict->iop = mos_iop_alloc();

	if (dict->open == 0)
		return (MOS_ERROR(dict->iop, EPHIDGET_CLOSED, "handle is not open"));

	wfr = NULL;

	bufsz = sizeof(buf);
	res = setKeyValue(dict->iop, buf, &bufsz, "", key, "", "");
	if (res != 0) {
		logerr("failed to create key value packet\n%N", dict->iop);
		MOS_ERROR(dict->iop, res, "failed to create key value packet");
		goto bad;
	}

	res = writeRequest(dict->iop, dict->nc, 0, MSG_DICTIONARY, SMSG_DICTGET, buf, (uint32_t)bufsz, &wfr);
	if (res != 0) {
		logerr("failed to write request\n%N", dict->iop);
		MOS_ERROR(dict->iop, res, "failed to send request to server");
		goto bad;
	}

	res = waitForReply(wfr);
	if (res != EPHIDGET_OK) {
		logerr("failed to receive reply from server");
		MOS_ERROR(dict->iop, res, "failed to receive reply from server");
		goto bad;
	}

	/* error occured */
	if (wfr->req.nr_type == MSG_COMMAND) {
		/*
		 * We already have the reply, this will just parse the boiler plate
		 * JSON and give us the result code sent from the client.
		 */
		res = simpleWaitForReply(&wfr, &rres);
		if (res == EPHIDGET_OK)
			res = rres;
		MOS_ERROR(dict->iop, res, "server side error");
		goto bad;
	}

	/* Should get MSG_DICTIONARY/SMSG_REPLY */
	if (wfr->req.nr_type != MSG_DICTIONARY || wfr->req.nr_stype != SMSG_REPLY) {
		logerr("invalid message type in reply from server %d/%d", wfr->req.nr_type, wfr->req.nr_stype);
		res = EPHIDGET_UNEXPECTED;
		MOS_ERROR(dict->iop, res, "invalid message type in reply from server %d/%d",
		  wfr->req.nr_type, wfr->req.nr_stype);
		goto bad;
	}

	/* copy directly into the user buffer */
	res = getKeyValue(dict->iop, &wfr->req, ns, sizeof(ns), buf, sizeof(buf), val, valsz,
	  act, sizeof(act));
	closeWaitForReply(&wfr);
	if (res != 0) {
		logerr("failed to get values from reply\n%N", dict->iop);
		MOS_ERROR(dict->iop, res, "failed to get values from server reply");
		goto bad;
	}

	mos_iop_release(&dict->iop);

	return (0);

bad:

	if (wfr)
		closeWaitForReply(&wfr);

	return (res);
}

static PhidgetReturnCode
PhidgetDictionary__getNamespace(PhidgetDictionaryHandle dict, char *buf, size_t bufsz) {

	if (dict->namespace == NULL) {
		buf[0] = '\0';
		return (EPHIDGET_OK);
	}

	mos_strlcpy(buf, dict->namespace, bufsz);
	return (EPHIDGET_OK);
}

static PhidgetReturnCode
PhidgetDictionary__getLastError(PhidgetDictionaryHandle dict, char *buf, size_t bufsz) {

	if (dict->iop == NULL) {
		mos_strlcpy(buf, "no error reported", bufsz);
		return (EPHIDGET_OK);
	}

	mos_snprintf(buf, bufsz, "%N", dict->iop);
	mos_iop_release(&dict->iop);
	return (EPHIDGET_OK);
}

static PhidgetReturnCode
PhidgetDictionary__setNamespace(PhidgetDictionaryHandle dict, const char *name, int create) {
	char buf[DICTIONARY_PATHMAX + 5];
	PhidgetReturnCode rres, res;
	WaitForReply *wfr;
	size_t bufsz;
	int flags;

	if (dict->iop)
		mos_iop_release(&dict->iop);
	dict->iop = mos_iop_alloc();

	if (dict->open == 0)
		return (MOS_ERROR(dict->iop, EPHIDGET_CLOSED, "handle is not open"));

	bufsz = sizeof(buf);
	res = setKeyValue(dict->iop, buf, &bufsz, name, "", "", "");
	if (res != 0) {
		logerr("failed to create key value packet\n%N", dict->iop);
		return (MOS_ERROR(dict->iop, res, "failed to create key value packet"));
	}

	if (create)
		flags = NRFDICT_PERSIST;	/* override the persist flag to mean create if necessary */
	else
		flags = 0;

	res = writeRequest(dict->iop, dict->nc, flags, MSG_DICTIONARY, SMSG_DICTSETNS, buf,
	  (uint32_t)bufsz, &wfr);
	if (res != 0) {
		logerr("failed to write request\n%N", dict->iop);
		return (MOS_ERROR(dict->iop, res, "failed to send request to server"));
	}

	res = simpleWaitForReply(&wfr, &rres);
	if (res == EPHIDGET_OK && rres == EPHIDGET_OK) {
		mos_free(dict->namespace, mos_strlen(dict->namespace) + 1);
		dict->namespace = mos_strdup(name, NULL);
		mos_iop_release(&dict->iop);
		return (0);
	}

	if (res != EPHIDGET_OK)
		return (MOS_ERROR(dict->iop, res, "failed to receive server reply"));
	return (MOS_ERROR(dict->iop, rres, "server indicated an error"));
}

static PhidgetReturnCode
PhidgetDictionary__openRemote(PhidgetDictionaryHandle dict, const char *addr, int port, const char *passwd) {
	PhidgetReturnCode res;

	TESTPTR(dict);
	TESTPTR(addr);
	TESTPTR(passwd);

	if (dict->iop)
		mos_iop_release(&dict->iop);
	dict->iop = mos_iop_alloc();

	res = clientConnect(AF_INET, addr, port, passwd, DICTSRV_PROTO, DICTSRV_PROTO_MAJOR, DICTSRV_PROTO_MINOR,
	  handleDictionaryRequest, &dict->nc);
	if (res != 0) {
		logerr("failed to connect to dictionary server '%s:%d [%s]'", addr, port, mos_notice_name(res));
		return (MOS_ERROR(dict->iop, res, "failed to connect to dictionary server '%s:%d [%s]'",
			addr, port, mos_notice_name(res)));
	}

	MOS_ASSERT(dict->nc->private == NULL);
	dict->nc->private = dict;
	mos_iop_release(&dict->iop);

	dict->open = 1;

	return (EPHIDGET_OK);
}

static PhidgetReturnCode
PhidgetDictionary__close(PhidgetDictionaryHandle dict) {
	PhidgetReturnCode res;
	WaitForReply *wfr;

	TESTPTR(dict);
	if (dict->open == 0)
		return (0);

	res = writeRequest(MOS_IOP_IGNORE, dict->nc, 0, MSG_DICTIONARY, SMSG_CLOSECONN, "", 0, &wfr);
	if (res == 0)
		simpleWaitForReply(&wfr, &res);

	stopAndWaitForPhidgetNetConnThread(dict->nc);
	PhidgetNetConnClose(dict->nc);
	PhidgetRelease(&dict->nc);

	return (0);
}

/** PUBLIC API **/

API_PRETURN_HDR
PhidgetDictionary_setOnFilterHandler(PhidgetDictionaryHandle dict, PhidgetDictionary_OnChangeCallback handler,
  void *ctx, const char *pattern) {
	PhidgetReturnCode res;

	TESTPTR(dict);
	TESTPTR(pattern);
	TESTPTR(handler);

	mos_mutex_lock(&dict->execlock);
	res = PhidgetDictionary__addFilter(dict, handler, ctx, pattern);
	mos_mutex_unlock(&dict->execlock);

	return (res);
}

API_PRETURN_HDR
PhidgetDictionary_removeFilterHandler(PhidgetDictionaryHandle dict, const char *pattern) {
	PhidgetReturnCode res;

	TESTPTR(dict);
	TESTPTR(pattern);

	mos_mutex_lock(&dict->execlock);
	res = PhidgetDictionary__removeFilterHandler(dict, pattern);
	mos_mutex_unlock(&dict->execlock);

	return (res);
}

API_PRETURN
PhidgetDictionary_add(PhidgetDictionaryHandle dict, const char *key, const char *val, int persist) {
	PhidgetReturnCode res;

	TESTPTR(dict);
	TESTPTR(key);
	TESTPTR(val);

	mos_mutex_lock(&dict->execlock);
	res = PhidgetDictionary__add(dict, key, val, persist);
	mos_mutex_unlock(&dict->execlock);

	return (res);
}

API_PRETURN_HDR
PhidgetDictionary_update(PhidgetDictionaryHandle dict, const char *key, const char *val, int persist, int add) {
	PhidgetReturnCode res;

	TESTPTR(dict);
	TESTPTR(key);
	TESTPTR(val);

	mos_mutex_lock(&dict->execlock);
	res = PhidgetDictionary__update(dict, key, val, persist);
	if(add && res == EPHIDGET_NOENT)
		res = PhidgetDictionary__add(dict, key, val, persist);
	mos_mutex_unlock(&dict->execlock);

	return (res);
}

API_PRETURN
PhidgetDictionary_remove(PhidgetDictionaryHandle dict, const char *key) {
	PhidgetReturnCode res;

	TESTPTR(dict);
	TESTPTR(key);

	mos_mutex_lock(&dict->execlock);
	res = PhidgetDictionary__remove(dict, key);
	mos_mutex_unlock(&dict->execlock);

	return (res);
}

API_PRETURN
PhidgetDictionary_get(PhidgetDictionaryHandle dict, const char *key, char *val, size_t valsz) {
	PhidgetReturnCode res;

	TESTPTR(dict);
	TESTPTR(key);
	TESTPTR(val);

	mos_mutex_lock(&dict->execlock);
	res = PhidgetDictionary__get(dict, key, val, valsz);
	mos_mutex_unlock(&dict->execlock);

	return (res);
}

API_PRETURN
PhidgetDictionary_getNamespace(PhidgetDictionaryHandle dict, char *buf, size_t bufsz) {
	PhidgetReturnCode res;

	TESTPTR(dict);
	TESTPTR(buf);

	mos_mutex_lock(&dict->execlock);
	res = PhidgetDictionary__getNamespace(dict, buf, bufsz);
	mos_mutex_unlock(&dict->execlock);

	return (res);
}

API_PRETURN
PhidgetDictionary_setNamespace(PhidgetDictionaryHandle dict, const char *name, int create) {
	PhidgetReturnCode res;

	TESTPTR(dict);
	TESTPTR(name);

	mos_mutex_lock(&dict->execlock);
	res = PhidgetDictionary__setNamespace(dict, name, create);
	mos_mutex_unlock(&dict->execlock);

	return (res);
}

API_PRETURN
PhidgetDictionary_getLastError(PhidgetDictionaryHandle dict, char *buf, size_t bufsz) {
	PhidgetReturnCode res;

	TESTPTR(dict);
	TESTPTR(buf);

	mos_mutex_lock(&dict->execlock);
	res = PhidgetDictionary__getLastError(dict, buf, bufsz);
	mos_mutex_unlock(&dict->execlock);

	return (res);
}

API_PRETURN
PhidgetDictionary_openRemote(PhidgetDictionaryHandle dict, const char *addr, int port, const char *passwd) {
	PhidgetReturnCode res;

	TESTPTR(dict);
	TESTPTR(addr);

	if (passwd == NULL)
		passwd = "";

	mos_mutex_lock(&dict->execlock);
	res = PhidgetDictionary__openRemote(dict, addr, port, passwd);
	mos_mutex_unlock(&dict->execlock);

	return (res);
}

API_PRETURN
PhidgetDictionary_close(PhidgetDictionaryHandle dict) {
	PhidgetReturnCode res;

	TESTPTR(dict);

	mos_mutex_lock(&dict->execlock);
	res = PhidgetDictionary__close(dict);
	mos_mutex_unlock(&dict->execlock);

	return (res);
}

API_PRETURN
PhidgetDictionary_create(PhidgetDictionaryHandle *dict) {

	TESTPTR(dict);

	*dict = mos_zalloc(sizeof(struct PhidgetDictionary));
	(*dict)->magic = DICTIONARY_MAGIC;
	(*dict)->namespace = mos_strdup("", NULL);
	RB_INIT(&(*dict)->filters);
	mos_mutex_init(&(*dict)->filterslock);
	mos_mutex_init(&(*dict)->execlock);

	return (0);
}

API_PRETURN
PhidgetDictionary_delete(PhidgetDictionaryHandle *dict) {
	struct changeFilter *a, *b;

	TESTPTR(dict);

	if ((*dict)->iop)
		mos_iop_release(&(*dict)->iop);

	mos_mutex_lock(&(*dict)->filterslock);
	RB_FOREACH_SAFE(a, filters, &(*dict)->filters, b)
		removeFilterHandler(MOS_IOP_IGNORE, *dict, a->cf_pattern);
	mos_mutex_unlock(&(*dict)->filterslock);

	if ((*dict)->nc) {
		PhidgetNetConnClose((*dict)->nc);
		PhidgetRelease(&(*dict)->nc);
	}
	mos_free((*dict)->namespace, mos_strlen((*dict)->namespace) + 1);
	mos_mutex_destroy(&(*dict)->filterslock);
	mos_mutex_destroy(&(*dict)->execlock);
	mos_free(*dict, sizeof(PhidgetDictionary));
	*dict = NULL;

	return (0);
}
