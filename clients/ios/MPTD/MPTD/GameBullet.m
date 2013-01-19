//
//  GameBullet.m
//  MPTD
//
//  Created by Karl-Johan Alm on 4/26/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import "GameBullet.h"
#import "GameSession.h"

@implementation GameBullet


- (id)initWithDamage:(double)damage speed:(double)speed sprite:(GameAnimatedSprite *)sprite frame:(CGRect)frame
{
    self = [super initWithSprite:sprite frame:frame];
    if (self) {
        _damage = damage;
        _speed = speed;
    }
    return self;
}

- (void)update
{
}

@end
