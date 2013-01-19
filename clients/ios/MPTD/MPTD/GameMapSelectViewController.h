//
//  GameMapSelectViewController.h
//  MPTD
//
//  Created by Karl-Johan Alm on 5/12/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface GameMapSelectViewController : UIViewController {
    IBOutlet UIButton *m1, *m2;
}

- (IBAction)didTapMap1;
- (IBAction)didTapMap2;
- (IBAction)didTapMenu;

@end
