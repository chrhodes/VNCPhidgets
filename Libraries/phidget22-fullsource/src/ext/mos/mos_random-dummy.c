#include "mos_os.h"
#include "mos_random.h"

/*
 * Presently, random numbers are only required by tools, and not used
 * in-kernel.  This implementation of the random interface simply
 * returns errors.
 */

MOSAPI int MOSCConv
mosrandom_getbytes(mosrandom_t *this, mosiop_t iop, uint8_t *tbuf,
  size_t len) {

	return (MOS_ERROR(iop, MOSN_NOSUP, "not implemented"));
}

MOSAPI int MOSCConv
mosrandom_alloc(mosiop_t iop, const uint8_t *seed, size_t seedlen,
  mosrandom_t **randp) {

	return (MOS_ERROR(iop, MOSN_NOSUP, "not implemented"));
}

MOSAPI void MOSCConv
mosrandom_free(mosrandom_t **randp) {
}
