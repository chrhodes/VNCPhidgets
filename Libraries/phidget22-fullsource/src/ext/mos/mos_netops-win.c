#define _WINSOCK_DEPRECATED_NO_WARNINGS 1

#include <winsock2.h>
#include <ws2tcpip.h>
#include <windows.h>
#include <tchar.h>
#include <strsafe.h>
#include <mstcpip.h>
#include <time.h>

#include "mos/mos_os.h"
#include "mos/mos_netops.h"
#include "mos/mos_assert.h"

void
_mos_netops_init() {
	WSADATA wsadata;
	int res;

	/*
	 * If it fails, that is too bad.. for now.
	 */
	res = WSAStartup(MAKEWORD(2, 2), &wsadata);
	MOS_ASSERT(res == NO_ERROR);
}

#define CHECKTCPSOCKET	do {										\
	if (sock == NULL)												\
		return (MOS_ERROR(iop, MOSN_INVALARG, "socket is null"));	\
	if (*sock == INVALID_SOCKET)									\
		return (MOS_ERROR(iop, MOSN_INVAL, "socket is closed"));	\
} while (0)

MOSAPI int MOSCConv
mos_netop_setsockopt(mos_socket_t *sock, int level, int opt, void *optval, socklen_t optlen) {

		return (setsockopt(*sock, level, opt, optval, optlen));
}

MOSAPI int MOSCConv
mos_netop_getbyname(mosiop_t iop, const char *nm, mos_af_t af, mos_sockaddr_t *addr) {
	ADDRINFOA hints;
	PADDRINFOA ai;
	int wsaerr;

	if (nm == NULL)
		return (MOS_ERROR(iop, MOSN_INVALARG, "null name specified"));

	memset(&hints, 0, sizeof (hints));
	switch(af) {
	case MOS_AF_INET4:
		hints.ai_family = AF_INET;
		break;
	case MOS_AF_INET6:
		hints.ai_family = AF_INET6;
		break;
	default:
		return (MOS_ERROR(iop, MOSN_NOSUP, "unsupported address family 0x%x", af));
	}

	wsaerr = getaddrinfo(nm, NULL, &hints, &ai);
	if (wsaerr != NO_ERROR)
		return (MOS_ERROR(iop, mos_windows_wsa_error(iop, wsaerr), "failed to get address info for %s", nm));
	addr->sa = *ai->ai_addr;	/* take the first */
	freeaddrinfo(ai);

	return (0);
}

MOSAPI int MOSCConv
mos_netop_addrmatchesname(mosiop_t iop, mos_sockaddr_t *addr, const char *nm, mos_af_t af, int *res) {
	struct addrinfo hints;
	struct addrinfo *walk;
	struct addrinfo *ai;
	int wsaerr;

	*res = 0;

	memset(&hints, 0, sizeof (hints));
	switch(af) {
	case MOS_AF_INET4:
		hints.ai_family = AF_INET;
		break;
	case MOS_AF_INET6:
		hints.ai_family = AF_INET;
		break;
	default:
		return (MOS_ERROR(iop, MOSN_NOSUP, "unsupported address family 0x%x", af));
	}

	wsaerr = getaddrinfo(nm, NULL, &hints, &ai);
	if (wsaerr != NO_ERROR)
		return (MOS_ERROR(iop, mos_windows_wsa_error(iop, wsaerr), "failed to get address info"));

	for (walk = ai; walk != NULL; walk = walk->ai_next) {
		if (memcmp(&addr->sa, walk->ai_addr, walk->ai_addrlen) == 0) {
			*res = 1;
			break;
		}
	}
	freeaddrinfo(ai);

	return (0);
}

MOSAPI int MOSCConv
mos_netop_getnameinfo(mos_sockaddr_t *addr, char *host, size_t hostn, char *svc, size_t svcn) {
	int err;

	err = getnameinfo(&addr->sa, sizeof (addr->sa), host, (DWORD)hostn, svc, (DWORD)svcn, 0);
	if (err != 0)
		return (mos_windows_wsa_error_to_err(WSAGetLastError()));
	return (0);
}

