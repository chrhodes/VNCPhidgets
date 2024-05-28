#include "mos_os.h"
#include "mos_lock.h"
#include "mos_assert.h"

void
_mos_lock_init(void) {
}

void
_mos_lock_fini(void) {
}

MOSAPI int MOSCConv
mos_namedlock_init(mos_namedlock_t **_lk, const char *name, int flags) {
	mos_namedlock_t *lk;

	if (_lk == NULL)
		return (MOSN_FAULT);

	lk = mos_malloc(sizeof (mos_namedlock_t));
	lk->handle = CreateMutexA(NULL, FALSE, name);
	if (lk->handle == NULL)
		return (mos_windows_error_to_err(GetLastError()));

	lk->name = mos_strdup(name, NULL);
	lk->locked = 0;

	*_lk = lk;

	return (0);
}

MOSAPI int MOSCConv
mos_namedlock_fini(mos_namedlock_t **_lk) {
	mos_namedlock_t *lk;

	if (_lk == NULL)
		return (MOSN_FAULT);
	lk = *_lk;
	if (lk == NULL)
		return (0);

	MOS_ASSERT(lk->locked == 0);
	mos_free(lk->name, MOSM_FSTR);
	CloseHandle(lk->handle);
	mos_free(lk, sizeof (*lk));

	*_lk = NULL;
	return (0);
}

MOSAPI int MOSCConv
mos_namedlock_lock(mos_namedlock_t *lk, uint64_t ns) {
	DWORD res;

	MOS_ASSERT(lk->locked == 0);

	res = WaitForSingleObject(lk->handle, ns / 1000000);
	switch (res) {
	case WAIT_OBJECT_0: /* FALL THROUGH */
	case WAIT_ABANDONED: /* owner died without unlocking */
		lk->locked = 1;
		return (0);
	case WAIT_TIMEOUT:
		return (MOSN_TIMEDOUT);
	case WAIT_FAILED: /* FALL THROUGH */
	default:
		return (mos_windows_error_to_err(GetLastError()));
	}
}

MOSAPI int MOSCConv
mos_namedlock_unlock(mos_namedlock_t *lk) {

	MOS_ASSERT(lk->locked == 1);

	if (lk == NULL)
		return (MOSN_FAULT);

	ReleaseMutex(lk->handle);
	lk->locked = 0;

	return (0);
}

MOSAPI int MOSCConv
mos_mutex_init(mos_mutex_t *ml) {

	InitializeCriticalSection(ml);
	return (0);
}

MOSAPI void MOSCConv
mos_mutex_destroy(mos_mutex_t *ml) {

	DeleteCriticalSection(ml);
}

MOSAPI void MOSCConv
mos_mutex_lock(mos_mutex_t *ml) {

	EnterCriticalSection(ml);
	if (ml->RecursionCount != 1) {
		fprintf(stderr, "lock %p failed: recursive mutex enter\n", ml);
		assert(ml->RecursionCount == 1);
		LeaveCriticalSection(ml);
		MOS_PANIC("recursive mutex");
	}
}

MOSAPI void MOSCConv
mos_mutex_unlock(mos_mutex_t *ml) {
//	assert(ml->OwningThread == (HANDLE)(uintptr_t)GetCurrentThreadId()
//		&& ml->RecursionCount == 1);
	LeaveCriticalSection(ml);
}

MOSAPI int MOSCConv
mos_mutex_trylock(mos_mutex_t *ml) {

	if (TryEnterCriticalSection(ml) == 0)
		return (MOSN_BUSY);
	return (0);
}

MOSAPI int MOSCConv
mos_cond_init(mos_cond_t *mc) {

	mc->mc_nwaiters = 0;
	mc->mc_generation = 0;
	mc->mc_release_count = 0;

	InitializeCriticalSection(&mc->mc_nwaiters_lock);

	/* create a manual-reset event */
	mc->mc_event = CreateEvent(NULL, TRUE, FALSE, NULL);
	MOS_ASSERT(mc->mc_event != NULL);

	return (0);
}

MOSAPI void MOSCConv
mos_cond_destroy(mos_cond_t *mc) {

	DeleteCriticalSection(&mc->mc_nwaiters_lock);
	CloseHandle(mc->mc_event);
}

