using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DM1
{
    class Program
    {
        static void Main(string[] args)
        {
            string line = Console.ReadLine() ;
            var parts = line.Split(' ');
            var sleighs = new ConcurrentDictionary<int, Sleigh>();
            foreach(var part in parts)
            {
                var match = Regex.Match(part.Trim(), @"^(\w+)(\d+)$");
                if (!match.Success) throw new InvalidOperationException();
                string colour = match.Groups[1].Value;
                var sleighNum = int.Parse(match.Groups[2].Value);
                var sleigh = sleighs.GetOrAdd(sleighNum, new Sleigh());
                sleigh.Add(colour);
            }
            
        }
    }

    class Sleigh
    {
        public void Add(string c)
        {
            if (!Presents.TryGetValue(c, out int current)) current = 0;
            Presents[c] = ++current;
        }

        public bool IsContigious => Presents.Keys.Distinct().Count() <= 1;
        public int MinPresentsOfOneColour => Presents.Values.Min();
        public int DifferentColours => Presents.Count();

        public bool TryRemove(out string c)
        {
            if(!Presents.Any())
            {
                c = default(string);
                return false;
            }
            c = Presents.Keys.First();
            if (--Presents[c] == 0) Presents.Remove(c);
            return true;
        }

        public Dictionary<string, int> Presents { get; } = new Dictionary<string, int>();
    }
}
