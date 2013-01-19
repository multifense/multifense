//
//  GameObject.m
//  MPTD
//
//  Created by Karl-Johan Alm on 4/19/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import "GameObject.h"
#import "GameAnimatedSprite.h"
#import "GameSession.h"
#import "Player.h"

@implementation GameObject

@synthesize sprite = _sprite;
@synthesize session = _session;
@synthesize type = _type;
@synthesize typeLabel = _typeLabel;
@synthesize cost = _cost;

- (id)initWithSprite:(GameAnimatedSprite *)sprite frame:(CGRect)frame
{
    self = [super initWithFrame:frame];
    if (self) {
        _sprite = sprite;
        self.image = _sprite.image;
        _frameDelay = 2; // 15 fps
        _numberOfFrames = 4;
        _stateID = 0;
    }
    return self;
}

- (void)updateLabelsWithPlayer:(Player *)player
{
    assert(_typeLabel != nil);
    //NSLog(@"updating %@ label %@: player %@", NSStringFromClass(self.class), NSStringFromCGRect(_typeLabel.frame), player.gold < _cost ? @"CANNOT afford" : @"AFFORDS");
    _typeLabel.textColor = player.gold < _cost ? [UIColor redColor] : [UIColor colorWithRed:0.1f green:1.f blue:0.1f alpha:1.f];
}

- (double)findDistance:(GameObject *)ob
{
    double vx = ob.frame.origin.x - self.frame.origin.x;
    double vy = ob.frame.origin.y - self.frame.origin.y;
    
    return sqrt(vx*vx + vy*vy);
}

- (void)update
{
    if (! _sprite.animates) return;
    if (_frameUpdateCounter == 0) {
        _currentFrame++;
        if (_currentFrame > _numberOfFrames - 1) {
            _currentFrame = 0;
        }
        // just update to next frame
        self.image = [_sprite updateSpriteRow:_currentFrame andCol:_stateID];
        _frameUpdateCounter = _frameDelay;
    } else {
        _frameUpdateCounter--;
    }
}

- (void)setSpriteFromSpriteSheet:(UIImage *)spriteSheet withSize:(CGSize)size
{
    _sprite = [[GameAnimatedSprite alloc] initWithSpriteSheet:spriteSheet size:size];
}

@end
