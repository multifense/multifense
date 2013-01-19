/*
 * SharedModule.h
 * This file is a "macro core" of the kernel, containing a bunch of little tweaks to make code less of a pain to write elsewhere.
 * It uses a number of existing "defines", such as _WIN32 which is DEFINED if the compiler is on a windows computer (even 64 bit).
 */
#ifndef INCLUDED_SHAREDMODULE_H
#define INCLUDED_SHAREDMODULE_H

// The JDK prefix for method names
#define KERNPREFIX Java_com_team4_mptd_Kernel_
//#define KERNPREFIX Java_se_mossan_AndroidJNITestActivity_

// If we are NOT on WIN 32 ...
#ifndef _WIN32
// we make sure that e.g. __declspec exists (but does nothing) -- it is something used by WIN32 DLL's; we also make sure that
// the integer type used is LONG for win32 and INT for others; this is due to C# ints being 64 bit
#  define __declspec(f)
#  define kint int
#  define kcbint int
#else
#  define kint int
#  define kcbint long
#endif

// JNI_USED is set manually in kernel.h for android compiles; I haven't found a way to automatically detect if the compilation is
// for JNI yet, but it would be nice. Here we simply make TDMP_JNI synonymous with JNI_USED (JNI_USED would be whatever expression
// defines JNI being used, so we can change that one as long as we then define TDMP_JNI).
#ifdef JNI_USED
#define TDMP_JNI
#endif

// For JNI we do not use __stdcall so we define our own macro STDCALL as empty for JNI, and __stdcall for others (in fact, we don't
// use __stdcall on Objective-C either, but we don't need to worry about that here).
#ifdef TDMP_JNI
 #define STDCALL 
#else
 #define STDCALL __stdcall
#endif

// We now define the oft-used macros EXPORT and SMME; export is simply saying "this method should be public", and SMME stands for
// Shared Module Method Expression
#define EXPORT  __declspec(dllexport)
#define SMME(type) EXPORT type 

// This is the same for everyone but it's here in case we ever need to change it across the globe; we define the return value 
// for kernel methods as void and we define a kernel call as SMME(KERNRET), i.e. SMME(void), i.e. EXPORT void, 
// i.e. __declspec(dllexport) void
#define KERNRET     void
#define KERNCALL    SMME(KERNRET)

// This define goes all the way down to #1# below (first #1# is at the else case)
#ifdef TDMP_JNI
#include <jni.h>

// We now define two separate macros for JNI -- KERNJNI for declaring methods, and KERNRETJNI for declaring methods with a
// non-void return type; note the weird dash-dash in there; ## is used to embed macro expressions inside of text without
// using spacing; if we have "#define animal monkey", and then express "animal##dance" we will get "monkeydance". 
#define KERNJNI(method) JNIEXPORT void JNICALL KERNPREFIX##method
#define KERNRETJNI(rettype, method) JNIEXPORT rettype JNICALL KERNPREFIX##method

// Here are simply some convenience methods used a lot in the kernel code
// DebugLog is used to send a debug message that can be printed on screen or whatever, on the client
// NetWrite is used to write to the network
#define DebugLog(s) CALLBACKString(debugLog,s)
#define NetWrite(s) CALLBACKString(netWrite,s)

// PlayerJoined is used to generate the callback for when a player has joined,
#define PlayerJoined(id,name) \
    {\
        jstring x = (lastEnv)->NewStringUTF(name); \
        CALLBACK(playerJoined, SIGPlayerWithName, id, x); \
    }
// MonsterDefinition is used to generate the callback for a monster definition
#define MonsterDefinition(type,sprite,health,speed,send_cost,income_increase,coloring) \
    {\
        monsterTypes[type] = monster(type,sprite,health,speed,send_cost,income_increase,coloring);\
        jstring x = (lastEnv)->NewStringUTF(sprite); \
        CALLBACK(monsterDefinition, SIGMonsterDefinition, type, x, health, speed, send_cost, income_increase, coloring); \
    }
// ... you get the picture.
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
#else // #1#
// here we are at the NON-JNI version of the above; we do the same stuff, but for C# and/or iOS
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
#endif // #1#

// There's no reason why this was split into a separate thing, except for readability; we now define a CALLBACKString
// method, one version for JNI and one for iOS/C#
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

// (NOTE: TDMP_JNIENTRY and JNIENTRYN are not used currently -- they are ideal, and you should definitely read the comment
// below anyway as it explains stuff; what is used, however, are the two macros BELOW these!)
// TDMP_JNIENTRY and TDMP_JNIENTRYN are used to define a JNI method named "method" with the given arguments; the macro simply 
// sets the JNI environment vars up and then calls the method with the given arguments, minus the JNI arguments. Note the 
// triple-dots after args -- this is a way to inform the precompiler that this macro takes ANY number of arguments; these are then
// used in turn as if they were several arguments, as you see in the call to defaultKern()->method()
// TDMP_JNIENTRYN is simply the same as the former, except it has no arguments aside from the JNI environment and object ones
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

// Further shorthands for when defining a method; TDMP_FUNFIX lets you declare a JNI method like so:
//   void myMethod(TDMP_FUNINIT, int foo)
// which will convert into
//   void myMethod(JNIEnv *env, jobject obj, int foo)
// and TDMP_FUNFIX sets the jni environment up in the kernel
#define TDMP_FUNINIT JNIEnv *env, jobject obj
#define TDMP_FUNFIX defaultKern()->jni(env,obj)

#else

// THIS version of the CALLBACK macro uses the internal delegate
// C# (or Windows, rather) extracts arguments (the dotdotdot stuff explained above) using __VA_ARGS__, while Objective-C uses
// args straight off
#ifdef _WIN32
#define CALLBACK(method,signature,...) \
    defaultKernel->cli->method(__VA_ARGS__)
#else
#define CALLBACK(method,signature,args...) \
    cli->method(args)
#endif
#endif

// Method definitions for CALLBACKS; these are only used by C# and iOS; on iOS something called blocks is used and this
// uses a special syntax (^methodname) whereas C# uses standard
#ifndef TDMP_JNI
#ifdef IOS
#define TDMP_FTDECL(name) typedef void (^name)
#else
#define TDMP_FTDECL(name) typedef void (STDCALL *name)
#endif
#endif

// We get a CSHARP definition for use when we want to target only CSHARP compiler
#ifdef _WIN32
#ifndef TDMP_JNI
#define CSHARP
#endif
#endif

#endif
