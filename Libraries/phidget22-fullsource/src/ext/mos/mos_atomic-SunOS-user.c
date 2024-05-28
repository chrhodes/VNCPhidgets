#include <atomic.h>
#include "mos_atomic.h"

void
mos_atomic_add_32(uint32_t *dst, int32_t delta) {

	atomic_add_32(dst, delta);
}

uint32_t
mos_atomic_add_32_nv(uint32_t *dst, int32_t delta) {

	return (atomic_add_32_nv(dst, delta));
}

void
mos_atomic_add_64(uint64_t *dst, int64_t delta) {

	atomic_add_64(dst, delta);
}

uint64_t
mos_atomic_add_64_nv(uint64_t *dst, int64_t delta) {

	return (atomic_add_64_nv(dst, delta));
}

uint32_t
mos_atomic_get_32(uint32_t *dst) {

	return (atomic_add_32_nv(dst, 0));
}

uint64_t
mos_atomic_get_64(uint64_t *dst) {

	return (atomic_add_64_nv(dst, 0LL));
}

uint32_t
mos_atomic_swap_32(uint32_t *dst, uint32_t nv) {

	return (atomic_swap_32(dst, nv));
}

uint64_t
mos_atomic_swap_64(uint64_t *dst, uint64_t nv) {

	return (atomic_swap_64(dst, nv));
}

void
_mos_atomic_init(void) {
}
