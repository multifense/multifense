//
//  Economy.m
//  MPTD
//
//  Created by Karl-Johan Alm on 4/26/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//
#if 0

#import "Economy.h"
#import "Monster.h"
#import "Player.h"

@implementation Economy

@synthesize mpIncome = _mpIncome;
@synthesize me = _me;

- (id)init
{
    self = [super init];
    if (self) {
        _spIncome = 10;
        _mpIncome = 10;
    }
    return self;
}

- (int)getSPIncome:(int)waveCount
{
    _spIncome += waveCount - 1;
    _me.income = _spIncome;
    return _spIncome;
}

- (int)getMonsterMoney:(Monster *)mon
{
    return pow((mon.maxHP + 32.f * mon.speed) / 54.f, 0.8f);
}

- (int)buyMonster:(Monster *)mon
{
    _mpIncome += pow(((double)mon.maxHP + (mon.maxHP / 3 * mon.speed)) / 104, 0.8);
    _me.income = _mpIncome;
    return _mpIncome;
}

@end
#endif
