#include "mos_os.h"
#include "mos_stacktrace.h"
#if defined(MOS_USE_DBGHELP)
#include <windows.h>
//NOTE: Windows header is generating this warning, so we have to ignore to compile
#pragma warning (disable: 4091)
#include "dbghelp.h"
#pragma warning (default: 4091)
#endif

MOSAPI size_t MOSCConv
mos_stacktrace(void **ip, size_t n) {

	return (CaptureStackBackTrace(0, (DWORD)n, ip, NULL));
}

#if defined(MOS_USE_DBGHELP)

#pragma comment(lib, "Dbghelp.lib")

MOSAPI size_t MOSCConv
mos_getsymbolname(const void *addr, char *cbuf, size_t len) {
	char sbuf[sizeof (SYMBOL_INFO) + MAX_SYM_NAME];
	static int initialized;
	DWORD64 displacement;
	SYMBOL_INFO *sym;

	if (!initialized) {
		SymSetOptions(SYMOPT_UNDNAME | SYMOPT_DEFERRED_LOADS);

		if (!SymInitialize(GetCurrentProcess(), NULL, TRUE)) {
			mos_snprintf(cbuf, len, "dbghelp init error");
			return (0);
		}

		initialized = 1;
	}

	sym = (SYMBOL_INFO *)sbuf;
	sym->SizeOfStruct = sizeof (SYMBOL_INFO);
	sym->MaxNameLen = MAX_SYM_NAME;

	if (!SymFromAddr(GetCurrentProcess(), (DWORD64)addr, &displacement, sym)) {
		mos_snprintf(cbuf, len, "%p", addr);
		return (0);
	}

	mos_snprintf(cbuf, len, "%.*s+0x%llx", sym->NameLen, sym->Name,
	  displacement);

	return (0);
}

#else /* !MOS_USE_DBGHELP */

MOSAPI size_t MOSCConv
mos_getsymbolname(const void *addr, char *cbuf, size_t len) {

	mos_snprintf(cbuf, len, "%p", addr);

	return (0);
}
#endif
