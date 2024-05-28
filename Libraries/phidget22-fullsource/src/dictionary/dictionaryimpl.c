/*
 * The protocol is a simple kv protocol, with a flag, len, [val], ...
 *
 * uint8_t flag
 * uint16_t len
 * uint8_t[len]
 * ...
 */

#include "phidgetbase.h"
#include "dictionary/phidgetdictionary.h"
#include "dictionary/dictionaryimpl.h"

#include "mos/mos_assert.h"

int
dictentcompare(dictent_t *a, dictent_t *b) {

	return (strcmp(a->de_key, b->de_key));
}

int
dictionarycompare(dictionary_t *a, dictionary_t *b) {

	return (strcmp(a->dc_key, b->dc_key));
}

RB_GENERATE(dictionary, _dictent, de_tlink, dictentcompare)
RB_GENERATE(dictionaries, _dictionary, dc_link, dictionarycompare)

static void freedictent(dictent_t **);

void
createdictionary(const char *name, dictionary_t **dict) {

	*dict = mos_malloc(sizeof(dictionary_t));
	mos_mutex_init(&(*dict)->dc_lock);
	(*dict)->dc_key = mos_strdup(name, NULL);
	RB_INIT(&(*dict)->dc_tree);
}

void
destroydictionary(dictionary_t **dict) {
	dictent_t *a, *b;

	RB_FOREACH_SAFE(a, dictionary, &(*dict)->dc_tree, b)
		releasedictent(&a, 0);

	mos_mutex_destroy(&(*dict)->dc_lock);
	mos_free((*dict)->dc_key, mos_strlen((*dict)->dc_key) + 1);
	mos_free(*dict, sizeof(dictionary_t));
}

void
releasefromdictionarybyowner(const char *ns, dictionary_t *dict, uint32_t owner, derelease_t releasing) {
	dictent_t *a, *b;

	mos_mutex_lock(&dict->dc_lock);
	RB_FOREACH_SAFE(a, dictionary, &dict->dc_tree, b) {
		if (a->de_owner == owner) {
			if (a->de_flags & DICTIONARY_PERSIST)
				continue;
			releasing(ns, a);
			releasedictent(&a, 1);
		}
	}
	mos_mutex_unlock(&dict->dc_lock);
}

static void
freedictent(dictent_t **de) {

	mos_free((*de)->de_key, mos_strlen((*de)->de_key) + 1);
	if ((*de)->de_val)
		mos_free((*de)->de_val, mos_strlen((*de)->de_val) + 1);
	mos_mutex_destroy(&(*de)->de_lock);
	mos_free(*de, sizeof(dictent_t));
	*de = NULL;
}

void
releasedictent(dictent_t **_de, int dictlocked) {
	dictent_t *de;

	if (_de == NULL || *_de == NULL)
		return;

	de = *_de;
	*_de = NULL;

	mos_mutex_lock(&de->de_lock);
	de->de_refcnt--;
	MOS_ASSERT(de->de_refcnt >= 0);

	if (de->de_refcnt != 0) {
		mos_mutex_unlock(&de->de_lock);
		return;
	}

	if (!dictlocked)
		mos_mutex_lock(&de->de_dictionary->dc_lock);
	MOS_ASSERT(de->de_dictionary->dc_count > 0);
	RB_REMOVE(dictionary, &de->de_dictionary->dc_tree, de);
	de->de_dictionary->dc_count--;

	if (!dictlocked)
		mos_mutex_unlock(&de->de_dictionary->dc_lock);

	mos_mutex_unlock(&de->de_lock);
	freedictent(&de);
}

static void
allocdictent(dictent_t **de, dictionary_t *dict, uint32_t owner, int flags, const char *key,
  const char *val) {

	*de = mos_zalloc(sizeof(dictent_t));
	(*de)->de_key = mos_strdup(key, NULL);
	if (val)
		(*de)->de_val = mos_strdup(val, NULL);
	(*de)->de_dictionary = dict;
	(*de)->de_refcnt = 1;
	(*de)->de_owner = owner;
	(*de)->de_flags = flags;
	mos_mutex_init(&(*de)->de_lock);
}

static void
updatedictent(dictent_t *de, uint32_t owner, int flags, const char *val) {
	if ((de)->de_val)
		mos_free((de)->de_val, mos_strlen((de)->de_val) + 1);
	if (val)
		(de)->de_val = mos_strdup(val, NULL);
	(de)->de_owner = owner;
	(de)->de_flags = flags;
}

