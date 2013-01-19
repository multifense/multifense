//
//  ViewController.m
//  MPTD
//
//  Created by Karl-Johan Alm on 4/18/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <AVFoundation/AVAudioPlayer.h>

#import "ViewController.h"
#import "GameViewController.h"
#import "AppDelegate.h"
#import "GameViewController.h"
#import "GameMapSelectViewController.h"
#import "GameSound.h"

@interface ViewController ()

@end

@implementation ViewController

@synthesize currentGame = _currentGame;

- (BOOL)textFieldShouldEndEditing:(UITextField *)textField
{
    return [self textFieldShouldReturn:textField];
}

- (BOOL)textFieldShouldReturn:(UITextField *)textField
{
    AppDelegate *appd = [AppDelegate appDelegate];
    if (textField.text.length > 0) {
        if ([textField.text cStringUsingEncoding:NSASCIIStringEncoding] == nil) {
            UIAlertView *av = [[UIAlertView alloc] initWithTitle:@"Error"
                                                         message:@"Your name may not contain extended characters."
                                                        delegate:nil 
                                               cancelButtonTitle:@"Dismiss"
                                               otherButtonTitles:nil];
            [av show];
            return NO;
        }
        [appd setPlayerName:textField.text];
    } else textField.text = [appd playerName];
    [textField resignFirstResponder];
    _quickMatchButton.enabled = (appd.playerName.length > 0);
    return  YES;
}

- (void)viewDidLoad
{
    [super viewDidLoad];
    
    NSUserDefaults *defaults = [NSUserDefaults standardUserDefaults];

    AppDelegate *appd = [AppDelegate appDelegate];
    appd.playerName = [defaults objectForKey:@"playerName"];
    
    _textFieldPlayerName.text = appd.playerName;
    _quickMatchButton.enabled = (appd.playerName.length > 0);
    
    [[UIApplication sharedApplication] setStatusBarOrientation:UIInterfaceOrientationLandscapeLeft];
    
    _resumeGameButton.enabled = _currentGame != nil;
}

- (void)viewWillAppear:(BOOL)animated
{
    [super viewWillAppear:animated];
    [[UIApplication sharedApplication] setStatusBarHidden:NO withAnimation:UIStatusBarAnimationFade];
 
    [[GameSound defaultInstance] switchBackgroundMusic:@"multower deplayer.mp3"];
    [[GameViewController currentGameViewController] shutdown];
    [[UIApplication sharedApplication] setIdleTimerDisabled:NO];
}

- (void)viewDidUnload
{
    [super viewDidUnload];
    // Release any retained subviews of the main view.
}

- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation
{
    return UIInterfaceOrientationIsLandscape(interfaceOrientation);
}

- (IBAction)didTapStartGame:(id)sender
{
#if 0
    GameViewController *gvc = [[GameViewController alloc] initGame:NO];
#endif
    [[GameViewController currentGameViewController] shutdown];
    GameMapSelectViewController *gvc = [[GameMapSelectViewController alloc] initWithNibName:@"GameMapSelectViewController" bundle:[NSBundle mainBundle]];
    setWindowRoot(gvc);
}

- (IBAction)didTapResumeGame:(id)sender
{
    if (_currentGame == nil) {
        [self didTapStartGame:sender];
        return;
    }
    setWindowRoot(_currentGame);
    [_currentGame resume];
}

- (IBAction)didTapMPGame:(id)sender
{
    [[GameViewController currentGameViewController] shutdown];
    GameViewController *gvc = [[GameViewController alloc] initGame:YES map:1];
    setWindowRoot(gvc);
}

@end

