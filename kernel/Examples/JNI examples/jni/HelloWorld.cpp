#include "se_mossan_AndroidJNITestActivity.h"
#include <jni.h>
#include "stdio.h"
#include <iostream>

using namespace std;

 JNIEXPORT void JNICALL 
 Java_se_mossan_AndroidJNITestActivity_print(JNIEnv *env, jobject obj)
 {
	 cout << "trolololo!\n" << endl;
     return;
 }
