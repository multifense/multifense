//
//  GameViewController.h
//  MPTD
//
//  Created by Karl-Johan Alm on 4/19/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>

@class GameSession, TowerMenuView, OpponentStatusView, MonsterMenuView, StatusMenu, GameMessageView;

@interface GameViewController : UIViewController <UIScrollViewDelegate> {
    GameSession *_session;
    BOOL _multiplayer;
    BOOL _searching;
    
    IBOutlet UIScrollView *_scrollView;
    IBOutlet StatusMenu *_statusView;
    IBOutlet OpponentStatusView *_opponentStatusView;
    IBOutlet TowerMenuView *_buildMenu;
    IBOutlet MonsterMenuView *_monsterMenu;
    IBOutlet UIButton *_buildButton;
    IBOutlet UIButton *_recruitButton;

    IBOutlet UIImageView *_loadingIV;
    IBOutlet UIView *_loadingView;
    IBOutlet UILabel *_loadingLabel;
    IBOutlet GameMessageView *_msgView;
    
    IBOutlet UILabel *_nextWaveLabel;
    
    IBOutlet UILabel *_playersFound;
    
    IBOutlet UIButton *_quitButton;
}

+ (GameViewController *)currentGameViewController;

- (id)initGame:(BOOL)multiplayer map:(int)mapID;

- (void)shutdown;

- (void)applicationDeactivating;
- (void)applicationActivating;
   
- (void)playerDidDie;

- (IBAction)didTapBuildButton:(UIButton *)sender;
- (IBAction)didTapMenuButton:(UIButton *)sender;
- (IBAction)didTapRecruitMonster:(UIButton *)sender;
- (void)didSingleTap:(UITapGestureRecognizer *)recognizer;

- (void)closeBuildMenu;

- (void)resume;

@property (nonatomic, readonly) UILabel *playersFound;
@property (nonatomic, readwrite) BOOL searching;
@property (nonatomic, readonly) GameSession *session;
@property (nonatomic, readonly) UIView *loadingView;
@property (nonatomic, readonly) UILabel *loadingLabel;
@property (nonatomic, readonly) UIImageView *loadingIV;

@end
