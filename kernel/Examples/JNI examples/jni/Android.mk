   LOCAL_PATH := $(call my-dir)

   include $(CLEAR_VARS)

   #LOCAL_MODULE    := HelloWorld
   #LOCAL_CFLAGS	   := -Werror
  # LOCAL_SRC_FILES := HelloWorld.cpp
 #  LOCAL_LDLIBS    := -llog
   
   LOCAL_MODULE    := Source1
   LOCAL_CFLAGS    := -Werror
   LOCAL_SRC_FILES := Source1.cpp
   LOCAL_LDLIBS    := -llog

   include $(BUILD_SHARED_LIBRARY)
