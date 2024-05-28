#ifndef _DICTIONARY_IMPL_H_
#define _DICTIONARY_IMPL_H_

#define mos__unused __unused

#include "mos/mos_os.h"
#include "mos/mos_iop.h"
#include "mos/bsdtree.h"
#include "mos/mos_lock.h"

#define DICTIONARY_NAMESPACEMAX	32
#define DICTIONARY_PATHMAX		255
#define DICTIONARY_VALMAX		1024
#define DICTIONARY_ACTMAX		255

#define DICTIONARY_KVSZ			(DICTIONARY_PATHMAX + 1 + DICTIONARY_VALMAX + 2)
#define DICTIONARY_BUFSZ		(DICTIONARY_NAMESPACEMAX + 1 + DICTIONARY_ACTMAX + 1 + DICTIONARY_KVSZ)

#define DICTIONARY_PERSIST	0x01	/* is not deleted when the owner detaches */

typedef union tkey {
	const char	*ckey;
	char		*key;
} tkey_t;

typedef struct _dictionary dictionary_t;

typedef struct _dictent {
	dictionary_t	*de_dictionary;
	uint32_t		de_owner;
	int				de_flags;
#define de_key de_tkey.key
	tkey_t			de_tkey;
	char			*de_val;
	int				de_refcnt;
	mos_mutex_t		de_lock;
	RB_ENTRY(_dictent) de_tlink;
} dictent_t;

typedef void(*derelease_t)(const char *, dictent_t *);

int dictentcompare(dictent_t *, dictent_t *);
typedef RB_HEAD(dictionary, _dictent) dicttree_t;
RB_PROTOTYPE(dictionary, _dictent, de_tlink, dictentcompare)

struct _dictionary {
#define dc_key dc_tkey.key
	tkey_t		dc_tkey;
	mos_mutex_t	dc_lock;
	dicttree_t	dc_tree;
	uint32_t	dc_count;
	RB_ENTRY(_dictionary) dc_link;
};

int dictionarycompare(dictionary_t *, dictionary_t *);
typedef RB_HEAD(dictionaries, _dictionary) dictionariestree_t;
RB_PROTOTYPE(dictionaries, _dictionary, dc_link, dictionarycompare)

void createdictionary(const char *, dictionary_t **);
void destroydictionary(dictionary_t **);
void releasefromdictionarybyowner(const char *, dictionary_t *, uint32_t, derelease_t);

int adddictentry(mosiop_t, dictionary_t *, uint32_t, int, const char *, const char *, dictent_t **);
int deldictentry(mosiop_t, dictionary_t *, int, const char *);
int getdictentry(mosiop_t, dictionary_t *, const char *, dictent_t **);
int getdictentry_re(mosiop_t, dictionary_t *, const char *, dictent_t **);
int updatedictentry(mosiop_t, dictionary_t *, uint32_t, int, const char *, const char *, dictent_t **);
void releasedictent(dictent_t **, int);

PhidgetReturnCode getKeyValue(mosiop_t, netreq_t *, char *, size_t, char *, size_t, char *, size_t,
  char *, size_t);
PhidgetReturnCode setKeyValue(mosiop_t, char *, size_t *, const char *, const char *,
  const char *, const char *);

#endif /* _DICTIONARY_IMPL_H_ */
