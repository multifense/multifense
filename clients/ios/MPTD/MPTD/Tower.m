//
//  Tower.m
//  MPTD
//
//  Created by Karl-Johan Alm on 4/18/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import "Tower.h"
#import "Monster.h"
#import "GameSession.h"
#import "GameMap.h"
#import "GameTrackingProjectile.h"
#import "Player.h"
#import "GameSound.h"
#import "RangeView.h"

@interface Tower ()

@property (nonatomic, readwrite) int soundID;

@end

@implementation Tower

@synthesize rangeView = _rangeView;
@synthesize position = _position;
@synthesize shotsFired = _shotsFired;
@synthesize range = _range;
@synthesize bulletSpeed = _bulletSpeed;
@synthesize bulletSprite = _bulletSprite;
@synthesize sound = _sound;
@synthesize soundID = _soundID;

- (void)setSound:(NSString *)sound
{
    _sound = sound;
    _soundID = [[GameSound defaultInstance] soundIndexForFile:sound];
}

- (void)setFrame:(CGRect)frame
{
    [super setFrame:frame];
    if (_rangeIndicator) {
                _rangeView.frame = (CGRect){frame.origin.x + frame.size.width/2.f - _range, frame.origin.y + frame.size.height/2.f - _range, _range * 2, _range * 2};
    }
}

- (BOOL)rangeIndicator 
{
    return _rangeIndicator;
}

- (void)setRangeIndicator:(BOOL)rangeIndicator
{
    if (rangeIndicator == _rangeIndicator) return;
    _rangeIndicator = rangeIndicator;
    if (_rangeIndicator) {
        _rangeView = [[RangeView alloc] initWithFrame:self.frame];
        [self.superview addSubview:_rangeView];
    } else {
        [_rangeView removeFromSuperview];
    }
}

- (id)init
{
    self = [super init];
    if (self) {
        _position = (CGPoint) {0,0};
        _damage = 1;
        _fireRate = 1.0;
        _nextShot = 0;
    }
    return self;
}

- (id)initWithSprite:(GameAnimatedSprite *)sprite position:(CGPoint)position fireRate:(double)fireRate damage:(NSInteger)damage range:(double)range bulletSpeed:(double)bulletSpeed bulletSprite:(GameAnimatedSprite *)bulletSprite
{
    self = [super initWithSprite:sprite frame:(CGRect){position, 50.f, 50.f}];
    if (self) {
        _position = position;
        _fireRate = fireRate;
        _damage = damage;
        _range = range;
        _nextShot = 0;
        _bulletSpeed = bulletSpeed;
        _bulletSprite = bulletSprite;
        _currentTarget = nil;
    }
    return self;
}

- (id)copy
{
    Tower *t = [[Tower alloc] initWithSprite:_sprite position:_position fireRate:_fireRate damage:_damage range:_range bulletSpeed:_bulletSpeed bulletSprite:_bulletSprite];
    t.cost = _cost;
    t.type = _type;
    t.soundID = _soundID;
    return t;
}

- (NSInteger)damage { return _damage; }
- (void)setDamage:(NSInteger)damage
{
    if (damage > 0) _damage = damage;
}

- (double)fireRate { return _fireRate; }
- (void)setFireRate:(double)fireRate
{
    if (fireRate > 0.f) _fireRate = fireRate;
}

- (void)selectTarget
{
    Monster *nearest = nil;
    double neardist = 0.0;
    for (Monster *m in _session.monsters) {
        double dist = [self findDistance:m];
        if (dist <= _range) {
            _currentTarget = m;
            return;
            if (nearest == nil || dist < neardist) {
                nearest = m;
                neardist = dist;
            }
        }
    }
    _currentTarget = nearest;
}

- (BOOL)fire
{
    if (_currentTarget.dead || _currentTarget == nil || [self findDistance:_currentTarget] > _range) {
        // we don't have a target or our current target is out of range
        [self selectTarget];
    }
    if (_currentTarget == nil) return NO;
    
    //NSLog(@"time since last shot = %f", [NSDate timeIntervalSinceReferenceDate] - lastshot);
    lastshot = [NSDate timeIntervalSinceReferenceDate];
    
    CGPoint orig = self.center;
    orig.x -= 25.f;
    orig.y -= 25.f;
    
    if (_soundID) playEffect(_soundID);
    //AudioServicesPlaySystemSound((SystemSoundID)soundID);
    //if (player.playing) player.currentTime = 0.0; else [player play];
    
    GameTrackingProjectile *proj = [[GameTrackingProjectile alloc] initWithDamage:_damage speed:_bulletSpeed target:_currentTarget sprite:_bulletSprite frame:(CGRect){orig, {25.f, 25.f}}];
    [_session addBullet:proj];
    return YES;
}

- (void)update
{
    [super update];
    _nextShot--;
    if (_nextShot <= 0) {
        if ([self fire])
            _nextShot += _fireRate;
        else {
            _nextShot = 1;
            return;
        }
    }
}

- (GameSession *)session
{ 
    return _session;
}
- (void)setSession:(GameSession *)session
{
    _session = session;
    _map = session.map;
}

- (GameAnimatedSprite *)towerSprite 
{
    return _sprite;
}

- (void)setTowerSprite:(GameAnimatedSprite *)towerSprite
{
    _sprite = towerSprite;
}

- (void)setPosition:(CGPoint)position
{
    _position = position;
    self.frame = (CGRect){position, 50.f, 50.f};
}

@end
