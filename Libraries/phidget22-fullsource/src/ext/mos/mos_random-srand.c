#ifdef Windows
#define _CRT_RAND_S
#endif

#include "mos_os.h"
#include "mos_random.h"

#include <time.h>

struct mosrandom {
	int seed;
};

MOSAPI int MOSCConv
mosrandom_alloc(mosiop_t iop, const uint8_t *seed, size_t seedlen,
  mosrandom_t **randp) {

	*randp = mos_malloc(sizeof (*randp));

	/* take first byte of seed */
	if (seedlen > 0)
		(*randp)->seed = seed[0];
	else
		(*randp)->seed = time(NULL);

	srand((*randp)->seed);

	return (0);
}

MOSAPI int MOSCConv
mosrandom_getbytes(mosrandom_t *state, mosiop_t iop, uint8_t *buf, size_t len) {
	unsigned int count = 0;
	unsigned int number;

	while (count < len) {

		number = rand();

		/* unpack to byte stream */
		buf[count++] = (uint8_t)((number >> 8) & 0xff);
		if (count > len)
			break;
		buf[count++] = (uint8_t)(number & 0xff);
	}

	return (0);
}

MOSAPI void MOSCConv
mosrandom_free(mosrandom_t **randp) {

	mos_free(*randp, sizeof (*randp));

	*randp = NULL;
}
