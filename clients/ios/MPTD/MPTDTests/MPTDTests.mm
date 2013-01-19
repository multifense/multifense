//
//  MPTDTests.m
//  MPTDTests
//
//  Created by Karl-Johan Alm on 4/18/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import "MPTDTests.h"
#import "Tower.h"
#import "Monster.h"
#import "GameMap.h"
#import "GameLoop.h"
#import "GameSession.h"
#import "GamePathPoint.h"
#import "GamePath.h"
#import "GameTrackingProjectile.h"
#import "MobWave.h"
#import "TowerMenuView.h"
#import "GameViewController.h"
#import "MPTDTestRecognizer.h"
#import "Player.h"
#import "GameSession+Kernel.h"
#import "ClientIphone.h"
#import "GameAnimatedSprite.h"
#import "TestSession.h"

@implementation MPTDTests

- (void)setUp
{
    [super setUp];

    
    session = [[TestSession alloc] initGame:NO onMap:1];
    STAssertNotNil(session, @"GameSession did not allocate");
    
    GamePath *path = [[GamePath alloc] init];
    [path addPathPoint:(CGPoint){0, 200}];
    [path addPathPoint:(CGPoint){130, 200}];
    [path addPathPoint:(CGPoint){130, 750}];
    [path addPathPoint:(CGPoint){460, 750}];
    [path addPathPoint:(CGPoint){460, 500}];
    [path addPathPoint:(CGPoint){700, 500}];
    [path addPathPoint:(CGPoint){700, 100}];
    [path addPathPoint:(CGPoint){910, 100}];
    [path addPathPoint:(CGPoint){910, 730}];
    [path addPathPoint:(CGPoint){1300, 730}];
    [path addPathPoint:(CGPoint){1300, 300}];
    [path addPathPoint:(CGPoint){1600, 300}];
    
    //GameMap *map = [session loadMap:[UIImage imageNamed:@"map.png"] path:path];
    //STAssertNotNil(map, @"Session map did not init correctly");
}

- (void)tearDown
{
    [super tearDown];
}

- (void)testGameObject
{
    GameAnimatedSprite *spriteImage = [[GameAnimatedSprite alloc] initWithSpriteSheet:[UIImage imageNamed:@"slimesprite.png"] size:(CGSize){100,100}];
    STAssertNotNil(spriteImage, @"sprite image for game object is nil");
    GameObject *go = [[GameObject alloc] initWithSprite:spriteImage frame:(CGRect){50,50,50,50}];
    STAssertNotNil(go, @"monster allocation is nil");
    STAssertNotNil(go.sprite, @"monster sprite is nil");
}

- (void)testTower
{
    GameAnimatedSprite *spriteImage = [[GameAnimatedSprite alloc] initWithSpriteSheet:[UIImage imageNamed:@"slimesprite.png"] size:(CGSize){100,100}];
    Tower *t = [[Tower alloc] initWithSprite:spriteImage position:(CGPoint){100,150} fireRate:2.5 damage:3 range:400 bulletSpeed:20 bulletSprite:spriteImage];
                //initWithSprite:[UIImage imageNamed:@"foo"] position:(CGPoint){100,150} fireRate:2.5 damage:3 range:400 ];

    STAssertNotNil(t, @"monster allocation is nil");
    STAssertEquals(t.position.x, 100.f,@"t.x position is invalid");
    STAssertEquals(t.position.y, 150.f,@"t.y position is invalid");
    STAssertEquals(t.fireRate, 2.5,@"t.fireRate is invalid");
    STAssertEquals(t.damage,3,@"t.damage is invalid");
    t.position = (CGPoint){200,200};
    STAssertEquals(t.position.x,200.f,@"t.position setter not working (x)");
    STAssertEquals(t.position.y,200.f,@"t.position setter not working (y)");
    t.fireRate = -1.0;
    STAssertFalse((int)t.fireRate == -1,@"t.fireRate allowed a value less than zero.");
    t.damage = -1;
    STAssertFalse(t.damage == -1,@"t.damage allowed a value less than zero.");
}

