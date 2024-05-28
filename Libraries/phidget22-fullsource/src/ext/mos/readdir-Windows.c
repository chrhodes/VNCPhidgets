
#include <sys/types.h>
#include <sys/stat.h>
#include <errno.h>

#include "mos_readdir.h"

MOSAPI int MOSCConv
mos_opendir(mosiop_t iop, const char *path, mos_dirinfo_t **di) {
	struct _stat sb;
	int err;

	err = _stat(path, &sb);
	if (err != 0)
		return (MOS_ERROR(iop, MOSN_ERR,
		  "failed to stat dirent '%s', errno: %d", path, errno));

	*di = mos_malloc(sizeof(mos_dirinfo_t));
	(*di)->first = 1;
	(*di)->flags = 0;
	(*di)->errcode = 0;
	(*di)->path = mos_strdup(path, NULL);
	(*di)->dirobj = mos_malloc(sizeof (HANDLE));

	return (0);
}

MOSAPI void MOSCConv
mos_closedir(mos_dirinfo_t **di) {

	FindClose(*((HANDLE *)((*di)->dirobj)));
	mos_free((*di)->dirobj, sizeof(HANDLE));
	mos_free((*di)->path, mos_strlen((*di)->path) + 1);
	mos_free((*di), sizeof(mos_dirinfo_t));
	*di = NULL;
}

MOSAPI int MOSCConv
mos_readdir(mosiop_t iop, mos_dirinfo_t *di) {
	char buf[MOS_PATH_MAX];
	WIN32_FIND_DATAA ffd;
	size_t end;
	BOOL res;

	di->errcode = 0;

	mos_strlcpy(buf, di->path, sizeof(buf));
	end = mos_strlen(buf) - 1;
	if (buf[end] != '/' && buf[end] != '\\')
		mos_strlcat(buf, "/", MOS_PATH_MAX);
	mos_strlcat(buf, "*", MOS_PATH_MAX);

	if (di->first) {
		di->first = 0;
		*((HANDLE *)(di->dirobj)) = FindFirstFileA(buf, &ffd);
		if (*((HANDLE *)(di->dirobj)) == INVALID_HANDLE_VALUE) {
			if (GetLastError() == ERROR_NO_MORE_FILES)
				di->errcode = MOSN_NOENT;
			return (0);
		}
	} else {
		res = FindNextFileA(*((HANDLE *)(di->dirobj)), &ffd);
		if (res == 0) {
			if (GetLastError() == ERROR_NO_MORE_FILES)
				di->errcode = MOSN_NOENT;
			return (0);
		}
	}

	di->flags = 0;
	if (ffd.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY)
		di->flags |= MOS_DIRINFO_ISDIR;
	if (ffd.dwFileAttributes & FILE_ATTRIBUTE_HIDDEN)
		di->flags |= MOS_DIRINFO_ISHIDDEN;
	mos_strncpy(di->filename, ffd.cFileName, sizeof(di->filename));

	return (0);
}
