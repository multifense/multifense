//
//  GameTrackingProjectile.m
//  MPTD
//
//  Created by Karl-Johan Alm on 4/26/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import "GameTrackingProjectile.h"
#import "Monster.h"
#import "GameSession.h"

@implementation GameTrackingProjectile

- (id)initWithDamage:(double)damage speed:(double)speed target:(GameObject *)target sprite:(GameAnimatedSprite *)sprite frame:(CGRect)frame
{
    self = [super initWithDamage:damage speed:speed sprite:sprite frame:frame];
    if (self) {
        _target = target;
    }
    return self;
}

- (void)update
{
    if (_target == nil || [(Monster *)_target dead]) {
        [_session removeBullet:self];
        return;
    }
    
    CGPoint tc = _target.center;
    CGPoint c = self.center;
    double distance = [self findDistance:_target];
    
    // grab movement x- and y based on the distance
    CGRect f = self.frame;
    
    f.origin.x += (tc.x - c.x)/(distance/_speed);
    f.origin.y += (tc.y - c.y)/(distance/_speed);
    
    self.frame = f;
    distance = [self findDistance:_target];

    if (distance <= 2 * _speed) {
        [(Monster *)_target takeDamage:_damage];
        _reachedTarget = YES;
        [_session removeBullet:self];
    }
}

- (BOOL)hasHit
{
    return _reachedTarget;
}

@end
