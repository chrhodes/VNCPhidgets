#include "mos_os.h"
#include "mos_assert.h"
#include "mos_iop.h"
#include "mos_lock.h"
#include "mos_task.h"

MOSAPI int MOSCConv
mos_rwlock_init(mos_rwlock_t *rw) {

	rw->rw_nreader = 0;
	rw->rw_nreaderwaiting = 0;
	rw->rw_nwriter = 0;
	rw->rw_writer = MOS_TASK_NONE;
	mos_mutex_init(&rw->rw_lock);
	mos_cond_init(&rw->rw_rcond);
	mos_cond_init(&rw->rw_wcond);

	return (0);
}

MOSAPI void MOSCConv
mos_rwlock_destroy(mos_rwlock_t *rw) {

	MOS_ASSERT(rw->rw_nreader == 0 && rw->rw_nwriter == 0);

	mos_mutex_destroy(&rw->rw_lock);
	mos_cond_destroy(&rw->rw_rcond);
	mos_cond_destroy(&rw->rw_wcond);
}

MOSAPI void MOSCConv
mos_rwlock_rdlock(mos_rwlock_t *rw) {

	mos_mutex_lock(&rw->rw_lock);

	rw->rw_nreaderwaiting++;
	while (rw->rw_nwriter > 0)
		mos_cond_wait(&rw->rw_rcond, &rw->rw_lock);

	MOS_ASSERT(rw->rw_nreaderwaiting > 0);
	rw->rw_nreaderwaiting--;
	rw->rw_nreader++;

	mos_mutex_unlock(&rw->rw_lock);
}

MOSAPI void MOSCConv
mos_rwlock_wrlock(mos_rwlock_t *rw) {

	mos_mutex_lock(&rw->rw_lock);

	rw->rw_nwriter++;
	while (rw->rw_nreader > 0 || rw->rw_writer != MOS_TASK_NONE)
		mos_cond_wait(&rw->rw_wcond, &rw->rw_lock);

	rw->rw_writer = mos_self();

	mos_mutex_unlock(&rw->rw_lock);
}

MOSAPI int MOSCConv
mos_rwlock_trywrlock(mos_rwlock_t *rw) {
	int res;

	/* XXX untested */

	mos_mutex_lock(&rw->rw_lock);

	if (rw->rw_nreader == 0 && rw->rw_nreaderwaiting == 0 && rw->rw_nwriter ==
	  0) {
		MOS_ASSERT(rw->rw_writer == MOS_TASK_NONE);
		rw->rw_nwriter++;
		rw->rw_writer = mos_self();

		res = 0;
	} else {
		res = MOSN_BUSY;
	}

	mos_mutex_unlock(&rw->rw_lock);

	return (res);
}

MOSAPI int MOSCConv
mos_rwlock_tryrdlock(mos_rwlock_t *rw) {
	int res;

	/* XXX untested */

	mos_mutex_lock(&rw->rw_lock);

	if (rw->rw_nwriter == 0) {
		rw->rw_nreader++;
		res = 0;
	} else {
		res = MOSN_BUSY;
	}

	mos_mutex_unlock(&rw->rw_lock);

	return (res);
}

MOSAPI void MOSCConv
mos_rwlock_unlock(mos_rwlock_t *rw) {

	mos_mutex_lock(&rw->rw_lock);

	if (rw->rw_writer == mos_self()) {
		MOS_ASSERT(rw->rw_nwriter > 0);
		rw->rw_nwriter--;
		rw->rw_writer = MOS_TASK_NONE;
	} else { /* reader */
		MOS_ASSERT(rw->rw_nreader > 0);
		rw->rw_nreader--;
	}

	if (rw->rw_nwriter > 0)
		mos_cond_signal(&rw->rw_wcond);
	else if (rw->rw_nreaderwaiting > 0)
		mos_cond_broadcast(&rw->rw_rcond);

	mos_mutex_unlock(&rw->rw_lock);
}
