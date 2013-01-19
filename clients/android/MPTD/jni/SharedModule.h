#ifndef INCLUDED_SHAREDMODULE_H
#define INCLUDED_SHAREDMODULE_H

#define KERNPREFIX Java_com_team4_mptd_Kernel_
//#define KERNPREFIX Java_se_mossan_AndroidJNITestActivity_

#ifndef _WIN32
#  define __declspec(f)
#  define kint int
#  define kcbint int
#else
#  define kint int
#  define kcbint long
#endif

#ifdef JNI_USED
#define TDMP_JNI
#endif

#ifdef TDMP_JNI
 #define STDCALL 
#else
 #define STDCALL __stdcall
#endif

#define EXPORT  __declspec(dllexport)
#define SMME(type) EXPORT type 

#define KERNRET     void
#define KERNCALL    SMME(KERNRET)

#ifdef TDMP_JNI
#include <jni.h>

#define KERNJNI(method) JNIEXPORT void JNICALL KERNPREFIX##method
#define KERNRETJNI(rettype, method) JNIEXPORT rettype JNICALL KERNPREFIX##method

#define DebugLog(s) CALLBACKString(debugLog,s)
#define NetWrite(s) CALLBACKString(netWrite,s)
#define PlayerJoined(id,name) \
    {\
        jstring x = (lastEnv)->NewStringUTF(name); \
        CALLBACK(playerJoined, SIGPlayerWithName, id, x); \
    }
#define MonsterDefinition(type,sprite,health,speed,send_cost,income_increase,coloring) \
    {\
        monsterTypes[type] = monster(type,sprite,health,speed,send_cost,income_increase,coloring);\
        jstring x = (lastEnv)->NewStringUTF(sprite); \
        CALLBACK(monsterDefinition, SIGMonsterDefinition, type, x, health, speed, send_cost, income_increase, coloring); \
    }
#define TowerDefinition(type,sprite,damage,time_between_shots,range,cost,proj_sprite,proj_speed,proj_sound) \
{\
    towerTypes[type] = tower(type,sprite,damage,time_between_shots,range,cost,proj_sprite,proj_speed,proj_sound);\
    jstring xsprite = (lastEnv)->NewStringUTF(sprite); \
    jstring xprojsprite = (lastEnv)->NewStringUTF(proj_sprite); \
    jstring xprojsound = (lastEnv)->NewStringUTF(proj_sound); \
    CALLBACK(towerDefinition, SIGTowerDefinition, type, xsprite, damage, time_between_shots, range, cost, xprojsprite, proj_speed,xprojsound); \
}
#define PathDefinition(x,y,dirx,diry,length) \
    CALLBACK(pathDefinition, SIGPathDefinition, x,y, dirx,diry, length)
#else
#define DebugLog(s) defaultKernel->cli->debugLog(s)
#define NetWrite(s) defaultKernel->cli->netWrite(s)
#define PlayerJoined(id,name)   defaultKernel->cli->playerJoined(id,name)
#define MonsterDefinition(type,sprite,health,speed,send_cost,income_increase,coloring) \
    monsterTypes[type] = monster(type,sprite,health,speed,send_cost,income_increase,coloring);\
    cli->monsterDefinition(type,sprite,health,speed,send_cost,income_increase,coloring)
#define TowerDefinition(type,sprite,damage,time_between_shots,range,cost,proj_sprite,proj_speed,proj_sound) \
    towerTypes[type] = tower(type,sprite,damage,time_between_shots,range,cost,proj_sprite,proj_speed,proj_sound);\
    cli->towerDefinition(type,sprite,damage,time_between_shots,range,cost,proj_sprite,proj_speed,proj_sound)
// path definition: x, y, dirx, diry, length
#define PathDefinition(x,y,dirx,diry,length) \
    cli->pathDefinition(x,y,dirx,diry,length)
#endif

#ifdef TDMP_JNI
#define CALLBACKString(method,s) \
  {\
    jstring x = (lastEnv)->NewStringUTF(s); \
    CALLBACK(method, SIGNetIO, x);\
  }
#else
#define CALLBACKString(method,s) \
  CALLBACK(method, SIGNetIO, s);
#endif

#ifdef TDMP_JNI
// CALLBACK is used to reroute callbacks to use the appropriate arguments depending on the destination client
// this version is for JNI and does the jclass trickery
#define CALLBACK(method,signature,args...) \
    {\
        jclass cls = (lastEnv)->GetObjectClass(lastObj); \
        jmethodID mid = (lastEnv)->GetMethodID(cls, #method, signature);\
        if (mid == 0) return;\
        (lastEnv)->CallVoidMethod(lastObj,mid,args);\
    }

#define TDMP_JNIENTRY(method,args...) \
    KERNJNI(method)(JNIEnv *env, jobject obj, args) {\
        tdmp::defaultKern()->jni(env,obj); \
        tdmp::defaultKern()->method(args); \
    }

#define TDMP_JNIENTRYN(method) \
  KERNJNI(method)(JNIEnv *env, jobject obj) {\
    tmp::defaultkern()->jni(env,obj); \
    tdmp::defaultKern()->method(); \
  }

#define TDMP_FUNINIT JNIEnv *env, jobject obj
#define TDMP_FUNFIX defaultKern()->jni(env,obj)

#else

// THIS version of the CALLBACK macro uses the internal delegate
#ifdef _WIN32
#define CALLBACK(method,signature,...) \
    defaultKernel->cli->method(__VA_ARGS__)
#else
#define CALLBACK(method,signature,args...) \
    cli->method(args)
#endif
#endif

#endif
