#include "mos_os.h"
#include "mos_fileio.h"
#include "mos_iop.h"

#include <shlwapi.h>
#pragma comment(lib, "shlwapi.lib")

MOSAPI int MOSCConv
mos_file_mkdir(const char *fmt, ...) {
	char path[MOS_PATH_MAX];
	va_list va;
	size_t n;
	BOOL res;

	va_start(va, fmt);
	n = mos_vsnprintf(path, sizeof (path), fmt, va);
	va_end(va);
	if (n >= sizeof (path))
		return (MOSN_NOSPC);

	res = CreateDirectoryA(path, NULL);
	if (res == FALSE)
		return (mos_windows_error_to_err(GetLastError()));
	return (0);
}

MOSAPI int MOSCConv
mos_file_rmdir(const char *fmt, ...) {
	char path[MOS_PATH_MAX];
	va_list va;
	size_t n;
	BOOL res;

	va_start(va, fmt);
	n = mos_vsnprintf(path, sizeof (path), fmt, va);
	va_end(va);
	if (n >= sizeof (path))
		return (MOSN_NOSPC);

	res = RemoveDirectoryA(path);
	if (res == FALSE)
		return (mos_windows_error_to_err(GetLastError()));
	return (0);
}

MOSAPI int MOSCConv
mos_file_exists(const char *fmt, ...) {
	char path[MOS_PATH_MAX];
	BOOL exists;
	va_list va;
	size_t n;

	va_start(va, fmt);
	n = mos_vsnprintf(path, sizeof (path), fmt, va);
	va_end(va);
	if (n >= sizeof (path))
		return (0);

	exists = PathFileExistsA(path);
	if (exists == TRUE)
		return (1);
	return (0);
}

MOSAPI int MOSCConv
mos_file_isdirectory(const char *fmt, ...) {
	char path[MOS_PATH_MAX];
	BOOL isdir;
	va_list va;
	size_t n;

	va_start(va, fmt);
	n = mos_vsnprintf(path, sizeof (path), fmt, va);
	va_end(va);
	if (n >= sizeof (path))
		return (0);

	isdir = PathIsDirectoryA(path);
	if (isdir == TRUE)
		return (1);
	return (0);
}

MOSAPI int MOSCConv
mos_file_unlink(const char *fmt, ...) {
	char path[MOS_PATH_MAX];
	BOOL res;
	va_list va;
	size_t n;

	va_start(va, fmt);
	n = mos_vsnprintf(path, sizeof (path), fmt, va);
	va_end(va);
	if (n >= sizeof (path))
		return (MOSN_NOSPC);

	res = DeleteFileA(path);
	if (res == FALSE)
		return (MOSN_IO);

	return (0);
}

MOSAPI int MOSCConv
mos_file_open(mosiop_t iop, mos_file_t **mf, int flags, const char *fmt, ...) {
	char path[MOS_PATH_MAX];
	DWORD created;
	DWORD access;
	DWORD shared;
	va_list va;
	DWORD res;
	size_t n;

	va_start(va, fmt);
	n = mos_vsnprintf(path, sizeof (path), fmt, va);
	va_end(va);
	if (n >= sizeof (path))
		return (MOSN_NOSPC);

	*mf = mos_malloc(sizeof (mos_file_t));

	if (mos_strcmp(path, MOS_FILE_STDIN) == 0) {
		(*mf)->handle = GetStdHandle(STD_INPUT_HANDLE);
		(*mf)->special = 1;
		if ((*mf)->handle == INVALID_HANDLE_VALUE || (*mf)->handle == NULL) {
			mos_free(*mf, sizeof(mos_file_t));
			*mf = NULL;
			return (MOS_ERROR(iop, MOSN_IO, "missing stdin handle"));
		}
		return (0);
	}
	if (mos_strcmp(path, MOS_FILE_STDOUT) == 0) {
		(*mf)->handle = GetStdHandle(STD_OUTPUT_HANDLE);
		(*mf)->special = 1;
		if ((*mf)->handle == INVALID_HANDLE_VALUE || (*mf)->handle == NULL) {
			mos_free(*mf, sizeof(mos_file_t));
			*mf = NULL;
			return (MOS_ERROR(iop, MOSN_IO, "missing stdout handle"));
		}
		return (0);
	}
	if (mos_strcmp(path, MOS_FILE_STDERR) == 0) {
		(*mf)->handle = GetStdHandle(STD_ERROR_HANDLE);
		(*mf)->special = 1;
		if ((*mf)->handle == INVALID_HANDLE_VALUE || (*mf)->handle == NULL) {
			mos_free(*mf, sizeof(mos_file_t));
			*mf = NULL;
			return (MOS_ERROR(iop, MOSN_IO, "missing error handle"));
		}
		return (0);
	}

	access = 0;
	shared = 0;
	if (flags & MOS_FILE_READ) {
		access |= GENERIC_READ;
		shared |= FILE_SHARE_READ;
	}
	if (flags & MOS_FILE_WRITE) {
		access |= GENERIC_WRITE;
		shared |= FILE_SHARE_WRITE;
	}

	created = 0;
	if (flags & MOS_FILE_CREATE) {
		if (flags & MOS_FILE_EXCL)
			created |= CREATE_NEW;
		else
			created |= OPEN_ALWAYS;
	} else if (flags & MOS_FILE_TRUNC) {
		created |= TRUNCATE_EXISTING;
	} else {
		created |= OPEN_EXISTING;
	}

	(*mf)->handle = CreateFileA(path, access, shared, NULL, created, FILE_ATTRIBUTE_NORMAL, NULL);
	(*mf)->special = 0;
	if ((*mf)->handle == INVALID_HANDLE_VALUE) {
		res = GetLastError();
		mos_free(*mf, sizeof (mos_file_t));
		*mf = NULL;
		if (res == ERROR_SHARING_VIOLATION)
			return (MOSN_BUSY);
		return (MOS_ERROR(iop, mos_windows_error(iop, GetLastError()), "CreateFile(%s) failed", path));
	}

	return (0);
}

