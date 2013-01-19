//
//  GameEndViewController.h
//  MPTD
//
//  Created by Karl-Johan Alm on 5/11/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>

typedef enum {
    GameEndSPVictory = 0,
    GameEndSPLoss = 1,
    GameEndMPVictory = 2,
    GameEndMPLoss = 3
} GameEndState;

@interface GameEndViewController : UIViewController {
    IBOutlet UIImageView *_imageView;
    
    GameEndState _state;
}

@property (nonatomic, readwrite) GameEndState state;

@end
