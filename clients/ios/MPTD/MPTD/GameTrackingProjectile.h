//
//  GameTrackingProjectile.h
//  MPTD
//
//  Created by Karl-Johan Alm on 4/26/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import "GameBullet.h"

@interface GameTrackingProjectile : GameBullet {
    GameObject *_target;
    BOOL _reachedTarget;
}

- (id)initWithDamage:(double)damage speed:(double)speed target:(GameObject *)target sprite:(GameAnimatedSprite *)sprite frame:(CGRect)frame;

- (BOOL)hasHit;

@end
