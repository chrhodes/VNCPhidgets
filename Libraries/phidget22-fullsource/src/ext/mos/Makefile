LIB=			mos
SHLIB_MAJOR=	1

CFLAGS+=		-DSHA2_UNROLL_TRANSFORM -DMOS_MOS_SOURCE

OPTIM_GROUPS=	TRUSTED

#
# Enable -Wshadow in the kernel for this lib only.
# We do this because this is the only place that has platform specific
# code that could see bugs creep in due to shadowing.  Pretty much everything
# else in the repo is compiled on various platforms and in both user and kern.
#
.if ${COMPILER} == "GCC" || ${COMPILER} == "CLANG"
. if ${_SPACE} == "kern"
CFLAGS+=		-Wshadow
. endif
.endif

.if ${COMPILER} == "CLANG"
WARNS=8	# turn off cast-align
.endif

SRCS=			addr.c \
				base.c \
				base64.c \
				byteorder.c \
				crc32.c \
				ctype.c \
				glob.c \
				glock.c \
				hexdump.c \
				iop.c \
				malloc.c \
				memchr.c \
				memcmp.c \
				memmem.c \
				mos_bits.c \
				mos_os.c \
				mos_tlock.c \
				mos_varchar.c \
				path.c \
				printf-smart.c \
				rand48.c \
				random.c \
				readline.c \
				rwrlock.c \
				sha1.c \
				sha2.c \
				pkcs5_pbkdf2.c \
				stats.c \
				strcasecmp.c \
				strchr.c \
				strstr.c \
				strnstr.c \
				strrchr.c \
				strrev.c \
				endswith.c \
				strcmp.c \
				strdup.c \
				strlcat.c \
				strlcpy.c \
				strlen.c \
				strtrim.c \
				strncmp.c \
				strncpy.c \
				strnsep.c \
				scanf.c \
				snprintf.c \
				strton.c \
				time.c \
				typeio.c \
				urldecode.c \
				urlencode.c \
				vasprintf.c

.if ${_SYSNAME} != "Darwin" || ${_SPACE} != "kern"
SRCS+=			md5c.c
.endif

INCS=			mos_assert.h \
				mos_atomic.h \
				mos_base64.h \
				mos_basic.h \
				mos_basic_types.h \
				mos_byteorder.h \
				mos_crc32.h \
				mos_crc32_impl.h \
				mos_dl.h \
				mos_endian-i386.h \
				mos_fmt.h \
				mos_iop.h \
				mos_hexdump.h \
				mos_lock.h \
				mos_os.h \
				mos_macrocompat.h \
				mos_md5.h \
				mos_random.h \
				mos_readline.h \
				mos_rwrlock.h \
				mos_service.h \
				mos_sha1.h \
				mos_sha2.h \
				mos_stacktrace.h \
				mos_task.h \
				mos_time.h \
				mos_tlock.h \
				mos_typeio.h \
				mos_urldecode.h \
				mos_urlencode.h \
				pkcs5_pbkdf2.h \
				bsdqueue.h \
				bsdtree.h

. if ${_SPACE} == "user"
.  if ${_SYSNAME} == "Windows"
SRCS+=			process-Windows.c
.  else
SRCS+=			process-unix.c
.  endif
SRCS+=			malloc-user.c mkdirp.c getenv.c getcwd.c
INCS+=			mos_getpasswd.h mos_readdir.h
. endif

.if ${_SYSNAME} == "Windows"
INCS+=			mos_basic_types-win.h \
				mos_mkstemp-Windows-user.h
. if ${_SPACE} != "kern"
SRCS+=			getopt.c \
				mos_setenv-Windows-user.c \
				mos_mkstemp-Windows-user.c
. else
SRCS+=			malloc-Windows-kern.c
SRCS+=			irp-Windows-kern.c
INCS+=			mos_irp.h
. endif
.endif

.if ${_SYSNAME} == "SunOS"
. if ${_SPACE} == "kern"
SRCS+=			malloc-SunOS-kern.c
. endif
.endif

.if ${_SYSNAME} == "FreeBSD"
. if ${_SPACE} == "kern"
SRCS+=			malloc-FreeBSD-kern.c
. endif
.endif

.if ${_SYSNAME} == "FreeBSD"

SRCS+=			mos_error-errno.c
INCS+=			mos_error-errno.h

. if ${_SPACE} == "user"
SRCS+=			readdir-unix.c \
				mos_dl-unix.c \
				mos_atomic-pthread.c \
				getpasswd-unix.c \
				mos_lock-pthread.c \
				mos_random-unix.c \
				mos_task-pthread.c \
				mos_time-unix.c \
				init_daemon.c

