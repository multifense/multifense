//
//  MPTDTestRecognizer.m
//  MPTD
//
//  Created by Karl-Johan Alm on 4/27/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import "MPTDTestRecognizer.h"

@implementation MPTDTestRecognizer

- (void)setLocation:(CGPoint)p inView:(UIView *)view
{
    _loc = p;
    _viw = view;
}

- (CGPoint)locationInView:(UIView *)view
{
    return [view convertPoint:_loc fromView:_viw];
}

@end
