//
//  GameNetwork.h
//  MPTD
//
//  Created by Karl-Johan Alm on 5/7/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>

@class GCDAsyncSocket;

typedef void (^GameNetworkDidRead)(NSData *str);
typedef void (^GameNetworkDidConnect)(void);
typedef void (^GameNetworkDidDisconnect)(void);

@interface GameNetwork : NSObject {
    GCDAsyncSocket *_socket;
    GameNetworkDidRead _didReadBlock;
    GameNetworkDidConnect _didConnectBlock;
    GameNetworkDidDisconnect _didDisconnectBlock;
}

- (id)initWithHost:(NSString *)host port:(UInt16)port;

- (void)write:(NSString *)string;

- (void)disconnect;

@property (nonatomic, copy) GameNetworkDidRead didReadBlock;
@property (nonatomic, copy) GameNetworkDidConnect didConnect;
@property (nonatomic, copy) GameNetworkDidDisconnect didDisconnect;

@end
