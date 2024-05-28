#include <libkern/OSAtomic.h>
#include "mos_atomic.h"

void
mos_atomic_add_32(uint32_t *dst, int32_t delta) {

	OSAtomicAdd32Barrier(delta, (int32_t *)dst);
}

uint32_t
mos_atomic_add_32_nv(uint32_t *dst, int32_t delta) {

	return ((uint32_t)OSAtomicAdd32Barrier(delta, (int32_t *)dst));
}

void
mos_atomic_add_64(uint64_t *dst, int64_t delta) {

	OSAtomicAdd64Barrier(delta, (int64_t *)dst);
}

uint64_t
mos_atomic_add_64_nv(uint64_t *dst, int64_t delta) {

	return ((uint64_t)OSAtomicAdd64Barrier(delta, (int64_t *)dst));
}

uint32_t
mos_atomic_get_32(uint32_t *dst) {


	return ((uint32_t)OSAtomicAdd32Barrier(0, (int32_t *)dst));
}

uint64_t
mos_atomic_get_64(uint64_t *dst) {

	return ((uint64_t)OSAtomicAdd64Barrier(0, (int64_t *)dst));
}

uint32_t
mos_atomic_swap_32(uint32_t *dst, uint32_t nv) {

	mos_glock(0);
	*dst = nv;
	mos_gunlock(0);

	return (*dst);
}

uint64_t
mos_atomic_swap_64(uint64_t *dst, uint64_t nv) {

	mos_glock(0);
	*dst = nv;
	mos_gunlock(0);

	return (*dst);
}

void
_mos_atomic_init(void) {
}
