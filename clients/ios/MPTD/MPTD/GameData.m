//
//  GameData.m
//  MPTD
//
//  Created by Karl-Johan Alm on 5/11/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import "GameData.h"

@implementation GameData

@synthesize monsterTypes = _monsterTypes;
@synthesize towerTypes = _towerTypes;

static GameData *_defaultGameData;

+ (GameData *)defaultGameData
{
    if (_defaultGameData == nil) {
        _defaultGameData = [[GameData alloc] init];
    }
    return _defaultGameData;
}

- (id)init
{
    self = [super init];
    if (self) {
        _monsterTypes = [[NSMutableDictionary alloc] init];
        _towerTypes = [[NSMutableDictionary alloc] init];
    }
    return self;
}

- (void)storeDataForMonsters:(NSDictionary *)monsterTypes towers:(NSDictionary *)towerTypes
{
    for (NSNumber *n in monsterTypes) {
        [_monsterTypes setObject:[[monsterTypes objectForKey:n] copy] forKey:n];
    }
    for (NSNumber *n in towerTypes) {
        [_towerTypes setObject:[[towerTypes objectForKey:n] copy] forKey:n];
    }
}

- (void)loadDataForMonsters:(NSMutableDictionary *)monsterTypes towers:(NSMutableDictionary *)towerTypes
{
    for (NSNumber *n in _monsterTypes) {
        [monsterTypes setObject:[[_monsterTypes objectForKey:n] copy] forKey:n];
    }
    for (NSNumber *n in _towerTypes) {
        [towerTypes setObject:[[_towerTypes objectForKey:n] copy] forKey:n];
    }
}

- (BOOL)loaded
{
    return _towerTypes.count > 0;
}

@end
