//
//  GameEndViewController.m
//  MPTD
//
//  Created by Karl-Johan Alm on 5/11/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import "GameEndViewController.h"
#import "AppDelegate.h"
#import "ViewController.h"
#import "GameSound.h"

@interface GameEndViewController ()

@end

@implementation GameEndViewController

@synthesize state = _state;

- (void)didTap
{
    ViewController *vc = [[ViewController alloc] initWithNibName:@"ViewController" bundle:[NSBundle mainBundle]];
    setWindowRoot(vc);
}

- (void)viewDidLoad
{
    [super viewDidLoad];
    // Do any additional setup after loading the view from its nib.
    
    UITapGestureRecognizer *tgr = [[UITapGestureRecognizer alloc] initWithTarget:self action:@selector(didTap)];
    [self.view addGestureRecognizer:tgr];
    
    switch (_state) {
        case GameEndSPVictory:
        case GameEndMPVictory:
            [[GameSound defaultInstance] switchBackgroundMusic:@"multower deplayer.mp3"];
            _imageView.image = [UIImage imageNamed:@"youwin.png"];
            break;
        case GameEndSPLoss:
            [[GameSound defaultInstance] switchBackgroundMusicNoLoop:@"multower slowed (defeat background music).mp3"];
            _imageView.image = [UIImage imageNamed:@"gameoversp.png"];
            break;
        case GameEndMPLoss:
            [[GameSound defaultInstance] switchBackgroundMusicNoLoop:@"multower slowed (defeat background music).mp3"];
            _imageView.image = [UIImage imageNamed:@"youLosemp.png"];
            break;
    }
}

- (void)viewDidUnload
{
    [super viewDidUnload];
    // Release any retained subviews of the main view.
    // e.g. self.myOutlet = nil;
}

- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation
{
    return UIInterfaceOrientationIsLandscape(interfaceOrientation);
}

@end
