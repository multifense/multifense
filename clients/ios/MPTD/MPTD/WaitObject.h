//
//  WaitObject.h
//  MPTD
//
//  Created by Karl-Johan Alm on 5/11/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import "GameObject.h"

@interface WaitObject : GameObject {
    int _waitTime;
}

- (id)initWithWaitTime:(int)waitTime;

@property (nonatomic, readonly) int waitTime;

@end
