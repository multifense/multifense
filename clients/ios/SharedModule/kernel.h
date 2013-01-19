//
//  tdmp_kernel.h
//  MPTD
//
//  Created by Karl-Johan Alm on 5/4/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#ifndef MPTD_tdmp_kernel_h
#define MPTD_tdmp_kernel_h

//#define JNI_USED // currently this is the only way to do this; MANUALLY remove this when buildign for non-java platforms

#include "SharedModule.h"
#include "kerneldata.h"
#include "kerneleconomy.h"

#include <iostream>
#include <map>

using namespace std;

// Whenever the definitions of CALL-IN methods in the kernel change, the com_team4_mptd_Kernel.h file MUST be regenerated or there
// will be crashes and burns; you regenerate this method using the android NDK; if things are set up correctly, you should be able
// to, from the command prompt:
// 1. cd into the src folder under android/MPTD
// 2. execute ndk-build from the android-ndk-r8 folder, wherever that may be
// 3. move com_team4_mptd_Kernel.h to android/MPTD/jni/
// 4. COMMIT TO SVN SO YOUR FRIENDS DON'T CRASH
// NOTE: You DO NOT have to regenerate this file unless you have changed the definitions of the CALL-IN methods!!!
// CALL-IN methods are methods such as: setNickname, didRead, didCreateTower, didSpawnMonster, ...
// If you change the CODE of one of the call-in methods, that's okay too. Only if you change (add, remove, modify) the actual
// definitions.
#ifdef TDMP_JNI
#include "com_team4_mptd_Kernel.h"
#endif

/*
 * This is a list of all the possible callback types; TDMP_FTDECL is a macro which roughly expands to
 *   typedef void (name)
 * but slightly different depending on the platform. If you need to add a callback which takes arguments
 * different from all of the combinations below, or which are not accurately described below, simply add it
 * to the list and to the method signatures directly below (for JNI). There's no magic. Use 
 *   TDMP_FTDECL(method name)(arguments)
 * but make sure to use kint instead of int, and const char * instead of string.
 */
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

// Corresponding method signatures (for JNI) -- these are used when triggering the CALLBACK macro(s). If you add something above
// BE SURE TO ADD IT HERE TOO!
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
    // JNI does not use a kernelDelegate object whatsoever; it instead relies on the JNI linking; iOS and C# both use
    // this though
#ifndef TDMP_JNI
    class kernelDelegate
    {
    public:
        // C# needs (sort of) a constructor and destructor in the kernelDelegate; iOS doesn't
#ifndef IOS
		kernelDelegate();
		~kernelDelegate();
#endif
        // Here we list the various callbacks; if you add one, figure out if it's game, data, misc or debugging, then
        // add it as appropriate; every client handles modifications to this list in different ways -- for starters,
        // C#: the TdmpKernel monolithic method at the very bottom of this file (and in the cpp file) has to include
        // a reference to each of these, in a given order, and the C# client has to be updated to add a delegate
        // as well as an instance of said delegate in the TdmpKernel call. Hacky, but it works.
        // Android: nothing, actually. All callbacks from kernel are prepended with a "find method by name" feature.
        // Obviously, the actual method has to be added to Kernel.java, though, or the code will crash with a 
        // method not found error.
        // iOS: The ClientIphone class as well as the GameSession (Kernel) category must be updated to include the changes.
        
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
    
    // the opponent class is simply a data container for keeping track of names, and player id's; it can, and should, be 
    // expanded to do more stuff!
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
    
    // this maps player id's to opponent instances
    typedef map<int, opponent> playerList;
    
    // the kernel class is the core class of the system
    class kernel
    {
    public:
        kernel();
        ~kernel();
        
        // for JNI, we declare a jni method inline (remember inline? the code is copy-pasted all over the place rather than being a real method) and for the others, we define a kernelDelegate which is simply cli (for client)
#ifdef TDMP_JNI
        inline void jni(JNIEnv *e, jobject o) {
            lastEnv = e;
            lastObj = o;
        };
#else
        kernelDelegate *cli;
#endif
      
        // C# is whiny about which instance of the kernel class does what; in fact, C# does some hacky willy-wonka-hat-track where a
        // static instance of the kernel is created; but this is made of paper because as soon as you try to use a variable in the
        // object, it CRASHES AND BURNS; more on this later; for now though, the C# client has three additional methods identical 
        // to others except that they have an underscore prefix
#ifdef CSHARP
        void _didRead(const char *);
        void _loadData();
        void _loadMapData(int mapid);
#endif
      
        /*
         * This is where all the CALL-IN methods are defined; if you change anything here, you MUST regenerate the com_..._Kernel.h
         * file via JNI (ndk-build). See top of this file for details.
         * Make sure to use kint instead of int, const char * instead of string, and KERNCALL as the return argument.
         * Whenever you change something here, you need to update the JNI definitions below!!
         */
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
        
        // We have some private objects here as well; these include monster and tower types, as well as a bunch of info about
        // the player themselves, such as health, nick, player ID, as well as a pointer to an economy instance named eco
        // lastly, for JNI, we also have lastEnv and lastObj defined here; these are set every single time android calls the kernel
        // to whatever arguments are given from the client; this ensures that we have a Java VM available and that we have the object
        // (Kernel.java instance) available, at all times
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

// this is not a variable, this is a method; it provides something called a singleton instance -- an instance which is sort of static
// to some object, in this case a kernel; this is used by all platforms to get the default standard kernel so that we don't end up
// with a bunch of different versions; note that there is no way to delete this object -- it will exists forever until your app is
// terminated; this is okay, though, because as long as your app is running, it will remember this object
tdmp::kernel *defaultKern();

#ifdef TDMP_JNI

// (I don't know why, or even if, these are necessary, but I'm not about to go find out right now, so there you have it.)
// These reflect the call-in methods above. They are defined in the com_..._Kernel.h file, and they're also defined here using
// the KERNJNI / KERNRETJNI macros.
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

// This is the C# monolithic method of doom, which lets C# set up all callback methods in the kernel -- change a callback?
// Better update this one! (And the C# client! (or tell Simon!))
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
