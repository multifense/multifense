//
//  GameMap.h
//  MPTD
//
//  Created by Karl-Johan Alm on 4/19/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import "GameObject.h"
#import "GamePathPoint.h"
#import "GamePath.h"

@interface GameMap : UIImageView {
    GamePath *_path;
    CGSize _size;
    NSMutableArray *_pathRects;
}

- (id)initWithImage:(UIImage *)image;

- (void)processPath;

- (BOOL)intersects:(CGRect)checkRect;

@property (nonatomic, readonly) GamePath *path;
@property (nonatomic, readonly) CGSize size;

@end
