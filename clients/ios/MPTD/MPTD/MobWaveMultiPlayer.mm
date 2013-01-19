//
//  MWMP.m
//  MPTD
//
//  Created by Karl-Johan Alm on 5/10/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import "MobWaveMultiPlayer.h"
#import "GameSession.h"
#import "Monster.h"
#import "Player.h"
#import "Economy.h"
#import "StatusMenu.h"
#import "kernel.h"
#import "Player.h"

@implementation MobWaveMultiPlayer

@synthesize session = _session;

- (id)initWithSession:(GameSession *)session
{
    self = [super init];
    if (self) {
        _waveLabel = _session.me.sm.wave;
        _didKillLast = YES;
        _session = session;
        _nextMonsterWave = [[NSMutableArray alloc] init];
    }
    return self;
}

- (void)addMonsterToNextMobWave:(id)monster
{
    [_nextMonsterWave addObject:monster];
}

- (void)spawnNextMobWave
{
    _waveNumber++;

    int i = 0;
    for (Monster *m in _nextMonsterWave) {
        i++;
        [_session addMonster:m];
        [m moveDistance:-i*60];
    }
    [_nextMonsterWave removeAllObjects];
}

- (void)setSpawnTimer:(int)timeToNextWave
{
    _isWaveOver = YES;
    _didKillLast = NO;
    _timer = [NSDate timeIntervalSinceReferenceDate] + timeToNextWave;
    _session.me.gold += defaultKern()->mpIncome();
    //_session.eco.mpIncome;
}

- (void)update
{
    if (_isWaveOver) {
        NSTimeInterval now = [NSDate timeIntervalSinceReferenceDate];
        if (now > _timer) {
            _isWaveOver = NO;
            if (_nextMonsterWave.count == 0) {
                defaultKern()->didKillLastMonster();
                _didKillLast = YES;
            }
            [self spawnNextMobWave];
        } else {
            int eta = _timer - now;
            _session.me.sm.waveTimer.text = [NSString stringWithFormat:@"%d", eta];
        }
    } else if (! _didKillLast) {
        if (_session.monsters.count == 0) {
            defaultKern()->didKillLastMonster();
            _didKillLast = YES;
        }
    }
}

@end
