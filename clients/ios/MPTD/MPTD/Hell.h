//
//  Hell.h
//  MPTD
//
//  Created by Karl-Johan Alm on 4/20/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//
#if 0

#import <Foundation/Foundation.h>

@class GameSession;

@interface Hell : NSObject {
    __unsafe_unretained GameSession *_session;
    double _spawnRate;
    double _lastSpawn;
}

- (id)initWithSession:(GameSession *)session;

- (void)tick;

@property (nonatomic, readwrite) double spawnRate;
@property (nonatomic, readwrite) double lastSpawn;

@end
#endif
