//
//  GameSession.m
//  MPTD
//
//  Created by Karl-Johan Alm on 4/19/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import "GameSession.h"
#import "GameMap.h"
#import "GameLoop.h"
#import "Tower.h"
#import "Monster.h"
#import "Hell.h"
#import "GameBullet.h"
//#import "TowerType.h"
#import "Player.h"
#import "ViewController.h"
#import "Economy.h"
#import "MobWave.h"
#import "GameSession+Kernel.h"
#import "GameNetwork.h"
#import "GameMessageView.h"
#import "MobWaveMultiPlayer.h"
#import "MobWaveEntry.h"
#import "GameEndViewController.h"
#import "GameAnimatedSprite.h"
#import "GameViewController.h"
#import "OpponentStatusView.h"
#import "AppDelegate.h"

@interface GameSession ()

- (void)setupMobWave;

@end

@implementation GameSession

@synthesize opponents = _opponents;
@synthesize gvc = _gvc;
@synthesize map = _map;
@synthesize loop = _loop;
@synthesize towers = _towers;
@synthesize monsters = _monsters;
@synthesize towerTypes = _towerTypes;
@synthesize monsterTypes = _monsterTypes;
@synthesize me = _me;
//@synthesize eco = _eco;
@synthesize loadingView = _loadingView;
@synthesize messageView = _messageView;
@synthesize net = _net;

- (id)init { assert(0); }

- (id)initGame:(BOOL)multiplayer onMap:(int)mapID
{
    self = [super init];
    if (self) {
        _me = [[Player alloc] init];
        _me.session = self;
        _me.income = 10;
        NSUserDefaults *userDefaults = [NSUserDefaults standardUserDefaults];
        _me.nick = [userDefaults objectForKey:@"playerName"];
        _opponents = [[NSMutableDictionary alloc] init];
        _multiplayer = multiplayer;
        _loop = [[GameLoop alloc] initWithSession:self];
        _towers = [[NSMutableArray alloc] init];
        _monsters = [[NSMutableArray alloc] init];
        _bullets = [[NSMutableArray alloc] init];
        
        _towerTypes = [[NSMutableDictionary alloc] init];
        
        _monsterTypes = [[NSMutableDictionary alloc] init];
        
        if (! _multiplayer) {
            [self kernelSetup:mapID];
        }
        
        if (_multiplayer) {
            _mpWave = [[MobWaveMultiPlayer alloc] initWithSession:self];
        } else {
            _mobWaves = [[NSMutableArray alloc] init];
            [self setupMobWave];
        }
    }
    return self;
}

- (void)shutdown
{
    if (_multiplayer) {
        defaultKern()->didSurrender();
    }
}

- (void)connectToHost:(NSString *)host port:(UInt16)port
{
    _net = [[GameNetwork alloc] initWithHost:host port:port];
    [self kernelSetup:1]; // all multiplayer games are on map 1 right now
}

#if 0
- (GameMap *)loadMap:(UIImage *)background path:(GamePath *)path
{
    _map = [[GameMap alloc] initWithImage:background path:path];
    return _map;
}
#endif

- (BOOL)collisionDetect:(CGRect)frame
{
    for (Tower *t in _towers) {
        if (CGRectIntersectsRect(frame, t.frame)) 
            return NO;
    }
    for (Monster *m in _monsters) {
        if (CGRectIntersectsRect(frame, m.frame))
            return NO;
    }
    if ([_map intersects:frame]) {
        return NO;
    }
    return YES;
}

