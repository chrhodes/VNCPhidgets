#include "mos_os.h"
#include <winsvc.h>
#include "mos_service.h"

MOSAPI int MOSCConv
mos_service_install(mosiop_t iop, const char *svcname, const char *svcdesc,
  const char *path) {
	SERVICE_DESCRIPTIONA sd;
	SC_HANDLE svc;
	SC_HANDLE scm;
	int err;

	char modpath[MAX_PATH];
	uint32_t wdesclen;
	char *wdesc;

	if (path == NULL) {
		if (!GetModuleFileNameA(NULL, modpath, MAX_PATH))
			return (MOS_ERROR(iop, mos_windows_error(iop, GetLastError()),
			  "failed to get process's filename"));
		path = modpath;
	}

	/* get a handle to the local SCM database with full access */
	scm = OpenSCManager(NULL, NULL, SC_MANAGER_ALL_ACCESS);
	if (scm == NULL)
		return (MOS_ERROR(iop, mos_windows_error(iop, GetLastError()),
		  "failed to open service control manager"));

	/* create the service */
	svc = CreateServiceA(scm, svcname, NULL, SERVICE_ALL_ACCESS,
	  SERVICE_WIN32_OWN_PROCESS, SERVICE_DEMAND_START, SERVICE_ERROR_NORMAL,
	  path, NULL, NULL, NULL, NULL, NULL);
	if (svc == NULL) {
		err = mos_windows_error(iop, GetLastError());
		CloseServiceHandle(scm);
		return (MOS_ERROR(iop, err, "failed to create service"));
	}

	/* set the description */
	wdesc = mos_strdup(svcdesc, &wdesclen);
	sd.lpDescription = wdesc;
	if (!ChangeServiceConfig2A(svc, SERVICE_CONFIG_DESCRIPTION, &sd)) {
		err = mos_windows_error(iop, GetLastError());
		MOS_ERROR(iop, err, "failed to set service description");
	} else {
		err = 0;
	}
	mos_free(wdesc, wdesclen);

	CloseServiceHandle(svc);
	CloseServiceHandle(scm);

	return (err);
}
