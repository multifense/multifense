//
//  TestSession.m
//  MPTD
//
//  Created by Karl-Johan Alm on 5/12/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import "TestSession.h"
#import "GameSession+Kernel.h"

@implementation TestSession

@synthesize success = _success;
@synthesize expectedString = _expectedString;
@synthesize gotString = _gotString;

- (void)expectNetWrite:(NSString *)msg
{
    // add request to queue
    if (_queue == nil) _queue = [[NSMutableArray alloc] init];
    [_queue addObject:msg];
    // we reset success flag on expect
    _success = YES;
}

- (void)netWrite:(NSString *)str
{
    // kernel is writing to network: check if the written string matches what was expected
    // this fails if queue is empty
    _success &= (_queue.count > 0);
    if (_success) {
        _expectedString = [_queue objectAtIndex:0];
        _gotString = str;
        [_queue removeObjectAtIndex:0];
        _success = [str isEqualToString:_expectedString];
    }
}

// test session allows you to set/get multiplayer flag, which regular session of course does not
- (void)setMultiplayer:(BOOL)multiplayer
{
    _multiplayer = multiplayer;
}

- (BOOL)multiplayer 
{
    return _multiplayer;
}

@end
