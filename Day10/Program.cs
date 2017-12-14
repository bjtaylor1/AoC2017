using System;
using Helpers;

namespace Day10
{
    class Program
    {
        static void Main(string[] args)
        {
            string line;

            while(!string.IsNullOrEmpty(line = Console.ReadLine()))
            {
                var knotHash = Helper.GetKnotHash(line);
                Console.WriteLine(knotHash);
            }
        }
    }
}
