//
//  tdmp_kernel.cpp
//  MPTD
//
//  Created by Karl-Johan Alm on 5/4/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#include <assert.h>

#include "kernel.h"

#include <string>
#include <stdlib.h>
#include <sstream>
#include <vector>

using namespace std;

#ifdef CSHARP
tdmp::kernelDelegate::kernelDelegate()
{
	cout << "kdelegate init: " << this << endl;
}

tdmp::kernelDelegate::~kernelDelegate()
{
	cout << "kdelegate destruct: " << this << endl;
}
#endif

// what you see below is called a singleton; this is explained in the kernel.h file but basically, it's a very very common 
// technique, at least in iOS, where you create a "default" instance of a given object that you can refer to from anywhere
static tdmp::kernel *defaultKernel = NULL;

tdmp::kernel *defaultKern() 
{
	if (defaultKernel == NULL) {
		defaultKernel = new tdmp::kernel();
	}
	return defaultKernel;
}

// the constructor of the kernel sets some of the values up as well as creates a kernelDelegate instance, for non-JNI clients
// the reason for the numbers used (1111, 2222) is debugging purposes; their initial value is of no consequence; what matters
// is that they're unique, and by seeing "111x" we know we're looking at the iteridx, and "222x" we know we're looking at
// the monsteridx
// if localPlayerID == -1, it's unset; if it's any other number, it means it's set (that includes 0). 
tdmp::kernel::kernel()
{
	cout << "kernel initializing: " << this << endl;
	localPlayerID = -1;
    iteridx = 1111;
    monsteridx = 2222;
#ifndef TDMP_JNI
	cli = new kernelDelegate();
#endif
}

tdmp::kernel::~kernel()
{
	cout << "kernel destructing: " << this << endl;
#ifndef TDMP_JNI
    delete cli;
#endif
}

// Creating a new game simply means resetting economy; later on, we may need to beef this guy up a bit
KERNRET tdmp::kernel::newGame()
{
  defaultKernel->eco = economy();
}

KERNRET tdmp::kernel::setNickname(const char *s)
{
    if (defaultKernel != this) {
        defaultKernel->setNickname(s);
        return;
    }
    // we use stringstreams to "build" strings; it's not like Java or such where you can do string s = "i am " + i + " years old"
    stringstream out;
    out << "0..0..NAME," << s << "\r\n";
    // out.str().c_str() is simply converting the stringstream to a string, and then converting that to a C string, i.e. a (const) char *
    NetWrite(out.str().c_str());
    
    // when you set the nick, you stop being registered and have to re-register
    registered = 0;
    nick = s;
    // below is simply debugging
    string dbg = "nickname set to " + nick;
    DebugLog(dbg.c_str());
}

// here some magic occurs so pay attention; we define didRead as a good old regular method but right after the method name, ...
KERNRET tdmp::kernel::didRead(const char *s)
// we check if this is C#. If it's C# we need to route into the defaultKernel object, because C# crashes if we use the variables
// in the called-to object; so we define didRead as a one-line method which calls _didRead (underscore didRead) in the 
// defaultKernel object
#ifdef CSHARP
{
    defaultKernel->_didRead(s);
}

