//
//  AppDelegate.h
//  MPTD
//
//  Created by Karl-Johan Alm on 4/18/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>

#define setWindowRoot(newRoot) [[[AppDelegate appDelegate] window] setRootViewController:newRoot]

@class ViewController;

@interface AppDelegate : UIResponder <UIApplicationDelegate> {
    NSString *_playerName;
}

+ (AppDelegate *)appDelegate;

@property (strong, nonatomic) UIWindow *window;

@property (strong, nonatomic) ViewController *viewController;

@property (nonatomic, retain) NSString *playerName;

@end
