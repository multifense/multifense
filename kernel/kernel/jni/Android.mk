   LOCAL_PATH := $(call my-dir)

   include $(CLEAR_VARS)

   #LOCAL_MODULE    := HelloWorld
   #LOCAL_CFLAGS	   := -Werror
  # LOCAL_SRC_FILES := HelloWorld.cpp
 #  LOCAL_LDLIBS    := -llog

   LOCAL_MODULE    := kernel
   LOCAL_CFLAGS    := -Werror
   LOCAL_SRC_FILES := kernel.cpp
   LOCAL_LDLIBS    := -llog

   include $(BUILD_SHARED_LIBRARY)
