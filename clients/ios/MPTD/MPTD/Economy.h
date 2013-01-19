//
//  Economy.h
//  MPTD
//
//  Created by Karl-Johan Alm on 4/26/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//
#if 0

#import <Foundation/Foundation.h>

@class Monster;
@class Player;

@interface Economy : NSObject {
    int _mpIncome;
    int _spIncome;
    __unsafe_unretained Player *_me;
}

- (int)getSPIncome:(int)waveCount;

- (int)getMonsterMoney:(Monster *)mon;

- (int)buyMonster:(Monster *)mon;

@property (nonatomic, readonly) int mpIncome;
@property (nonatomic, assign) Player *me;

@end
#endif