// and then (still in the C# ifdef) we redefine _didRead, and then the #ifdef ends and the didRead method begins
// in other words, for C# we have
// didRead(s): 
//   defaultKernel->_didRead(s)
// _didRead(s):
//   ...
// and for the others we have
// didRead(s):
//   ...
void tdmp::kernel::_didRead(const char *s)
#endif
{
    // we define str as a string with 's' as its value
    string str(s);
    string dbg = "didRead(" + str + ")";
    DebugLog(dbg.c_str());
    
    // crash and burn if str does NOT end with \r\n -- the windows "CRLF"
    assert(str.substr(str.length()-2) == "\r\n");
    
    // we split str into sender and argument ("a..b..c..d,arg,arg2,arg3,arg4" into "a..b..c..d" and "arg,arg2,arg3,arg4")
    // if arguments exist
    string sender;
    string argument;
    size_t delim = str.find(',', 0);
    if (delim == string::npos) {
        // no arguments
        sender = str.substr(0,str.length()-2); // remove rn
        argument = "";
    } else {
        // arguments
        sender = str.substr(0, delim);
        argument = str.substr(delim + 1, str.length()-2 - delim - 1);
    }
    
    // this is where the ugly hack begins; this code should be rewritten to call an instance method depending on the 
    // "START", "SPAWN", etc. part of the sender string, but I haven't had the time to look into how to do this
    // As it is, it's one big if else hairball. Don't ever presume to think this is okay. You'll get fired quicker than
    // you can say リストラ.
    if (sender == "0..0..START") {
        // game starting
        // this is the first time we see this so I will comment: the assert(expression && "text") is a quick and dirty way to
        // make a message appear that you can understand, when you encounter an assert; what it logically says is,
        //   make sure that <something to check> is not 0, AND THAT "message" is not 0
        // but you realize that "message" is not ever ever going to be 0, even if it happened to be "0", so the latter is always
        // true; thus, if the former is true, the assert is true, and if the former is false, the assert is false
        assert(registered && "start without registered");
        // we may want to set the default health somewhere more global
        health = 10;
        // set the timer (or rather, the "seconds to wait variable") to the argument
        kcbint timer = atoi(argument.c_str());
        // first time we do a callback so going to explain a bit; the CALLBACK macro is sort of like a sieve; it takes arguments
        // required for all 3 platforms, and then picks out the ones it needs and makes the callback; for C# and iPhone, you 
        // obviously don't care about the method signature (SIGPlayerWithTime, here) but it's passed because this exact macro is
        // precompiled for JNI as well, and it IS used there! You do CALLBACK(method_name, signature, argument1, argument2, ......)
        // and that's that; all callbacks which take at least one string have a macro of their own, however, such as PlayerJoined,
        // which you will see below; look at SharedModule.h around the bottom and you'll see all of them ultimately using the
        // CALLBACK macro
        // There's nothing magic about it. Think of it as any old function, but it's a function for calling methods, and it wants
        // a signature argument before the actual method arguments.
        CALLBACK(didFindGame, SIGPlayerWithTime, localPlayerID, timer);
    } else if (sender == "0..0..SPAWN") {
        // someone sent (us) a monster
        // args : monster type, health, player id
        assert(registered && "spawn without registered");
        // this gooey part may need some explaining, though it's quite simple
        // (atoi = "string to integer" method in C)
        // we are stepping through the argument string, one step per existing comma (",")
        // the first time we look for the first comma, so nothing odd there
        delim = argument.find(',', 0);
        kcbint player = atoi(argument.substr(0,delim).c_str());
        // the second time, we have to look starting from the position after the first comma (delim+1), or
        // we'll simply find the same comma again
        size_t delim2 = argument.find(',', delim+1);
        // the substr goes from the position after the first comma, to...
        // the position of the second comma,
        // but since we STARTED at "delim+1", we now have to subtract delim-1 to get the number of letters
        // in between the two commas
        kcbint type = atoi(argument.substr(delim+1,delim2 - delim - 1).c_str());
        // that second comma is supposedly the final one, so we grab everything after that comma and stuff it into health
        kcbint health = atoi(argument.substr(delim2+1).c_str());
        
		cout << "type = " << type << ", health left = " << health << endl << ", sender = " << player << endl;
        CALLBACK(monsterWithHealthWasSentByPlayer, SIGReferenceWithHealthViaPlayer, type, health, player);
    } else if (sender == "0..0..REGISTERED") {
        localPlayerID = atoi(argument.c_str());
        registered = 1;
    } else if (sender == "0..0..MOBWAVE") {
        // next mobwave begins in <time>
        assert(registered && "mobwave without registered");
        kcbint time = atoi(argument.c_str());
        CALLBACK(nextWaveTime, SIGReference, time);
    } else if (sender == "0..0..GAMEROOM") {
        // game room has updated
        //  0..0..GAMEROOM,Player1Name:id1,Player2Name:id2,Player3Name:id3, etc... (Vilka spelare som är i gameroomet!)
        assert(registered && "gameroom without registered");
        int done = 0;
        // iteridx is updated every time we do this; it is used to determine when a player was NOT accounted for in the loop below; if a player is not accounted for
        // they have left the game
        iteridx++;
        vector<opponent> newPlayers = vector<opponent>();

        // this while loop is quite messy, but its purpose is to:
        // 1. send a message to the clients when a player it has never seen before appeared in the game room
        // 2. send a message when a player that was there before is no longer there
        // 3. NOT send messages about players the clients already know about
        size_t delim2;
        while (! done) {
            string v;
            // see above explain; delim is the position of the first comma
            delim = argument.find(',',0);
            // string::npos means "no position" or something like that; this basically means we DID NOT FIND a comma
            if (delim == string::npos) {
                // and that means this is the last person in the list
                done = 1;
                v = argument;
                argument = "";
            } else {
                // we actually update argument each loop to make this less of a pain; we also first set v to
                // the first player in the list
                v = argument.substr(0, delim);
                argument = argument.substr(delim+1);
            }
            
            // It's playername:playerid --- so we split by ":"
            delim2 = v.find(':',0);
            // we also REQUIRE that we found a ":"
            assert (delim2 != string::npos);
            string pname = v.substr(0,delim2);
            int pid = atoi(v.substr(delim2+1).c_str());
            // if this is ourselves (which it is, sometimes), we don't do anything, we know about us already, and we will never
            // leave a game we're still in
            if (pid != localPlayerID) {
                // two cases -- we have this player in our list of players, or we don't; if we don't, yay, we found a new player!
                //playerList::iterator i = players.find(pid); 
                if (players.find(pid) != players.end()) {
                    // player is in the list; we set lastcheck to iteridx, because later on, we test every player in our player list if they have been
                    // appropriately updated; if not, they left the game!
                    players[pid].lastcheck = iteridx;
                } else {
                    // player is not in the list -- that means they joined
                    players[pid] = opponent(pid, pname);
                    players[pid].lastcheck = iteridx;
                    newPlayers.push_back(players[pid]);
                }
            }
        }
        
        // after looping thru, we now check the lastcheck versus the iteridx value as described above
        playerList::iterator it = players.begin();
        while (it != players.end()) {
            const int cid = it->first;
            opponent opp = players[cid];
            if (opp.lastcheck != iteridx) {
                // this player was not in the room list
                CALLBACK(playerLeft, SIGPlayer, cid);
                players.erase(it);
                it = players.begin();
            } else {
                it++;
            }
        }
        // finally, we loop through the newPlayers vector and send a message about players joining
        for (vector<opponent>::iterator it = newPlayers.begin(); it != newPlayers.end(); it++) {
            PlayerJoined(it->pid, it->name.c_str());
        }
    } else if (sender == "0..0..DAMAGE") {
        //  0..0..DAMAGE,int taker, int giver, int damage (taker took damage damage because of giver)
        // args : int taker, int giver, int damage
        assert(registered && "damage without registered");
        delim = argument.find(',', 0);
        kcbint taker = atoi(argument.substr(0,delim).c_str());
        size_t delim2 = argument.find(',', delim+1);
        kcbint giver = atoi(argument.substr(delim+1,delim2 - delim - 1).c_str());
        kcbint damage = atoi(argument.substr(delim2+1).c_str());
        if (giver == localPlayerID) {
            giver = 0;
            health++;
            stringstream out;
            out << localPlayerID << "..0..LIFE," << health << ",0\r\n";
            NetWrite(out.str().c_str());
        }
        
        CALLBACK(playerWasDamagedByPlayer, SIGPlayerVersusPlayerWithArgument, taker, giver, damage);
    } else if (sender == "0..0..VICTOR") {
        assert(registered && "victor without registered");
        kcbint player = atoi(argument.c_str()); // .substr(0,delim).c_str()
        CALLBACK(victorWasDecided, SIGPlayer, player == localPlayerID ? 0 : player);
        players.clear();
    } else {
        dbg = "UNRECOGNIZED COMMAND: " + str;
        DebugLog(dbg.c_str());
    }
}

