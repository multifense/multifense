//
//  MobWaveEntry.h
//  MPTD
//
//  Created by Karl-Johan Alm on 5/11/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>

@class Monster;

@interface MobWaveEntry : NSObject {
    int _nextSpawn;
    int _burstCounter;
    int _waveCounter;
    int _timeBetweenBursts;
    int _monstersPerBurst;
    Monster *_monster;
    BOOL _finished;
}

- (id)initWithMonster:(Monster *)monster startTime:(int)startTime timeBetweenBursts:(int)timeBetweenBursts numberOfBursts:(int)numberOfBursts monstersPerBurst:(int)monstersPerBurst;

- (void)spendTime:(int)time;

- (Monster *)spawnNext;

- (void)prepareBurst;

@property (nonatomic, readonly) int nextSpawn;
@property (nonatomic, readonly) BOOL finished;

@end
