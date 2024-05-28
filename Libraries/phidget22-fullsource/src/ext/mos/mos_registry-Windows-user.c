#include "mos_os.h"
#include "mos_assert.h"

/*
 * Caller must free value with mos_free().
 */
MOSAPI int MOSCConv
mos_registry_getmachinevalue(mosiop_t iop, const char *key, const char *value,
  void **bufp, uint32_t *buflenp, uint32_t *dataoffp, uint32_t *datalenp) {
	uint32_t buflen;
	DWORD datalen;
	void *buf;
	HKEY h;
	LONG l;

	/*
	 * Open the registry for reading, and do not translate to the
	 * 32-bit registry view.
	 *
	 * KEY_WOW_64KEY tells the function that on 64-bit windows it should allow
	 * for 64-bit values to exist in the 64-bit area.
	 *
	 * KEY_WOW_32KEY tells the function that on 64-bit windows it should allow
	 * for 32-bit values to exist in the 64-bit area.
	 *
	 * On 32-bit windows these flags have no effect and the first call is
	 * generally expected to succeed.
	 *
	 * On 64-bit windows, the first call will generally succeed, but in certain
	 * cases we need to fall back to the second call.  Most of our registry
	 * entries are considered "32-bit", so we want the function to look for
	 * those 32-bit values in the 64-bit area, where our installer puts them.
	 * In the case of the pubkey or a license installed via the GUI, these are
	 * 64-bit values also in the 64-bit area and on the failure of the first
	 * call we will fall back to the KEY_WOW64_64KEY call.
	 *
	 * We want to call the KEY_WOW64_64KEY version first because that will be
	 * the absolute native call on both platforms.  On 64-bit, we only want to
	 * fall back to 32-bit entries in the fail case because if a 64-bit value is
	 * available (such as the DigitalProductID requested by the licensing
	 * display), we want to default to the 64-bit value.
	 */
	l = RegOpenKeyExA(HKEY_LOCAL_MACHINE, key, 0, KEY_QUERY_VALUE |
	  KEY_WOW64_64KEY, &h);
	if (l != ERROR_SUCCESS) {
		l = RegOpenKeyExA(HKEY_LOCAL_MACHINE, key, 0, KEY_QUERY_VALUE |
		  KEY_WOW64_32KEY, &h);
		if (l != ERROR_SUCCESS) {
			return (MOS_ERROR(iop, mos_windows_error(iop, l),
			  "failed to open key '%s' [System Error Code: %d]", key, l));
		}
	}

	buflen = 256;

again:

	buf = mos_alloc(buflen, MOSM_PG | MOSM_SLP);
	datalen = buflen;
	l = RegQueryValueExA(h, value, NULL, NULL, buf, &datalen);
	if (l == ERROR_MORE_DATA) {
		mos_free(buf, buflen);
		MOS_ASSERT(datalen > buflen);
		/* datalen tells us how big the buffer needs to be */
		buflen = datalen;
		goto again;
	}
	RegCloseKey(h);
	if (l != ERROR_SUCCESS)
		return (MOS_ERROR(iop, mos_windows_error(iop, l),
		  "failed to get value '%s' [System Error Code: %d]", value, l));

	*bufp = buf;
	*buflenp = buflen;
	*dataoffp = 0;
	*datalenp = datalen;

	return (0);
}

/*
 * Caller must free value with mos_free().
 */
MOSAPI int MOSCConv
mos_registry_setmachinevalue(mosiop_t iop, const char *key, const char *value,
  const void *buf, uint32_t buflen) {
	HKEY h;
	LONG l;

	/*
	 * Open the registry for reading, and do not translate to the
	 * 32-bit registry view.
	 *
	 * See above for details.
	 */
	l = RegOpenKeyExA(HKEY_LOCAL_MACHINE, key, 0, KEY_SET_VALUE |
	  KEY_WOW64_64KEY, &h);
	if (l != ERROR_SUCCESS) {
		l = RegOpenKeyExA(HKEY_LOCAL_MACHINE, key, 0, KEY_SET_VALUE |
		  KEY_WOW64_32KEY, &h);
		if (l != ERROR_SUCCESS) {
			return (MOS_ERROR(iop, mos_windows_error(iop, l),
			  "failed to open key '%s' [System Error Code: %d]", key, l));
		}
	}

	l = RegSetValueExA(h, value, 0, REG_BINARY, buf, buflen);
	RegCloseKey(h);
	if (l != ERROR_SUCCESS)
		return (MOS_ERROR(iop, mos_windows_error(iop, l),
		  "failed to get value '%s' [System Error Code: %d]", value, l));

	return (0);
}

static int
iswideascii(const char *dbuf, uint32_t dbufsz) {
	uint32_t n;

	if (dbufsz == 0)
		return (0);

	for (n = 0; n < dbufsz - 1; n += 2)
		if (dbuf[n + 1] != '\0')
			return (0);

	return (1);
}

/*
 * Retrieves a numeric value from the registry.  Uses the given default
 * if there is no entry, otherwise returns an error (e.g. couldn't access
 * registry, or parse value).
 */
int
mos_registry_getu32(mosiop_t iop, const char *key, const char *val,
  uint32_t *resp, uint32_t def) {
	uint32_t regbuflen;
	uint32_t datalen;
	uint32_t dataoff;
	char *regbuf;
	uint32_t n;
	int err;

	uint32_t numstrsz;
	char *numstr;

	err = mos_registry_getmachinevalue(NULL, key, val, &regbuf, &regbuflen,
	  &dataoff, &datalen);
	if (err == MOSN_NOENT) {
		*resp = def;
		return (0);
	}
	if (err != 0)
		return (MOS_ERROR(iop, err,
		  "failed to get registry entry for %s\\%s", key, val));
	if (datalen == 0) {
		if (regbuflen > 0)
			mos_free(regbuf, regbuflen);
		return (MOS_ERROR(iop, MOSN_INVAL, "empty value (registry %s\\%s)", key,
		  val));
	}

	/*
	 * The registry interfaces aren't good about telling us when wide
	 * characters have been used, so find out for ourselves.
	 */
	if (iswideascii(regbuf + dataoff, datalen)) {
		numstrsz = datalen / sizeof (uint16_t) + 1;
		numstr = mos_malloc(numstrsz);
		for (n = 0; n < datalen; n += 2)
			numstr[n / 2] = regbuf[dataoff + n];
		numstr[numstrsz - 1] = '\0';
	} else {
		/* NUL-terminate */
		numstrsz = datalen + 1;
		numstr = mos_malloc(numstrsz);
		memcpy(numstr, regbuf + dataoff, datalen);
		numstr[datalen] = '\0';
	}

	err = mos_strtou32(numstr, 0, resp);
	mos_free(regbuf, regbuflen);
	if (err != 0) {
		err = MOS_ERROR(iop, err,
		  "invalid numeric value '%s' from registry (%s\\%s)", numstr, key,
		  val);
		mos_free(numstr, numstrsz);
		return (err);
	}
	mos_free(numstr, numstrsz);

	return (0);
}
