//
//  GameViewController.m
//  MPTD
//
//  Created by Karl-Johan Alm on 4/19/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <QuartzCore/QuartzCore.h>
#import "AppDelegate.h"

#import "GameViewController.h"
#import "GameSession.h"
#import "GamePathPoint.h"
#import "GameMap.h"
#import "Tower.h"
#import "TowerMenuView.h"
#import "MonsterMenuView.h"
//#import "TowerType.h"
#import "Player.h"
#import "Economy.h"
#import "ViewController.h"
#import "GameSound.h"
#import "StatusMenu.h"
#import "OpponentStatusView.h"
#import "GameNetwork.h"

@interface GameViewController ()

- (void)didSingleTap:(UITapGestureRecognizer *)recognizer;

@end

@implementation GameViewController

@synthesize searching = _searching;
@synthesize loadingView = _loadingView;
@synthesize loadingLabel = _loadingLabel;
@synthesize loadingIV = _loadingIV;
@synthesize playersFound = _playersFound;

static GameViewController *_currentGame = nil;

+ (GameViewController *)currentGameViewController
{
    //assert (_currentGame);
    return _currentGame;
}

@synthesize session = _session;

- (void)dealloc
{
    [[UIApplication sharedApplication] setIdleTimerDisabled:NO];
}

- (void)connectToServer
{
    [_session connectToHost:@"130.229.154.15" port:1337]; //@"83.179.38.37"
    //[_session connectToHost:@"83.179.38.37" port:1337];
}

- (id)initGame:(BOOL)multiplayer map:(int)mapID
{
    self = [super initWithNibName:@"GameViewController" bundle:[NSBundle mainBundle]];
    if (self) {
        _currentGame = self;
        _multiplayer = multiplayer;
        _session = [[GameSession alloc] initGame:multiplayer onMap:mapID];
        _searching = _multiplayer;
        
        if (multiplayer) {
            [_session stopGame];
            [self connectToServer];
            mapID = 1;
        }

#if 0
        GamePath *path = [[GamePath alloc] init];
        switch (mapID) {
            case 2:
                [path addPathPoint:(CGPoint){200,0}];
                [path addPathPoint:(CGPoint){200,400}];
                [path addPathPoint:(CGPoint){100,400}];
                [path addPathPoint:(CGPoint){100,600}];
                [path addPathPoint:(CGPoint){1400,600}];
                [path addPathPoint:(CGPoint){1400,150}];
                [path addPathPoint:(CGPoint){550,150}];
                [path addPathPoint:(CGPoint){550,800}];
                [path addPathPoint:(CGPoint){1150,800}];
                [path addPathPoint:(CGPoint){1150,960}];
                _session.map = [_session loadMap:[UIImage imageNamed:@"awsomemap.png"] path:path];
                break;
                
            default:
                [path addPathPoint:(CGPoint){0, 192}];
                [path addPathPoint:(CGPoint){140, 192}]; // 118, 211
                [path addPathPoint:(CGPoint){140, 750}]; // 118, 790
                [path addPathPoint:(CGPoint){483, 750}];
                [path addPathPoint:(CGPoint){483, 487}];
                [path addPathPoint:(CGPoint){700, 487}];
                [path addPathPoint:(CGPoint){700, 67}];
                [path addPathPoint:(CGPoint){915, 67}];
                [path addPathPoint:(CGPoint){915, 751}];
                [path addPathPoint:(CGPoint){1308, 751}];
                [path addPathPoint:(CGPoint){1308, 285}];
                [path addPathPoint:(CGPoint){1600, 285}];
                _session.map = [_session loadMap:[UIImage imageNamed:@"map1600x960.png"] path:path];
                break;
        }
#endif
    }
    return self;
}

- (void)viewDidLoad
{
    [super viewDidLoad];
    
    if (! _multiplayer) {
        // disable recruit button!
        _monsterMenu.hidden = YES;
        _recruitButton.hidden = YES;
        _nextWaveLabel.hidden = YES;
        _statusView.waveTimer.hidden = YES;
        _statusView.userInteractionEnabled = YES;
    }
    
    // we bring loadingview to front simply cause it's a pain in the ass to grok xib if it's at front there
    [_loadingView.superview bringSubviewToFront:_loadingView];
    _loadingView.hidden = !_multiplayer;
    _loadingView.userInteractionEnabled = NO;

    _session.messageView = _msgView;
    _session.me.osv = _opponentStatusView;
    _session.gvc = self;
    _session.loadingView = _loadingIV;
    
    UITapGestureRecognizer *tgr = [[UITapGestureRecognizer alloc] initWithTarget:self action:@selector(didSingleTap:)];
    [_session.map addGestureRecognizer:tgr];
    _session.map.userInteractionEnabled = YES;

    [_scrollView addSubview:_session.map];
    _scrollView.contentSize = _session.map.size;
    _buildMenu.userInteractionEnabled = NO;
    _buildMenu.session = _session;
    _buildMenu.gameVC = self;
    _monsterMenu.userInteractionEnabled = NO;
    _monsterMenu.session = _session;

    _session.me.sm = _statusView;
}

