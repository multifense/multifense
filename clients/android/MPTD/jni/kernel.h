//
//  tdmp_kernel.h
//  MPTD
//
//  Created by Karl-Johan Alm on 5/4/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#ifndef MPTD_tdmp_kernel_h
#define MPTD_tdmp_kernel_h

#define JNI_USED // currently this is the only way to do this; MANUALLY remove this when buildign for non-java platforms

#include "SharedModule.h"
#include "kerneldata.h"
#include "kerneleconomy.h"

#include <iostream>
#include <map>

#ifdef TDMP_JNI
//#include "se_mossan_AndroidJNITestActivity.h"
#include "com_team4_mptd_Kernel.h"
#endif

#ifdef _WIN32
#ifndef TDMP_JNI
#define CSHARP
#endif
#endif

using namespace std;

#ifndef TDMP_JNI
// Method definitions for CALLBACKS (game)
#ifdef IOS
#define TDMP_FTDECL(name) typedef void (^name)
#else
#define TDMP_FTDECL(name) typedef void (STDCALL *name)
#endif

TDMP_FTDECL(cbPlayerReferenceWithTypeAndCoords)(kint, kint, kint, kint, kint);
TDMP_FTDECL(cbPlayerReferenceWithType)(kint, kint, kint);
TDMP_FTDECL(cbReference)(kint);
TDMP_FTDECL(cbTime)(kint);
TDMP_FTDECL(cbPlayer)(kint);
TDMP_FTDECL(cbPlayerWithTime)(kint, kint);
TDMP_FTDECL(cbPlayerWithName)(kint, const char *);
TDMP_FTDECL(cbPlayerVersusPlayerWithArgument)(kint, kint, kint);
TDMP_FTDECL(cbVoid)();
TDMP_FTDECL(cbReferenceWithHealthViaPlayer)(kint, kint, kint);

// monster definition: type, sprite, health, speed, cost to send, income inc, coloring
TDMP_FTDECL(cbMonsterDefinition)(kint, const char *, kint, kint, kint, kint, kint);
// tower definition: type, sprite, damage, time between shots, range, cost, projektile sprite, projectile speed, projectile sound effect
TDMP_FTDECL(cbTowerDefinition)(kint, const char *, kint, kint, kint, kint, const char *, kint, const char *);
// path definition: x, y, dirx, diry, length
TDMP_FTDECL(cbPathDefinition)(kint,kint, kint,kint, kint);

// Method definitions for CALLBACKS (miscellaneous)
TDMP_FTDECL(cbNetIO)(const char *);
#endif

// Corresponding method signatures (for JNI)
#define SIGPlayerReferencWithTypeAndCoords "(IIIII)V"
#define SIGPlayerReferenceWithType          "(III)V"
#define SIGReference                        "(I)V"
#define SIGTime                             "(I)V"
#define SIGPlayer                           "(I)V"
#define SIGPlayerWithTime                   "(II)V"
#define SIGPlayerWithName                   "(ILjava/lang/String;)V"
#define SIGPlayerVersusPlayerWithArgument   "(III)V"
#define SIGVoid                             "()V"
#define SIGReferenceWithHealthViaPlayer     "(III)V"
#define SIGNetIO                            "(Ljava/lang/String;)V"
#define SIGMonsterDefinition                "(ILjava/lang/String;IIIII)V"
#define SIGTowerDefinition                  "(ILjava/lang/String;IIIILjava/lang/String;ILjava/lang/String;)V"
#define SIGPathDefinition                   "(IIIII)V"

namespace tdmp
{
#ifndef TDMP_JNI
    class kernelDelegate
    {
    public:
#ifndef IOS
		kernelDelegate();
		~kernelDelegate();
#endif
        // game
        cbPlayerReferenceWithTypeAndCoords  towerWasCreated;
        cbPlayerReferenceWithType           monsterWasCreated;
        cbReference                         monsterWasKilled;
        cbPlayer                            waveWasKilledForPlayer;
        cbPlayerVersusPlayerWithArgument    playerWasDamagedByPlayer;
        cbPlayer                            playerDied;
        cbPlayer                            playerSurrendered;
        cbReferenceWithHealthViaPlayer      monsterWithHealthWasSentByPlayer;
        cbTime                              nextWaveTime;
        cbPlayer                            victorWasDecided;

        // data
        cbMonsterDefinition                 monsterDefinition;
        cbTowerDefinition                   towerDefinition;
        cbPathDefinition                    pathDefinition;
        
        // misc
        cbNetIO                             netWrite;
        cbPlayerWithName                    playerJoined;
        cbPlayer                            playerLeft;
        cbPlayerWithTime                    didFindGame; // player = your own player ID; set me.playerID = id on call
        // debugging
        cbNetIO                             debugLog;
    };
#endif
    
    class opponent
    {
    public:
        string name;
        int lastcheck;
        int pid;
        
        opponent(int _pid, string _name) {
            pid = _pid;
            name = _name;
        };
        
        opponent() {};
    };
    
    typedef map<int, opponent> playerList;
    