- (void)testMonster
{
    GameAnimatedSprite *spriteImage = [[GameAnimatedSprite alloc] initWithSpriteSheet:[UIImage imageNamed:@"slimesprite.png"] size:(CGSize){100,100}];
    Monster *m = [[Monster alloc] initWithSprite:spriteImage frame:(CGRect){50,50,50,50}];
    STAssertNotNil(m, @"monster allocation is nil");
    STAssertNotNil(m.sprite, @"monster sprite is nil");
    m.hp = 5;
    m.speed = 3.0;
    STAssertFalse(m.dead, @"monster dead is not false");
    STAssertEquals(m.hp, 5.0, @"hp is not valid");
}

- (void)testTrackingProjectile
{
    GameAnimatedSprite *spriteImage = [[GameAnimatedSprite alloc] initWithSpriteSheet:[UIImage imageNamed:@"slimesprite.png"] size:(CGSize){100,100}];
    Monster *mon = [[Monster alloc] initWithSprite:spriteImage frame:(CGRect){0,0,50,50}];
    GameTrackingProjectile *s = [[GameTrackingProjectile alloc] initWithDamage:40 speed:1 target:mon sprite:nil frame:(CGRect){50,50,5,5}];
    
    STAssertNotNil(s, @"tracking projectile was not created");
    
    for (int i = 0; i < 500; i++) {
        [s update];
    }
    
    //STAssertTrue([s hasHit], @"tracking projectile has not hit the monster (s=%@)", s);
}

- (void)testSession
{
    GameMap *map = session.map;
    GamePath *path = map.path;
    int countOffset = path.pointCount;
    [path addPathPoint:(CGPoint){100,-100}];
    [path addPathPoint:(CGPoint){100,100}];
    [path addPathPoint:(CGPoint){200,100}];
    [path addPathPoint:(CGPoint){200,800}];
    
    // TODO: fix tests better
    STAssertTrue(path.pointCount == countOffset + 4, @"moster path is off");
    // after standards: ensure path length is correct
    
#if 0
    Monster *mt = [session.monsterTypes objectForKey:MonsterType(101)];
    Monster *m = [mt spawnMonster];
    //[[Monster alloc] initWithSprite:[UIImage imageNamed:@"Monster1.bmp"] frame:(CGRect){0,50,50,50}];
    Monster *m2 = [mt spawnMonster];
    // [[Monster alloc] initWithSprite:[UIImage imageNamed:@"Monster1.bmp"] frame:(CGRect){50,50,50,50}];
    Monster *m3 = [mt spawnMonster];
    // [[Monster alloc] initWithSprite:[UIImage imageNamed:@"Monster1.bmp"] frame:(CGRect){100,50,50,50}];
    [session addMonster:m];
    [session addMonster:m2];
    [session addMonster:m3];
    NSArray *a = [NSArray arrayWithObjects:m,m2,m3, nil];
    STAssertTrue([a isEqualToArray:[session monsters]], @"mosnters were not properly added");
    [session removeMonster:m2];
    STAssertFalse([[session monsters] containsObject:m2], @"monster was not properly deleted");
    
    [session removeMonster:m];
    [session removeMonster:m3];
    
    STAssertTrue(session.monsters.count == 0, @"monsters exist even though all of them were deleted");
#endif
    
    [session startGame];
    STAssertTrue([session.loop isRunning], @"session.loop did not start running");
}

- (void)testTowerCollision
{
    GameAnimatedSprite *spriteImage = [[GameAnimatedSprite alloc] initWithSpriteSheet:[UIImage imageNamed:@"slimesprite.png"] size:(CGSize){100,100}];
    Tower *t = [[Tower alloc] initWithSprite:spriteImage position:(CGPoint){0,0} fireRate:2.5 damage:3 range:400 bulletSpeed:20 bulletSprite:spriteImage];
    Tower *t2 = [[Tower alloc] initWithSprite:spriteImage position:(CGPoint){50,0} fireRate:2.5 damage:3 range:400 bulletSpeed:20 bulletSprite:spriteImage];
    Tower *t3 = [[Tower alloc] initWithSprite:spriteImage position:(CGPoint){100,0} fireRate:2.5 damage:3 range:400 bulletSpeed:20 bulletSprite:spriteImage];
    [session addTower:t];
    [session addTower:t2];
    [session addTower:t3];
    NSArray *a = [NSArray arrayWithObjects:t,t2,t3, nil];
    STAssertTrue([a isEqualToArray:session.towers], @"towers were not properly added (%@)");
    
    Tower *t4 = [[Tower alloc] initWithSprite:spriteImage position:(CGPoint){50,0} fireRate:2.5 damage:3 range:400 bulletSpeed:20 bulletSprite:spriteImage];
    STAssertFalse([session addTower:t4], @"tower was added on top of another tower");
    STAssertFalse([session.towers containsObject:t4], @"addTower returned false, but tower was added anyway");
    
    [session removeTower:t];
    [session removeTower:t2];
    [session removeTower:t3];
    [session removeTower:t4];
}

