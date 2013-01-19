//
//  GameMessageView.h
//  MPTD
//
//  Created by Karl-Johan Alm on 5/9/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface GameMessageView : UILabel {
    NSString *messages[3];
    NSUInteger index;
}

- (void)pushMessage:(NSString *)message;

@end
