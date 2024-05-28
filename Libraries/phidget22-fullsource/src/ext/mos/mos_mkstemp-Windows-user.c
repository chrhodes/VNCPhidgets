#include <errno.h>
#include <fcntl.h>
#include <io.h>
#include <stdio.h>
#include <stdlib.h>

#include "mos_os.h"
#include "mos_mkstemp-Windows-user.h"

MOSAPI int MOSCConv
mos_mkstemp(char *template) {
	char *origtemplate;
	size_t bufsz;
	uint32_t attr;
	int err;
	int fd;

	bufsz = strlen(template) + 1;
	origtemplate = malloc(bufsz);
	if (origtemplate == NULL)
		return (-1);
	mos_strlcpy(origtemplate, template, bufsz);

again:
	err = _mktemp_s(template, bufsz);
	if (err != 0)
		return (-1);

	fd = _open(template, O_RDWR | O_EXCL | O_CREAT);
	if (fd < 0 && errno == EEXIST) {
		mos_strlcpy(template, origtemplate, bufsz);
		goto again;
	}

	free(origtemplate);

	/* clear readonly attribute */
	if (fd >= 0) {
		attr = GetFileAttributes(template);
		SetFileAttributes(template, attr & ~FILE_ATTRIBUTE_READONLY);
	}

	return (fd);
}
