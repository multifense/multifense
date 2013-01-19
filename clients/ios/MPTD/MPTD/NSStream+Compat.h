//
//  NSStream+Compat.h
//  MPTD
//
//  Created by Karl-Johan Alm on 5/4/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface NSStream (Compat)

+ (void)getStreamsToHostNamed:(NSString *)hostName
                         port:(NSInteger)port
                  inputStream:(NSInputStream **)inputStream
                 outputStream:(NSOutputStream **)outputStream;

@end
