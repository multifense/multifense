//
//  GameLoop.m
//  MPTD
//
//  Created by Karl-Johan Alm on 4/19/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import "GameLoop.h"
#import "GameSession.h"
#import <QuartzCore/QuartzCore.h>
#import "GameObject.h"

@interface GameLoop ()

- (void)gameLoop:(CADisplayLink *)sender;

@end

@implementation GameLoop

- (id)initWithSession:(GameSession *)session
{
    self = [super init];
    if (self) {
        _session = session;
        _dl = [CADisplayLink displayLinkWithTarget:self selector:@selector(gameLoop:)];
        _lastFrame = 0;
    }
    return self;
}

- (void)gameLoop:(CADisplayLink *)sender
{
    CFTimeInterval ts = sender.timestamp;
    while (ts - _lastFrame >= SPF) {
        if (_lastFrame < 1) _lastFrame = ts; // first time we get 0 here
        _lastFrame += SPF;
        [_session update];
    }
}


- (BOOL)isRunning { return _isRunning; }
- (void)setIsRunning:(BOOL)isRunning
{
    if (isRunning == _isRunning) return;
    _isRunning = isRunning;
    if (_isRunning) {
        [_dl addToRunLoop:[NSRunLoop mainRunLoop] forMode:NSRunLoopCommonModes];
        _startStamp = [NSDate timeIntervalSinceReferenceDate];
        _lastFrame = 0;
    } else {
        [_dl removeFromRunLoop:[NSRunLoop mainRunLoop] forMode:NSRunLoopCommonModes];
        _runtime += ([NSDate timeIntervalSinceReferenceDate] - _startStamp);
    }
}

- (double)runtime
{
    return _runtime + (_isRunning ? [NSDate timeIntervalSinceReferenceDate] - _startStamp : 0.0);
}

@end
