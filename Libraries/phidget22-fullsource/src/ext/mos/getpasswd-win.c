#include <conio.h>
#include "mos_os.h"
#include "mos_getpasswd.h"

#define MAX_PASSWD_SIZE	64

MOSAPI int MOSCConv
mos_getpasswd(mosiop_t iop, const char *prompt, char *pwd, size_t pwdsz) {
	size_t n;
	int c;

	n = 0;

	if (pwdsz == 0)
		return (MOS_ERROR(iop, MOSN_INVALARG, "pwd buffer too small"));

	_cputs(prompt);

	for (;;) {
		c = _getch();
		if (c == 0) {
			/* skip extended characters */
			(void)_getch();
			continue;
		} else if (c == '\b') {
			/* backspace */
			if (n > 0)
				n--;
		} else if (c == 27 || c == 21) {
			/* kill input on escape or ^U */
			n = 0;
		} else if (c == '\r' || c == '\n') {
			/* stop at newline */
			break;
		} else if (n < pwdsz - 1 && isprint(c)) {
			/* add character to password */
			pwd[n++] = (char)c;
		}
	}

	/* null-terminate */
	pwd[n] = '\0';

	_cputs("\n");

	return (0);
}
