//
//  StatusMenu.h
//  MPTD
//
//  Created by Karl-Johan Alm on 4/26/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface StatusMenu : UIView {
    __unsafe_unretained IBOutlet UILabel *_health;
    __unsafe_unretained IBOutlet UILabel *_wave;
    __unsafe_unretained IBOutlet UILabel *_kills;
    __unsafe_unretained IBOutlet UILabel *_gold;
    __unsafe_unretained IBOutlet UILabel *_waveTimer;
    __unsafe_unretained IBOutlet UILabel *_income;
}

@property (nonatomic, assign) UILabel *health;
@property (nonatomic, assign) UILabel *wave;
@property (nonatomic, assign) UILabel *kills;
@property (nonatomic, assign) UILabel *gold;
@property (nonatomic, assign) UILabel *waveTimer;
@property (nonatomic, assign) UILabel *income;

@end
