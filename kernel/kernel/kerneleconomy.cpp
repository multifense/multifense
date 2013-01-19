//
//  kerneleconomy.cpp
//  MPTD
//
//  Created by Karl-Johan Alm on 5/14/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#include <iostream>
#include <cmath>

#include "kerneleconomy.h"

tdmp::economy::economy()
{
    spIncome = 10;
    mpIncome = 10;
}

int tdmp::economy::updateSPIncome(int waveCount)
{
    spIncome *= 2;
    return spIncome;
}

int tdmp::economy::getMonsterMoney(monster mon)
{
    // 300 hp, 3 speed
    return (int)(pow((double)(mon.hp + (mon.hp / 3.0) * mon.speed) / 104.0, 0.8) * 3.0 - 2.0);
    //return Convert.ToInt32(Math.Pow(((double)mon.maxHP + (mon.maxHP / 3) * mon.maxSpeed) / 104, 0.8) * 3 - 2);
}

int tdmp::economy::buyMonster(monster mon)
{
    mpIncome += pow(((double)mon.hp + (mon.hp / 3 * mon.speed)) / 104.0, 0.8);
    return mpIncome;
}
