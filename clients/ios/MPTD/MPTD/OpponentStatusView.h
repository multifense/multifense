//
//  OpponentStatusView.h
//  MPTD
//
//  Created by Karl-Johan Alm on 5/13/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "Player.h"

@interface OpponentStatusView : UIView {
    NSMutableArray *_players;
    NSMutableArray *_playerViews;
}

- (void)updatePlayer:(Player *)player;
- (void)removePlayer:(Player *)player;

@end
