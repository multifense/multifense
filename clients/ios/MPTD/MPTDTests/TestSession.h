//
//  TestSession.h
//  MPTD
//
//  Created by Karl-Johan Alm on 5/12/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import "GameSession.h"

@interface TestSession : GameSession {
    NSString *_gotString;
    NSString *_expectedString;
    NSMutableArray *_queue;
    BOOL _success;
}

- (void)expectNetWrite:(NSString *)msg;

@property (nonatomic, readonly) NSString *expectedString;
@property (nonatomic, readonly) NSString *gotString;
@property (nonatomic, readonly) BOOL success;
@property (nonatomic, readwrite) BOOL multiplayer;

@end
