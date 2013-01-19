//
//  Tower.h
//  MPTD
//
//  Created by Karl-Johan Alm on 4/18/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "GameObject.h"
#import <AudioToolbox/AudioToolbox.h>

@class GameSession, RangeView, GameMap, Monster, Player, AVAudioPlayer;

@interface Tower : GameObject {
@private
    int _soundID;
    CGPoint _position;
    double _fireRate;
    double _range;
    NSInteger _damage;

    Monster *_currentTarget;
    
    __unsafe_unretained GameMap *_map;
    NSInteger _shotsFired;
    int _nextShot;
    
    double _bulletSpeed;
    GameAnimatedSprite *_bulletSprite;
    NSTimeInterval lastshot;
    
    NSString *_sound;
    NSURL *_soundFileURL;
    
    RangeView *_rangeView;
    BOOL _rangeIndicator;
}

- (id)initWithSprite:(GameAnimatedSprite *)sprite position:(CGPoint)position fireRate:(double)fireRate damage:(NSInteger)damage range:(double)range bulletSpeed:(double)bulletSpeed bulletSprite:(GameAnimatedSprite *)bulletSprite;

- (void)update; 

- (void)selectTarget;

@property (nonatomic, readwrite) BOOL rangeIndicator;
@property (nonatomic, readwrite) RangeView *rangeView;
@property (nonatomic, readwrite) CGPoint position;
@property (nonatomic, readwrite) double fireRate;
@property (nonatomic, readwrite) NSInteger damage;
@property (nonatomic, readwrite) double range;

@property (nonatomic, readonly) NSInteger shotsFired;

@property (nonatomic, readwrite) double bulletSpeed;
@property (nonatomic, readwrite) GameAnimatedSprite *bulletSprite;
@property (nonatomic, readwrite) GameAnimatedSprite *towerSprite;
@property (nonatomic, retain) NSString *sound;

@end
