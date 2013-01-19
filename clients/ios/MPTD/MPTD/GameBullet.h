//
//  GameBullet.h
//  MPTD
//
//  Created by Karl-Johan Alm on 4/26/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import "GameObject.h"

@class GameSession;

@interface GameBullet : GameObject {
    double _damage;
    double _speed;
}

- (id)initWithDamage:(double)damage speed:(double)speed sprite:(GameAnimatedSprite *)sprite frame:(CGRect)frame;

- (void)update;

@end
