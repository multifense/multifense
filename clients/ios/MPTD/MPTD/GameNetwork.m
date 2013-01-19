//
//  GameNetwork.m
//  MPTD
//
//  Created by Karl-Johan Alm on 5/7/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import "GameNetwork.h"
#import "GCDAsyncSocket.h"

@interface GameNetwork () <GCDAsyncSocketDelegate>
@end

@implementation GameNetwork

@synthesize didReadBlock = _didReadBlock;
@synthesize didConnect = _didConnectBlock;
@synthesize didDisconnect = _didDisconnectBlock;

- (void)dealloc
{
    [_socket disconnect];
}

- (id)initWithHost:(NSString *)host port:(UInt16)port
{
    self = [super init];
    if (self) {
        _socket = [[GCDAsyncSocket alloc] initWithDelegate:self delegateQueue:dispatch_get_main_queue()];
        _socket.autoDisconnectOnClosedReadStream = YES;
        
        NSError *error;
        if (! [_socket connectToHost:host onPort:port error:&error]) {
            NSLog(@"Failure to connect to %@:%d - %@", host, port, error);
        }
    }
    return self;
}

- (void)disconnect
{
    [_socket disconnect];
    _socket = nil;
}

- (void)write:(NSString *)string
{
    NSData *d = [string dataUsingEncoding:NSASCIIStringEncoding];
    [_socket writeData:d withTimeout:-1.0 tag:0];
}

- (void)socket:(GCDAsyncSocket *)sock didConnectToHost:(NSString *)host port:(uint16_t)port
{
    NSLog(@"connected to host:%@", host);
    
    [_socket readDataToData:[GCDAsyncSocket CRLFData] withTimeout:-1.0 tag:0];
    
    if (_didConnectBlock) _didConnectBlock();
}

- (void)socketDidCloseReadStream:(GCDAsyncSocket *)sock
{
    NSLog(@"host disconnected");
    if (_didDisconnectBlock) _didDisconnectBlock();
}

/*- (void)socket:(GCDAsyncSocket *)sock didWriteDataWithTag:(long)tag
{
    
}*/

- (void)socket:(GCDAsyncSocket *)sock didReadData:(NSData *)data withTag:(long)tag
{
    if (_didReadBlock) _didReadBlock(data);
    NSString *str = [[NSString alloc] initWithData:data encoding:NSASCIIStringEncoding];
    NSLog(@"socket read data:\n%@", str);

    [_socket readDataToData:[GCDAsyncSocket CRLFData] withTimeout:-1.0 tag:0];
}

- (void)socketDidDisconnect:(GCDAsyncSocket *)sock withError:(NSError *)err
{
    NSLog(@"socket was disconnected (err = %@)", err);
}

@end
