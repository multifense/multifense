//
//  GameObject.h
//  MPTD
//
//  Created by Karl-Johan Alm on 4/19/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>

#define FPS (1.0/0.033)
#define SPF (0.033)

typedef int GameObjectTypeID;

@class GameSession, GameAnimatedSprite, Player;

@interface GameObject : UIImageView {
    int _type;
    __strong GameAnimatedSprite *_sprite;
    __unsafe_unretained GameSession *_session;
    
    __unsafe_unretained UILabel *_typeLabel;

    GameObjectTypeID typeID;
    
    int _cost;
    int _stateID;
    int _frameDelay;
    int _frameUpdateCounter;
    int _currentFrame;
    int _numberOfFrames;
}

- (id)initWithSprite:(GameAnimatedSprite *)sprite frame:(CGRect)frame;

- (double)findDistance:(GameObject *)ob;

- (void)setSpriteFromSpriteSheet:(UIImage *)spriteSheet withSize:(CGSize)size;

- (void)update;

- (void)updateLabelsWithPlayer:(Player *)player;

@property (nonatomic, readwrite)    int         cost;
@property (nonatomic, assign)       UILabel     *typeLabel;
@property (nonatomic, readonly)     GameAnimatedSprite *sprite;
@property (nonatomic, assign)       GameSession *session;
@property (nonatomic, readwrite)    int         type;

@end
