//
//  TowerMenuView.m
//  MPTD
//
//  Created by Karl-Johan Alm on 4/26/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import "TowerMenuView.h"
#import "Tower.h"
#import "GameSession.h"
#import "GameViewController.h"
#import "RangeView.h"
#import "Player.h"
#import "GameMap.h"

@interface TowerMenuView ()

- (void)panning:(UIPanGestureRecognizer *)recognizer;
- (void)closeMenu;

@end

@implementation TowerMenuView

@synthesize selectedTowerType = _selectedTowerType;
@synthesize gameVC = _gameVC;

- (GameSession *)session 
{ 
    return _session;
}

- (id)initWithCoder:(NSCoder *)aDecoder
{
    self = [super initWithCoder:aDecoder];
    if (self) {
        UITapGestureRecognizer *tr = [[UITapGestureRecognizer alloc] initWithTarget:self action:@selector(closeMenu)];
        UIPanGestureRecognizer *pr = [[UIPanGestureRecognizer alloc] initWithTarget:self action:@selector(panning:)];
        [tr requireGestureRecognizerToFail:pr];
        [self addGestureRecognizer:tr];
        [self addGestureRecognizer:pr];
    }
    return self;
}

- (void)setSession:(GameSession *)session
{
    // 41,26, 254,62
    
    _towerButtons = [[NSMutableArray alloc] init];
    _session = session;
    _towerTypes = [session.towerTypes allValues];// mutableCopy];
    
    NSUInteger i = 0;
    for (Tower *tt in _towerTypes) {
        UILabel *l = [[UILabel alloc] initWithFrame:(CGRect){41 + i * 75.f, 26, 50, 12}];
        l.text = [NSString stringWithFormat:@"$%d", tt.cost];
        l.textColor = [UIColor colorWithRed:0.f green:0.7f blue:0.f alpha:1.f];
        l.textAlignment = UITextAlignmentCenter;
        l.font = [UIFont boldSystemFontOfSize:12.f];
        l.backgroundColor = [UIColor clearColor];
        tt.typeLabel = l;
        [self addSubview:l];
        Tower *t = [tt copy];
        t.position = (CGPoint){41 + i * 75.f, 38};
        [_towerButtons addObject:t];
        t.userInteractionEnabled = YES;
        [self addSubview:t];
        i++;
        [tt updateLabelsWithPlayer:session.me];
    }
}

- (void)doSelect:(Tower *)tower
{
    NSLog(@"spawn %@", tower);
    _selectedTowerType = tower.type;
    _draggedTower = [tower copy];
}

- (BOOL)checkTap:(CGPoint)position
{
    //self.userInteractionEnabled = YES;
    UIView *v = [self hitTest:position withEvent:nil];
    if ([v isKindOfClass:[Tower class]]) {
        [self doSelect:(Tower *)v];
        return YES;
    }
    return NO;
}

- (void)resetSelectedTowerType
{
    _selectedTowerType = 0;
}

// called on tap; not called if panning
- (void)closeMenu
{
    [_gameVC closeBuildMenu];
}

- (void)panning:(UIPanGestureRecognizer *)recognizer
{
    if (recognizer.state == UIGestureRecognizerStateBegan) {
        [self checkTap:[recognizer locationInView:self]];
        if (! _selectedTowerType) {
            // user "panned" over a non-tower spot; we take TAP as close menu, and PAN as a desire to bring a tower out
            _draggedTower = nil;
        } else {
            _anchor = (CGRect){[recognizer locationInView:_session.map], _draggedTower.frame.size};
            _draggedTower.frame = _anchor;
            [_session.map addSubview:_draggedTower];
            [_draggedTower setRangeIndicator:YES];
            [_gameVC closeBuildMenu];
        }
    }
    
    // we don't care about this anymore, if we dont have a selected tower
    if (! _draggedTower) return;
    
    CGPoint trans = [recognizer translationInView:self];

    if (! _pendingRangeIndicatorUpdate) {
        _pendingRangeIndicatorUpdate = YES;
        dispatch_async(dispatch_get_global_queue(DISPATCH_QUEUE_PRIORITY_LOW, 0), ^{
            usleep(1000);
            dispatch_async(dispatch_get_main_queue(), ^{
                _pendingRangeIndicatorUpdate = NO;
                _draggedTower.frame = (CGRect){_anchor.origin.x + trans.x, _anchor.origin.y + trans.y, _anchor.size};
                _draggedTower.rangeView.available = [_session canPlaceTower:_draggedTower];
            });
        });
    }

    if (recognizer.state == UIGestureRecognizerStateEnded) {
        [_draggedTower setRangeIndicator:NO];
        [_draggedTower removeFromSuperview];
        if (_session.me.gold >= _draggedTower.cost &&
            [_session addTower:_draggedTower]) {
            _session.me.gold -= _draggedTower.cost;
        }
        _draggedTower = nil;
    }
}

@end