MOSAPI int MOSCConv
mos_file_close(mosiop_t iop, mos_file_t **mf) {

	if (mf == NULL)
		return (MOS_ERROR(iop, MOSN_INVALARG, "file pointer is null"));

	if ((*mf)->handle == INVALID_HANDLE_VALUE)
		return (MOS_ERROR(iop, MOSN_INVALARG, "file handle is invalid"));

	if ((*mf)->special == 0)
		CloseHandle((*mf)->handle);
	mos_free(*mf, sizeof (mos_file_t));
	*mf = NULL;

	return (0);
}

MOSAPI int MOSCConv
mos_file_getsize(mosiop_t iop, mos_file_t *mf, uint64_t *size) {
	LARGE_INTEGER li;
	BOOL res;

	if (mf == NULL)
		return (MOS_ERROR(iop, MOSN_INVALARG, "file pointer is null"));

	if (size == NULL)
		return (MOS_ERROR(iop, MOSN_INVALARG, "size is null"));

	res = GetFileSizeEx(mf->handle, &li);
	if (res == FALSE)
		return (MOS_ERROR(iop, mos_windows_error(iop, GetLastError()), "GetFileSizeEx() failed"));

	*size = li.QuadPart;
	return (0);
}

MOSAPI int MOSCConv
mos_file_trunc(mosiop_t iop, mos_file_t *mf, uint64_t off) {
	uint64_t cur;
	BOOL res;
	int err2;
	int err;

	if (mf == NULL)
		return (MOS_ERROR(iop, MOSN_INVALARG, "file pointer is null"));

	err = mos_file_getoffset(iop, mf, &cur);
	if (err != 0)
		return (MOS_ERROR(iop, err, "failed to get current offset"));

	err = mos_file_seek(iop, mf, off);
	if (err != 0)
		return (MOS_ERROR(iop, err, "failed to set offset"));

	res = SetEndOfFile(mf->handle);
	if (res == FALSE)
		err = MOS_ERROR(iop, mos_windows_error(iop, GetLastError()), "SetEndOfFile() failed");

	err2 = mos_file_seek(iop, mf, cur);
	if (err2 != 0)
		MOS_ERROR(iop, err2, "failed to reset offset");

	if (err != 0)
		return (err);
	if (err2 != 0)
		return (err2);
	return (0);
}

MOSAPI int MOSCConv
mos_file_seek(mosiop_t iop, mos_file_t *mf, uint64_t off) {
	LARGE_INTEGER li;
	BOOL res;

	if (mf == NULL)
		return (MOS_ERROR(iop, MOSN_INVALARG, "file pointer is null"));

	li.QuadPart = off;

	res = SetFilePointerEx(mf->handle, li, NULL, FILE_BEGIN);
	if (res == FALSE)
		return (MOS_ERROR(iop, mos_windows_error(iop, GetLastError()), "SetFilePointerEx() failed"));

	return (0);
}

MOSAPI int MOSCConv
mos_file_getoffset(mosiop_t iop, mos_file_t *mf, uint64_t *off) {
	LARGE_INTEGER li, lic;
	BOOL res;

	if (mf == NULL)
		return (MOS_ERROR(iop, MOSN_INVALARG, "file pointer is null"));

	if (off == NULL)
		return (MOS_ERROR(iop, MOSN_INVALARG, "offset is null"));

	li.QuadPart = 0;
	res = SetFilePointerEx(mf->handle, li, &lic, FILE_CURRENT);
	if (res == FALSE)
		return (MOS_ERROR(iop, mos_windows_error(iop, GetLastError()), "SetFilePointerEx() failed"));

	*off = lic.QuadPart;
	return (0);
}