SRCS+=			mos_stacktrace-gnu.c

INCS+=			init_daemon.h
. else

SRCS+=			mos_atomic-FreeBSD-kern.c \
				mos_lock-FreeBSD-kern.c \
				mos_random-dummy.c \
				mos_task-FreeBSD-kern.c \
				mos_time-FreeBSD-kern.c \
				mos_stacktrace-dummy.c

. endif # user
.endif # FreeBSD

.if ${_SYSNAME} == "Darwin"

SRCS+=			mos_error-errno.c
INCS+=			mos_error-errno.h

. if ${_SPACE} == "user"
SRCS+=			mos_error-errno.c \
				readdir-unix.c \
				mos_dl-unix.c \
				mos_atomic-Darwin-user.c \
				getpasswd-unix.c \
				mos_lock-pthread.c \
				mos_random-unix.c \
				mos_stacktrace-dummy.c \
				mos_task-pthread.c \
				mos_time-unix.c \
				init_daemon.c

INCS+=			init_daemon.h

.else # Kern

SRCS+=			mos_atomic-Darwin-kern.c \
				malloc-Darwin-kern.c \
				mos_lock-Darwin-kern.c \
				mos_random-Darwin-kern.c \
				mos_rwlock-generic.c \
				mos_stacktrace-gnu.c \
				mos_task-Darwin-kern.c \
				mos_time-Darwin-kern.c

. endif #!user
.endif # Darwin

.if ${_SYSNAME} == "Linux"

. if ${_SPACE} == "user"
SRCS+=			mos_error-errno.c \
				readdir-unix.c \
				mos_dl-unix.c \
				mos_atomic-Linux-user.c \
				getpasswd-unix.c \
				mos_lock-pthread.c \
				mos_random-unix.c \
				mos_stacktrace-gnu.c \
				mos_task-pthread.c \
				mos_time-unix.c \
				init_daemon.c

INCS+=			init_daemon.h \
				mos_error-errno.h

. endif
.endif # Linux

.if ${_SYSNAME} == "SunOS"

. if ${_SPACE} == "user"
SRCS+=			mos_error-errno.c \
				readdir-unix.c \
				mos_dl-unix.c \
				mos_atomic-SunOS-user.c \
				getpasswd-SunOS-user.c \
				mos_lock-pthread.c \
				mos_random-unix.c \
				mos_stacktrace-SunOS-user.c \
				mos_task-pthread.c \
				mos_time-unix.c \
				init_daemon.c

INCS+=			init_daemon.h \
				mos_error-errno.h

. else			# Solaris kernel

SRCS+=			mos_atomic-SunOS-kern.c \
				mos_lock-SunOS-kern.c \
				mos_random-dummy.c \
				mos_task-SunOS-kern.c \
				mos_time-SunOS-kern.c

.  if ${_MACHINE} == "i386"
SRCS+=			mos_stacktrace-gnu.c
.  else /* not x86 */
SRCS+=			mos_stacktrace-dummy.c
.  endif

. endif
.endif # SunOS

.if ${_SYSNAME} == "Windows"

# include in user and kern mode
SRCS+=			mos_license-Windows.c
INCS+=			mos_license-Windows.h \
				mos_registry-Windows.h

. if ${_SPACE} == "user"
SRCS+=			mos_atomic-Windows-user.c \
				readdir-Windows.c \
				mos_dl-win.c \
				mos_error-Windows-user.c \
				mos_error-errno.c \
				getpasswd-win.c \
				mos_lock-Windows-user.c \
				mos_random-Windows-user.c \
				mos_registry-Windows-user.c \
				mos_service-Windows-user.c \
				mos_stacktrace-Windows-user.c \
				mos_rwlock-generic.c \
				mos_task-Windows-user.c \
				mos_time-Windows-user.c

INCS+=			mos_error-errno.h

. elif ${_SPACE} == "kern"
SRCS+=			mos_atomic-Windows-kern.c \
				mos_error-Windows-kern.c \
				mos_lock-Windows-kern.c \
				mos_random-Windows-kern.c \
				mos_registry-Windows-kern.c \
				mos_rwlock-generic.c \
				mos_stacktrace-Windows-kern.c \
				mos_task-Windows-kern.c \
				mos_time-Windows-kern.c

. else
.  error SPACE not set
. endif # user
.endif # Windows

INCS+=			mos_os-${_SYSNAME}-${_SPACE}.h

userclean:
	@rm -f *.So

.include <switch.mk>
