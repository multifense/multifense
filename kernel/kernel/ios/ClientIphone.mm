//
//  ClientIphone.cpp
//  MPTD
//
//  Created by Karl-Johan Alm on 5/7/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#include "ClientIphone.h"

#import "GameSession.h"
#import "GameSession+Kernel.h"

ClientIphone::ClientIphone(GameSession *sess)
{
    session = sess;
    
    /// debugLog
    debugLog =
    [^(const char *msg) {
     NSLog(@"[kernel log]: %s", msg);
     } copy];
    
    netWrite =  
    [^(const char *msg) {
     NSString *s = [NSString stringWithCString:msg encoding:NSUTF8StringEncoding];
     [session netWrite:s];
     } copy];
    
    /// player joined
    playerJoined =
    [^(int player, const char *pname) {
        [session playerJoined:player withName:[NSString stringWithCString:pname encoding:NSUTF8StringEncoding]];
     } copy];
    
    playerLeft =
    [^(int player) {
     [session playerLeft:player];
     } copy];
    
    didFindGame =
    [^(int player, int timer) {
        [session didFindGame:player startingIn:timer];
     } copy];

    towerWasCreated =
    [^(int player, int tower, int type, int x, int y) {
     [session towerWasCreated:player tower:tower type:type x:x y:y];
     } copy];
    
    monsterWasCreated =
    [^(int player, int monster, int type) {
     [session monsterWasCreated:player monster:monster type:type];
     } copy];
    
    monsterWasKilled = 
    [^(int monster) {
     [session monsterWasKilledKERN:monster];
     } copy];
    
    waveWasKilledForPlayer = 
    [^(int player) {
     [session waveWasKilledForPlayer:player];
     } copy];
    
    playerWasDamagedByPlayer =
    [^(int player, int opponent, int damage) {
     [session player:player wasDamaged:damage byPlayer:opponent];
     } copy];
    
    playerDied =
    [^(int player) {
     GameSession *fiskmas = session;
     NSLog(@"session = %@", fiskmas);
     [session playerDied:player];
     } copy];
    
    playerSurrendered =
    [^(int player) {
     [session playerSurrendered:player];
     } copy];
    
    monsterWithHealthWasSentByPlayer =
    [^(int monster, int health, int player) {
        [session monster:monster withHealth:health wasSentByPlayer:player];
     } copy];
    
    nextWaveTime =
    [^(int timer) {
        [session nextWaveStartingIn:timer];
    } copy];
    
    NSLog(@" = %@", debugLog);
    NSLog(@" = %@", netWrite);
    NSLog(@" = %@", playerJoined);
    NSLog(@" = %@", playerLeft);
    NSLog(@" = %@", didFindGame);
    NSLog(@" = %@", towerWasCreated);
    NSLog(@" = %@", monsterWasCreated);
    NSLog(@" = %@", monsterWasKilled);
    NSLog(@" = %@", waveWasKilledForPlayer);
    NSLog(@" = %@", playerWasDamagedByPlayer);
    NSLog(@" = %@", playerDied);
    NSLog(@" = %@", playerSurrendered);
    NSLog(@" = %@", monsterWithHealthWasSentByPlayer);
    NSLog(@" = %@", nextWaveTime);

    TdmpKernelWithDelegate(this);
}
