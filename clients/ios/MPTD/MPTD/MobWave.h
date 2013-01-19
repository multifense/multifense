//
//  MobWave.h
//  MPTD
//
//  Created by Karl-Johan Alm on 4/26/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "Monster.h"

@class GameSession, MobWaveEntry;

@interface MobWave : NSObject {
    int _spawnCountdown;
    int _currentSpawnPointer;
    int _afterNext;
    BOOL _done;
    BOOL _isWaveOver;
    MobWaveEntry *_first;
    GameSession *_session;
    NSMutableArray *_wave;
    NSMutableArray *_entries;
}

- (id)initWithSession:(GameSession *)session;

- (void)addEntry:(MobWaveEntry *)entry;

- (void)createWave;

- (void)update;

@property (nonatomic, readonly) BOOL isWaveOver;

@end
