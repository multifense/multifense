//
//  GameSound.h
//  MPTD
//
//  Created by Karl-Johan Alm on 5/12/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>

#import "CDAudioManager.h"

/// defines from some random tutorial somewhere

// This is a shared header
// Don't mind too much what is in this file yet, it'll be explained below.
/** CDAudioManager supports two long audio source channels called left and right
 typedef enum {
 kASC_Left = 0,
 kASC_Right = 1
 } tAudioSourceChannel;	*/
#define CGROUP_BG       kASC_Left    // Channel for background music
#define CGROUP_EFFECTS  kASC_Right   // Channel for sound effects

/*#define SND_BG_MENUS    1       // menu background music
#define SND_BG_GAME     2       // game background music
#define SND_BG_WIN      3       // game win background music
#define SND_BG_LOSE     4       // game loss background music*/

#define SND_CANNON      10      // cannon fire sound
#define SND_FIRE        11      // fire fire
#define SND_ICE         12      // ice fire
#define SND_MAGIC       13      // magic fire

#define SND_CLICK       20      // click? click!

// Helper macro for playing sound effects
#define playEffect(__ID__)      [[CDAudioManager sharedManager].soundEngine playSound:__ID__ sourceGroupId:CGROUP_EFFECTS pitch:1.0f pan:0.0f gain:0.4f loop:NO]

/// end of random dfeines


//@class AVAudioPlayer;

@interface GameSound : NSObject {
    NSMutableDictionary *_indices;
}

+ (GameSound *)defaultInstance;

- (int)soundIndexForFile:(NSString *)file;

- (void)switchBackgroundMusic:(NSString *)soundName;
- (void)switchBackgroundMusicNoLoop:(NSString *)soundName;

/*- (void)restartBG;
- (void)switchBackgroundMusicNoFade:(NSString *)soundName;*/

//@property (nonatomic, retain) AVAudioPlayer *backgroundPlayer;

@end