MOSAPI int MOSCConv
mos_netop_tcp_opensocket(mosiop_t iop, mos_socket_t *sock, mos_sockaddr_t *addr) {
	SOCKET s;
	int err;

	switch(addr->sa.sa_family) {
	case MOS_AF_INET4:
	case MOS_AF_INET6:
		s = socket(addr->sa.sa_family, SOCK_STREAM, IPPROTO_TCP);
		if (s == INVALID_SOCKET)
			return (MOS_ERROR(iop, mos_windows_wsa_error(iop, WSAGetLastError()), "socket() failed"));

		err = connect(s, &addr->sa, sizeof (addr->sa));
		if (err != 0) {
			err = mos_windows_wsa_error(iop, WSAGetLastError());
			closesocket(s);
			return (MOS_ERROR(iop, err, "failed to connect socket to 0x%x", *addr));
		}

		*sock = s;
		return (0);
	default:
		return (MOS_ERROR(iop, MOSN_NOSUP, "address family not supported"));
	}
}

MOSAPI int MOSCConv
mos_netop_tcp_openserversocket(mosiop_t iop, mos_socket_t *sock, mos_sockaddr_t *addr) {
	SOCKET s;
	int err;
	int on;

	s = INVALID_SOCKET;

	switch(addr->sa.sa_family) {
	case MOS_AF_INET4:
	case MOS_AF_INET6:
		s = socket(addr->sa.sa_family, SOCK_STREAM, IPPROTO_TCP);
		if (s == INVALID_SOCKET)
			return (MOS_ERROR(iop, mos_windows_wsa_error(iop, WSAGetLastError()), "socket() failed"));

		on = 1;
		err = setsockopt(s, SOL_SOCKET, SO_REUSEADDR, (char *)&on, sizeof (on));
		if (err != 0) {
			err = mos_windows_wsa_error(iop, WSAGetLastError());
			MOS_ERROR(iop, err, "setsockopt(SO_RESUSEADDR) failed");
			goto fail;
		}

		switch(addr->sa.sa_family) {
		case MOS_AF_INET4:
			err = bind(s, &addr->sa, sizeof (addr->s4));
			break;
		case MOS_AF_INET6:
			err = bind(s, &addr->sa, sizeof (addr->s6));
			break;
		}
		if (err != 0) {
			err = mos_windows_wsa_error(iop, WSAGetLastError());
			MOS_ERROR(iop, err, "bind() failed");
			goto fail;
		}

		listen(s, SOMAXCONN);
		*sock = s;

		return (0);

	default:
		return (MOS_ERROR(iop, MOSN_NOSUP, "address family not supported"));
	}

fail:

	if (s != INVALID_SOCKET)
		closesocket(s);

	return (err);
}

MOSAPI int MOSCConv
mos_netop_tcp_closesocket(mosiop_t iop, mos_socket_t *sock) {
	struct linger l;
	int len;
	int err;

	CHECKTCPSOCKET;

	len = (int)sizeof (l);
	if (getsockopt(*sock, SOL_SOCKET, SO_LINGER, (char *)&l, &len) == 0) {
		if (l.l_onoff == 0)
			WSASendDisconnect(*sock, NULL);
	}

	err = closesocket(*sock);
	if (err != 0)
		return (MOS_ERROR(iop, mos_windows_wsa_error(iop, WSAGetLastError()), "closesocket() failed"));

	*sock = INVALID_SOCKET;

	return (0);
}

MOSAPI int MOSCConv
mos_netop_tcp_accept(mosiop_t iop, mos_socket_t *sock, mos_socket_t *s, mos_sockaddr_t *addr) {
	struct sockaddr sa;
	socklen_t alen;
	SOCKET cs;

	CHECKTCPSOCKET;

	if (sock == NULL)
		return (MOS_ERROR(iop, MOSN_INVALARG, "server socket is NULL"));

again:

	alen = sizeof (struct sockaddr);
	cs = accept(*sock, &sa, &alen);
	if (cs == INVALID_SOCKET) {
		switch(WSAGetLastError()) {
		case WSAEINTR:
			goto again;
		default:
			return (MOS_ERROR(iop, mos_windows_wsa_error(iop, WSAGetLastError()),
			  "failed to accept connection"));
		}
	}

	*s = cs;
	if (addr != NULL)
		memcpy(&addr->sa, &sa, sizeof (sa));

	return (0);
}