static int
_mos_cond_timedwait(mos_cond_t *mc, mos_mutex_t *ml, DWORD timeout_ms) {
	int my_generation;
	int done;
	int res;

	/* avoid race conditions */
	EnterCriticalSection(&mc->mc_nwaiters_lock);
	mc->mc_nwaiters++;
	my_generation = mc->mc_generation;
	LeaveCriticalSection(&mc->mc_nwaiters_lock);

	/* relinquish caller's mutex while we wait */
	LeaveCriticalSection(ml);

	for (;;) {
		/*
		 * If the condvar was signalled, the event will still be set and we
		 * will fall through this call.
		 */
		res = WaitForSingleObject(mc->mc_event, timeout_ms);

		EnterCriticalSection(&mc->mc_nwaiters_lock);

		/*
		 * While we were unlocked, the condvar was signalled, so break.
		 */
		done = mc->mc_release_count > 0 && mc->mc_generation !=
		  my_generation;

		/*
		 * If we timed out or were signalled, we are done.
		 */
		if (done || res == WAIT_TIMEOUT)
			break;

		LeaveCriticalSection(&mc->mc_nwaiters_lock);

		/*
		 * Detect when we've been woken up for a signal that happened
		 * before our generation.  We'll spin until the other waiter
		 * decrements the release count, and we think that's OK.
		 *
		 * We give up the CPU now that we've dropped the lock, and give
		 * the other waiters a chance to see the signalled event.  If we
		 * don't do this, we often can get the lock again, starving the
		 * other waiter for seconds at a time.
		 */
		if (mc->mc_release_count > 0 && !done)
			mos_yield();
	}

	/* We are waiting, so this better not be 0 */
	MOS_ASSERT(mc->mc_nwaiters > 0);
	mc->mc_nwaiters--;

	/*
	 * If we were signalled, decrement the release count
	 */
	if (done)
		mc->mc_release_count--;

	/* we're the last waiter to be notified, so clear the event */
	if (mc->mc_release_count == 0)
		ResetEvent(mc->mc_event);

	LeaveCriticalSection(&mc->mc_nwaiters_lock);
	EnterCriticalSection(ml);

	return (res);
}

MOSAPI void MOSCConv
mos_cond_wait(mos_cond_t *mc, mos_mutex_t *ml) {

	_mos_cond_timedwait(mc, ml, INFINITE);
}

MOSAPI int MOSCConv
mos_cond_timedwait(mos_cond_t *mc, mos_mutex_t *ml, uint64_t nsec) {
	int res;

	res = _mos_cond_timedwait(mc, ml, (DWORD)(nsec / 1000000));
	if (res == WAIT_TIMEOUT)
		return (MOSN_TIMEDOUT);
	return (0);
}

MOSAPI void MOSCConv
mos_cond_signal(mos_cond_t *mc) {

	EnterCriticalSection(&mc->mc_nwaiters_lock);
	if (mc->mc_nwaiters > mc->mc_release_count) {
		/* signal manual-release event */
		SetEvent(mc->mc_event);
		mc->mc_release_count++;
		mc->mc_generation++;
	}
	LeaveCriticalSection(&mc->mc_nwaiters_lock);
}

MOSAPI void MOSCConv
mos_cond_broadcast(mos_cond_t *mc) {

	EnterCriticalSection(&mc->mc_nwaiters_lock);
	if (mc->mc_nwaiters > 0) {
		SetEvent(mc->mc_event);
		/* release all threads in this generation */
		mc->mc_release_count = mc->mc_nwaiters;
		mc->mc_generation++;
	}
	LeaveCriticalSection(&mc->mc_nwaiters_lock);
}

MOSAPI void MOSCConv
mos_yield(void) {

	/*
	 * A value of zero causes the thread to relinquish the remainder of its time slice to any
	 * other thread that is ready to run. If there are no other threads ready to run, the function
	 * returns immediately, and the thread continues execution.
	 */
	Sleep(0);
}

MOSAPI int MOSCConv
mos_mutex_lockedbyme(mos_mutex_t *ml) {
	//It's locked, and by me
	return (ml->OwningThread == (HANDLE)(uintptr_t)GetCurrentThreadId()
		&& ml->RecursionCount == 1);
}
