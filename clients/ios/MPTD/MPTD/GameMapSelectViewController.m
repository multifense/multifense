//
//  GameMapSelectViewController.m
//  MPTD
//
//  Created by Karl-Johan Alm on 5/12/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <QuartzCore/QuartzCore.h>

#import "GameMapSelectViewController.h"
#import "GameViewController.h"
#import "AppDelegate.h"
#import "ViewController.h"

@interface GameMapSelectViewController ()

@end

@implementation GameMapSelectViewController

- (id)initWithNibName:(NSString *)nibNameOrNil bundle:(NSBundle *)nibBundleOrNil
{
    self = [super initWithNibName:nibNameOrNil bundle:nibBundleOrNil];
    if (self) {
        // Custom initialization
    }
    return self;
}

- (void)viewDidLoad
{
    [super viewDidLoad];
    // Do any additional setup after loading the view from its nib.
    m2.layer.borderColor = m1.layer.borderColor = [[UIColor blackColor] CGColor];
    m2.layer.borderWidth = m1.layer.borderWidth = 1.f;
}

- (void)viewDidUnload
{
    [super viewDidUnload];
    // Release any retained subviews of the main view.
    // e.g. self.myOutlet = nil;
}

- (void)startSingleplayerGameOnMap:(int)mapIndex
{
    GameViewController *gvc = [[GameViewController alloc] initGame:NO map:mapIndex];
    setWindowRoot(gvc);
}

- (IBAction)didTapMap1
{
    [self startSingleplayerGameOnMap:1];
}

- (IBAction)didTapMap2
{
    [self startSingleplayerGameOnMap:2];
}

- (IBAction)didTapMenu
{
    ViewController *vc = [[ViewController alloc] initWithNibName:@"ViewController" bundle:[NSBundle mainBundle]];
    setWindowRoot(vc);
}

- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation
{
    return UIInterfaceOrientationIsLandscape(interfaceOrientation);
}

@end
