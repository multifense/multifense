//
//  ClientIphone.h
//  MPTD
//
//  Created by Karl-Johan Alm on 5/7/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#ifndef MPTD_ClientIphone_h
#define MPTD_ClientIphone_h

#include "kernel.h"

@class GameSession;

class ClientIphone : tdmp::kernelDelegate
{
public:
    GameSession *session;

    ClientIphone(GameSession *sess);
    
    //void initWithSession(GameSession *sess);
    void setSession(GameSession *sess);
};

#endif
