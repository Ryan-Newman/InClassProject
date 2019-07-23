using System.Collections.Generic;
using System;
using System.Collections;
class Harness
{
    void F()
    {
        int a = 0;
        int b = 0;
        int c = 0;
        int result = 0;

        result = Max(a, b, c);
    }
    int Max(int a, int b, int c)
    {
        int d = Math.Max(a, b);
        int e = Math.Max(b, c);
        if(d > e)
        { return d; }
        else { return e; }
    }
    static void Main()
    {
        F();
    }
}
