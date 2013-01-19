#include "se_mossan_AndroidJNITestActivity.h"
#include <jni.h>
#include "stdio.h"
#include <iostream>

using namespace std;

 JNIEXPORT jstring JNICALL 
 Java_se_mossan_AndroidJNITestActivity_print1(JNIEnv *env, jobject obj)
 {
	 return (*env).NewStringUTF("bajskaka");
 }
