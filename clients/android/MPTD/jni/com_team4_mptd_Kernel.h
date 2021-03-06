/* DO NOT EDIT THIS FILE - it is machine generated */
#include <jni.h>
/* Header for class com_team4_mptd_Kernel */

#ifndef _Included_com_team4_mptd_Kernel
#define _Included_com_team4_mptd_Kernel
#ifdef __cplusplus
extern "C" {
#endif
/*
 * Class:     com_team4_mptd_Kernel
 * Method:    loadData
 * Signature: ()V
 */
JNIEXPORT void JNICALL Java_com_team4_mptd_Kernel_loadData
  (JNIEnv *, jobject);

/*
 * Class:     com_team4_mptd_Kernel
 * Method:    setNickname
 * Signature: (Ljava/lang/String;)V
 */
JNIEXPORT void JNICALL Java_com_team4_mptd_Kernel_setNickname
  (JNIEnv *, jobject, jstring);

/*
 * Class:     com_team4_mptd_Kernel
 * Method:    didRead
 * Signature: (Ljava/lang/String;)V
 */
JNIEXPORT void JNICALL Java_com_team4_mptd_Kernel_didRead
  (JNIEnv *, jobject, jstring);

/*
 * Class:     com_team4_mptd_Kernel
 * Method:    didCreateTower
 * Signature: (IIII)V
 */
JNIEXPORT void JNICALL Java_com_team4_mptd_Kernel_didCreateTower
  (JNIEnv *, jobject, jint, jint, jint, jint);

/*
 * Class:     com_team4_mptd_Kernel
 * Method:    didSpawnMonster
 * Signature: (II)V
 */
JNIEXPORT void JNICALL Java_com_team4_mptd_Kernel_didSpawnMonster
  (JNIEnv *, jobject, jint, jint);

/*
 * Class:     com_team4_mptd_Kernel
 * Method:    didKillMonster
 * Signature: (I)V
 */
JNIEXPORT void JNICALL Java_com_team4_mptd_Kernel_didKillMonster
  (JNIEnv *, jobject, jint);

/*
 * Class:     com_team4_mptd_Kernel
 * Method:    didKillLastMonster
 * Signature: ()V
 */
JNIEXPORT void JNICALL Java_com_team4_mptd_Kernel_didKillLastMonster
  (JNIEnv *, jobject);

/*
 * Class:     com_team4_mptd_Kernel
 * Method:    didRecruitMonster
 * Signature: (II)V
 */
JNIEXPORT void JNICALL Java_com_team4_mptd_Kernel_didRecruitMonster
  (JNIEnv *, jobject, jint, jint);

/*
 * Class:     com_team4_mptd_Kernel
 * Method:    didSurrender
 * Signature: ()V
 */
JNIEXPORT void JNICALL Java_com_team4_mptd_Kernel_didSurrender
  (JNIEnv *, jobject);

/*
 * Class:     com_team4_mptd_Kernel
 * Method:    didTakeDamage
 * Signature: (II)V
 */
JNIEXPORT void JNICALL Java_com_team4_mptd_Kernel_didTakeDamage
  (JNIEnv *, jobject, jint, jint);

/*
 * Class:     com_team4_mptd_Kernel
 * Method:    didDie
 * Signature: ()V
 */
JNIEXPORT void JNICALL Java_com_team4_mptd_Kernel_didDie
  (JNIEnv *, jobject);

/*
 * Class:     com_team4_mptd_Kernel
 * Method:    findGame
 * Signature: (I)V
 */
JNIEXPORT void JNICALL Java_com_team4_mptd_Kernel_findGame
  (JNIEnv *, jobject, jint);

/*
 * Class:     com_team4_mptd_Kernel
 * Method:    sharedModuleSelfTest
 * Signature: ()V
 */
JNIEXPORT void JNICALL Java_com_team4_mptd_Kernel_sharedModuleSelfTest
  (JNIEnv *, jobject);

/*
 * Class:     com_team4_mptd_Kernel
 * Method:    newGame
 * Signature: ()V
 */
JNIEXPORT void JNICALL Java_com_team4_mptd_Kernel_newGame
  (JNIEnv *, jobject);

/*
 * Class:     com_team4_mptd_Kernel
 * Method:    loadMapData
 * Signature: (I)V
 */
JNIEXPORT void JNICALL Java_com_team4_mptd_Kernel_loadMapData
  (JNIEnv *, jobject, jint);

/*
 * Class:     com_team4_mptd_Kernel
 * Method:    getNameForMap
 * Signature: (I)Ljava/lang/String;
 */
JNIEXPORT jstring JNICALL Java_com_team4_mptd_Kernel_getNameForMap
  (JNIEnv *, jobject, jint);

/*
 * Class:     com_team4_mptd_Kernel
 * Method:    getImageNameForMap
 * Signature: (I)Ljava/lang/String;
 */
JNIEXPORT jstring JNICALL Java_com_team4_mptd_Kernel_getImageNameForMap
  (JNIEnv *, jobject, jint);

/*
 * Class:     com_team4_mptd_Kernel
 * Method:    updateSPIncome
 * Signature: (I)I
 */
JNIEXPORT jint JNICALL Java_com_team4_mptd_Kernel_updateSPIncome
  (JNIEnv *, jobject, jint);

/*
 * Class:     com_team4_mptd_Kernel
 * Method:    getBountyForMonster
 * Signature: (I)I
 */
JNIEXPORT jint JNICALL Java_com_team4_mptd_Kernel_getBountyForMonster
  (JNIEnv *, jobject, jint);

/*
 * Class:     com_team4_mptd_Kernel
 * Method:    updateMPIncomeForBuyingMonster
 * Signature: (I)I
 */
JNIEXPORT jint JNICALL Java_com_team4_mptd_Kernel_updateMPIncomeForBuyingMonster
  (JNIEnv *, jobject, jint);

/*
 * Class:     com_team4_mptd_Kernel
 * Method:    mpIncome
 * Signature: ()I
 */
JNIEXPORT jint JNICALL Java_com_team4_mptd_Kernel_mpIncome
  (JNIEnv *, jobject);

/*
 * Class:     com_team4_mptd_Kernel
 * Method:    spIncome
 * Signature: ()I
 */
JNIEXPORT jint JNICALL Java_com_team4_mptd_Kernel_spIncome
  (JNIEnv *, jobject);

#ifdef __cplusplus
}
#endif
#endif
