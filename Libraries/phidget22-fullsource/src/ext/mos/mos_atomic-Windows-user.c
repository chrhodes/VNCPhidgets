
#include "mos_atomic.h"

static CRITICAL_SECTION global_lock;
static int initialized;

void 
_mos_atomic_init(void) {

	if (!initialized) {
		InitializeCriticalSection(&global_lock);
		initialized = 1;
	}
}

MOSAPI void MOSCConv
mos_atomic_add_32(uint32_t *dst, int32_t delta) {

	EnterCriticalSection(&global_lock);
	*dst += delta;
	LeaveCriticalSection(&global_lock);
}

MOSAPI uint32_t MOSCConv
mos_atomic_add_32_nv(uint32_t *dst, int32_t delta) {
	uint32_t res;

	EnterCriticalSection(&global_lock);
	res = *dst + delta;
	*dst = res;
	LeaveCriticalSection(&global_lock);

	return (res);
}

MOSAPI void MOSCConv
mos_atomic_add_64(uint64_t *dst, int64_t delta) {

	EnterCriticalSection(&global_lock);
	*dst += delta;
	LeaveCriticalSection(&global_lock);
}

MOSAPI uint64_t MOSCConv
mos_atomic_add_64_nv(uint64_t *dst, int64_t delta) {
	uint64_t res;

	EnterCriticalSection(&global_lock);
	res = *dst + delta;
	*dst = res;
	LeaveCriticalSection(&global_lock);

	return (res);
}

MOSAPI uint32_t MOSCConv
mos_atomic_get_32(uint32_t *dst) {
	uint32_t res;

	EnterCriticalSection(&global_lock);
	res = *dst;
	LeaveCriticalSection(&global_lock);

	return (res);
}


MOSAPI uint64_t MOSCConv
mos_atomic_get_64(uint64_t *dst) {
	uint64_t res;

	EnterCriticalSection(&global_lock);
	res = *dst;
	LeaveCriticalSection(&global_lock);

	return (res);
}

MOSAPI uint32_t MOSCConv
mos_atomic_swap_32(uint32_t *dst, uint32_t nv) {
	uint32_t ov;

	EnterCriticalSection(&global_lock);
	ov = *dst;
	*dst = nv;
	LeaveCriticalSection(&global_lock);

	return (ov);
}

MOSAPI uint64_t MOSCConv
mos_atomic_swap_64(uint64_t *dst, uint64_t nv) {
	uint64_t ov;

	EnterCriticalSection(&global_lock);
	ov = *dst;
	*dst = nv;
	LeaveCriticalSection(&global_lock);

	return (ov);
}