- (void)testMobWave
{
#if 0
    MobWave *wave = [[MobWave alloc] initWithSession:session];

    //+            List<Sprite> listToPrint = new List<Sprite>();
    [wave addMonsterTypeIDToWave:1];
    [wave addMonsterTypeIDToWave:2];
    //+            MobWave mobwave = new MobWave(mo1, mo2, ref listToPrint);
    [wave addMonsterTypeIDToWave:1];
    [wave addMonsterTypeIDToWave:2];
    [wave spawn];
    STAssertEquals(session.monsters.count, (NSUInteger)4, @"session monster count not the expected value (was %d, expected 4)", session.monsters.count);
    //+            Assert.AreEqual(listToPrint.Count, 2);
#endif
}

- (void)testTowerPlacing
{
    GameViewController *gvc = [[GameViewController alloc] initWithNibName:@"GameViewController" bundle:nil];
    [gvc view]; // force loading of gvc views
    gvc.session.me.gold = 100;
    int gold = gvc.session.me.gold;
    int costDefault = [[[gvc.session.towerTypes allValues] objectAtIndex:0] cost];
    MPTDTestRecognizer *tgr = [[MPTDTestRecognizer alloc] init];
    [tgr setLocation:(CGPoint){300,200} inView:gvc.view];
    [gvc didSingleTap:(UITapGestureRecognizer *)tgr];
    // should work
    gold -= costDefault;
    STAssertEquals(gold, gvc.session.me.gold, @"placed tower but got a failure (this is probably a test error)");
    // again, we touch the same location; this should fail the tower placement and no money should go thru
    [gvc didSingleTap:(UITapGestureRecognizer *)tgr];
    STAssertEquals(gold, gvc.session.me.gold, @"tried placing tower on top of other tower -- money was deducted but should not have been!");
}

BOOL canReach(Tower *a, GameObject *b) {
    return [a findDistance:b] <= a.range;
}

- (void)testTowerRange
{
    GameAnimatedSprite *spriteImage = [[GameAnimatedSprite alloc] initWithSpriteSheet:[UIImage imageNamed:@"slimesprite.png"] size:(CGSize){100,100}];
    Tower *t = [[Tower alloc] initWithSprite:spriteImage position:(CGPoint){500,500} fireRate:2.5 damage:3 range:400 bulletSpeed:20 bulletSprite:spriteImage];
    [session addTower:t];
    Monster *m = [[Monster alloc] initWithSprite:spriteImage frame:(CGRect){480,480,40,40}];
    STAssertTrue(canReach(t,m), @"tower with monster on same spot said it cannot reach");
    m.center = (CGPoint){0,500};
    STAssertFalse(canReach(t, m), @"tower can reach monster at 500 distance (left) but tower only has 400 range");
    m.center = (CGPoint){500,0};
    STAssertFalse(canReach(t, m), @"tower can reach monster at 500 distance (up) but tower only has 400 range");
    m.center = (CGPoint){1000,500};
    STAssertFalse(canReach(t, m), @"tower can reach monster at 500 distance (right) but tower only has 400 range");
    m.center = (CGPoint){500,1000};
    STAssertFalse(canReach(t, m), @"tower can reach monster at 500 distance (down) but tower only has 400 range");
    m.center = (CGPoint){100,100};
    STAssertFalse(canReach(t, m), @"tower can reach monster at (100,100) when tower is at (500,500) but tower only has 400 range.");
}

