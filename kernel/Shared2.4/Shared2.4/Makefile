CC=g++
CFLAGS=-c -Wall

all: SharedModule.so

SharedModule.so: MathFuncsDll.o
	$(CC) -shared MathFuncsDll.o -o SharedModule.so

MathFuncsDll.o: MathFuncsDll.cpp
	$(CC) $(CFLAGS) MathFuncsDll.cpp

clean:
	rm -rf *.o SharedModule.so
