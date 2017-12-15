using System;
using System.Linq;

namespace Day15
{
    class Program
    {
        static void Main(string[] args)
        {
            string line;
            while (!string.IsNullOrEmpty(line = Console.ReadLine()))
            {
                long[] parts = line.Split(',').Select(long.Parse).ToArray();
                long a = parts[0], b = parts[1];
                long matches = 0;
                for (long i = 0; i < 40e6; i++)
                {
                    a = (a * 16807) % 2147483647;
                    b = (b * 48271) % 2147483647;
                    if (a%4 == 0 /*&& b%8 == 0*/)
                    {
                        if ((a & 0xffff) == (b & 0xffff))
                        {
                            matches++;
                        }
                    }
                }
                Console.WriteLine(matches);
            }
        }
    }
}
