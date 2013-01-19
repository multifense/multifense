//
//  WaitObject.m
//  MPTD
//
//  Created by Karl-Johan Alm on 5/11/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import "WaitObject.h"

@implementation WaitObject

@synthesize waitTime = _waitTime;

- (id)initWithWaitTime:(int)waitTime
{
    self = [super initWithSprite:nil frame:CGRectZero];
    if (self) {
        _waitTime = waitTime;
        _type = 99;
    }
    return self;
}

@end
