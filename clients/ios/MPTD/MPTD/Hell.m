//
//  Hell.m
//  MPTD
//
//  Created by Karl-Johan Alm on 4/20/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//
#if 0

#import "Hell.h"
#import "GameObject.h"
#import "GameSession.h"

@implementation Hell

@synthesize spawnRate = _spawnRate;
@synthesize lastSpawn = _lastSpawn;

- (id)initWithSession:(GameSession *)session
{
    self = [super init];
    if (self) {
        _session = session;
        _spawnRate = 2.0;
        _lastSpawn = 0.0;
    }
    return self;
}

- (void)tick
{
    _lastSpawn -= SPF;
    if (_lastSpawn < 0.0) {
        _lastSpawn += _spawnRate;
        [_session spawnMonster];
    }
}

- (double)spawnRate 
{
    return _spawnRate;
}

- (void)setSpawnRate:(double)spawnRate
{
    _spawnRate = _lastSpawn = spawnRate;
}

@end
#endif