MOSAPI int MOSCConv
mos_netop_tcp_rpoll2(mosiop_t iop, mos_socket_t *s1, mos_socket_t *s2, int *socks, uint32_t msec) {
	struct timeval timeval;
	fd_set rfds;
	int nfds;
	int res;

	FD_ZERO(&rfds);
	if (*s1 != MOS_INVALID_SOCKET)
		FD_SET(*s1, &rfds);
	if (*s2 != MOS_INVALID_SOCKET)
		FD_SET(*s2, &rfds);
	timeval.tv_sec = msec / 1000;
	timeval.tv_usec = (msec % 1000) * 1000;

	nfds = MOS_MAX(*s1, *s2);
	res = select(nfds + 1, &rfds, NULL, NULL, &timeval);
	if (res < 0)
		return (MOS_ERROR(iop, mos_windows_wsa_error(iop, WSAGetLastError()), "select() failed"));

	*socks = 0;
	if (*s1 != MOS_INVALID_SOCKET && FD_ISSET(*s1, &rfds))
		*socks |= 1;
	if (*s2 != MOS_INVALID_SOCKET && FD_ISSET(*s2, &rfds))
		*socks |= 2;

	if (*socks == 0)
		return (MOSN_TIMEDOUT);

	return (0);
}

MOSAPI int MOSCConv
mos_netop_tcp_rpoll(mosiop_t iop, mos_socket_t *sock, uint32_t msec) {
	struct timeval timeval;
	fd_set rfds;
	int res;

	FD_ZERO(&rfds);
	FD_SET(*sock, &rfds);
	timeval.tv_sec = msec / 1000;
	timeval.tv_usec = (msec % 1000) * 1000;

	res = select((int)*sock, &rfds, NULL, NULL, &timeval);
	if (res < 0)
		return (MOS_ERROR(iop, mos_windows_wsa_error(iop, WSAGetLastError()), "select() failed"));
	if (!FD_ISSET(*sock, &rfds))
		return (MOSN_TIMEDOUT);

	return (0);
}

MOSAPI int MOSCConv
mos_netop_tcp_available(mosiop_t iop, mos_socket_t *sock, int *len) {
	int res;

	CHECKTCPSOCKET;

	res = ioctlsocket(*sock, FIONREAD, (u_long *)len);
	if (res < 0)
		return (MOS_ERROR(iop, mos_windows_wsa_error(iop, WSAGetLastError()), "ioctlsocket(FIONREAD)"));

	return (0);
}

MOSAPI int MOSCConv
mos_netop_tcp_setnonblocking(mosiop_t iop, mos_socket_t *sock, int on) {
	int res;

	CHECKTCPSOCKET;

	res = ioctlsocket(*sock, FIONBIO, (u_long *)&on);
	if (res != NO_ERROR)
		return (MOS_ERROR(iop, mos_windows_wsa_error(iop, WSAGetLastError()),
		  "ioctlsocket(FIONBIO, %d) failed"), on);

	return (0);
}

MOSAPI int MOSCConv
mos_netop_tcp_read(mosiop_t iop, mos_socket_t *sock, void *v, size_t *len) {
	ssize_t res;

	CHECKTCPSOCKET;

	res = recv(*sock, v, (int)*len, 0);
	if (res < 0)
		return (MOS_ERROR(iop, mos_windows_wsa_error(iop, WSAGetLastError()), "recv() failed"));

	*len = (size_t)res;

	return (0);
}

MOSAPI int MOSCConv
mos_netop_tcp_write(mosiop_t iop, mos_socket_t *sock, const void *v, size_t *len) {
	ssize_t res;

	CHECKTCPSOCKET;

	res = send(*sock, v, (int)*len, 0);
	if (res < 0)
		return (MOS_ERROR(iop, mos_windows_wsa_error(iop, WSAGetLastError()), "send() failed"));

	*len = (size_t)res;

	return (0);
}

MOSAPI int MOSCConv
mos_netop_gethostname(mosiop_t iop, char *name, size_t namelen) {

	return (gethostname(name, (int)namelen));
}

MOSAPI int MOSCConv
mos_netop_getsockname(mosiop_t iop, mos_socket_t *sock, mos_sockaddr_t *addr) {
	struct sockaddr sa;
	socklen_t saln;
	int err;

	CHECKTCPSOCKET;

	saln = sizeof (sa);
	err = getsockname(*sock, &sa, &saln);
	if (err != 0)
		return (MOS_ERROR(iop, MOSN_ERR, "getsockname() failed: %s", strerror(errno)));

	addr->sa = sa;

	return (0);
}

