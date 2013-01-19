//
//  MPTDTestRecognizer.h
//  MPTD
//
//  Created by Karl-Johan Alm on 4/27/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface MPTDTestRecognizer : UIGestureRecognizer {
    UIView *_viw;
    CGPoint _loc;
}

- (void)setLocation:(CGPoint)p inView:(UIView *)view;

@end
