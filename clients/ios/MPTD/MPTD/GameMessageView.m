//
//  GameMessageView.m
//  MPTD
//
//  Created by Karl-Johan Alm on 5/9/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import "GameMessageView.h"

@implementation GameMessageView

- (void)GameMessageView
{
    messages[2] = @"";
    messages[1] = @"";
    messages[0] = @"";
    self.text = @"";
}

- (id)initWithFrame:(CGRect)frame
{
    self = [super initWithFrame:frame];
    if (self) [self GameMessageView];
    return self;
}

- (id)initWithCoder:(NSCoder *)aDecoder
{
    self = [super initWithCoder:aDecoder];
    if (self) [self GameMessageView];
    return self;
}

- (void)pushMessage:(NSString *)message
{
    index++;
    NSUInteger msgid = index;
    messages[2] = messages[1];
    messages[1] = messages[0];
    messages[0] = message;
    self.text = [NSString stringWithFormat:@"%@\n%@\n%@", messages[2], messages[1], messages[0]];
    dispatch_async(dispatch_get_global_queue(DISPATCH_QUEUE_PRIORITY_LOW, 0), ^{
        sleep(5);
        dispatch_async(dispatch_get_main_queue(), ^{
            // index == msgid means messages[0] goes
            // index == msgid + 1 means messages[1] goes
            // index == msgid + 2 means messages[2] goes
            // index > msgid + 2 means nothing goes
            if (index > msgid + 2) return;
            assert(index >= msgid);
            NSUInteger idx = index - msgid;
            messages[idx] = @"";
            self.text = [NSString stringWithFormat:@"%@\n%@\n%@", messages[2], messages[1], messages[0]];
        });
    });
}

@end
