//
//  OpponentStatusView.m
//  MPTD
//
//  Created by Karl-Johan Alm on 5/13/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import "OpponentStatusView.h"

@implementation OpponentStatusView

- (void)updatePlayer:(Player *)player
{
    if (_players == nil) {
        _players = [[NSMutableArray alloc] init];
        _playerViews = [[NSMutableArray alloc] init];

        UIView *pview = [[UIView alloc] initWithFrame:(CGRect){5,0,self.frame.size.width-5,15}];
        UILabel *l = [[UILabel alloc] initWithFrame:(CGRect){0,0,pview.frame.size.width*2.f/3.f,25}];
        l.font = [UIFont boldSystemFontOfSize:12];
        l.textColor = [UIColor redColor];
        l.backgroundColor = [UIColor clearColor];
        l.text = @"Player name";
        [pview addSubview:l];
        l = [[UILabel alloc] initWithFrame:(CGRect){pview.frame.size.width*2.f/3.f,0,pview.frame.size.width/3.f,25}];
        l.font = [UIFont systemFontOfSize:12];
        l.textColor = [UIColor greenColor];
        l.backgroundColor = [UIColor clearColor];
        l.text = @"Health";
        [pview addSubview:l];
        [self addSubview:pview];
    }
    
    NSInteger idx = [_players indexOfObject:player];
    
    if (idx == NSNotFound) {
        // we have a new player
        idx = _players.count;
        [_players addObject:player];
        UIView *pview = [[UIView alloc] initWithFrame:(CGRect){5,(1+idx)*15,self.frame.size.width-5,15}];
        UILabel *l = [[UILabel alloc] initWithFrame:(CGRect){0,0,pview.frame.size.width*2.f/3.f,15}];
        l.font = [UIFont systemFontOfSize:12];
        l.textColor = [UIColor redColor];
        l.backgroundColor = [UIColor clearColor];
        l.text = player.nick;
        [pview addSubview:l];
        l = [[UILabel alloc] initWithFrame:(CGRect){pview.frame.size.width*2.f/3.f,0,pview.frame.size.width/3.f,15}];
        l.font = [UIFont systemFontOfSize:12];
        l.textColor = [UIColor greenColor];
        l.backgroundColor = [UIColor clearColor];
        [pview addSubview:l];
        [_playerViews addObject:pview];
        [self addSubview:pview];
    }
    
    assert(idx < _players.count);
    
    UIView *pview = [_playerViews objectAtIndex:idx];
    UILabel *l = [[pview subviews] objectAtIndex:1];
    l.text = [NSString stringWithFormat:@"%d", player.hp];
}

- (void)removePlayer:(Player *)player
{
    NSInteger idx = [_players indexOfObject:player];
    if (idx != NSNotFound) {
        [[_playerViews objectAtIndex:idx] removeFromSuperview];
        [_players removeObjectAtIndex:idx];
        [_playerViews removeObjectAtIndex:idx];
    }
}

@end