// here we do the same trick we did for didRead above
KERNRET tdmp::kernel::loadData()
#ifdef CSHARP
{
    defaultKernel->_loadData();
}

void tdmp::kernel::_loadData()
#endif
{
    monsterTypes.clear();
    towerTypes.clear();
    
    // monster definition: type, sprite, health, speed, cost to send, income inc, coloring
    MonsterDefinition(101, "bombsprite.png", 300, 3, 20, 4, 0);
    MonsterDefinition(102, "slimesprite.png", 75, 3, 5, 1, 0);
    MonsterDefinition(103, "slimesprite.png", 170, 4, 12, 2, 0);
    MonsterDefinition(104, "hatty.png", 2100, 3, 110, 18, 0);
    MonsterDefinition(105, "rocky.png", 3000, 3, 150, 25, 0);
    
    // tower definition: type, sprite, damage, time between shots, range, cost, projektile sprite, projectile speed
    TowerDefinition(1, "cannontower.png", 10, 30, 200, 15, "cannonproj.png", 15, "cannon.mp3");
    TowerDefinition(2, "magictower.png", 25, 75, 300, 25, "magicproj.png", 30, "magic.mp3");
    TowerDefinition(3, "firetower.png", 7, 3, 150, 125, "fireprojsprite.png", 15, "fire.mp3");
    TowerDefinition(4, "igloo.png", 100, 90, 350, 90    , "igloproj.png", 40, "ice.mp3");
}

