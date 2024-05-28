#include "mos_os.h"
#include <wincrypt.h>
#include "mos_random.h"

struct mosrandom {
	HCRYPTPROV provider;
};

MOSAPI int MOSCConv
mosrandom_alloc(mosiop_t iop, const uint8_t *seed, size_t seedlen,
  mosrandom_t **randp) {

	if (seedlen > 0)
		return (MOS_ERROR(iop, MOSN_INVAL,
		  "seedable source not implemented"));

	*randp = mos_malloc(sizeof (*randp));

	if (CryptAcquireContextW(&(*randp)->provider, NULL, NULL, PROV_RSA_FULL,
	  CRYPT_VERIFYCONTEXT) == FALSE) {
		mos_free(*randp, sizeof (*randp));
		return (MOS_ERROR(iop, mos_windows_error(iop, GetLastError()),
		  "failed to get crypto context"));
	}

	return (0);
}

MOSAPI void MOSCConv
mosrandom_free(mosrandom_t **randp) {
	size_t rsz;

	rsz = sizeof (*randp);

	CryptReleaseContext((*randp)->provider, 0);

	mos_free(*randp, rsz);

	*randp = NULL;
}

MOSAPI int MOSCConv
mosrandom_getbytes(mosrandom_t *this, mosiop_t iop, uint8_t *buf,
  size_t len) {

	if (!CryptGenRandom(this->provider, (DWORD)len, buf))
		return (MOS_ERROR(iop, mos_windows_error(iop, GetLastError()),
		  "failed to generate bytes"));

	return (0);
}
