//
//  MobWave.m
//  MPTD
//
//  Created by Karl-Johan Alm on 4/26/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import "MobWave.h"
#import "GameSession.h"
#import "Player.h"
#import "Monster.h"
#import "StatusMenu.h"
#import "MobWaveEntry.h"
#import "WaitObject.h"

@interface MobWave ()

//- (void)adjustMonsterPosition:(Monster *)m withIndex:(int)index;

@end

@implementation MobWave

@synthesize isWaveOver = _isWaveOver;

- (id)initWithSession:(GameSession *)session
{
    self = [super init];
    if (self) {
        _session = session;
        _currentSpawnPointer = 0;
        _isWaveOver = NO;
        _wave = [[NSMutableArray alloc] init];
        _entries = [[NSMutableArray alloc] init];
    }
    return self;
}

- (void)addEntry:(MobWaveEntry *)entry
{
    [_entries addObject:entry];
}

- (void)createWave
{
    // while entries remain
    while (! _done) {
        _first = nil;
        for (MobWaveEntry *entry in _entries) {
            if (_first == nil && ![entry finished]) {
                _first = entry;
            }
            if (_first != nil && (entry.nextSpawn < _first.nextSpawn) && (! [entry finished])) {
                _first = entry;
            }
        }
        _afterNext = _first.nextSpawn;
        
        [_wave addObject:[[WaitObject alloc] initWithWaitTime:_afterNext]];
        
        for (MobWaveEntry *entry in _entries) {
            [entry spendTime:_afterNext];
        }
        [_wave addObject:_first.spawnNext];
        
        _done = YES;
        for (MobWaveEntry *entry in _entries) {
            _done &= entry.finished;
        }
    }
}

- (void)update
{
    if (_currentSpawnPointer < _wave.count) {
        _spawnCountdown--;
        //if the counter is done and the object to handle is a waitObject 
        if (_spawnCountdown < 1) {
            GameObject *ob = [_wave objectAtIndex:_currentSpawnPointer];
            if (ob.type == 99) {
                //sets the counter to the time found in our wait object
                // TODO: fix bug ere; spawncountdown should be +'d wait time, not set to wait time
                _spawnCountdown = [(WaitObject *)ob waitTime];
            } else {
                //if the counter is done and the object to be handled is a monster
                [_session addMonster:(Monster *)ob];
            }
            _currentSpawnPointer++;
        }
    } else {
        _isWaveOver = YES;
    }
        
}

@end
