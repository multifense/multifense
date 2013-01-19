//
//  GamePath.h
//  MPTD
//
//  Created by Karl-Johan Alm on 4/26/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>

@class GamePathPoint;

@interface GamePath : NSObject {
@private
    GamePathPoint *_pathEnd;
    NSUInteger _pointCount;
    double _pathLength;
@public
    GamePathPoint *_pathStart;
}

- (void)addPathPoint:(CGPoint)p;

- (void)addCodedPath:(CGPoint)p dir:(CGPoint)dir length:(CGFloat)length;

- (BOOL)loadPathFromFile:(NSString *)filename;

@property (nonatomic, readonly) GamePathPoint *pathStart;
@property (nonatomic, readonly) NSUInteger pointCount;

@end