MOSAPI int MOSCConv
mos_file_read(mosiop_t iop, mos_file_t *mf, void *buf, size_t *bufsz) {
	DWORD out;

	if (mf == NULL)
		return (MOS_ERROR(iop, MOSN_INVALARG, "file pointer is null"));

	if (mf->handle == INVALID_HANDLE_VALUE)
		return (MOS_ERROR(iop, MOSN_INVALARG, "file handle is invalid"));

	if (buf == NULL)
		return (MOS_ERROR(iop, MOSN_INVALARG, "buf is null"));

	if (bufsz == NULL)
		return (MOS_ERROR(iop, MOSN_INVALARG, "bufsz is null"));

	if (ReadFile(mf->handle, buf, (DWORD)*bufsz, &out, NULL) == FALSE)
		return (MOS_ERROR(iop, mos_windows_error(iop, GetLastError()), "ReadFile() failed"));

	*bufsz = (size_t)out;
	return (0);
}

MOSAPI int MOSCConv
mos_file_write(mosiop_t iop, mos_file_t *mf, const void *buf, size_t bufsz) {
	DWORD out;

	if (mf == NULL)
		return (MOS_ERROR(iop, MOSN_INVALARG, "file pointer is null"));

	if (mf->handle == INVALID_HANDLE_VALUE)
		return (MOS_ERROR(iop, MOSN_INVALARG, "file handle is invalid"));

	if (buf == NULL)
		return (MOS_ERROR(iop, MOSN_INVALARG, "buf is null"));

	/* XXX Can a short write ever occur without an error? */
	if (WriteFile(mf->handle, buf, (DWORD)bufsz, &out, NULL) == FALSE)
		return (MOS_ERROR(iop, mos_windows_error(iop, GetLastError()), "WriteFile() failed"));

	return (0);
}

MOSAPI int MOSCConv
mos_file_getsizex(mosiop_t iop, uint64_t *size, const char *fmt, ...) {
	char path[MOS_PATH_MAX];
	struct _stat64 sb;
	va_list va;
	size_t n;
	int err;

	if (size == NULL)
		return (MOS_ERROR(iop, MOSN_INVALARG, "size is null"));

	va_start(va, fmt);
	n = mos_vsnprintf(path, sizeof (path), fmt, va);
	va_end(va);
	if (n >= sizeof (path))
		return (MOS_ERROR(iop, MOSN_NOSPC, "path too long"));

	err = _stati64(path, &sb);
	if (err != 0) {
		if (errno == ENOENT)
			return (MOS_ERROR(iop, MOSN_NOENT, "_stati64(%s) failed", path));
		return (MOS_ERROR(iop, MOSN_ERR, "_stati64(%s) failed", path));
	}

	*size = sb.st_size;

	return (0);
}

MOSAPI int MOSCConv
mos_file_readx(mosiop_t iop, void *buf, size_t *bufsz, const char *fmt, ...) {
	char path[MOS_PATH_MAX];
	va_list va;
	DWORD res;
	HANDLE fh;
	size_t n;

	DWORD out;

	va_start(va, fmt);
	n = mos_vsnprintf(path, sizeof (path), fmt, va);
	va_end(va);
	if (n >= sizeof (path))
		return (MOS_ERROR(iop, MOSN_NOSPC, "path too long"));

	fh = CreateFileA(path, GENERIC_READ, 0, NULL, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL);
	if (fh == INVALID_HANDLE_VALUE) {
		res = GetLastError();
		if (res == ERROR_SHARING_VIOLATION)
			return (MOSN_BUSY);
		return (MOS_ERROR(iop, mos_windows_error(iop, GetLastError()), "CreateFile(%s) failed", path));
	}

	if (ReadFile(fh, buf, (DWORD)*bufsz, &out, NULL) == FALSE) {
		res = GetLastError();
		CloseHandle(fh);
		return (MOS_ERROR(iop, mos_windows_error(iop, res), "ReadFile(%s) failed", path));
	}
	CloseHandle(fh);
	*bufsz = (size_t)out;

	return (0);
}

MOSAPI int MOSCConv
mos_file_writex(mosiop_t iop, const void *buf, size_t bufsz, const char *fmt, ...) {
	char path[MOS_PATH_MAX];
	va_list va;
	DWORD res;
	HANDLE fh;
	size_t n;

	DWORD out;

	va_start(va, fmt);
	n = mos_vsnprintf(path, sizeof (path), fmt, va);
	va_end(va);

	if (n >= sizeof (path))
		return (MOS_ERROR(iop, MOSN_NOSPC, "path too long"));

	fh = CreateFileA(path, GENERIC_WRITE, 0, NULL, CREATE_ALWAYS, FILE_ATTRIBUTE_NORMAL, NULL);
	if (fh == INVALID_HANDLE_VALUE) {
		res = GetLastError();
		if (res == ERROR_SHARING_VIOLATION)
			return (MOSN_BUSY);
		return (MOS_ERROR(iop, mos_windows_error(iop, GetLastError()), "CreateFile(%s) failed", path));
	}

	if (WriteFile(fh, buf, (DWORD)bufsz, &out, NULL) == FALSE) {
		res = GetLastError();
		CloseHandle(fh);
		return (MOS_ERROR(iop, mos_windows_error(iop, res), "WriteFile(%s) failed", path));
	}
	CloseHandle(fh);

	return (0);
}