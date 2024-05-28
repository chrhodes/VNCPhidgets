#include <winsock2.h>
#include <windows.h>

#include "mos_os.h"
#include "mos_iop.h"
#include "mos_os-Windows-user.h"

MOSAPI int MOSCConv
mos_windows_error_to_err(DWORD error) {

	switch (error) {
	case ERROR_FILE_TOO_LARGE:
		return (MOSN_2BIG);
	case ERROR_FILE_NOT_FOUND:
	case ERROR_PATH_NOT_FOUND:
		return (MOSN_NOENT);
	case ERROR_ACCESS_DENIED:
		return (MOSN_ACCESS);
	case ERROR_SHARING_VIOLATION:
		return (MOSN_ACCESS);
	case ERROR_FILE_EXISTS:
	case ERROR_ALREADY_EXISTS:
		return (MOSN_EXIST);
	case ERROR_NOT_SUPPORTED:
		return (MOSN_NOSUP);
	case ERROR_TOO_MANY_OPEN_FILES:
		return (MOSN_MFILE);
	case ERROR_INVALID_HANDLE:
	case ERROR_INVALID_NAME:
		return (MOSN_INVAL);
	case ERROR_NOT_ENOUGH_MEMORY:
	case ERROR_OUTOFMEMORY:
		return (MOSN_MEM);
	case ERROR_CURRENT_DIRECTORY:
	case ERROR_PATH_BUSY:
	case ERROR_BUSY:
		return (MOSN_BUSY);
	case ERROR_DISK_FULL:
		return (MOSN_NOSPC);
	case WAIT_TIMEOUT:
		return (MOSN_TIMEDOUT);
	case ERROR_INVALID_ADDRESS:
		return (MOSN_FAULT);
	default:
		return (MOSN_ERR);
	}
}

MOSAPI int MOSCConv
mos_windows_error(mosiop_t iop, DWORD error) {
	char buf[256];
	char *c;
	int res;
	int err;

	if (FormatMessage(FORMAT_MESSAGE_FROM_SYSTEM, NULL, error, 0, (void *)buf,
	  sizeof (buf), NULL) != 0) {
		/*
		 * Trim trailing
 so the message can be embedded.
		 */
		if (strlen(buf) > 1) {
			c = buf + strlen(buf) - 2;
			if (c[0] == '\r')
				c[0] = '\0';
		}
		err = mos_windows_error_to_err(error);
		if (err == MOSN_ERR)
			res = MOS_ERROR(iop, err, "%s (0x%x)", &buf, error);
		else
			res = MOS_ERROR(iop, err, "%s", &buf);
	} else {
		res = MOS_ERROR(iop, MOSN_ERR, "Windows error 0x%x", error);
	}

	return (res);
}

MOSAPI int MOSCConv
mos_windows_wsa_error_to_err(int error) {

	switch (error) {
	case WSAEAFNOSUPPORT:
		return (MOSN_NOSUP);
	case WSAEWOULDBLOCK:
		return (MOSN_AGAIN);
	case WSAECONNREFUSED:
		return (MOSN_CONNREF);
	case WSAECONNABORTED:
		return (MOSN_PIPE);
	case WSAETIMEDOUT:
		return (MOSN_TIMEDOUT);
	case WSAECONNRESET:
		return (MOSN_PIPE);
	case WSAENETRESET:
		return (MOSN_PIPE);
	case WSATRY_AGAIN:
		return (MOSN_AGAIN);
	case WSA_INVALID_HANDLE:
		return (MOSN_INVALARG);
	case WSAEINTR:
		return (MOSN_INTR);
	case WSAEBADF:
		return (MOSN_INVALARG);
	case WSAEACCES:
		return (MOSN_ACCESS);
	default:
		return (MOSN_ERR);
	}
}

MOSAPI int MOSCConv
mos_windows_wsa_error(mosiop_t iop, int err) {
	const char *msg;

	switch (err) {
	case WSA_IO_PENDING:
		msg = "Overlapped operations will complete later";
		break;
	case WSA_IO_INCOMPLETE:
		msg = "Overlapped I/O event object not in signalled state";
		break;
	case WSA_INVALID_HANDLE:
		msg = "Specified event object handle is invalid";
		break;
	case WSA_INVALID_PARAMETER:
		msg = "One or more parameters are invalid";
		break;
	case WSA_NOT_ENOUGH_MEMORY:
		msg = "Insufficient memory available";
		break;
	case WSA_OPERATION_ABORTED:
		msg = "Overlapped operation aborted";
		break;
	case WSAEINTR:
		msg = "Interrupted function call";
		break;
	case WSAEBADF:
		msg = "File handle is not valid";
		break;
	case WSAEACCES:
		msg = "Permission denied";
		break;
	case WSAEFAULT:
		msg = "Bad address";
		break;
	case WSAEINVAL:
		msg = "Invalid argument";
		break;
	case WSAEMFILE:
		msg = "Too many open files";
		break;
	case WSASYSNOTREADY:
		msg = "underlying network subsystem not ready";
		break;
	case WSAVERNOTSUPPORTED:
		msg = "version not supported";
		break;
	case WSAEINPROGRESS:
		msg = "blocking operation already in progress";
		break;
	case WSAEPROCLIM:
		msg = "implementation task limit exceeded";
		break;
	case WSANOTINITIALISED:
		msg = "mos_netops_init()/WSAStartup() not yet invoked";
		break;
	case WSAENETDOWN:
		msg = "network failed";
		break;
	case WSAHOST_NOT_FOUND:
		msg = "host not found";
		break;
	case WSATRY_AGAIN:
		msg = "try again";
		break;
	case WSANO_RECOVERY:
		msg = "nonrecoverable error";
		break;
	case WSANO_DATA:
		msg = "no matching data for existing name";
		break;
	case WSAEAFNOSUPPORT:
		msg = "address family not supported";
		break;
	case WSAENOBUFS:
		msg = "no buffer space available";
		break;
	case WSAEPROTONOSUPPORT:
		msg = "protocol not supported";
		break;
	case WSAEPROTOTYPE:
		msg = "specified protocol doesn't match socket";
		break;
	case WSAESOCKTNOSUPPORT:
		msg = "specified socket type not supported in address family";
		break;
	case WSAEWOULDBLOCK:
		msg = "operation would block";
		break;
	case WSAEALREADY:
		msg = "operation already in progress";
		break;
	case WSAECONNREFUSED:
		msg = "connection refused";
		break;
	case WSAECONNABORTED:
		msg = "connection aborted";
		break;
	case WSAENETUNREACH:
		msg = "network unreachable";
		break;
	case WSAEHOSTUNREACH:
		msg = "host unreachable";
		break;
	case WSAETIMEDOUT:
		msg = "timeout";
		break;
	case WSAENOTSOCK:
		msg = "not a socket";
		break;
	case WSAECONNRESET:
		msg = "connection reset";
		break;
	case WSAEADDRINUSE:
		msg = "address in use";
		break;
	case WSAEADDRNOTAVAIL:
		msg = "address not available";
		break;
	default:
		return (MOS_ERROR(iop, MOSN_ERR, "unknown error %d", err));
	}

	err = mos_windows_wsa_error_to_err(err);
	MOS_ERROR(iop, err, msg);

	return (err);
}