/****
 **** MAP METHODS
 ****/

KERNRET tdmp::kernel::loadMapData(int mapid)
#ifdef CSHARP
{
    defaultKernel->_loadMapData(mapid);
}

void tdmp::kernel::_loadMapData(int mapid)
#endif
{
    switch (mapid) {
        case 2:
            //              x       y       dirx    diry    length
            PathDefinition( 200,    0,      0,      1,      400);
            PathDefinition( 200,    400,    -1,     0,      100);
            PathDefinition( 100,    400,    0,      1,      200);
            PathDefinition( 100,    600,    1,      0,      1300);
            PathDefinition( 1400,   600,    0,      -1,     450);
            PathDefinition( 1400,   150,    -1,     0,      850);
            PathDefinition( 550,    150,    0,      1,      650);
            PathDefinition( 550,    800,    1,      0,      600);
            PathDefinition( 1150,   800,    0,      1,      160);
            PathDefinition( 1150,   800,    0,      1,      1);
            break;
            
        default:
            //              x       y       dirx    diry    length
            PathDefinition( 0,      190,    1,      0,      140);
            PathDefinition( 140,    190,    0,      1,      545);
            PathDefinition( 140,    735,    1,      0,      350);
            PathDefinition( 490,    735,    0,      -1,     250);
            PathDefinition( 490,    485,    1,      0,      205);
            PathDefinition( 695,    485,    0,      -1,     400);
            PathDefinition( 695,    85,    1,      0,      230);
            PathDefinition( 925,    85,    0,      1,      660);
            PathDefinition( 925,    745,    1,      0,      385);
            PathDefinition( 1310,   745,    0,      -1,     450);
            PathDefinition( 1310,   295,    1,      0,      290);
            PathDefinition( 1600,   295,    1,      0,      1);
            break;
    }
}

SMME(const char *) tdmp::kernel::getNameForMap(int mapid)
{
    switch (mapid) {
        case 2:
            return "Fields of Dechamak";
        default:
            return "Glacier of Despair";
    }
}

