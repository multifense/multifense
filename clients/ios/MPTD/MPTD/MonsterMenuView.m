//
//  MonsterMenuView.m
//  MPTD
//
//  Created by Karl-Johan Alm on 5/11/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import "MonsterMenuView.h"
#import "monster.h"
#import "GameSession.h"

@implementation MonsterMenuView

@synthesize selectedMonsterType = _selectedMonsterType;

- (GameSession *)session 
{ 
    return _session;
}

- (id)initWithCoder:(NSCoder *)aDecoder
{
    self = [super initWithCoder:aDecoder];
    if (self) {
        self.image = [UIImage imageNamed:@"scroll.png"];
        self.contentMode = UIViewContentModeScaleToFill;
    }
    return self;
}

- (void)setSession:(GameSession *)session
{
    // 45,29, 410,58
    
    _monsterButtons = [[NSMutableArray alloc] init];
    _session = session;
    _monsterTypes = [session.monsterTypes allValues];
    NSUInteger i = 0;
    CGFloat x = 410 + 45 - 50; //self.frame.size.width - 87.f;
    for (Monster *tt in _monsterTypes) {
        UILabel *l = [[UILabel alloc] initWithFrame:(CGRect){x, 29, 50, 12}];
        l.text = [NSString stringWithFormat:@"$%d", tt.cost];
        l.textColor = [UIColor colorWithRed:0.f green:0.7f blue:0.f alpha:1.f];
        l.textAlignment = UITextAlignmentCenter;
        l.font = [UIFont boldSystemFontOfSize:12.f];
        l.backgroundColor = [UIColor clearColor];
        tt.typeLabel = l;
        [self addSubview:l];
        Monster *t = [tt copy];
        t.frame = (CGRect){x, 36, 50, 50};
        [_monsterButtons addObject:t];
        t.userInteractionEnabled = YES;
        [self addSubview:t];
        i++;
        x -= 75.f;
        [tt updateLabelsWithPlayer:session.me];
    }
}

- (void)doSelect:(Monster *)monster
{
    NSLog(@"spawn %@", monster);
    [_session recruitMonster:monster];
}

- (BOOL)checkTap:(CGPoint)position
{
    self.userInteractionEnabled = YES;
    UIView *v = [self hitTest:position withEvent:nil];
    self.userInteractionEnabled = NO;
    if ([v isKindOfClass:[Monster class]]) {
        [self doSelect:(Monster *)v];
        return YES;
    }
    return NO;
}

@end
