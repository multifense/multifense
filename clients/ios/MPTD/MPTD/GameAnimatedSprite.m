//
//  GameAnimatedSprite.m
//  MPTD
//
//  Created by Karl-Johan Alm on 5/11/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <QuartzCore/QuartzCore.h>
#import "GameAnimatedSprite.h"
#import "CSTintedImageView.h"

@implementation GameAnimatedSprite

@synthesize image = _image;
@synthesize animates = _animates;

- (id)initWithSpriteSheet:(UIImage *)sheet size:(CGSize)spriteSize coloring:(int)coloring
{
    if (! coloring) return [self initWithSpriteSheet:sheet size:spriteSize];
    
    CSTintedImageView *iv = [[CSTintedImageView alloc] initWithImage:sheet];
    
    switch (coloring) {
        case 1:
            // yellow
            iv.tintColor = [UIColor yellowColor];
            break;
        case 2:
            // brown
            iv.tintColor = [UIColor brownColor];
            break;
        case 3:
            // shadow
            iv.tintColor = [UIColor grayColor];
            break;
    }

    UIGraphicsBeginImageContext(sheet.size);
    [iv.layer renderInContext:UIGraphicsGetCurrentContext()];
    UIImage *img = UIGraphicsGetImageFromCurrentImageContext();
    UIGraphicsEndImageContext();

    return [self initWithSpriteSheet:img size:spriteSize];
}

- (id)initWithSpriteSheet:(UIImage *)sheet size:(CGSize)spriteSize
{
    self = [super init];
    if (self) {
        _animates = sheet.size.width > spriteSize.width;
        if (!_animates) {
            _image = sheet;
            return self;
        }
        // sprite sheet loading
        NSLog(@"loading from sprite sheet of size %@", NSStringFromCGSize(sheet.size));
        CGImageRef sheetRef = sheet.CGImage;

        // loop 4x4 (rows and columns) to pop image array
        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 4; j++) {
                CGImageRef imageRef = CGImageCreateWithImageInRect(sheetRef, 
                                                                   (CGRect){i*spriteSize.width,j*spriteSize.height,spriteSize});
                sprite[i][j] = [[UIImage alloc] initWithCGImage:imageRef];
                CGImageRelease(imageRef);
            }
        }
        _row = _col = 0;
        _image = sprite[0][0];
    }
    return self;
}

- (UIImage *)updateSpriteRow:(int)row andCol:(int)col
{
    _row = row;
    _col = col;
    _image = sprite[row][col];
    return _image;
}

@end
