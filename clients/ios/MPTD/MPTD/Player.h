//
//  Player.h
//  MPTD
//
//  Created by Karl-Johan Alm on 4/26/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>

@class StatusMenu, GameSession, OpponentStatusView;

@interface Player : NSObject {
    NSString *_nick;
    int _pid;
    __unsafe_unretained GameSession *_session;
    int _hp;
    int _gold;
    int _wave;
    int _kills;
    int _income;
    BOOL _singlePlayer;
    BOOL _dead;
    
    __unsafe_unretained StatusMenu *_sm;
    __unsafe_unretained OpponentStatusView *_osv;
}

@property (nonatomic, retain) NSString *nick;
@property (nonatomic, readwrite) int pid;
@property (nonatomic, assign) GameSession *session;
@property (nonatomic, assign) StatusMenu *sm;
@property (nonatomic, assign) OpponentStatusView *osv;
@property (nonatomic, readwrite) int hp;
@property (nonatomic, readwrite) int gold;
@property (nonatomic, readwrite) int wave;
@property (nonatomic, readwrite) int kills;
@property (nonatomic, readwrite) int income;
@property (nonatomic, readwrite) BOOL singlePlayer;
@property (nonatomic, readwrite) BOOL dead;

@end
