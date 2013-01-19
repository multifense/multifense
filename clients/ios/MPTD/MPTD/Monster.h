//
//  Monster.h
//  MPTD
//
//  Created by Karl-Johan Alm on 4/19/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import "GameObject.h"

@class GameSession, GameMap, GamePathPoint;

typedef int MonsterTypeID;
#define MonsterType(i) [NSNumber numberWithInt:i]

@interface Monster : GameObject {
@private
    int _owner;
    UIView *_hpBar;
    double _maxHP;
    double _hp;
    NSInteger _armor;
    double _speed;
    BOOL _dead;

    int _incomeIncrease;
    int _coloring;
    
    __unsafe_unretained GameMap *_map;
    GamePathPoint *_pathPoint;
    
    double _currDistance;
    double _totalMetersWalked;
    CGPoint _direction;
}

- (id)initWithSprite:(GameAnimatedSprite *)sprite position:(CGPoint)position;
- (id)initWithSprite:(GameAnimatedSprite *)sprite frame:(CGRect)frame;

- (void)takeDamage:(double)damage;
- (void)update; 
- (void)pushBack:(double)distance;

- (void)moveDistance:(double)distance;

@property (nonatomic, readwrite)    int         owner;
@property (nonatomic, readwrite)    double      maxHP;
@property (nonatomic, readwrite)    double      hp;
@property (nonatomic, readwrite)    NSInteger   armor;
@property (nonatomic, readwrite)    double      speed;
@property (nonatomic, readwrite)    int         incomeIncrease;
@property (nonatomic, readwrite)    int         coloring;

@property (nonatomic, readonly)     BOOL        dead;

@property (nonatomic, readonly)     double      currDistance;

@property (nonatomic, readonly)     CGPoint     direction;

@end
