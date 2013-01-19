//
//  GameSound.m
//  MPTD
//
//  Created by Karl-Johan Alm on 5/12/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import "GameSound.h"
//#import <AVFoundation/AVFoundation.h>

@implementation GameSound

static GameSound *_defaultInstance;
+ (GameSound *)defaultInstance
{
    if (_defaultInstance == nil) _defaultInstance = [[GameSound alloc] init];
    return _defaultInstance;
}

- (id)init
{
    self = [super init];
    if (self) {
        _indices = [[NSMutableDictionary alloc] init];
        CDSoundEngine *sse = [CDAudioManager sharedManager].soundEngine;
        
        /**
         A source group is another name for a channel
         Here I have 2 channels, the first index allows for only a single effect... my background music
         The second channel I have reserved for my sound effects.  This is set to 31 because you can
         have up to 32 effects at once
         */
        NSArray *sourceGroups = [NSArray arrayWithObjects:[NSNumber numberWithInt:1], [NSNumber numberWithInt:31], nil];
        [sse defineSourceGroups:sourceGroups];
        
        //Initialise audio manager asynchronously as it can take a few seconds
        /** Different modes of the engine
         typedef enum {
         kAMM_FxOnly,					//!Other apps will be able to play audio
         kAMM_FxPlusMusic,				//!Only this app will play audio
         kAMM_FxPlusMusicIfNoOtherAudio,	//!If another app is playing audio at start up then allow it to continue and don't play music
         kAMM_MediaPlayback,				//!This app takes over audio e.g music player app
         kAMM_PlayAndRecord				//!App takes over audio and has input and output
         } tAudioManagerMode;*/
        [CDAudioManager initAsynchronously:kAMM_FxPlusMusicIfNoOtherAudio];
        
        //Load sound buffers asynchrounously
        NSMutableArray *loadRequests = [[NSMutableArray alloc] init];
        
        /**
         Here we set up an array of sounds to load
         Each CDBufferLoadRequest takes an integer as an identifier (to call later)
         and the file path.  Pretty straightforward here.
         */
        
        [_indices setObject:[NSNumber numberWithInt:10] forKey:@"cannon.mp3"];
        [_indices setObject:[NSNumber numberWithInt:11] forKey:@"fire.mp3"];
        [_indices setObject:[NSNumber numberWithInt:12] forKey:@"ice.mp3"];
        [_indices setObject:[NSNumber numberWithInt:13] forKey:@"magic.mp3"];

        //[_indices setObject:[NSNumber numberWithInt:20] forKey:@"magic.mp3"];
        
        
#define LOADREQ(id,file) \
        [loadRequests addObject:[[CDBufferLoadRequest alloc] init:id filePath:file]]
        
        /*LOADREQ(SND_BG_MENUS,   @"multower deplayer.mp3");
        LOADREQ(SND_BG_GAME,    @"deflowered torpedo.mp3");
        LOADREQ(SND_BG_WIN,     @"multower deplayer.mp3");
        LOADREQ(SND_BG_LOSE,    @"multower deplayer.mp3");*/

        LOADREQ(SND_CANNON,     @"cannon.mp3");
        LOADREQ(SND_FIRE,       @"fire.mp3");
        LOADREQ(SND_ICE,        @"ice.mp3");
        LOADREQ(SND_MAGIC,      @"magic.mp3");
        
        //[loadRequests addObject:[[CDBufferLoadRequest alloc] init:SND_CLICK filePath:@"click.mp3"]];
        [sse loadBuffersAsynchronously:loadRequests];
        /*
        dispatch_async(dispatch_get_global_queue(DISPATCH_QUEUE_PRIORITY_LOW, 0), ^{
            while (1) {
            playEffect(SND_CANNON);
            sleep(1);
            playEffect(SND_FIRE);
            sleep(1);
            playEffect(SND_ICE);
            sleep(1);
            playEffect(SND_MAGIC);
            sleep(1);
            }
        });
         */
    }
    return self;
}

- (void)switchBackgroundMusic:(NSString *)soundName
{
    [[CDAudioManager sharedManager] playBackgroundMusic:soundName loop:YES];
}

- (void)switchBackgroundMusicNoLoop:(NSString *)soundName
{
    [[CDAudioManager sharedManager] playBackgroundMusic:soundName loop:NO];
}

- (int)soundIndexForFile:(NSString *)file
{
    return [[_indices objectForKey:file] intValue];
}

#if 0
- (void)fadeUp:(useconds_t)rate
{
    while (backgroundPlayer.volume < 1.f) {
        usleep(rate);
        dispatch_async(dispatch_get_main_queue(), ^{
            if (backgroundPlayer.volume < 1.f) {
                backgroundPlayer.volume += 0.1f;
            } else {
                backgroundPlayer.volume = 1.f;
            }
        });
    }
}

- (void)fadeDown:(useconds_t)rate
{
    while (backgroundPlayer.volume > 0.1f) {
        usleep(rate);
        dispatch_async(dispatch_get_main_queue(), ^{
            if (backgroundPlayer.volume > 0.1f) {
                backgroundPlayer.volume -= 0.1f;
            } else {
                backgroundPlayer.volume = 0.f;
            }
        });
    }
}

#define FADE_RATE 100000

- (void)restartBG
{
    dispatch_async(dispatch_get_global_queue(DISPATCH_QUEUE_PRIORITY_LOW, 0), ^{
        [self fadeDown:FADE_RATE];
        [self fadeUp:FADE_RATE];
    });
}

- (void)setSound:(NSString *)soundName
{
}

- (void)switchBackgroundMusicNoFade:(NSString *)soundName
{
    
    
    if (backgroundPlayer != nil) {
        [backgroundPlayer stop];
    }
    NSString *soundFilePath = [[NSBundle mainBundle] pathForResource:soundName ofType:@"mp3"];
    NSURL *soundFileURL = [NSURL fileURLWithPath:soundFilePath];
    AVAudioPlayer *player = [[AVAudioPlayer alloc] initWithContentsOfURL:soundFileURL error:nil];
    player.numberOfLoops = -1; //infinite
    
    [player play];
    backgroundPlayer = player;
}

- (void)switchBackgroundMusic:(NSString *)soundName
{
    if (backgroundPlayer == nil) {
        [self switchBackgroundMusicNoFade:soundName];
        return;
    }
    dispatch_async(dispatch_get_global_queue(DISPATCH_QUEUE_PRIORITY_LOW, 0), ^{
        [self fadeDown:FADE_RATE];
        [self switchBackgroundMusicNoFade:soundName];
        [self fadeUp:FADE_RATE];
    });
}
#endif

@end
