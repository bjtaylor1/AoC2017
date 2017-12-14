using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Day13
{
    class Program
    {
        static void Main(string[] args)
        {
            string line;
            var poses = new List<Pos>();
            var sw = Stopwatch.StartNew();
            while (!string.IsNullOrEmpty(line = Console.ReadLine()))
            {
                var parts = line.Split(new[] { ": " }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                var pos = new Pos(parts[0], parts[1]);
                poses.Add(pos);
            }
            bool found = false;
            for (int delay = 0; !found ; delay++)
            {
                var spos = poses.Select(p => p.Amplitude - Math.Abs(((p.Depth + delay) % (2 * p.Amplitude)) - p.Amplitude) == 0).ToArray();
                var whereCaught = poses.Where(p => p.Amplitude - Math.Abs(((p.Depth + delay) % (2 * p.Amplitude)) - p.Amplitude) == 0).ToArray();
                if(!whereCaught.Any())
                {
                    sw.Stop();
                    Console.Out.WriteLine(delay);
                    Console.Out.WriteLine(sw.Elapsed);
                    found = true;
                }
            }
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
