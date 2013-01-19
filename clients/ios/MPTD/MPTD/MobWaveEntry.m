//
//  MobWaveEntry.m
//  MPTD
//
//  Created by Karl-Johan Alm on 5/11/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import "MobWaveEntry.h"
#import "Monster.h"

@implementation MobWaveEntry

@synthesize finished = _finished;
@synthesize nextSpawn = _nextSpawn;

- (id)initWithMonster:(Monster *)monster startTime:(int)startTime timeBetweenBursts:(int)timeBetweenBursts numberOfBursts:(int)numberOfBursts monstersPerBurst:(int)monstersPerBurst
{
    self = [super init];
    if (self) {
        assert(monster != nil);
        _nextSpawn = startTime;
		_burstCounter = 0;
		_waveCounter = numberOfBursts;
		_timeBetweenBursts = timeBetweenBursts;
		_monstersPerBurst = monstersPerBurst;
		_monster = monster;
        _finished = NO;
        
		[self prepareBurst];
    }
    return self;
}

- (void)spendTime:(int)time
{
    _nextSpawn -= time;
}

- (Monster *)spawnNext
{
    _burstCounter --;
    if (_burstCounter == 0) {
        // the whole burst has been sent, prepare for the next one
        [self prepareBurst];
        _nextSpawn = _timeBetweenBursts;
        // if the waveCounter is less then zero the finished boolean is set to true
        _finished = (_waveCounter < 0);
    } else {
        // TODO: fix bug: nextSpawn should be increased by this time, not set to this time, because we will sometimes be at negative values
        _nextSpawn = (int) (60/[_monster speed]);
    }
    return [_monster copy];
}

- (void)prepareBurst
{
    _burstCounter = _monstersPerBurst;
    _waveCounter --;
}

@end