int
adddictentry(mosiop_t iop, dictionary_t *dict, uint32_t owner, int flags, const char *key, const char *val,
  dictent_t **_de) {
	dictent_t *de, *det;
	size_t n;

	if (key == NULL)
		return (MOS_ERROR(iop, MOSN_INVALARG, "key is null"));

	if ((n = mos_strlen(key)) >= DICTIONARY_PATHMAX)
		return (MOS_ERROR(iop, MOSN_INVAL, "key is too long [%d]", n));
	if (val && (n = mos_strlen(val)) >= DICTIONARY_VALMAX)
		return (MOS_ERROR(iop, MOSN_INVAL, "value is too long [%d]", n));

	allocdictent(&de, dict, owner, flags, key, val);
	mos_mutex_lock(&dict->dc_lock);
	det = RB_INSERT(dictionary, &dict->dc_tree, de);
	if (!det)
		dict->dc_count++;
	mos_mutex_unlock(&dict->dc_lock);
	if (det) {
		freedictent(&de);
		return MOSN_EXIST;
	}

	if (_de) {
		mos_mutex_lock(&de->de_lock);
		de->de_refcnt++;
		mos_mutex_unlock(&de->de_lock);
		*_de = de;
	}

	return (0);
}

/*
 * requires tree lock to be held.
 */
static void
getdictent(dictionary_t *dict, const char *key, regex_t *regex, dictent_t **de) {
	dictent_t skey;

	if (key) {
		skey.de_tkey.ckey = key;
		*de = RB_FIND(dictionary, &dict->dc_tree, &skey);
		return;
	}

	RB_FOREACH(*de, dictionary, &dict->dc_tree) {
		if (regexec(regex, (*de)->de_key, 0, NULL, 0) == 0)
			return;
	}
	*de = NULL;
}

int
updatedictentry(mosiop_t iop, dictionary_t *dict, uint32_t owner, int flags, const char *key, const char *val,
	dictent_t **_de) {
	dictent_t *de;
	size_t n;

	if (key == NULL)
		return (MOS_ERROR(iop, MOSN_INVALARG, "key is null"));

	if ((n = mos_strlen(key)) >= DICTIONARY_PATHMAX)
		return (MOS_ERROR(iop, MOSN_INVAL, "key is too long [%d]", n));
	if (val && (n = mos_strlen(val)) >= DICTIONARY_VALMAX)
		return (MOS_ERROR(iop, MOSN_INVAL, "value is too long [%d]", n));

	mos_mutex_lock(&dict->dc_lock);
	getdictent(dict, key, NULL, &de);
	if (de)
		updatedictent(de, owner, flags, val);
	mos_mutex_unlock(&dict->dc_lock);

	if (!de)
		return (MOSN_NOENT);

	if (_de) {
		mos_mutex_lock(&de->de_lock);
		de->de_refcnt++;
		mos_mutex_unlock(&de->de_lock);
		*_de = de;
	}

	return (0);
}

int
deldictentry(mosiop_t iop, dictionary_t *dict, int flags, const char *key) {
	dictent_t *de;

	if (key == NULL)
		return (MOS_ERROR(iop, MOSN_INVALARG, "key is null"));

	mos_mutex_lock(&dict->dc_lock);
	getdictent(dict, key, NULL, &de);
	mos_mutex_unlock(&dict->dc_lock);
	if (de) {
		releasedictent(&de, 0);
		return (0);
	}
	return (MOSN_NOENT);
}

int
getdictentry_re(mosiop_t iop, dictionary_t *dict, const char *re, dictent_t **de) {
	regex_t regex;

	if (re == NULL)
		return (MOS_ERROR(iop, MOSN_INVALARG, "regular expression is null"));

	if (de == NULL)
		return (MOS_ERROR(iop, MOSN_INVALARG, "return pointer is null"));

	if (regcomp(&regex, re, REG_EXTENDED) != 0)
		return (MOS_ERROR(iop, MOSN_INVAL, "invalid regular expression"));

	mos_mutex_lock(&dict->dc_lock);
	getdictent(dict, NULL, &regex, de);
	regfree(&regex);
	if (*de) {
		(*de)->de_refcnt++;
		mos_mutex_unlock(&dict->dc_lock);
		return (0);
	}

	mos_mutex_unlock(&dict->dc_lock);
	return (MOSN_NOENT);
}

int
getdictentry(mosiop_t iop, dictionary_t *dict, const char *key, dictent_t **de) {

	if (key == NULL)
		return (MOS_ERROR(iop, MOSN_INVALARG, "key is null"));

	if (de == NULL)
		return (MOS_ERROR(iop, MOSN_INVALARG, "return pointer is null"));

	mos_mutex_lock(&dict->dc_lock);
	getdictent(dict, key, NULL, de);
	if (*de) {
		(*de)->de_refcnt++;
		mos_mutex_unlock(&dict->dc_lock);
		return (0);
	}

	mos_mutex_unlock(&dict->dc_lock);
	return (MOSN_NOENT);
}

