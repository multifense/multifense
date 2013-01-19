//
//  GameSession+Kernel.m
//  MPTD
//
//  Created by Karl-Johan Alm on 5/7/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import "GameViewController.h"
#import "GameSession+Kernel.h"
#include "ClientIphone.h"
#import "GameNetwork.h"
#import "GameMessageView.h"

@implementation GameSession (Kernel)

static ClientIphone *_cli = NULL;

+ (ClientIphone *)sessionKernelClient 
{
    return _cli;
}

- (void)kernelSetup
{
    if (_cli == NULL) {
        _cli = new ClientIphone(self);
        defaultKern()->setNickname("Kalle");
    }
    
    _net.didReadBlock = ^(NSData *data) {
        NSString *s = [[NSString alloc] initWithData:data encoding:NSASCIIStringEncoding];
        defaultKern()->didRead([s cStringUsingEncoding:NSASCIIStringEncoding]);
    };
    
    defaultKern()->findGame();
}

- (void)netWrite:(NSString *)str
{
    assert([str hasSuffix:@"\r\n"]);
    [_net write:str];
}

- (void)playerJoined:(int)player withName:(NSString *)name
{
    NSLog(@"player %d joined: %@", player, name);
    [_messageView pushMessage:[NSString stringWithFormat:@"%@ joined the game.", name]];
}

- (void)playerLeft:(int)player
{
    NSLog(@"player left: %d", player);
    [_messageView pushMessage:[NSString stringWithFormat:@"Player %d left the game.", player]];
}

- (void)didFindGame:(int)myPlayerID startingIn:(int)seconds
{
    [_messageView pushMessage:[NSString stringWithFormat:@"Game begins in %d seconds...", seconds]];
    GameViewController *gvc = [GameViewController currentGameViewController];
    gvc.loadingLabel.hidden = NO;
    NSLog(@"DID FIND GAME WOOOOHOOOOO");
    dispatch_async(dispatch_get_global_queue(DISPATCH_QUEUE_PRIORITY_LOW, 0), ^{
        for (int i = 9; i >= 0; i--) {
            sleep(1);
            dispatch_async(dispatch_get_main_queue(), ^{
                gvc.loadingLabel.text = [NSString stringWithFormat:@"%d", i];
                if (i == 0) {
                    gvc.loadingView.hidden = YES;
                    [self startGame];
                }
            });
        }
    });
}

- (void)towerWasCreated:(int)player tower:(int)tower type:(int)type x:(int)x y:(int)y
{
    
}

- (void)monsterWasCreated:(int)player monster:(int)monster type:(int)type
{
    
}

- (void)monsterWasKilledKERN:(int)monster
{
    
}

- (void)waveWasKilledForPlayer:(int)player
{
    NSLog(@"Player killed their wave: %d", player);
    [_messageView pushMessage:[NSString stringWithFormat:@"Player %d killed off their wave.", player]];
}

- (void)player:(int)player wasDamaged:(int)damageTaken byPlayer:(int)opponent
{
    NSLog(@"player %d took %d damage because of player %d", player, damageTaken, opponent);
    [_messageView pushMessage:[NSString stringWithFormat:@"Player %d took %d damage because of player %d.", player, damageTaken, opponent]];
}

- (void)playerDied:(int)player
{
    NSLog(@"player died: %d", player);
    [_messageView pushMessage:[NSString stringWithFormat:@"Player %d has died!", player]];
}

- (void)playerSurrendered:(int)player
{
    NSLog(@"player surrendered: %d", player);
    [_messageView pushMessage:[NSString stringWithFormat:@"Player %d has surrendered!", player]];
}

- (void)monster:(int)monster withHealth:(int)health wasSentByPlayer:(int)player
{
    NSLog(@"player sent monster here: %d", player);
    [_messageView pushMessage:[NSString stringWithFormat:@"Player %d sent a %d monster with health %d to you!", player, monster, health]];
    [self spawnMonster];
}

- (void)nextWaveStartingIn:(int)seconds
{
    
}

@end