- (void)viewWillAppear:(BOOL)animated
{
    [super viewWillAppear:animated];
    [[UIApplication sharedApplication] setStatusBarHidden:YES withAnimation:UIStatusBarAnimationFade];
    [_session startGame];
    [[GameSound defaultInstance] switchBackgroundMusic:@"deflowered torpedo.mp3"];
    [[UIApplication sharedApplication] setIdleTimerDisabled:YES];
}

- (void)viewWillDisappear:(BOOL)animated
{
    [_session stopGame];
}

- (void)viewDidUnload
{
    [super viewDidUnload];
    // Release any retained subviews of the main view.
    // e.g. self.myOutlet = nil;
}

- (void)shutdown
{
    [_session shutdown];
}

- (void)applicationActivating
{
    if (! _multiplayer) {
        [_session startGame];
    }
}

- (void)applicationDeactivating
{
    if (! _multiplayer) {
        [_session stopGame];
    }
}

- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation
{
    return UIInterfaceOrientationIsLandscape(interfaceOrientation);
}

- (void)playerDidDie
{
    _recruitButton.hidden = YES;
    _buildButton.hidden = YES;
    _monsterMenu.hidden = YES;
    _buildMenu.hidden = YES;
}

- (void)didSingleTap:(UITapGestureRecognizer *)recognizer
{
    NSLog(@"tapped %@", NSStringFromCGPoint([recognizer locationInView:_session.map]));
    
    if (_searching) {
        [_session cancelSearch];
        [self didTapMenuButton:nil];
        return;
    }
    
    if (_multiplayer) {
        // we may be flipping status and opponent views around
        if (_statusView.hidden) {
            if (CGRectContainsPoint(_opponentStatusView.frame, [recognizer locationInView:self.view])) {
                _opponentStatusView.hidden = YES;
                _statusView.hidden = NO;
            }
        } else {
            // hack: we may be tapping the X to close the game; we check that first, as we were forced to set userInteractionEnabled to NO on status menu
            if (CGRectContainsPoint(_quitButton.frame, [recognizer locationInView:_statusView])) {
                [self didTapMenuButton:nil];
            } else if (CGRectContainsPoint(_statusView.frame, [recognizer locationInView:self.view])) {
                _opponentStatusView.hidden = NO;
                _statusView.hidden = YES;
            }
        }
    }
    
    if (! _monsterMenu.hidden) {
        CGPoint p = [recognizer locationInView:_monsterMenu];
        if (p.y >= 0.f) {
            if (! [_monsterMenu checkTap:[recognizer locationInView:_monsterMenu]]) {
                _monsterMenu.hidden = YES;
                _recruitButton.hidden = NO;
            }
            return;
        }
    }
}

- (IBAction)didTapBuildButton:(UIButton *)sender
{
    if (_monsterMenu.hidden) {
        _buildMenu.hidden = NO;
        _buildMenu.userInteractionEnabled = YES;
        _buildButton.hidden = YES;
    } else {
        _monsterMenu.hidden = YES;
        _recruitButton.hidden = NO;
    }
}

- (void)closeBuildMenu
{
    _buildMenu.hidden = YES;
    _buildButton.hidden = NO;
}

- (IBAction)didTapMenuButton:(id)sender
{
    // single player game
    [_session stopGame];
    ViewController *vc = [[ViewController alloc] initWithNibName:@"ViewController" bundle:[NSBundle mainBundle]];
    vc.currentGame = self;
    [[[[UIApplication sharedApplication] delegate] window] setRootViewController:vc];
}

- (IBAction)didTapRecruitMonster:(UIButton *)sender
{
    _monsterMenu.hidden = NO;
    sender.hidden = YES;
}

- (void)resume
{
    if (_multiplayer) {
        [self connectToServer];
    }
}

@end
