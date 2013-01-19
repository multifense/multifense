//
//  GameSession.h
//  MPTD
//
//  Created by Karl-Johan Alm on 4/19/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "Monster.h"
//#import "TowerType.h"

@class GameViewController, GameSound, GamePath, GameMap, Tower, GameLoop, Monster, GameBullet, Player, StatusMenu, /*Economy,*/ MobWave, GameNetwork, GameMessageView, MobWaveMultiPlayer;
    
@interface GameSession : NSObject {
    GameSound *_snd;
    UIImageView *_loadingView;
    __unsafe_unretained GameMessageView *_messageView;
    __unsafe_unretained GameViewController *_gvc;
    
    GameNetwork *_net;
    StatusMenu *_status;
    Player *_me;
    NSMutableDictionary *_opponents;
    //Economy *_eco;
    NSMutableArray *_mobWaves;
    int _monsterWaveNumber;
    MobWaveMultiPlayer *_mpWave;
    NSMutableArray *_otherPlayers;
    
    NSMutableArray *_towers;
    NSMutableArray *_monsters;
    NSMutableArray *_bullets;
    
    NSMutableDictionary *_towerTypes;     // Holds all the towers.
    NSMutableDictionary *_monsterTypes;
    
    GameMap *_map;
    //NSTimeInterval fstart, fend;
    
    NSTimeInterval _startStamp;
    double _gameTime;
    
    Tower *_myCurrentTower;         // Holds the tower that is currently selected.
    
    BOOL _multiplayer;
}

- (id)initGame:(BOOL)multiplayer onMap:(int)mapID;

- (void)shutdown;

- (void)connectToHost:(NSString *)host port:(UInt16)port; // multiplayer

//- (GameMap *)loadMap:(UIImage *)background path:(GamePath *)path;

- (BOOL)canPlaceTower:(Tower *)tower;

- (BOOL)addTower:(Tower *)tower;
- (BOOL)addMonster:(Monster *)monster;
- (BOOL)removeTower:(Tower *)tower;
- (BOOL)removeMonster:(Monster *)monster;
- (BOOL)addBullet:(GameBullet *)bullet;
- (BOOL)removeBullet:(GameBullet *)bullet;

- (void)monsterReachedEnd:(Monster *)monster;
- (void)monsterWasKilled:(Monster *)monster;

- (Monster *)monsterTypeFromID:(MonsterTypeID)mtid;

- (Monster *)oldestMonster;

- (void)cancelSearch;
- (void)startGame;
- (void)stopGame;
- (void)update; // internal - used by game thread
- (void)spawnMonster;
- (void)recruitMonster:(Monster *)monster;

// number of seconds since the game started
- (double)gameTime;
- (void)waveEnded;

@property (nonatomic, readonly) GameNetwork *net;
@property (nonatomic, assign) GameViewController *gvc;
@property (nonatomic, readonly) GameMap *map;
@property (nonatomic, readonly) GameLoop *loop;
@property (nonatomic, readonly) NSMutableArray *towers;
@property (nonatomic, readonly) NSMutableArray *monsters;
@property (nonatomic, readonly) NSMutableDictionary *towerTypes;
@property (nonatomic, readonly) NSMutableDictionary *monsterTypes;
@property (nonatomic, readonly) Player *me;
@property (nonatomic, retain) NSMutableDictionary *opponents;
//@property (nonatomic, retain) Economy *eco;
@property (nonatomic, retain) UIImageView *loadingView;
@property (nonatomic, assign) GameMessageView *messageView;

@end
