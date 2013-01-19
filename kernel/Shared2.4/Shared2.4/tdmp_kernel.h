//
//  tdmp_kernel.h
//  MPTD
//
//  Created by Karl-Johan Alm on 5/4/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#ifndef MPTD_tdmp_kernel_h
#define MPTD_tdmp_kernel_h

#define SMMET static __declspec(dllexport)

#include <iostream>

#include "SharedModule.h"

using namespace std;

namespace tdmp
{
    class kernel
    {
    public:
        SMMET
        string didDie();
    };
}

#endif
