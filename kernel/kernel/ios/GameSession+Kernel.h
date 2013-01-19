//
//  GameSession+Kernel.h
//  MPTD
//
//  Created by Karl-Johan Alm on 5/7/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import "GameSession.h"
#import "ClientIphone.h"

@interface GameSession (Kernel)

+ (ClientIphone *)sessionKernelClient;

- (void)kernelSetup;

- (void)netWrite:(NSString *)str;
- (void)playerJoined:(int)player withName:(NSString *)name;
- (void)playerLeft:(int)player;
- (void)didFindGame:(int)myPlayerID startingIn:(int)seconds;
- (void)towerWasCreated:(int)player tower:(int)tower type:(int)type x:(int)x y:(int)y;
- (void)monsterWasCreated:(int)player monster:(int)monster type:(int)type;
- (void)monsterWasKilledKERN:(int)monster;
- (void)waveWasKilledForPlayer:(int)player;
- (void)player:(int)player wasDamaged:(int)damageTaken byPlayer:(int)opponent;
- (void)playerDied:(int)player;
- (void)playerSurrendered:(int)player;
- (void)monster:(int)monster withHealth:(int)health wasSentByPlayer:(int)player;
- (void)nextWaveStartingIn:(int)seconds;

@end
/*
extern "C" KERNCALL TdmpKernel(cbNetIO,// callback for debug messages
                               cbNetIO,                             // callback for sending strings to server
                               cbPlayer,                            // playerJoined;
                               cbPlayer,                            // playerLeft;
                               cbPlayer,                            // didFindGame; player = your own player ID; set me.playerID = id on call
                               cbPlayerReferenceWithTypeAndCoords,  // towerWasCreated
                               cbPlayerReferenceWithType,           // monsterWasCreated
                               cbReference,                         // monsterWasKilled
                               cbPlayer,                            // waveWasKilledForPlayer
                               cbPlayerVersusPlayerWithArgument,    // playerWasDamagedByPlayer
                               cbPlayer,                            // playerDied
                               cbPlayer,                            // playerSurrendered
                               cbReferenceViaPlayer                 // monsterWasSentByPlayer
                               );

 TDMP_FTDECL(cbPlayerReferenceWithTypeAndCoords)(int, int, int, int, int);    // pointer to callback error handler
 TDMP_FTDECL(cbPlayerReferenceWithType)(int, int, int);                       // pointer to callback error handler
 TDMP_FTDECL(cbReference)(int);                                               // , const char*
 TDMP_FTDECL(cbPlayer)(int);
 TDMP_FTDECL(cbPlayerVersusPlayerWithArgument)(int, int, int);
 TDMP_FTDECL(cbVoid)();
 TDMP_FTDECL(cbReferenceViaPlayer)(int, int);
 // Method definitions for CALLBACKS (miscellaneous)
 TDMP_FTDECL(cbNetIO)(const char *);
 #endif

*/