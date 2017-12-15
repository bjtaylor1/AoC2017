using System;
using System.Collections.Generic;
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
                Queue<long> aVals = new Queue<long>(), bVals = new Queue<long>();
                int valuesChecked = 0;
                while(valuesChecked < 5e6)
                {
                    a = (a * 16807) % 2147483647;
                    b = (b * 48271) % 2147483647;
                    if (a % 4 == 0) aVals.Enqueue(a);
                    if (b % 8 == 0) bVals.Enqueue(b);
                    while (aVals.Any() && bVals.Any())
                    {
                        valuesChecked++;
                        var nextA = aVals.Dequeue();
                        var nextB = bVals.Dequeue();
                        if ((nextA & 0xffff) == (nextB & 0xffff))
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
