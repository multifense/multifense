//
//  GameData.h
//  MPTD
//
//  Created by Karl-Johan Alm on 5/11/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface GameData : NSObject {
    NSMutableDictionary *_monsterTypes;
    NSMutableDictionary *_towerTypes;
    BOOL _loaded;
}

+ (GameData *)defaultGameData;

- (void)storeDataForMonsters:(NSDictionary *)monsterTypes towers:(NSDictionary *)towerTypes;
- (void)loadDataForMonsters:(NSMutableDictionary *)monsterTypes towers:(NSMutableDictionary *)towerTypes;

@property (nonatomic, readonly) BOOL loaded;
@property (nonatomic, readonly) NSMutableDictionary *monsterTypes;
@property (nonatomic, readonly) NSMutableDictionary *towerTypes;

@end