MOSAPI int MOSCConv
mos_netop_getpeername(mosiop_t iop, mos_socket_t *sock, mos_sockaddr_t *addr) {
	struct sockaddr sa;
	socklen_t saln;
	int err;

	CHECKTCPSOCKET;

	saln = sizeof (sa);
	err = getpeername(*sock, &sa, &saln);
	if (err != 0)
		return (MOS_ERROR(iop, MOSN_ERR, "getpeername() failed: %s", strerror(errno)));

	addr->sa = sa;

	return (0);
}

MOSAPI int MOSCConv
mos_netop_usekeepalive(mosiop_t iop, mos_socket_t *sock, uint32_t intervalms) {
	struct tcp_keepalive v;
	DWORD d;
	int err;
	BOOL on;

	CHECKTCPSOCKET;

	memset(&v, 0, sizeof (v));
	v.onoff = 1;
	v.keepalivetime = intervalms;
	v.keepaliveinterval = 1000;
	on = 1;

	err = setsockopt(*sock, SOL_SOCKET, SO_KEEPALIVE, (void *)&on, sizeof (on));
	if (err != 0)
		return (MOS_ERROR(iop, mos_windows_wsa_error(iop, WSAGetLastError()), "setsockopt(SO_KEEPALIVE)"));

	d = 0;
	err = WSAIoctl(*sock, SIO_KEEPALIVE_VALS, &v, sizeof (v), NULL, 0, &d, NULL, NULL);
	if (err != 0)
		return (MOS_ERROR(iop, mos_windows_wsa_error(iop, WSAGetLastError()),
		  "WSAIoctl(SIO_KEEPALIVE_VALS)"));

	return (0);
}

MOSAPI int MOSCConv
mos_netop_setrecvtimeout(mosiop_t iop, mos_socket_t *sock, uint32_t ms) {
	DWORD d;
	int err;

	CHECKTCPSOCKET;

	d = ms;
	err = setsockopt(*sock, SOL_SOCKET, SO_RCVTIMEO, (void *)&d, sizeof (d));
	if (err != 0)
		return (MOS_ERROR(iop, mos_windows_wsa_error(iop, WSAGetLastError()), "setsockopt(SO_RCVTIMEO)"));

	return (0);
}

MOSAPI int MOSCConv
mos_netop_setrecvbufsize(mosiop_t iop, mos_socket_t *sock, uint32_t size) {
	DWORD d;
	int err;

	d = size;
	err = setsockopt(*sock, SOL_SOCKET, SO_RCVBUF, (void *)&d, sizeof (d));
	if (err != 0)
		return (MOS_ERROR(iop, mos_windows_wsa_error(iop, WSAGetLastError()), "setsockopt(SO_RCVBUF)"));

	return (0);
}

MOSAPI int MOSCConv
mos_netop_setsendtimeout(mosiop_t iop, mos_socket_t *sock, uint32_t ms) {
	DWORD d;
	int err;

	CHECKTCPSOCKET;

	d = ms;
	err = setsockopt(*sock, SOL_SOCKET, SO_SNDTIMEO, (void *)&d, sizeof (d));
	if (err != 0)
		return (MOS_ERROR(iop, mos_windows_wsa_error(iop, WSAGetLastError()), "setsockopt(SO_SNDTIMEO)"));

	return (0);
}

/* inet_pton() doesn't exist before Vista */
#if (WINVER < 0x600)
MOSAPI int MOSCConv
mos_netop_inet_pton(int af, const char *src, void *dst) {
	char src_copy[INET6_ADDRSTRLEN+1];
	struct sockaddr_storage ss;
	int size;

	size = sizeof(ss);

	ZeroMemory(&ss, sizeof(ss));
	/* stupid non-const API */
	mos_strlcpy(src_copy, src, sizeof (src_copy));

	if (WSAStringToAddressA(src_copy, af, NULL, (struct sockaddr *)&ss, &size) == 0) {
		switch(af) {
		case AF_INET:
			*(struct in_addr *)dst = ((struct sockaddr_in *)&ss)->sin_addr;
			return (1);
		case AF_INET6:
			*(struct in6_addr *)dst = ((struct sockaddr_in6 *)&ss)->sin6_addr;
			return (1);
		}
	}
	return (0);
}

MOSAPI const char * MOSCConv
mos_netop_inet_ntop(int af, const void *src, char *dst, socklen_t size) {
	struct sockaddr_storage ss;
	unsigned long s;

	s = size;

	ZeroMemory(&ss, sizeof(ss));
	ss.ss_family = af;

	switch(af) {
	case AF_INET:
		((struct sockaddr_in *)&ss)->sin_addr = *(struct in_addr *)src;
		break;
	case AF_INET6:
		((struct sockaddr_in6 *)&ss)->sin6_addr = *(struct in6_addr *)src;
		break;
	default:
		return (NULL);
	}

	/* cannot direclty use &size because of strict aliasing rules */
	return ((WSAAddressToStringA((struct sockaddr *)&ss, sizeof(ss), NULL, dst, &s) == 0)? dst : NULL);
}
#endif