SMME(const char *) tdmp::kernel::getImageNameForMap(int mapid)
{
    switch (mapid) {
        case 2:
            return "awsomemap.png";
        default:
            return "map1600x960.png";
    }
}

/****
 **** ECONOMY METHODS
 ****/

SMME(kint) tdmp::kernel::mpIncome() 
{
  return defaultKernel->eco.mpIncome;
}

SMME(kint) tdmp::kernel::spIncome()
{
  return defaultKernel->eco.spIncome;
}

SMME(kint) tdmp::kernel::getBountyForMonster(int type)
{
    if (defaultKernel != this) {
        return defaultKernel->getBountyForMonster(type);
    }
    assert(monsterTypes.find(type) != monsterTypes.end() && "monster type not found!");
    monster m = monsterTypes[type];
    return eco.getMonsterMoney(m);
}

SMME(kint) tdmp::kernel::updateSPIncome(int waveCount)
{
    return defaultKernel->eco.updateSPIncome(waveCount);
}

SMME(kint) tdmp::kernel::updateMPIncomeForBuyingMonster(int type)
{
    if (defaultKernel != this) {
        return defaultKernel->updateMPIncomeForBuyingMonster(type);
    }
    assert(monsterTypes.find(type) != monsterTypes.end() && "monster type not found!");
    monster m = monsterTypes[type];
    return eco.buyMonster(m);
}

// this method was meant to do all kinds of tests that could pass, but it ended up being a test for testing that asserts work
// it should probably be upgraded
KERNRET tdmp::kernel::sharedModuleSelfTest()
{
    // there is one test, and it fails, until we confirm that all clients fail on failures
    assert(0);
}

KERNRET tdmp::kernel::findGame(kint players)
{
    defaultKernel->players.clear();
    stringstream out;
    out << defaultKernel->localPlayerID << "..0..JOINRAN," << players << "\r\n";
    NetWrite(out.str().c_str());
}

// user created a tower
KERNRET tdmp::kernel::didCreateTower(kint tower, kint type, kint x, kint y)
{
    // IGNORED
}

// a monster was spawned on user's field
KERNRET tdmp::kernel::didSpawnMonster(kint monster, kint type) 
{
    // IGNORED
}

// user's tower(s) killed the given monster
KERNRET tdmp::kernel::didKillMonster(kint monster)
{
    // IGNORED
}

// user's tower(s) killed the last monster in the wave
KERNRET tdmp::kernel::didKillLastMonster() 
{
    stringstream out;
    out << defaultKernel->localPlayerID << "..0..LASTMONSTERKILLED\r\n";
    NetWrite(out.str().c_str());
}

// user recruited a monster of the given type (which is forwarded as appropriate)
EXPORT void  tdmp::kernel::didRecruitMonster(kint type, kint health)
{
    if (defaultKernel != this) {
        defaultKernel->didRecruitMonster(type, health);
        return;
    }
    monsteridx++;
    stringstream out;
    out << defaultKernel->localPlayerID << "..0..RECMONSTER," << monsteridx << "," << type << "," << health << "\r\n";
    NetWrite(out.str().c_str());
}

// user surrendered
KERNRET tdmp::kernel::didSurrender()
{
    stringstream out;
    out << defaultKernel->localPlayerID << "..0..QUIT\r\n";
    NetWrite(out.str().c_str());
}

// user took damage
KERNRET tdmp::kernel::didTakeDamage(kint damage, kint culprit)
{
#ifdef CSHARP
    if (defaultKernel != this) return defaultKernel->didTakeDamage(damage, culprit);
#endif
    health -= damage;
    stringstream out;
    out << localPlayerID << "..0..LIFE," << health << "," << culprit << "\r\n";
    NetWrite(out.str().c_str());
}

// user died from damage
KERNRET tdmp::kernel::didDie()
{
    assert(health == 0);
}

