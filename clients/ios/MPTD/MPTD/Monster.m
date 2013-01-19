//
//  Monster.m
//  MPTD
//
//  Created by Karl-Johan Alm on 4/19/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import "Monster.h"
#import "GameSession.h"
#import "GameMap.h"
#import "GamePath.h"
#import "GamePathPoint.h"

@interface Monster ()

- (void)updateState;

@end

@implementation Monster

@synthesize owner = _owner;
@synthesize hp = _hp;
@synthesize armor = _armor;
@synthesize speed = _speed;
@synthesize dead = _dead;
@synthesize currDistance = _currDistance;
@synthesize direction = _direction;
@synthesize incomeIncrease = _incomeIncrease;
@synthesize coloring = _coloring;

- (void)Monster
{
    _totalMetersWalked = _currDistance = 0.0;
    _hpBar = [[UIView alloc] initWithFrame:(CGRect){CGPointZero, self.frame.size.width, 5}];
    _hpBar.autoresizingMask = UIViewAutoresizingFlexibleWidth;
    _hpBar.backgroundColor = [UIColor greenColor];
    [self addSubview:_hpBar];
}

- (id)initWithSprite:(GameAnimatedSprite *)sprite frame:(CGRect)frame
{
    self = [super initWithSprite:sprite frame:frame];
    if (self) {
        [self Monster];
    }
    return self;
}

- (id)initWithSprite:(GameAnimatedSprite *)sprite position:(CGPoint)position
{
    self = [super initWithSprite:sprite frame:(CGRect){position, 100,100}];
    if (self) {
        [self Monster];
    }
    return self;
}

- (id)copy
{
    Monster *m = [[Monster alloc] initWithSprite:_sprite frame:self.frame];
    m.maxHP = _maxHP;
    m.armor = _armor;
    m.speed = _speed;
    m.cost = _cost;
    m.incomeIncrease = _incomeIncrease;
    m.coloring = _coloring;
    m.type = _type;
    return m;
}

- (void)moveDistance:(double)d
{
    CGPoint p = self.frame.origin;
    p.x += d * _direction.x;
    p.y += d * _direction.y;
    self.frame = (CGRect) {p, self.frame.size};
    _currDistance -= d;
    _totalMetersWalked += d;
}

- (void)move
{
    // a monster moves speed * SPF meters per frame
    [self moveDistance:_speed];
    if (_currDistance <= 0.f) {
        // we reached the next point
        static Monster *mum = nil;
        if (mum == nil) mum = self;
        if (mum == self) {
            NSLog(@"reached next point");
        }
        double remains = -_currDistance;
        _pathPoint = _pathPoint.next;
        if (_pathPoint != nil) {
            // we're not done yet
            _direction = _pathPoint.dir;
            [self updateState];
            //self.frame = (CGRect){_pathPoint.p, self.frame.size};
            self.center = _pathPoint.p;
            _totalMetersWalked -= remains;
            if (remains) {
                [self moveDistance:remains];
            }
        } else {
            // we're done
            [_session monsterReachedEnd:self];
            [_session removeMonster:self];
        }
        _currDistance = _pathPoint.length - remains;
    }
}

- (void)takeDamage:(double)damage
{
    //NSLog(@"hp: %f, taking damage %f", _hp, damage);
    _hp -= damage;
    if (_hp <= 0) {
        _dead = YES;
        [_session monsterWasKilled:self];
        [_session removeMonster:self];
        return;
    }
    CGRect r = _hpBar.frame;
    r.size.width = self.frame.size.width * (_hp / _maxHP);
    _hpBar.frame = r;
    
    r = self.frame;
    r.origin.y -= 25.f;
    r.size.width = 50.f;
    UILabel *l = [[UILabel alloc] initWithFrame:r];
    l.backgroundColor = [UIColor clearColor];
    l.textColor = [UIColor redColor];
    l.text = @"OW!";
    [self.superview addSubview:l];
    [UIView beginAnimations:nil context:nil];
    [UIView setAnimationDuration:1.0];
    l.alpha = 0.f;
    [UIView commitAnimations];
    dispatch_async(dispatch_get_global_queue(DISPATCH_QUEUE_PRIORITY_LOW, 0), ^{
        sleep(1);
        dispatch_async(dispatch_get_main_queue(), ^{
            [l removeFromSuperview];
        });
    });
}

- (void)pushBack:(double)distance
{
    [self moveDistance:-distance];
}

- (void)update
{
    [super update];
    [self move];
}

- (GameSession *)session
{ 
    return _session;
}

- (void)setSession:(GameSession *)session
{
    _maxHP = _hp;
    assert(_maxHP > 0);
    _session = session;
    _map = session.map;
    assert(_map.path.pointCount);
    _pathPoint = [_map.path pathStart];
    _direction = _pathPoint.dir;
    [self updateState];
    //self.frame = (CGRect){_pathPoint.p.x,_pathPoint.p.y, self.frame.size};
    self.center = _pathPoint.p;
    _totalMetersWalked = 0.0;
    _currDistance = _pathPoint.length;
}

- (void)setSpeed:(double)speed
{
    _speed = speed;
}

- (double)maxHP
{
    return _maxHP;
}

- (void)setMaxHP:(double)maxHP
{
    _maxHP = _hp = maxHP;
}

- (void)updateState
{
    if (_direction.y > .5f) {
        _stateID = 1;
    } else if (_direction.y < -.5f) {
        _stateID = 0;
    } else if (_direction.x < -.5f) {
        _stateID = 3;
    } else {
        _stateID = 2;
    }
}

@end
