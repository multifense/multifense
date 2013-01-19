//
//  GameMap.m
//  MPTD
//
//  Created by Karl-Johan Alm on 4/19/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import "GameMap.h"
#import "GamePathPoint.h"

@interface GameMap ()

- (void)initiatePathRect;

@end

@implementation GameMap

@synthesize path = _path;
@synthesize size = _size;

- (id)initWithImage:(UIImage *)image
{
    self = [super initWithImage:image];
    if (self) {
        _path = [[GamePath alloc] init];
        _size = image.size;
        self.frame = (CGRect){CGPointZero, image.size};
        //self.autoresizingMask = UIViewAutoresizingFlexibleWidth | UIViewAutoresizingFlexibleHeight | UIViewAutoresizingFlexibleRightMargin | UIViewAutoresizingFlexibleBottomMargin;
    }
    return self;
}

- (void)processPath
{
    [self initiatePathRect];
}

- (BOOL)intersects:(CGRect)checkRect
{
    for (NSValue *v in _pathRects) {
        CGRect r = [v CGRectValue];
        if (CGRectIntersectsRect(r, checkRect)) {
            return YES;
        }
    }
    return NO;
}

- (void)initiatePathRect
{
    _pathRects = [[NSMutableArray alloc] init];
    
    // for all gamepath points construct rects stretching between all rects.
    // path width 100.
    int width = 50; // 100/2 either way.
    GamePathPoint *pointStart = _path.pathStart;
    while (pointStart.next != nil) {
        CGPoint p = pointStart.p;
        // creates a new rect with left, top == x - width, y - width, right,
        // bottom == next.x + width, next.y + width
        CGRect tempRect = CGRectMake((p.x - width), (p.y - width),
                                     (pointStart.next.p.x + 2 * width - p.x),
                                     (pointStart.next.p.y + 2 * width - p.y));
        
        /*UIView *v = [[UIView alloc] initWithFrame:tempRect];
        v.backgroundColor = [UIColor colorWithRed:1.f green:0.f blue:0.f alpha:.7f];
        [self addSubview:v];*/
        
        [_pathRects addObject:[NSValue valueWithCGRect:tempRect]];
        pointStart = pointStart.next;
    }
}



@end
