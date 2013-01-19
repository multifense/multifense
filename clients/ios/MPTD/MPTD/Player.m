//
//  Player.m
//  MPTD
//
//  Created by Karl-Johan Alm on 4/26/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import "Player.h"
#import "GameSession.h"

#import "StatusMenu.h"
#import "OpponentStatusView.h"

@implementation Player

@synthesize osv = _osv;
@synthesize sm = _sm;
@synthesize hp = _hp;
@synthesize gold = _gold;
@synthesize wave = _wave;
@synthesize kills = _kills;
@synthesize session = _session;
@synthesize singlePlayer = _singlePlayer;
@synthesize nick = _nick;
@synthesize pid = _pid;

- (id)init
{
    self = [super init];
    if (self) {
        _hp = 10;
        _wave = 0;
        _kills = 0;
        _gold = 100;
        _pid = -1;
        _income = 10;
    }
    return self;
}

- (BOOL)dead
{
    return _dead;
}

- (void)setDead:(BOOL)dead
{
    if (dead == _dead) return;
    _dead = dead;
    _gold = 0;
}

- (void)setHp:(int)hp
{
    _hp = hp;
    _sm.health.text = [NSString stringWithFormat:@"%d", hp];
    if (_hp <= 0) self.dead = YES;
}

- (void)setWave:(int)wave
{
    _wave = wave;
    _sm.wave.text = [NSString stringWithFormat:@"%d", wave];
}

- (void)setKills:(int)kills
{
    _kills = kills;
    _sm.kills.text = [NSString stringWithFormat:@"%d", kills];
}

- (void)setGold:(int)gold
{
    if (_pid == -1 && ! _dead) {
        _gold = gold;
        _sm.gold.text = [NSString stringWithFormat:@"%d", gold];
        [[_session.towerTypes allValues] makeObjectsPerformSelector:@selector(updateLabelsWithPlayer:) withObject:self];
        [[_session.monsterTypes allValues] makeObjectsPerformSelector:@selector(updateLabelsWithPlayer:) withObject:self];
    }
}

- (void)setSm:(StatusMenu *)sm
{
    _sm = sm;
    self.hp = _hp;
    self.wave = _wave;
    self.kills = _kills;
    self.gold = _gold;
    self.income = _income;
}

- (void)setOsv:(OpponentStatusView *)osv
{
    _osv = osv;
}

- (void)setIncome:(int)income
{
    _income = income;
    _sm.income.text = [NSString stringWithFormat:@"%d", _income];
}

- (int)income
{
    return _income;
}

@end