- (void)monsterReachedEnd:(Monster *)monster
{
    _me.hp--;
    if (_multiplayer) {
        defaultKern()->didRecruitMonster(monster.type, monster.hp);
        defaultKern()->didTakeDamage(1, monster.owner);
        Player *culprit = [_opponents objectForKey:[NSNumber numberWithInt:monster.owner]];
        if (culprit.hp > 0) culprit.hp++;
        [_me.osv updatePlayer:culprit];
    }
    if (_me.hp <= 0) {
        _me.dead = YES;
        [_messageView pushMessage:@"You have died!"];
        [_gvc playerDidDie];
        // we've died
        if (! _multiplayer) {
            [self stopGame];
            GameEndViewController *gev = [[GameEndViewController alloc] initWithNibName:@"GameEndViewController" bundle:[NSBundle mainBundle]];
            gev.state = GameEndSPLoss;
            setWindowRoot(gev);
        }
    }
}

- (void)monsterWasKilled:(Monster *)monster
{
    _me.kills++;
    _me.gold += defaultKern()->getBountyForMonster(monster.type);
}

- (void)waveEnded
{
    assert(! _multiplayer);
    _me.wave++;
    _me.gold += defaultKern()->updateSPIncome(_me.wave);
}

- (BOOL)canPlaceTower:(Tower *)tower
{
    return [self collisionDetect:tower.frame];
}

- (BOOL)addTower:(Tower *)tower
{
    if (! [self collisionDetect:tower.frame]) return NO;
    tower.session = self;
    [_towers addObject:tower];
    [_map addSubview:tower];
    return YES;
}

- (BOOL)addMonster:(Monster *)monster
{
    monster.session = self;
    [_monsters addObject:monster];
    [_map addSubview:monster];
    return YES;
}

- (BOOL)addBullet:(GameBullet *)bullet
{
    bullet.session = self;
    [_bullets addObject:bullet];
    [_map addSubview:bullet];
    return YES;
}

- (BOOL)removeTower:(Tower *)tower
{
    if (! [_towers containsObject:tower]) return NO;
    [tower removeFromSuperview];
    [_towers removeObject:tower];
    return YES;
}

- (BOOL)removeBullet:(GameBullet *)bullet
{
    if (! [_bullets containsObject:bullet]) return NO;
    [bullet removeFromSuperview];
    [_bullets removeObject:bullet];
    return YES;
}

- (BOOL)removeMonster:(Monster *)monster
{
    /*static bool y = NO;
    if (!y) {
        y = YES;
        fend = [NSDate timeIntervalSinceReferenceDate];
        NSLog(@"removing first monster: time taken = %f", fend - fstart);
    }*/
    if (! [_monsters containsObject:monster]) return NO;
    [monster removeFromSuperview];
    [_monsters removeObject:monster];
    return YES;
}

- (Monster *)oldestMonster
{
    for (Monster *m in _monsters) {
        if (m.frame.origin.x + m.frame.size.width >= 0.f &&
            m.frame.origin.y + m.frame.size.height >= 0.f &&
            m.frame.origin.x <= 480 &&
            m.frame.origin.y <= 320) {
            return m;
        }
    }
    return nil;
}

- (Monster *)monsterTypeFromID:(MonsterTypeID)typeID
{
    assert(typeID > 0 && typeID <= _monsterTypes.count);
    return [_monsterTypes objectForKey:MonsterType(typeID)];
}

- (double)gameTime
{
    return _loop.runtime;
}

- (void)recruitMonster:(Monster *)monster
{
    if (_me.gold >= monster.cost) {
        _me.gold -= monster.cost;
        _me.income = defaultKern()->updateMPIncomeForBuyingMonster(monster.type);
        defaultKern()->didRecruitMonster(monster.type, monster.hp);
    }
}

- (void)spawnMonster
{
    assert(0);
    /*static bool b = NO;
    if (b) return;
    b = YES;*/
    /*NSString *sprite = arc4random() % 10 ? @"bombmonster1.png" : @"slimemonster.png";
    Monster *m = [[Monster alloc] initWithSprite:[[GameAnimatedSprite alloc] initWithSpriteSheet:[UIImage imageNamed:sprite] size:s] frame:(CGRect){0,0,50,50}];
    m.speed = 2.0;
    m.hp = 100.0;
    
    [self addMonster:m];*/
    /*static bool x = NO;
    if (!x) {
        fstart = [NSDate timeIntervalSinceReferenceDate];
        x = YES;
        NSLog(@"started now");
    }*/
}

