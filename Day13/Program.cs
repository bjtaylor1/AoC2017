using System;
using System.Collections.Generic;
using System.Linq;

namespace Day13
{
    class Program
    {
        static void Main(string[] args)
        {
            string line;
            var poses = new List<Pos>();
            while(!string.IsNullOrEmpty(line = Console.ReadLine()))
            {
                var parts = line.Split(new[] { ": " }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray() ;
                var pos = new Pos(parts[0], parts[1]);
                poses.Add(pos);
            }
            var whereCaught = poses.Where(p => p.Amplitude - Math.Abs((p.Depth % (2*p.Amplitude)) - p.Amplitude) == 0).ToArray();
            var severity = whereCaught.Sum(p => p.Range * p.Depth);
            
            Console.Out.WriteLine(severity);
        }
    }

    class Pos
    {
        public Pos(int depth, int range)
        {
            Depth = depth;
            Range = range;
        }

        public int Depth { get; }
        public int Range { get; }
        public int Amplitude => Range - 1;

        public override string ToString()
        {
            return $"{Depth}: {Range}";
        }
    }
    
}
