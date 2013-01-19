//
//  kerneleconomy.h
//  MPTD
//
//  Created by Karl-Johan Alm on 5/14/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#ifndef MPTD_kerneleconomy_h
#define MPTD_kerneleconomy_h

#include "kerneldata.h"

namespace tdmp
{
    class economy
    {
    public:
        int spIncome;
        int mpIncome;
        economy();
        int updateSPIncome(int waveCount);
        int getMonsterMoney(monster mon);
        int buyMonster(monster mon);
    };
}    

#endif