- (void)testSharedModule
{
    // we set up the kernel just in case; this also ensures that the kernel uses THIS session and not some other session
    // on the iOS client
    [session kernelSetup:1];
    // we also force the multiplayer flag to YES, or our game code won't send any didTakeDamage or such messages to the kernel
    session.multiplayer = YES;
    // set things up so we're registered
    defaultKern()->setNickname("Pelle");
    defaultKern()->didRead("0..0..REGISTERED,14\r\n");
    // pretend a game room was set up for us
    defaultKern()->didRead("0..0..GAMEROOM,Pelle:14\r\n");
    // there should be zero opponents now (we are the only ones in the list)
    STAssertEquals(session.opponents.count, (NSUInteger)0, @"Pelle should not be counted as an opponent!");
    // we now "add" Lisa to the room and test things with other players
    defaultKern()->didRead("0..0..GAMEROOM,Pelle:14,Lisa:15\r\n");
    STAssertEquals(session.opponents.count, (NSUInteger)1, @"Lisa should be counted as a new opponent!");
    defaultKern()->didRead("0..0..GAMEROOM,Mammamus:16,Pelle:14\r\n");
    STAssertEquals(session.opponents.count, (NSUInteger)1, @"Lisa left, Mammamus joined, but opponent count is no longer 1!");
    defaultKern()->didRead("0..0..GAMEROOM,Pelle:14,Jocke:17,Ralle:18\r\n");
    STAssertEquals(session.opponents.count, (NSUInteger)2, @"Mammamus left, Jocke & Ralle joined, but opponent count is not 2!");
    
    // we now start the game up 
    defaultKern()->didRead("0..0..START,10\r\n");
    Player *me = session.me;
    // grab a ref for everyone
    Player *jocke = [session.opponents objectForKey:[NSNumber numberWithInt:17]];
    Player *ralle = [session.opponents objectForKey:[NSNumber numberWithInt:18]];
    STAssertNotNil(jocke, @"Jocke was not found among opponents.");
    STAssertNotNil(ralle, @"Ralle was not found among opponents.");

    // we test that hp for everyone is 10
    STAssertEquals(me.hp, 10, @"My own hp is not 10!");
    STAssertEquals(jocke.hp, 10, @"Jocke's hp is not 10!");
    STAssertEquals(ralle.hp, 10, @"Ralle's hp is not 10!");
    
    // we pretend that JOCKE let a monster thru that was sent by RALLE
    // JOCKE HP --, RALLE HP ++
    defaultKern()->didRead("0..0..DAMAGE,17,18,1\r\n");
    STAssertEquals(jocke.hp, 9, @"Jocke should have 1 less hp due to taking damage");
    STAssertEquals(ralle.hp, 11, @"Ralle's should have 1 more hp due to stealing damage from Jocke");
    
    // we pretend that WE let a monster thru that was sent by RALLE
    // US HP --, RALLE HP ++
    // this is done locally and not via kernel but is tested here anyway
    Monster *m = [[[session.monsterTypes allValues] objectAtIndex:0] copy];
    m.owner = ralle.pid;

    // before we say monster reached end we want to put in some expected netWrites
    // the kernel will get
    //   didRecruitMonster(monster.type, monster.hp);
    //   didTakeDamage(1, monster.owner);
    // and is expected to send 
    //   14..0..RECMONSTER,0,__m.type__,__m.hp__\r\n
    //   14..0..LIFE,9,18\r\n
    [session expectNetWrite:[NSString stringWithFormat:@"14..0..RECMONSTER,2223,%d,%d\r\n",
                             m.type, (int)m.hp]];
    [session expectNetWrite:@"14..0..LIFE,9,18\r\n"];
    
    [session monsterReachedEnd:m];
    
    if (! session.success) {
        STAssertEquals(session.expectedString, session.gotString, @"Unexpected string in netWrite()");
        STAssertNil(self, @"Just making sure we do get an assertion -- if the above assert does not trigger, something's terribly wrong!");
    }
    
    STAssertEquals(me.hp, 9, @"We should have 1 less hp due to taking damage");
    STAssertEquals(ralle.hp, 12, @"Ralle's should have 1 more hp due to stealing damage from Jocke");
    
    // we pretend that JOCKE let a monster thru that was sent by US
    // JOCKE HP --, US HP ++
    defaultKern()->didRead("0..0..DAMAGE,17,14,1\r\n");
    STAssertEquals(jocke.hp, 8, @"Jocke should have 1 less hp due to taking damage");
    STAssertEquals(me.hp, 10, @"We should have 1 more hp due to stealing damage from Jocke");
}

- (void)testNetworking
{
}

@end