/*
 * Utility code
 */
PhidgetReturnCode
setKeyValue(mosiop_t iop, char *buf, size_t *bufsz, const char *ns, const char *path, const char *val,
  const char *act) {
	size_t nlen;
	size_t plen;
	size_t vlen;
	size_t alen;
	char *p;

	nlen = mos_strlen(ns);
	plen = mos_strlen(path);
	vlen = mos_strlen(val);
	alen = mos_strlen(act);

	if (nlen > DICTIONARY_NAMESPACEMAX)
		return (MOS_ERROR(iop, EPHIDGET_INVALIDARG, "namespace length is too long"));
	if (plen > DICTIONARY_PATHMAX)
		return (MOS_ERROR(iop, EPHIDGET_INVALIDARG, "path length is too long"));
	if (vlen > DICTIONARY_VALMAX)
		return (MOS_ERROR(iop, EPHIDGET_INVALIDARG, "value length is too long"));
	if (alen > DICTIONARY_ACTMAX)
		return (MOS_ERROR(iop, EPHIDGET_INVALIDARG, "action length is too long"));

	if (*bufsz < nlen + plen + vlen + alen + 5)
		return (MOS_ERROR(iop, EPHIDGET_INVALIDARG, "result buffer is too small '%u' < '%u'",
			*bufsz, nlen + plen + vlen + 5));

	p = buf;
	*p = (uint8_t)nlen & 0xff;
	p++;
	memcpy(p, ns, nlen);
	p += nlen;

	*p = plen & 0xff;
	p++;
	memcpy(p, path, plen);
	p += plen;

	*p = vlen & 0xff;
	p++;
	*p = (vlen >> 8) & 0xff;
	p++;
	memcpy(p, val, vlen);
	p += vlen;

	*p = (uint8_t)alen;
	p++;
	memcpy(p, act, alen);

	*bufsz = nlen + plen + vlen + alen + 5;

	return (0);
}


PhidgetReturnCode
getKeyValue(mosiop_t iop, netreq_t *req, char *ns, size_t nslen, char *path, size_t pathlen,
  char *val, size_t vallen, char *act, size_t actlen) {
	uint16_t vlen;
	uint8_t nlen;
	uint8_t klen;
	uint8_t alen;
	uint8_t *p;

	if (req->nr_len < 6)
		return (MOS_ERROR(iop, EPHIDGET_INVALIDARG, "request is too short to hold key value pair"));

	p = req->nr_data;

	nlen = *p;
	p++;
	if (nlen > nslen)
		return (MOS_ERROR(iop, EPHIDGET_INVALIDARG, "namespace is too long '%u'", nlen));
	if ((uint32_t)nlen + 1 > req->nr_len)
		return (MOS_ERROR(iop, EPHIDGET_INVALIDARG, "request is too short to hold ns'%u' vs '%u'",
			nlen + 1, req->nr_len));

	memcpy(ns, p, nlen);
	ns[nlen] = '\0';
	p += nlen;

	klen = *p;
	p++;

	if (klen > pathlen)
		return (MOS_ERROR(iop, EPHIDGET_INVALIDARG, "key is too long '%u'", klen));
	if ((uint32_t)klen + 1 > req->nr_len)
		return (MOS_ERROR(iop, EPHIDGET_INVALIDARG, "request is too short to hold key'%u' vs '%u'",
			klen + 1, req->nr_len));

	memcpy(path, p, klen);
	path[klen] = '\0';
	p += klen;

	vlen = *p | (*(p + 1) << 8);
	p += 2;

	if (vlen > vallen)
		return (MOS_ERROR(iop, EPHIDGET_INVALIDARG, "value is too long '%u'", vlen));
	if ((uint32_t)klen + 1 + vlen + 2 > req->nr_len)
		return (MOS_ERROR(iop, EPHIDGET_INVALIDARG, "request is too short to hold key and value '%u' vs '%u'",
			klen + 1 + vlen + 2, req->nr_len));

	memcpy(val, p, vlen);
	val[vlen] = '\0';
	p += vlen;

	alen = *p;
	p++;

	if (alen > actlen)
		return (MOS_ERROR(iop, EPHIDGET_INVALIDARG, "action is too long '%u'", alen));
	if ((uint32_t)klen + 1 + vlen + 2 + alen + 1 > req->nr_len)
		return (MOS_ERROR(iop, EPHIDGET_INVALIDARG, "request is too short to hold key value and action "
			"'%u' vs '%u'", klen + 1 + vlen + 2 + alen + 1, req->nr_len));

	memcpy(act, p, alen);
	act[alen] = '\0';

	return (0);
}
