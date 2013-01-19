//
//  GameMapPathPoint.h
//  MPTD
//
//  Created by Karl-Johan Alm on 4/19/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface GamePathPoint : NSObject {
    GamePathPoint *_next;
    CGPoint _dir;
    double _length;
    CGPoint _p;
}

- (id)initWithPoint:(CGPoint)p;
- (id)initWithPoint:(CGPoint)p dir:(CGPoint)dir length:(double)length;

- (void)connectWithPathPoint:(GamePathPoint *)pathPoint;

@property (nonatomic, assign)   GamePathPoint *next;
@property (nonatomic, readonly) CGPoint p;
@property (nonatomic, readonly) CGPoint dir;
@property (nonatomic, readonly) double length;

@end
