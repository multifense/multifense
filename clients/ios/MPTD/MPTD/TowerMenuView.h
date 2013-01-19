//
//  TowerMenuView.h
//  MPTD
//
//  Created by Karl-Johan Alm on 4/26/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>

@class GameSession, Tower, GameViewController;

@interface TowerMenuView : UIView {
    NSArray *_towerTypes;
    NSMutableArray *_towerButtons;
    __unsafe_unretained GameSession *_session;
    __unsafe_unretained GameViewController *_gameVC;
    NSInteger _selectedTowerType;
    
    Tower *_draggedTower;
    CGRect _anchor;
    BOOL _pendingRangeIndicatorUpdate;
}

- (BOOL)checkTap:(CGPoint)position;

- (void)resetSelectedTowerType;

@property (nonatomic, assign) GameViewController *gameVC;
@property (nonatomic, assign) GameSession *session;
@property (nonatomic, readonly) NSInteger selectedTowerType;

@end
