#include "mos_os.h"
//#include <Mmsystem.h>	 /* timeGetTime() LEAN_AND_MEAN */
#include "mos_assert.h"
#include "mos_time.h"

MOSAPI int MOSCConv
mostimestamp_now(mostimestamp_t *mts) {
	struct tm ptm;
	time_t epoch;

	epoch = _time64(NULL);

	if (_gmtime64_s(&ptm, &epoch) != 0)
		return (-1);

	mts->mts_flags = 0;
	mts->mts_year = 1900 + ptm.tm_year;
	mts->mts_month = ptm.tm_mon + 1;
	mts->mts_day = ptm.tm_mday;
	mts->mts_hour = ptm.tm_hour;
	mts->mts_minute = ptm.tm_min;
	mts->mts_second = ptm.tm_sec;
	mts->mts_msecond = 0;

	return (0);
}

MOSAPI int MOSCConv
mostimestamp_localnow(mostimestamp_t *mts) {
	struct tm ptm;
	time_t epoch;

	epoch = _time64(NULL);
	if (_localtime64_s(&ptm, &epoch) != 0)
		return (-1);

	mts->mts_flags = MOSTIME_LOCAL;
	mts->mts_year = 1900 + ptm.tm_year;
	mts->mts_month = ptm.tm_mon + 1;
	mts->mts_day = ptm.tm_mday;
	mts->mts_hour = ptm.tm_hour;
	mts->mts_minute = ptm.tm_min;
	mts->mts_second = ptm.tm_sec;
	mts->mts_msecond = 0;

	return (0);
}

MOSAPI int MOSCConv
mos_gettzbias(int32_t *min) {
	TIME_ZONE_INFORMATION tzi;
	DWORD res;

	res = GetTimeZoneInformation(&tzi);
	if (res == TIME_ZONE_ID_INVALID)
		return (-1);

	*min = (uint32_t)tzi.Bias;
	if (res == TIME_ZONE_ID_DAYLIGHT)
		*min += tzi.DaylightBias;

	return (0);
}

#if 0
MOSAPI mostime_t MOSCConv
mos_gettime_usec() {
	static uint32_t wrapcnt;
	static int first = 1;
	static uint64_t ltm;
	uint64_t res;
	uint64_t tm;

	mos_glock(NULL);
	if (first) {
		first = 0;
		ltm = (uint64_t)timeGetTime();
		ltm *= 1000ULL;		/* milliseconds to microseconds */
		mos_gunlock(NULL);
		return (ltm);
	}

	tm = (uint64_t)timeGetTime();
	tm *= 1000ULL;			/* milliseconds to microseconds */
	if (tm < ltm)
		wrapcnt++;
	ltm = tm;

	res = (uint64_t)wrapcnt << 32 | tm;

	mos_gunlock(NULL);
	return (res);
}
#endif

MOSAPI mostime_t MOSCConv
mos_gettime_usec() {
	static LARGE_INTEGER frequency;
	static LARGE_INTEGER ltm;
	LARGE_INTEGER tm, etm;

	if (ltm.QuadPart == 0) {
		mos_glock(NULL);
		if (ltm.QuadPart == 0) {
			QueryPerformanceFrequency(&frequency);
			QueryPerformanceCounter(&ltm);
		}
		mos_gunlock(NULL);
	}

	QueryPerformanceCounter(&tm);
	etm.QuadPart = tm.QuadPart - ltm.QuadPart;
	etm.QuadPart *= 1000000;
	etm.QuadPart /= frequency.QuadPart;

	return (etm.QuadPart);
}

mostime_t
mos_getsystime_usec() {
	time_t ltime;
	mostime_t mt;

	/*
	 * time_t is a 64-bit value
	 * time() is a wrapper around  _time64()
	 * time() returns seconds since epoch
	 */
	time(&ltime);

	mt = ltime * 1000LL * 1000LL;	/* seconds to microseconds */

	return (mt);
}

MOSAPI void MOSCConv
mos_usleep(mostime_t t) {

	Sleep((DWORD)(t / 1000));
}
