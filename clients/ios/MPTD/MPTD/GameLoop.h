//
//  GameLoop.h
//  MPTD
//
//  Created by Karl-Johan Alm on 4/19/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>

@class GameSession;

@interface GameLoop : NSObject {
    GameSession *_session;
    BOOL _isRunning;
    CADisplayLink *_dl;
    CFTimeInterval _lastFrame;
    double _runtime;
    NSTimeInterval _startStamp;
}

- (id)initWithSession:(GameSession *)session;

@property (nonatomic, readwrite) BOOL isRunning;
@property (nonatomic, readonly)  double runtime;

@end
