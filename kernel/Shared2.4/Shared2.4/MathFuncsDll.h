#include "SharedModule.h"

namespace MathFuncs
{
    class MyMathFuncs
    {
    public:
        // Returns a + b
        static __declspec(dllexport) bool Start(char *ip,  char *port);

        // Returns a - b
        static __declspec(dllexport) void SendMsg(char *msg);

        // Returns a * b
        static __declspec(dllexport) char *Receive();

    };
}
