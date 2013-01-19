//
//  GameAnimatedSprite.h
//  MPTD
//
//  Created by Karl-Johan Alm on 5/11/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>

// ROWS
// 0 = up
// 1 = down
// 2 = right
// 3 = left

// COLS
// 0..4

@interface GameAnimatedSprite : NSObject {
    UIImage *_image;
    UIImage *sprite[4][4];
    BOOL _animates;
    
    int _row;
    int _col;
}

- (id)initWithSpriteSheet:(UIImage *)sheet size:(CGSize)spriteSize;
- (id)initWithSpriteSheet:(UIImage *)sheet size:(CGSize)spriteSize coloring:(int)coloring;

- (UIImage *)updateSpriteRow:(int)row andCol:(int)col;

@property (nonatomic, readonly) BOOL animates;
@property (nonatomic, readonly) UIImage *image;

@end
