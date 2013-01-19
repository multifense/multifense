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
#import "Player.h"
#import "MobWaveMultiPlayer.h"
#import "Monster.h"
#import "Tower.h"
#import "GameData.h"
#import "GameAnimatedSprite.h"
#import "GameEndViewController.h"
#import "OpponentStatusView.h"
#import "AppDelegate.h"
#import "ViewController.h"
#import "GameMap.h"

@implementation GameSession (Kernel)

static ClientIphone *_cli = NULL;

+ (ClientIphone *)sessionKernelClient 
{
    return _cli;
}

- (void)kernelSetup:(int)mapID
{
    if (_cli == NULL) {
        _cli = new ClientIphone(self);
        assert(_me != nil);
    } else {
        _cli->setSession(self);
    }
    
    if (_me.nick == nil) {
        _me.nick = @"Kalle";
    }

    GameData *gd = [GameData defaultGameData];
    
    if (! [gd loaded]) {
        _monsterTypes = [[NSMutableDictionary alloc] init];
        _towerTypes = [[NSMutableDictionary alloc] init];
        defaultKern()->loadData();
        [gd storeDataForMonsters:_monsterTypes towers:_towerTypes];
    } else {
        [gd loadDataForMonsters:_monsterTypes towers:_towerTypes];
    }
    
    _map = [[GameMap alloc] initWithImage:[UIImage imageNamed:[NSString stringWithCString:defaultKern()->getImageNameForMap(mapID)
                                                                                 encoding:NSASCIIStringEncoding]]];
    defaultKern()->loadMapData(mapID);
    [_map processPath];
    
    BOOL mp = _multiplayer;
    
    _net.didReadBlock = ^(NSData *data) {
        NSString *s = [[NSString alloc] initWithData:data encoding:NSASCIIStringEncoding];
        defaultKern()->didRead([s cStringUsingEncoding:NSASCIIStringEncoding]);
    };
    _net.didConnect = ^{
        assert(_me.nick != nil);
        defaultKern()->setNickname([_me.nick cStringUsingEncoding:NSASCIIStringEncoding]);
        if (mp) {
            _loadingView.image = [UIImage imageNamed:@"searching-top.png"];
            _gvc.playersFound.text = _me.nick;
            dispatch_async(dispatch_get_global_queue(DISPATCH_QUEUE_PRIORITY_LOW, 0), ^{
                dispatch_async(dispatch_get_main_queue(), ^{
                    defaultKern()->findGame(0);
                });
            });
        }
    };
    _net.didDisconnect = ^{
        if (_gvc.searching) {
            ViewController *vc = [[ViewController alloc] initWithNibName:@"ViewController" bundle:[NSBundle mainBundle]];
            setWindowRoot(vc);
            UIAlertView *av = [[UIAlertView alloc] initWithTitle:@"Disconnected"
                                                         message:@"The server has disconnected or is unavailable. Please try again."
                                                        delegate:nil 
                                               cancelButtonTitle:@"Dismiss"
                                               otherButtonTitles:nil];
            [av show];
        }
    };
}

- (void)netWrite:(NSString *)str
{
    NSLog(@"[net write] '%@'", str);
    assert([str hasSuffix:@"\r\n"]);
    [_net write:str];
}

- (void)playerJoined:(int)player withName:(NSString *)name
{
    NSLog(@"player %d joined: %@", player, name);
    [_messageView pushMessage:[NSString stringWithFormat:@"%@ joined the game.", name]];
    Player *p = [[Player alloc] init];
    p.nick = name;
    p.pid = player;
    [_opponents setObject:p forKey:[NSNumber numberWithInt:player]];
    _gvc.playersFound.text = [NSString stringWithFormat:@"%@\n%@", _gvc.playersFound.text, name];
    [_me.osv updatePlayer:p];
}

- (void)playerLeft:(int)player
{
    NSLog(@"player left: %d", player);
    Player *p = [_opponents objectForKey:[NSNumber numberWithInt:player]];
    [_messageView pushMessage:[NSString stringWithFormat:@"%@ left the game.", p.nick]];
    [_opponents removeObjectForKey:[NSNumber numberWithInt:player]];
    NSString *pf = _gvc.playersFound.text;
    NSArray *pfarr = [pf componentsSeparatedByString:[NSString stringWithFormat:@"\n%@", p.nick]];
    pf = [pfarr componentsJoinedByString:@""];
    _gvc.playersFound.text = pf;
    [_me.osv removePlayer:p];
}