- (void)startGame
{
    _loop.isRunning = YES;
    [_messageView pushMessage:@"Game starts!"];
}

- (void)stopGame
{
    _loop.isRunning = NO;
    [_messageView pushMessage:@"Game paused."];
}

- (void)update
{
    [_towers makeObjectsPerformSelector:@selector(update)];
    [_monsters makeObjectsPerformSelector:@selector(update)];
    [_bullets makeObjectsPerformSelector:@selector(update)];
    //[_hell tick];
    if (_multiplayer) {
        [_mpWave update];
    } else {
        [self updateWave];
    }
}

- (void)setMe:(Player *)me
{
    _me = me;
    _me.session = self;
}

- (void)cancelSearch
{
    defaultKern()->didSurrender();
}

- (void)setupMobWave
{
    MobWave *w1 = [[MobWave alloc] initWithSession:self];
    MobWave *w2 = [[MobWave alloc] initWithSession:self];
    MobWave *w3 = [[MobWave alloc] initWithSession:self];
    MobWave *w4 = [[MobWave alloc] initWithSession:self];
    MobWave *w5 = [[MobWave alloc] initWithSession:self];
    MobWave *w6 = [[MobWave alloc] initWithSession:self];
    MobWave *w7 = [[MobWave alloc] initWithSession:self];
    MobWave *w8 = [[MobWave alloc] initWithSession:self];
    [_mobWaves addObject:w1];
    [_mobWaves addObject:w2];
    [_mobWaves addObject:w3];
    [_mobWaves addObject:w4];
    [_mobWaves addObject:w5];
    [_mobWaves addObject:w6];
    [_mobWaves addObject:w7];
    [_mobWaves addObject:w8];
    
    // 	public MobWaveEntry(Monster _monster, int startTime, int _timeBetweenBursts, int _numberOfBursts, int _monstersPerBurst){


    [w1 addEntry:[[MobWaveEntry alloc] initWithMonster:[_monsterTypes objectForKey:MonsterType(102)] startTime:0 timeBetweenBursts:0 numberOfBursts:1 monstersPerBurst:1]];
    [w1 addEntry:[[MobWaveEntry alloc] initWithMonster:[_monsterTypes objectForKey:MonsterType(102)] startTime:120 timeBetweenBursts:100 numberOfBursts:3 monstersPerBurst:2]];

    [w2 addEntry:[[MobWaveEntry alloc] initWithMonster:[_monsterTypes objectForKey:MonsterType(102)] startTime:0 timeBetweenBursts:280 numberOfBursts:2 monstersPerBurst:3]];
    [w2 addEntry:[[MobWaveEntry alloc] initWithMonster:[_monsterTypes objectForKey:MonsterType(101)] startTime:200 timeBetweenBursts:250 numberOfBursts:2 monstersPerBurst:1]];
    [w2 addEntry:[[MobWaveEntry alloc] initWithMonster:[_monsterTypes objectForKey:MonsterType(101)] startTime:500 timeBetweenBursts:0 numberOfBursts:1 monstersPerBurst:2]];
    [w2 addEntry:[[MobWaveEntry alloc] initWithMonster:[_monsterTypes objectForKey:MonsterType(102)] startTime:550 timeBetweenBursts:0 numberOfBursts:1 monstersPerBurst:2]];
    
    [w3 addEntry:[[MobWaveEntry alloc] initWithMonster:[_monsterTypes objectForKey:MonsterType(101)] startTime:0 timeBetweenBursts:120 numberOfBursts:2 monstersPerBurst:1]];
    [w3 addEntry:[[MobWaveEntry alloc] initWithMonster:[_monsterTypes objectForKey:MonsterType(102)] startTime:20 timeBetweenBursts:80 numberOfBursts:2 monstersPerBurst:3]];
    
    [w4 addEntry:[[MobWaveEntry alloc] initWithMonster:[_monsterTypes objectForKey:MonsterType(103)] startTime:0 timeBetweenBursts:170 numberOfBursts:2 monstersPerBurst:3]];
    [w4 addEntry:[[MobWaveEntry alloc] initWithMonster:[_monsterTypes objectForKey:MonsterType(102)] startTime:20 timeBetweenBursts:140 numberOfBursts:2 monstersPerBurst:4]];
    [w4 addEntry:[[MobWaveEntry alloc] initWithMonster:[_monsterTypes objectForKey:MonsterType(102)] startTime:300 timeBetweenBursts:120 numberOfBursts:1 monstersPerBurst:2]];
    [w4 addEntry:[[MobWaveEntry alloc] initWithMonster:[_monsterTypes objectForKey:MonsterType(102)] startTime:480 timeBetweenBursts:120 numberOfBursts:1 monstersPerBurst:3]];
    [w4 addEntry:[[MobWaveEntry alloc] initWithMonster:[_monsterTypes objectForKey:MonsterType(103)] startTime:500 timeBetweenBursts:120 numberOfBursts:1 monstersPerBurst:5]];

    [w5 addEntry:[[MobWaveEntry alloc] initWithMonster:[_monsterTypes objectForKey:MonsterType(101)] startTime:0 timeBetweenBursts:190 numberOfBursts:2 monstersPerBurst:6]];
    [w5 addEntry:[[MobWaveEntry alloc] initWithMonster:[_monsterTypes objectForKey:MonsterType(103)] startTime:60 timeBetweenBursts:230 numberOfBursts:2 monstersPerBurst:5]];

    [w6 addEntry:[[MobWaveEntry alloc] initWithMonster:[_monsterTypes objectForKey:MonsterType(104)] startTime:0 timeBetweenBursts:0 numberOfBursts:1 monstersPerBurst:1]];

    [w7 addEntry:[[MobWaveEntry alloc] initWithMonster:[_monsterTypes objectForKey:MonsterType(105)] startTime:0 timeBetweenBursts:0 numberOfBursts:1 monstersPerBurst:1]];
    
    [w8 addEntry:[[MobWaveEntry alloc] initWithMonster:[_monsterTypes objectForKey:MonsterType(105)] startTime:0 timeBetweenBursts:0 numberOfBursts:1 monstersPerBurst:10]];
    
    [w1 createWave];
    [w2 createWave];
    [w3 createWave];
    [w4 createWave];
    [w5 createWave];
    [w6 createWave];
    [w7 createWave];
    [w8 createWave];
}

