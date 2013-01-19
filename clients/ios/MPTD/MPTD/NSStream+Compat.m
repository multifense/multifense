//
//  NSStream+Compat.m
//  MPTD
//
//  Created by Karl-Johan Alm on 5/4/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import "NSStream+Compat.h"

@implementation NSStream (Compat)

+ (void)getStreamsToHostNamed:(NSString *)hostName
                         port:(NSInteger)port
                  inputStream:(NSInputStream **)inputStream
                 outputStream:(NSOutputStream **)outputStream

{
    
    CFHostRef host;
    CFReadStreamRef readStream;
    CFWriteStreamRef writeStream;
    
    readStream = NULL;
    writeStream = NULL;
    
    host = CFHostCreateWithName(NULL, (__bridge CFStringRef) hostName);
    
    if (host != NULL) {
        (void) CFStreamCreatePairWithSocketToCFHost(NULL, host, port, &readStream, &writeStream);
        CFRelease(host);
    }
    
    if (inputStream == NULL) {
        if (readStream != NULL) {
            CFRelease(readStream);
        }
    } else {
        *inputStream = (__bridge NSInputStream *) readStream;
    }
    
    if (outputStream == NULL) {
        if (writeStream != NULL) {
            CFRelease(writeStream);
        }
    } else {
        *outputStream = (__bridge NSOutputStream *) writeStream;
    }
    
}

@end
