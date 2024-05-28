#include "mos_stacktrace.h"

MOSAPI size_t MOSCConv
mos_stacktrace(void **ip, size_t n) {

	return (0);
}

MOSAPI size_t MOSCConv
mos_getsymbolname(const void *addr, char *cbuf, size_t len) {

	if (len > 0)
		cbuf[0] = '\0';

	return (0);
}
