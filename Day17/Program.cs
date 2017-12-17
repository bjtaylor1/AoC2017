using System;

namespace Day17
{
    class Program
    {
        static void Main(string[] args)
        {
            string line;
            while (!string.IsNullOrEmpty(line = Console.ReadLine()))
            {
                int N = int.Parse(line);
                long pnprev = 0;
                long? valueAfter0 = null;
                for (int n = 1; n <= 50e6; n++)
                {
                    long pn = ((pnprev + N) % n) + 1;
                    if (pn == 1) valueAfter0 = n;
                    pnprev = pn;
                }
                Console.WriteLine(valueAfter0);
            }
        }
    }
}
