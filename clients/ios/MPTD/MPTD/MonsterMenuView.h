//
//  MonsterMenuView.h
//  MPTD
//
//  Created by Karl-Johan Alm on 5/11/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>

@class GameSession;

@interface MonsterMenuView : UIImageView {
    NSArray *_monsterTypes;
    NSMutableArray *_monsterButtons;
    __unsafe_unretained GameSession *_session;
    NSInteger _selectedMonsterType;
}

- (BOOL)checkTap:(CGPoint)position;

@property (nonatomic, assign) GameSession *session;
@property (nonatomic, readonly) NSInteger selectedMonsterType;

@end