- (void)didFindGame:(int)myPlayerID startingIn:(int)seconds
{
    _gvc.searching = NO;    
    _loadingView.image = [UIImage imageNamed:@"getready-top.png"];
    [_messageView pushMessage:[NSString stringWithFormat:@"Game begins in %d seconds...", seconds]];
    _gvc.loadingLabel.hidden = NO;
    NSLog(@"DID FIND GAME WOOOOHOOOOO");

    // initial label is time
    _gvc.loadingLabel.text = [NSString stringWithFormat:@"%d", seconds];
    for (int i = 1; i < seconds; i++) {
        dispatch_time_t popTime = dispatch_time(DISPATCH_TIME_NOW, (double)i * NSEC_PER_SEC);
        dispatch_after(popTime, dispatch_get_main_queue(), ^(void){
            _gvc.loadingLabel.text = [NSString stringWithFormat:@"%d", seconds-i];
        });
    }

    dispatch_time_t popTime = dispatch_time(DISPATCH_TIME_NOW, (double)seconds * NSEC_PER_SEC);
    dispatch_after(popTime, dispatch_get_main_queue(), ^(void){
        _gvc.loadingView.hidden = YES;
        [self startGame];
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
    Player *p = [_opponents objectForKey:[NSNumber numberWithInt:player]];
    NSLog(@"Player killed their wave: %d", player);
    [_messageView pushMessage:[NSString stringWithFormat:@"%@ killed off their wave.", p.nick]];
}

- (void)player:(int)player wasDamaged:(int)damageTaken byPlayer:(int)opponent
{
    Player *p = [_opponents objectForKey:[NSNumber numberWithInt:player]];
    Player *o = opponent == 0 ? _me : [_opponents objectForKey:[NSNumber numberWithInt:opponent]];

    NSLog(@"player %d took %d damage because of player %d", player, damageTaken, opponent);
    [_messageView pushMessage:[NSString stringWithFormat:@"%@ took %d damage because of %@.", p.nick, damageTaken, opponent == 0 ? @"you" : o.nick]];

    if (opponent == 0) _me.hp++;
    
    for (Player *p in [_opponents allValues]) {
        if (p.pid == player) {
            // p took damage
            p.hp--;
            [_me.osv updatePlayer:p];
        }
        if (p.pid == opponent) {
            // p stole hp
            p.hp++;
            [_me.osv updatePlayer:p];
        }
    }
}

- (void)playerDied:(int)player
{
    Player *p = [_opponents objectForKey:[NSNumber numberWithInt:player]];
    NSLog(@"player died: %d", player);
    [_messageView pushMessage:[NSString stringWithFormat:@"%@ has died!", p.nick]];
}

- (void)playerSurrendered:(int)player
{
    Player *p = [_opponents objectForKey:[NSNumber numberWithInt:player]];
    NSLog(@"player surrendered: %d", player);
    [_messageView pushMessage:[NSString stringWithFormat:@"%@ has surrendered!", p.nick]];
}

- (void)monster:(int)monster withHealth:(int)health wasSentByPlayer:(int)player
{
    Player *p = [_opponents objectForKey:[NSNumber numberWithInt:player]];
    NSLog(@"player sent monster here: %d", player);

    Monster *m = [[_monsterTypes objectForKey:MonsterType(monster)] copy];
    if (m == nil) {
        [_messageView pushMessage:[NSString stringWithFormat:@"[DEBUG] an invalid monster was sent by %@ (monster type id = %d)", [[_opponents objectForKey:[NSNumber numberWithInt:player]] nick], monster]];
    }
    m.owner = player;
    m.hp = health;
    [m takeDamage:0]; // to get HP bar updated even if taking no damage

    [_messageView pushMessage:[NSString stringWithFormat:@"%@ sent a $%d monster with health %d to you!", p.nick, m.cost, health]];
    
    [_mpWave addMonsterToNextMobWave:m];
}

- (void)nextWaveStartingIn:(int)seconds
{
    NSLog(@"Next wave in %d", seconds);
    [_messageView pushMessage:[NSString stringWithFormat:@"Next wave will begin in %d seconds!", seconds]];
    [_mpWave setSpawnTimer:seconds];
}

- (void)monsterDefinition:(int)type sprite:(NSString *)sprite health:(int)health speed:(int)speed sendCost:(int)sendCost incomeIncrease:(int)incomeIncrease coloring:(int)coloring
{
    Monster *m = [[Monster alloc] initWithSprite:[[GameAnimatedSprite alloc] initWithSpriteSheet:[UIImage imageNamed:sprite] size:(CGSize){100,100} coloring:coloring] frame:(CGRect){0,0,50,50}];
    m.type = type;
    m.maxHP = health;
    m.speed = speed;
    m.cost = sendCost;
    m.incomeIncrease = incomeIncrease;
    m.coloring = coloring;
    [_monsterTypes setObject:m forKey:[NSNumber numberWithInt:type]];
}

- (void)towerDefinition:(int)type sprite:(NSString *)sprite damage:(int)damage timeBetweenShots:(int)timeBetweenShots range:(int)range cost:(int)cost projSprite:(NSString *)projSprite projSpeed:(int)projSpeed projSound:(NSString *)projSound
{
    Tower *t = [[Tower alloc] initWithSprite:[[GameAnimatedSprite alloc] initWithSpriteSheet:[UIImage imageNamed:sprite] size:(CGSize){100,100}]
                                    position:CGPointZero
                                    fireRate:timeBetweenShots 
                                      damage:damage
                                       range:range
                                 bulletSpeed:projSpeed
                                bulletSprite:[[GameAnimatedSprite alloc] initWithSpriteSheet:[UIImage imageNamed:projSprite] size:(CGSize){25,25}]];
    t.type = type;
    t.cost = cost;
    t.sound = projSound;
    [_towerTypes setObject:t forKey:[NSNumber numberWithInt:type]];
}

- (void)pathDefinition:(CGPoint)p direction:(CGPoint)dir length:(CGFloat)length
{
    [_map.path addCodedPath:p dir:dir length:length];
}

- (void)victorWasDecided:(int)player
{
    GameEndViewController *gev = [[GameEndViewController alloc] initWithNibName:@"GameEndViewController" bundle:[NSBundle mainBundle]];
    if (player == 0) {
        // WE WON! FOR GREAT JUSTICE!!!!
        [_messageView pushMessage:[NSString stringWithFormat:@"WE WIN! FOR GREAT JUSTICE!"]];
        gev.state = GameEndMPVictory;
    } else {
        // DEFEAT!!!!
        [_messageView pushMessage:[NSString stringWithFormat:@"WE HAVE LOST!"]];
        gev.state = GameEndMPLoss;
    }
    setWindowRoot(gev);
}

@end
