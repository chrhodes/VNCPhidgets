#include "mos_os.h"

MOSAPI int MOSCConv
mos_setenv(const char *name, const char *val, int overwrite) {
	uint32_t asz;
	char *a;
	int err;

	mos_asprintf(&a, &asz, "%s=%s", name, val);
	err = _putenv(a);
	mos_free(a, asz);

	return (err);
}

MOSAPI int MOSCConv
mos_unsetenv(const char *name) {

	return (mos_setenv(name, "", 1));
}