// unfortunately, we have two separate TdmpKernel methods and both should be updated whenever callbacks are added or modified
// just copy paste whatever is there and update as appropriate

#ifndef TDMP_JNI
#ifdef IOS
extern "C" void TdmpKernelWithDelegate(tdmp::kernelDelegate *dlg)
{
    int cbctr = 0;
    
    // self sanity asserts
    assert(sizeof(cbNetIO) == sizeof(cbPlayerReferenceWithTypeAndCoords));

    defaultKern()->cli = dlg;
    
    // asserts check 2 things at once; whether defaultkern has all the available callbacks (i.e. no callback was forgotten in the sets above), 
    // and whether none of the provided callbacks were NULL
    
    defaultKernel->cli->debugLog("kernel (delegate set) callbacks setup (asserting ...)");
    
#define CBAssert(a) cbctr++; assert (defaultKernel->cli->a)
    CBAssert (debugLog != NULL);
    CBAssert (netWrite != NULL);
    CBAssert (playerJoined != NULL);
    CBAssert (playerLeft != NULL);
    CBAssert (didFindGame != NULL);
    CBAssert (towerWasCreated != NULL);
    CBAssert (monsterWasCreated != NULL);
    CBAssert (monsterWasKilled != NULL);
    CBAssert (waveWasKilledForPlayer != NULL);
    CBAssert (playerWasDamagedByPlayer != NULL);
    CBAssert (playerDied != NULL);
    CBAssert (playerSurrendered != NULL);
    CBAssert (monsterWithHealthWasSentByPlayer != NULL);
    CBAssert (nextWaveTime != NULL);
    CBAssert (victorWasDecided != NULL);
    CBAssert (monsterDefinition != NULL);
    CBAssert (towerDefinition != NULL);
    CBAssert (pathDefinition != NULL);
    
    assert((cbctr * sizeof(cbNetIO)) == sizeof(tdmp::kernelDelegate));
    
    defaultKernel->cli->debugLog("kernel sanity checks complete");
}
#endif

