//
//  GameMapPathPoint.m
//  MPTD
//
//  Created by Karl-Johan Alm on 4/19/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import "GamePathPoint.h"

@implementation GamePathPoint

@synthesize dir = _dir;
@synthesize length = _length;
@synthesize p = _p;

- (id)initWithPoint:(CGPoint)p
{
    self = [super init];
    if (self) {
        _p = p;
        _next = nil;
    }
    return self;
}

- (id)initWithPoint:(CGPoint)p dir:(CGPoint)dir length:(double)length
{
    self = [super init];
    if (self) {
        _p = p;
        _dir = dir;
        _length = length;
        _next = nil;
    }
    return self;
}

- (void)connectWithPathPoint:(GamePathPoint *)pathPoint
{
    _next = pathPoint;
    _dir = (CGPoint){_next.p.x - _p.x, _next.p.y - _p.y};
    _length = sqrt(_dir.x * _dir.x + _dir.y * _dir.y);
    _dir.x /= _length;
    _dir.y /= _length;
}

- (void)setNext:(GamePathPoint *)next
{
    _next = next;
}

- (BOOL)isEqual:(id)object
{
    GamePathPoint *pp = (GamePathPoint *)object;
    if (! [pp isKindOfClass:[GamePathPoint class]]) return NO;
    return CGPointEqualToPoint(_p, pp.p);
}

- (CGPoint)p
{
    return _p;
}

- (GamePathPoint *)next 
{
    return _next;
}

@end
