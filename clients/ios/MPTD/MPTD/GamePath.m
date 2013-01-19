//
//  GamePath.m
//  MPTD
//
//  Created by Karl-Johan Alm on 4/26/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import "GamePath.h"
#import "GamePathPoint.h"

@interface GamePath ()

- (void)appendPathPoint:(GamePathPoint *)pathPoint;

@end

@implementation GamePath

@synthesize pathStart = _pathStart;

- (id)init
{
    self = [super init];
    if (self) {
        _pointCount = 0;
        _pathLength = 0.0;
    }
    return self;
}

/*
 * method      : addPoint
 * arguments   : x and y coordinates
 *
 * Creates a new GamePathPoint object based on the given coords, and adds it to the system. 
 * This method uses an internal, private method appendPathPoint which takes a GamePathPoint 
 * object. (see private section below)
 */
- (void)addPathPoint:(CGPoint)p
{
    GamePathPoint *pp = [[GamePathPoint alloc] initWithPoint:p];
    [self appendPathPoint:pp];
}

- (void)addCodedPath:(CGPoint)p dir:(CGPoint)dir length:(CGFloat)length
{
    _pathLength += length;
    _pointCount ++;
    GamePathPoint *pp = [[GamePathPoint alloc] initWithPoint:p dir:dir length:length];
    if (_pathStart == nil) {
        _pathStart = _pathEnd = pp;
    } else {
        _pathEnd = _pathEnd.next = pp;
    }
}

/*
 * method      : loadPathFromFile
 * arguments   : filename = filename from which to read the data
 * returns     : boolean describing success loading the path info from the file in question
 *
 * Load binary values for map pathing from a file on disk. See bottom of this document for further
 * information.
 */
- (BOOL)loadPathFromFile:(NSString *)filename
{
    return NO;
}

/* 
 * method      : appendPathPoint
 * arguments   : pathPoint = the pathPoint to add
 *
 * If the point is the first point (i.e. if pathStart == null), pathStart and pathEnd are 
 * both set to the new object and spawnPoint is set to the given point (p). 
 * Otherwise pathEnd's connectWithPathPoint method is called, with the newly created object 
 * as argument.
 * In all cases, pathEnd is set to the object.
 */
- (void)appendPathPoint:(GamePathPoint *)pathPoint
{
    _pointCount ++;
    if (_pathStart == nil) {
        _pathStart = _pathEnd = pathPoint;
    } else {
        [_pathEnd connectWithPathPoint:pathPoint];
        _pathLength += _pathEnd.length;
        _pathEnd = pathPoint;
    }
}

- (NSUInteger)pointCount
{
    return _pointCount;
}

@end
