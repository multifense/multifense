//
//  data.h
//  MPTD
//
//  Created by Karl-Johan Alm on 5/14/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#ifndef MPTD_kerneldata_h
#define MPTD_kerneldata_h

namespace tdmp
{
    class gameObject
    {
    public:
        int     type;
        int     cost;
        gameObject() {};
        gameObject(int _type, int _cost) 
        {
            type = _type;
            cost = _cost;
        };
    };
    
    class monster : gameObject
    {
    public:
        double  hp;
        int     armor;
        double  speed;
        int     incomeIncrease;
        int     coloring;
        monster() : gameObject() {};
        monster(int _type, const char *_sprite, int _health, int _speed, int _send_cost, int _income_increase, int _coloring) 
        : gameObject(_type, _send_cost) 
        {
            hp = _health;
            speed = _speed;
            incomeIncrease = _income_increase;
            coloring = _coloring;
        };
    };
    
    class tower : gameObject
    {
    public:
        double  fireRate;
        double  range;
        int     damage;
        double  projSpeed;
        tower () : gameObject() {};
        tower(int _type, const char *_sprite, int _damage, int _time_between_shots, int _range, int _cost, const char *_proj_sprite, int _proj_speed, const char *_proj_sound) 
        : gameObject(_type, _cost)
        {
            damage = _damage;
            fireRate = _time_between_shots;
            range = _range;
            projSpeed = _proj_speed;
        };
    };
}

#endif
