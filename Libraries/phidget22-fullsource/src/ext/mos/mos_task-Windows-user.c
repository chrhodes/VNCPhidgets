
#include "mos_task.h"
#include "mos_os.h"
#include "mos_assert.h"

#include <aclapi.h>

/*
* We have to set up a restrictive SECURITY_ATTRIBUTES for thread creation
* so that Mono on Windows will not hang during shutdown.
* Mono calls OpenThread(THREAD_ALL_ACCESS, TRUE, threadId); on any native
* threads that it's seen via callbacks - we want this to fail, so we don't
* allow the full THREAD_ALL_ACCESS access. fixes Unity3D hang bug.
*/

int mosUseThreadSecurity = 0;

static PSECURITY_ATTRIBUTES pSA;
static PSECURITY_DESCRIPTOR pSD;
static SECURITY_ATTRIBUTES sa;
static PSID pEveryoneSID;
static PACL pACL;

void
_mos_task_fini() {

	if (pEveryoneSID) {
		FreeSid(pEveryoneSID);
		pEveryoneSID = NULL;
	}
	if (pACL) {
		LocalFree(pACL);
		pACL = NULL;
	}
	if (pSD) {
		LocalFree(pSD);
		pSD = NULL;
	}
	pSA = NULL;
}

void
_mos_task_init() {
	SID_IDENTIFIER_AUTHORITY SIDAuthWorld = SECURITY_WORLD_SID_AUTHORITY;
	EXPLICIT_ACCESS ea[1];
	DWORD dwRes;

	if (pSA != NULL || !mosUseThreadSecurity)
		return;


	// Create a well-known SID for the Everyone group.
	if (!AllocateAndInitializeSid(&SIDAuthWorld, 1, SECURITY_WORLD_RID, 0, 0, 0, 0, 0, 0, 0, &pEveryoneSID))
		MOS_PANIC("AllocateAndInitializedSid() failed %u", GetLastError());

	// Initialize an EXPLICIT_ACCESS structure for an ACE.
	// The ACE will limit access to everyone, so that OpenThread(THREAD_ALL_ACCESS) will fail from Mono
	ZeroMemory(&ea, sizeof(EXPLICIT_ACCESS));
	ea[0].grfAccessPermissions = STANDARD_RIGHTS_READ | THREAD_GET_CONTEXT | THREAD_QUERY_INFORMATION |
	  STANDARD_RIGHTS_EXECUTE | SYNCHRONIZE;
	ea[0].grfAccessMode = SET_ACCESS;
	ea[0].grfInheritance = NO_INHERITANCE;
	ea[0].Trustee.TrusteeForm = TRUSTEE_IS_SID;
	ea[0].Trustee.TrusteeType = TRUSTEE_IS_WELL_KNOWN_GROUP;
	ea[0].Trustee.ptstrName = (LPTSTR)pEveryoneSID;

	// Create a new ACL that contains the new ACEs.
	dwRes = SetEntriesInAcl(1, ea, NULL, &pACL);
	if (ERROR_SUCCESS != dwRes)
		MOS_PANIC("SetEntriesInAcl() failed %u", GetLastError());

	// Initialize a security descriptor.
	pSD = (PSECURITY_DESCRIPTOR)LocalAlloc(LPTR, SECURITY_DESCRIPTOR_MIN_LENGTH);
	if (NULL == pSD)
		MOS_PANIC("LocalAlloc() failed %u", GetLastError());

	if (!InitializeSecurityDescriptor(pSD, SECURITY_DESCRIPTOR_REVISION))
		MOS_PANIC("InitializeSecurityDescriptor() failed %u", GetLastError());

	// Add the ACL to the security descriptor.
	if (!SetSecurityDescriptorDacl(pSD, TRUE, pACL, FALSE))
		MOS_PANIC("SetSecurityDescriptorDacl() failed %u", GetLastError());

	// Initialize a security attributes structure.
	sa.nLength = sizeof(SECURITY_ATTRIBUTES);
	sa.lpSecurityDescriptor = pSD;
	sa.bInheritHandle = FALSE;

	pSA = &sa;
}

const DWORD MS_VC_EXCEPTION = 0x406D1388;

#pragma pack(push,8)
typedef struct tagTHREADNAME_INFO {
	DWORD dwType; // Must be 0x1000.
	LPCSTR szName; // Pointer to name (in user addr space).
	DWORD dwThreadID; // Thread ID (-1=caller thread).
	DWORD dwFlags; // Reserved for future use, must be zero.
} THREADNAME_INFO;
#pragma pack(pop)

/*
 * Raises an exception that the debugger can use to record the thread name.
 */
void
mos_task_setname(const char *threadName) {

	THREADNAME_INFO info;
	info.dwType = 0x1000;
	info.szName = threadName;
	info.dwThreadID = (DWORD)-1;	/* current thread */
	info.dwFlags = 0;

	__try {
		RaiseException(MS_VC_EXCEPTION, 0, sizeof(info) / sizeof(ULONG_PTR), (ULONG_PTR*)&info);
	} __except (EXCEPTION_EXECUTE_HANDLER) {
		;
	}
}

/*
 * Creates a task.
 */
MOSAPI int MOSCConv
mos_task_create(mos_task_t *task, MOS_TASK_RESULT (*start)(void *), void *arg) {

	return (mos_task_create2(task, 0, start, arg));
}

MOSAPI int MOSCConv
mos_task_create2(mos_task_t *task, uint32_t stacksz, MOS_TASK_RESULT (*start)(void *), void *arg) {
	DWORD tid;
	HANDLE t;
	int res;

	t = CreateThread(pSA, stacksz, start, arg, 0, &tid);
	if (!t) {
		res = mos_windows_error_to_err(GetLastError());
	} else {
		res = 0;
		if (task != NULL)
			*task = tid;
		CloseHandle(t);
	}

	return (res);
}

MOSAPI int MOSCConv
mos_task_exit(int ecode) {

	ExitThread(ecode);
	/*NOTREACHED*/
}

MOSAPI mos_task_t MOSCConv
mos_self(void) {

	return (GetCurrentThreadId());
}

MOSAPI int MOSCConv
mos_task_equal(mos_task_t t1, mos_task_t t2) {

	return (t1 == t2);
}

#if 0	/* left as and example, but a cross platform version is difficult and not encouraged. */
MOSAPI int MOSCConv
mos_task_join(mos_task_t t1, int *_res) {
	HANDLE handle;
	DWORD res;

	handle = OpenThread(SYNCHRONIZE, 0, t1);
	if (handle == NULL)
		return (MOSN_INVAL);

	res = WaitForSingleObject(handle, INFINITE);
	if (_res)
		*_res = res;

	CloseHandle(handle);
	return (0);
}
#endif