- (void)updateWave
{
    // me.wave is the same as monsterWaveNumber, no???
    if (_me.wave >= _mobWaves.count && _monsters.count == 0) {
        [self stopGame];
        GameEndViewController *gev = [[GameEndViewController alloc] initWithNibName:@"GameEndViewController" bundle:[NSBundle mainBundle]];
        gev.state = GameEndSPVictory;
        [[[[UIApplication sharedApplication] delegate] window] setRootViewController:gev];
        return;
    }
    
    if (_monsterWaveNumber < _mobWaves.count) {
        MobWave *wave = [_mobWaves objectAtIndex:_monsterWaveNumber];
        [wave update];
        if ([wave isWaveOver] && _monsters.count == 0) {
            _monsterWaveNumber++;
            int waveIncome = defaultKern()->updateSPIncome(_me.wave);
            _me.income = waveIncome;
            //[_eco getSPIncome:_me.wave];
            // Updates text displayed in statsgui
            [_messageView pushMessage:[NSString stringWithFormat:@"Got gold for surviving wave: %d", waveIncome]];
            _me.gold += waveIncome;
            // Updates text displayed in statsgui
            [_messageView pushMessage:[NSString stringWithFormat:@"New wave incoming!"]];
            _me.wave++;
            // wave count
        }
    }
}

@end
