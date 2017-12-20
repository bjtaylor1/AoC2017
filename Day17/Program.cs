using System;

namespace Day17
{
    class Program
    {
        static int N = 370; //(my puzzle input)

        static int F(int n)
        {
            if (n == 0) return 0;
            return ((G(n - 1, F(n-2)) + N) % n) + 1;
        }
        static int G(int n, int prevn)
        {
            if (n == 0) return 0;
            return ((F(n - 1) + N) % n) + 1;
        }
        static void Main()
        {
            Console.WriteLine(F(50000000));
        }
    }
}
