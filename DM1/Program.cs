using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DM1
{
    class Program
    {
        static void Main(string[] args)
        {
            var numbers = Regex.Split(File.ReadAllText("input.txt").Trim(), @"\s+").Select(s => s.Trim()).Select(int.Parse).ToArray();
            int numSleighs = (int)Math.Sqrt(numbers.Length);
            if (numSleighs * numSleighs != numbers.Length) throw new InvalidOperationException();

            var sleighs = new ConcurrentDictionary<int, Sleigh>();
            int i = 0;
            for(int sleighNum = 0; sleighNum < numSleighs; sleighNum++)
            {
                Sleigh sleigh = sleighs.GetOrAdd(sleighNum, new Sleigh());
                for (int colour = 0; colour < numSleighs; colour++)
                {
                    sleigh.Add(colour, numbers[i++]);
                }
            }

            var permutations = new List<int[]>();
            void AddPermutation(Stack<int> s, List<int[]> p)
            {
                if (s.Count == 10) p.Add(s.ToArray());
                else
                {
                    for (int n = 0; n < 10; n++)
                    {
                        if (!s.Contains(n))
                        {
                            s.Push(n);
                            AddPermutation(s, p);
                            s.Pop();
                        }
                    }
                }
            }
            AddPermutation(new Stack<int>(), permutations);

            var totalOfEachColour = Enumerable.Range(0, numSleighs).Select(s => sleighs.Sum(k => k.Value.Presents[s])).ToArray();
            var targetOfEachColour = totalOfEachColour.Select(n => n / numSleighs).ToArray();
            var numMoves = sleighs.Select(sleigh => sleigh.Value.Presents.Select(p => Math.Abs(targetOfEachColour[p.Key] - p.Value)).Sum()).Sum();
            Console.Out.WriteLine(numMoves);
        }
    }

    class Sleigh
    {
        public void Add(int c, int num)
        {
            if (Presents.ContainsKey(c)) throw new InvalidOperationException();
            Presents.Add(c, num);
        }

        public bool IsContigious => Presents.Keys.Distinct().Count() <= 1;
        public int MinPresentsOfOneColour => Presents.Values.Min();
        public int DifferentColours => Presents.Count();

        public bool TryRemove(out int c)
        {
            if(!Presents.Any())
            {
                c = -1;
                return false;
            }
            c = Presents.Keys.First();
            if (--Presents[c] == 0) Presents.Remove(c);
            return true;
        }

        public Dictionary<int, int> Presents { get; } = new Dictionary<int, int>();
    }
}
