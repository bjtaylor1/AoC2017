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
            var sPos = poses.Select(p => p.Range - Math.Abs((p.Depth % (2*p.Range)) - p.Amplitude)).ToArray();

            //var severity = whereCaught.Sum(p => p.Range * p.Depth);
            Console.Out.WriteLine();
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
        public int Amplitude => Range + 1;

        public override string ToString()
        {
            return $"{Depth}: {Range}";
        }
    }
    
}
