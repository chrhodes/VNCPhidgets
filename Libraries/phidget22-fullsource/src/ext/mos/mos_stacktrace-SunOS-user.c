
#include "mos_stacktrace.h"

#if defined(SUNOS_RELEASE_MINOR) && SUNOS_RELEASE_MINOR > 10

#include <ucontext.h>
#include <execinfo.h>

size_t
mos_stacktrace(void **ip, size_t n) {

	return (backtrace(ip, n));
}

size_t
mos_getsymbolname(const void *addr, char *buf, size_t len) {
	char symstr[2048];
	size_t slen;

	union {
		const void *cv;
		void *v;
	} deconstify;

	deconstify.cv = addr;

	slen = addrtosymstr(deconstify.v, symstr, sizeof (symstr));
	if (slen > sizeof (symstr))
		slen = sizeof (symstr);

	if (slen > len) /* print at least trailing part */
		mos_strlcpy(buf, symstr + (slen - len - 1), len);
	else
		mos_strlcpy(buf, symstr, len);

	return (0);
}

#else

size_t
mos_stacktrace(void **ip, size_t n) {

	return (0);
}

size_t
mos_getsymbolname(const void *addr, char *buf, size_t len) {

	mos_strlcpy(buf, "<unknown>", len);

	return (0);
}
#endif
