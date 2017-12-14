using System;
using System.Linq;

namespace Day14
{
    class Program
    {
        static void Main(string[] args)
        {
            string line;
            int CountBits(int i) => i > 0 ? i % 2 + CountBits(i >> 1) : 0;
            var bitsSet = Enumerable.Range(0, 0x10).ToDictionary(i => $"{i:x}"[0], CountBits);
            while (!string.IsNullOrEmpty(line = Console.ReadLine()))
            {
                var keyStrings = Enumerable.Range(0, 128).Select(i => $"{line}-{i}").ToArray();
                var hashes = keyStrings.Select(Day10.Program.GetKnotHash).ToArray();
                var used = hashes.Sum(h => h.Sum(c => bitsSet[c]));
                Console.WriteLine(used);
            }
        }
    }
}
