//
//  RangeView.m
//  MPTD
//
//  Created by Karl-Johan Alm on 5/12/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import "RangeView.h"

@implementation RangeView

- (id)initWithFrame:(CGRect)frame
{
    self = [super initWithFrame:frame];
    if (self) {
        //self.clearsContextBeforeDrawing = YES;
        self.backgroundColor = [UIColor clearColor];
    }
    return self;
}

- (BOOL)available
{
    return _available;
}
- (void)setAvailable:(BOOL)available
{
    _available = available;
    [self setNeedsDisplay];
}

- (void)drawRect:(CGRect)rect
{
    CGContextRef cref = UIGraphicsGetCurrentContext();
    
    //CGContextSetRGBFillColor(cref, _available ? 0 : 1, _available ? 1 : 0, 0, .2f);
    CGContextSetRGBStrokeColor(cref, _available ? 0 : 1, _available ? 1 : 0, 0, .8f);
    
    //CGContextFillEllipseInRect(cref, (CGRect){CGPointZero, self.frame.size});
    CGRect r = (CGRect){CGPointZero, self.frame.size};
    while (r.size.width > 150) {
        CGContextStrokeEllipseInRect(cref, r);
        r.origin.x += 75.f;
        r.origin.y += 75.f;
        r.size.width -= 150.f;
        r.size.height -= 150.f;
    }
}

@end