extern "C" KERNCALL STDCALL TdmpKernel(cbNetIO debugLog,
                                       cbNetIO netWrite,
                                       cbPlayerWithName playerJoined,
                                       cbPlayer playerLeft,
                                       cbPlayerWithTime didFindGame,
                                       cbPlayerReferenceWithTypeAndCoords towerWasCreated,
                                       cbPlayerReferenceWithType monsterWasCreated,
                                       cbReference monsterWasKilled,
                                       cbPlayer waveWasKilledForPlayer,
                                       cbPlayerVersusPlayerWithArgument playerWasDamagedByPlayer,
                                       cbPlayer playerDied,
                                       cbPlayer playerSurrendered,
                                       cbReferenceWithHealthViaPlayer monsterWasSentByPlayer,
                                       cbTime nextWaveTime,
                                       cbPlayer victorWasDecided,
                                       cbMonsterDefinition monsterDefinition,
                                       cbTowerDefinition towerDefinition,
                                       cbPathDefinition pathDefinition
                                       )
{
    int cbctr = 0;
    
    cout << "cli = " << defaultKern()->cli << endl;
	cout << "in TdmpKernel defaultKern() == " << defaultKernel << endl;

    // self sanity asserts
    assert(sizeof(cbNetIO) == sizeof(cbPlayerReferenceWithTypeAndCoords));
    
    defaultKernel->cli->debugLog = debugLog;
    defaultKernel->cli->netWrite = netWrite;
	cout << "netwrite = " << defaultKernel->cli->netWrite << endl;
    defaultKernel->cli->playerJoined = playerJoined;
    defaultKernel->cli->playerLeft = playerLeft;
    defaultKernel->cli->didFindGame = didFindGame;
    defaultKernel->cli->towerWasCreated = towerWasCreated;
    defaultKernel->cli->monsterWasCreated = monsterWasCreated;
    defaultKernel->cli->monsterWasKilled = monsterWasKilled;
    defaultKernel->cli->waveWasKilledForPlayer = waveWasKilledForPlayer;
    defaultKernel->cli->playerWasDamagedByPlayer = playerWasDamagedByPlayer;
    defaultKernel->cli->playerDied = playerDied;
    defaultKernel->cli->playerSurrendered = playerSurrendered;
    defaultKernel->cli->monsterWithHealthWasSentByPlayer = monsterWasSentByPlayer;
    defaultKernel->cli->nextWaveTime = nextWaveTime;
    defaultKernel->cli->victorWasDecided = victorWasDecided;
    defaultKernel->cli->monsterDefinition = monsterDefinition;
    defaultKernel->cli->towerDefinition = towerDefinition;
    defaultKernel->cli->pathDefinition = pathDefinition;

    // asserts check 2 things at once; whether defaultkern has all the available callbacks (i.e. no callback was forgotten in the sets above), 
    // and whether none of the provided callbacks were NULL

    defaultKernel->cli->debugLog("kernel (manual set) callbacks setup (asserting ...)");

#define CBAssert(a) cbctr++; assert (defaultKernel->cli->a)
    CBAssert (debugLog != NULL);
    CBAssert (netWrite != NULL);
    CBAssert (playerJoined != NULL);
    CBAssert (playerLeft != NULL);
    CBAssert (didFindGame != NULL);
    CBAssert (towerWasCreated != NULL);
    CBAssert (monsterWasCreated != NULL);
    CBAssert (monsterWasKilled != NULL);
    CBAssert (waveWasKilledForPlayer != NULL);
    CBAssert (playerWasDamagedByPlayer != NULL);
    CBAssert (playerDied != NULL);
    CBAssert (playerSurrendered != NULL);
    CBAssert (monsterWithHealthWasSentByPlayer != NULL);
    CBAssert (nextWaveTime != NULL);
    CBAssert (victorWasDecided != NULL);
    CBAssert (monsterDefinition != NULL);
    CBAssert (towerDefinition != NULL);
    CBAssert (pathDefinition != NULL);
    
    assert((cbctr * sizeof(cbNetIO)) == sizeof(tdmp::kernelDelegate));

    defaultKernel->cli->debugLog("kernel sanity checks complete");
}
#endif

#ifdef TDMP_JNI

// The com_team4_mptd_Kernel.h file declares a bunch of methods.
// The content of those methods are here. Thus, if you update call-in methods, you must also
// ensure that the section below is up to date. 

JNIEXPORT void JNICALL Java_com_team4_mptd_Kernel_setNickname(TDMP_FUNINIT, jstring s)
{
    TDMP_FUNFIX;
    const char *nativeString = (env)->GetStringUTFChars(s, NULL);
    defaultKernel->setNickname(nativeString);
    (env)->ReleaseStringUTFChars(s, nativeString);
}

JNIEXPORT void JNICALL Java_com_team4_mptd_Kernel_didRead(TDMP_FUNINIT, jstring s)
{
  TDMP_FUNFIX;
  const char *nativeString = (env)->GetStringUTFChars(s, NULL);
  defaultKernel->didRead(nativeString);
  (env)->ReleaseStringUTFChars(s, nativeString);
}

JNIEXPORT void JNICALL Java_com_team4_mptd_Kernel_didCreateTower(TDMP_FUNINIT, jint tower, jint type, jint x, jint y)
{
  TDMP_FUNFIX;
  defaultKernel->didCreateTower(tower,type,x,y);
}

JNIEXPORT void JNICALL Java_com_team4_mptd_Kernel_didKillMonster(TDMP_FUNINIT, jint monster)
{
  TDMP_FUNFIX;
  defaultKernel->didKillMonster(monster);
}

JNIEXPORT void JNICALL Java_com_team4_mptd_Kernel_didKillLastMonster(TDMP_FUNINIT) 
{
  TDMP_FUNFIX;
  defaultKernel->didKillLastMonster();
}

