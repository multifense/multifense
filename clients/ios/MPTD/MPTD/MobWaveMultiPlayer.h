//
//  MWMP.h
//  MPTD
//
//  Created by Karl-Johan Alm on 5/10/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>

@class GameSession, Monster;

@interface MobWaveMultiPlayer : NSObject {
    BOOL _isWaveOver;
    BOOL _didKillLast;
    NSMutableArray *_nextMonsterWave;
    __unsafe_unretained GameSession *_session;
    
    NSTimeInterval _timer;
    int _waveNumber;
    int _monstersInThisWave;
    
    UILabel *_waveLabel;
}

- (id)initWithSession:(GameSession *)session;

- (void)addMonsterToNextMobWave:(Monster *)monster;
- (void)spawnNextMobWave;
- (void)setSpawnTimer:(int)timeToNextWave;
- (void)update;

@property (nonatomic, assign) GameSession *session;

@end