MOSAPI int MOSCConv
mos_netop_udp_openserversocket(mosiop_t iop, mos_socket_t *sock, mos_sockaddr_t *addr) {
	struct sockaddr_in sa;
	socklen_t saln;
	SOCKET s;
	int err;

	if (addr->sa.sa_family != AF_INET)
		return (MOS_ERROR(iop, MOSN_NOSUP, "unsupported address family"));

	s = socket(AF_INET, SOCK_DGRAM, IPPROTO_UDP);
	if (s == INVALID_SOCKET)
		return (MOS_ERROR(iop, mos_windows_wsa_error(iop, WSAGetLastError()), "socket() failed"));

	err = bind(s, &addr->sa, sizeof (addr->s4));
	if (err != 0) {
		err = mos_windows_wsa_error(iop, WSAGetLastError());
		closesocket(s);
		return (MOS_ERROR(iop, err, "bind() failed"));
	}

	/*
	 * If the supplied port was 0, the system will assign a random port: find that port.
	 */
	if (addr->s4.sin_port == 0) {
		saln = sizeof (sa);
		err = getsockname(s, (struct sockaddr *)&sa, &saln);
		if (err != 0) {
			closesocket(s);
			return (MOS_ERROR(iop, MOSN_ERR, "getsockname() failed: %s", strerror(errno)));
		}
		addr->s4.sin_port = sa.sin_port;
	}

	*sock = s;

	return (0);
}

MOSAPI int MOSCConv
mos_netop_udp_opensocket(mosiop_t iop, mos_socket_t *sock, mos_sockaddr_t *addr) {
	SOCKET s;
	int err;

	if (addr->sa.sa_family != MOS_AF_INET4)
		return (MOS_ERROR(iop, MOSN_NOSUP, "address family not supported"));

	s = socket(AF_INET, SOCK_DGRAM, IPPROTO_UDP);
	if (s == INVALID_SOCKET)
		return (MOS_ERROR(iop, mos_windows_wsa_error(iop, WSAGetLastError()), "socket() failed"));

	err = connect(s, &addr->sa, sizeof (addr->sa));
	if (err != 0) {
		err = mos_windows_wsa_error(iop, WSAGetLastError());
		closesocket(s);
		return (MOS_ERROR(iop, err, "failed to connect UDP socket to 0x%x", *addr));
	}

	*sock = s;
	return (0);
}

MOSAPI int MOSCConv
mos_netop_udp_closesocket(mosiop_t iop, mos_socket_t *sock) {
	int err;

	err = closesocket(*sock);
	if (err != 0)
		return (MOS_ERROR(iop, mos_windows_wsa_error(iop, WSAGetLastError()), "closesocket() failed"));

	*sock = INVALID_SOCKET;

	return (0);
}

MOSAPI int MOSCConv
mos_netop_udp_send(mosiop_t iop, mos_socket_t *sock, const void *v, size_t *len) {
	ssize_t res;

	res = send(*sock, v, (int)*len, 0);
	if (res < 0)
		return (MOS_ERROR(iop, mos_windows_wsa_error(iop, WSAGetLastError()), "send() failed"));

	*len = (size_t)res;

	return (0);
}

MOSAPI int MOSCConv
mos_netop_udp_recv(mosiop_t iop, mos_socket_t *sock, void *v, size_t *len) {
	ssize_t res;

	res = recv(*sock, v, (int)*len, 0);
	if (res < 0)
		return (MOS_ERROR(iop, mos_windows_wsa_error(iop, WSAGetLastError()), "recv() failed"));

	*len = (size_t)res;

	return (0);
}

MOSAPI int MOSCConv
mos_netop_udp_setnonblocking(mosiop_t iop, mos_socket_t *sock, int on) {
	int res;

	res = ioctlsocket(*sock, FIONBIO, (u_long *)&on);
	if (res != NO_ERROR)
		return (MOS_ERROR(iop, mos_windows_wsa_error(iop, WSAGetLastError()),
		  "ioctlsocket(FIONBIO, %d) failed"), on);

	return (0);
}