JNIEXPORT void JNICALL Java_com_team4_mptd_Kernel_didRecruitMonster(TDMP_FUNINIT, jint type, jint health)
{
  TDMP_FUNFIX;
  defaultKernel->didRecruitMonster(type, health);
}

JNIEXPORT void JNICALL Java_com_team4_mptd_Kernel_didSurrender(TDMP_FUNINIT)
{
  TDMP_FUNFIX;
  defaultKernel->didSurrender();
}

JNIEXPORT void JNICALL Java_com_team4_mptd_Kernel_didTakeDamage(TDMP_FUNINIT, jint damage, jint culprit)
{
  TDMP_FUNFIX;
  defaultKernel->didTakeDamage(damage,culprit);
}

JNIEXPORT void JNICALL Java_com_team4_mptd_Kernel_didDie(TDMP_FUNINIT)
{
  TDMP_FUNFIX;
  defaultKernel->didDie();
}

JNIEXPORT void JNICALL Java_com_team4_mptd_Kernel_loadData(TDMP_FUNINIT)
{
    TDMP_FUNFIX;
    defaultKernel->loadData();
}

JNIEXPORT void JNICALL Java_com_team4_mptd_Kernel_loadMapData(TDMP_FUNINIT, jint mapid)
{
    TDMP_FUNFIX;
    defaultKernel->loadMapData(mapid);
}

JNIEXPORT jstring JNICALL Java_com_team4_mptd_Kernel_getNameForMap(TDMP_FUNINIT, jint mapid)
{
    TDMP_FUNFIX;
    return (env)->NewStringUTF(defaultKernel->getNameForMap(mapid));
}

JNIEXPORT jstring JNICALL Java_com_team4_mptd_Kernel_getImageNameForMap(TDMP_FUNINIT, jint mapid)
{
    TDMP_FUNFIX;
    return (env)->NewStringUTF(defaultKernel->getImageNameForMap(mapid));
}

JNIEXPORT jint JNICALL Java_com_team4_mptd_Kernel_updateSPIncome
(TDMP_FUNINIT, jint waveCount)
{
  TDMP_FUNFIX;
  return defaultKernel->updateSPIncome(waveCount);
}

JNIEXPORT jint JNICALL Java_com_team4_mptd_Kernel_getBountyForMonster
(TDMP_FUNINIT, jint type)
{
  TDMP_FUNFIX;
  return defaultKernel->getBountyForMonster(type);
}

JNIEXPORT jint JNICALL Java_com_team4_mptd_Kernel_updateMPIncomeForBuyingMonster
(TDMP_FUNINIT, jint type) 
{
  TDMP_FUNFIX;
  return defaultKernel->updateMPIncomeForBuyingMonster(type);
}

JNIEXPORT jint JNICALL Java_com_team4_mptd_Kernel_mpIncome
  (TDMP_FUNINIT)
{
  TDMP_FUNFIX;
  return defaultKernel->mpIncome();
}

JNIEXPORT jint JNICALL Java_com_team4_mptd_Kernel_spIncome
(TDMP_FUNINIT)
{
  TDMP_FUNFIX;
  return defaultKernel->spIncome();
}

JNIEXPORT void JNICALL Java_com_team4_mptd_Kernel_findGame(TDMP_FUNINIT, jint players)
{
  TDMP_FUNFIX;
  defaultKernel->findGame(players);
}

JNIEXPORT void JNICALL Java_com_team4_mptd_Kernel_sharedModuleSelfTest(TDMP_FUNINIT)
{
  TDMP_FUNFIX;
  defaultKernel->sharedModuleSelfTest();
}

JNIEXPORT void JNICALL Java_com_team4_mptd_Kernel_newGame(TDMP_FUNINIT)
{
  TDMP_FUNFIX;
  defaultKernel->newGame();
}
#endif