    class kernel
    {
    public:
      kernel();
	  ~kernel();

#ifdef TDMP_JNI
        inline void jni(JNIEnv *e, jobject o) {
            lastEnv = e;
            lastObj = o;
        };
#else
        kernelDelegate *cli;
#endif
      
#ifdef CSHARP
        void _didRead(const char *);
        void _loadData();
        void _loadMapData(int mapid);
#endif

        KERNCALL setNickname(const char *);                             // set nickname
        KERNCALL didRead(const char *);                                 // read string 
        KERNCALL didCreateTower(kint tower, kint type, kint x, kint y); // user created a tower
        KERNCALL didSpawnMonster(kint monster, kint type);              // a monster was spawned on user's field
        KERNCALL didKillMonster(kint monster);                          // user's tower(s) killed the given monster
        KERNCALL didKillLastMonster();                                  // user's tower(s) killed the last monster in the wave
        KERNCALL didRecruitMonster(kint type, kint health);             // user recruited a monster of the given type (which is forwarded as appropriate), or it went thru
        KERNCALL didSurrender();                                        // user surrendered
        KERNCALL didTakeDamage(kint damage, kint culprit);              // user took damage
        KERNCALL didDie();                                              // user died from damage
        SMME(kint) updateSPIncome(kint waveCount);                      // (economy) update single player wave count 
        SMME(kint) getBountyForMonster(kint type);                      // (economy) how much $ should player get for killing mob
        SMME(kint) updateMPIncomeForBuyingMonster(kint type);           // (economy) increase and return new income for buying mob
        SMME(kint) mpIncome();
        SMME(kint) spIncome();

        // miscellaneous
        KERNCALL loadData();                                            // load data
        KERNCALL loadMapData(kint mapid);                               // load path points for a given map
        SMME(const char *) getNameForMap(kint mapid);                   // obtain the name of a given map
        SMME(const char *) getImageNameForMap(kint mapid);                 // obtain the .png name of a given map
        KERNCALL findGame(kint players);                                // find a game
	KERNCALL newGame();                                             // new game
        KERNCALL sharedModuleSelfTest();                                // self test to ensure functionality
        
    private:
        map<int,monster> monsterTypes;
        map<int,tower> towerTypes;
        long monsteridx;
        playerList players;
        short registered;
        int health;
        int iteridx;
        string nick;
        kcbint localPlayerID;
        economy eco;
        
#ifdef TDMP_JNI
        JNIEnv *lastEnv;
        jobject lastObj;
#endif
    };
}

tdmp::kernel *defaultKern();

#ifdef TDMP_JNI

KERNJNI(setNickname)(TDMP_FUNINIT, jstring s);
KERNJNI(didRead)(TDMP_FUNINIT, jstring s);
KERNJNI(didCreateTower)(TDMP_FUNINIT, jint tower, jint type, jint x, jint y);
KERNJNI(didKillMonster)(TDMP_FUNINIT, jint monster);
KERNJNI(didKillLastMonster)(TDMP_FUNINIT);
KERNJNI(didRecruitMonster)(TDMP_FUNINIT, jint type, jint health);
KERNJNI(didSurrender)(TDMP_FUNINIT);
KERNJNI(didTakeDamage)(TDMP_FUNINIT, jint damage, jint culprit);
KERNJNI(didDie)(TDMP_FUNINIT);
KERNJNI(findGame)(TDMP_FUNINIT, jint players);
KERNJNI(loadData)(TDMP_FUNINIT);
KERNJNI(sharedModuleSelfTest)(TDMP_FUNINIT);
KERNJNI(loadMap)(TDMP_FUNINIT, jint mapid);
KERNRETJNI(jstring, getNameForMap)(TDMP_FUNINIT, jint mapid);
KERNRETJNI(jstring, getImageNameForMap)(TDMP_FUNINIT, jint mapid);

#else

#ifdef IOS
extern "C" KERNCALL TdmpKernelWithDelegate(tdmp::kernelDelegate *dlg);
#endif

extern "C" KERNCALL STDCALL TdmpKernel(cbNetIO,// callback for debug messages
                                       cbNetIO,                             // callback for sending strings to server
                                       cbPlayerWithName,                    // playerJoined;
                                       cbPlayer,                            // playerLeft;
                                       cbPlayerWithTime,                    // didFindGame; player = your own player ID; set me.playerID = id on call
                                       cbPlayerReferenceWithTypeAndCoords,  // towerWasCreated
                                       cbPlayerReferenceWithType,           // monsterWasCreated
                                       cbReference,                         // monsterWasKilled
                                       cbPlayer,                            // waveWasKilledForPlayer
                                       cbPlayerVersusPlayerWithArgument,    // playerWasDamagedByPlayer
                                       cbPlayer,                            // playerDied
                                       cbPlayer,                            // playerSurrendered
                                       cbReferenceWithHealthViaPlayer,      // monsterWithHealthWasSentByPlayer
                                       cbTime,                              // next wave in (time) 
                                       cbPlayer,                            // victor was decided
                                       cbMonsterDefinition,                 // monster definition
                                       cbTowerDefinition,                   // tower definition
                                       cbPathDefinition                     // path definition
                                       );

#endif

#endif
