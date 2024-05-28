#include <winsock2.h>
#include "mos_dl.h"

MOSAPI void * MOSCConv
mos_dlopen(const char *path, int flags) {

	return (LoadLibraryA(path));	/* XXX Wide char */
}

MOSAPI int MOSCConv
mos_dlclose(void *hdl) {

	/* return 0 on success */
	return (!FreeLibrary(hdl));
}

MOSAPI void * MOSCConv
mos_dlsym(void * __restrict hdl, const char * __restrict symname) {

	return (GetProcAddress(hdl, symname));
}

MOSAPI const char * MOSCConv
mos_dlerror(char *msgbuf, size_t msgbufsz) {

	/*
	 * Formats a message for the last error into a buffer, and returns
	 * a copy of that buffer (which must be freed by the caller).
	 */

	if (!FormatMessageA(
	  FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS,
	  NULL, GetLastError(), MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT),
	  msgbuf, (DWORD)msgbufsz, NULL))
		return ("unknown error");

	return (msgbuf);
}
