//
//  ViewController.h
//  MPTD
//
//  Created by Karl-Johan Alm on 4/18/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>

@class GameViewController;

@interface ViewController : UIViewController <UITextFieldDelegate> {
    GameViewController *_currentGame;
    IBOutlet UIButton *_resumeGameButton;
    IBOutlet UIButton *_quickMatchButton;
    IBOutlet UITextField *_textFieldPlayerName;
}

- (IBAction)didTapStartGame:(id)sender;
- (IBAction)didTapResumeGame:(id)sender;
- (IBAction)didTapMPGame:(id)sender;

@property (nonatomic, retain) GameViewController *currentGame;

@end
