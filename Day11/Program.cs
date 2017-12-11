using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Day11
{
    class Program
    {
        private static readonly Pos StartPos = new Pos(0, 0, 0);
        static void Main(string[] args)
        {
            string line;
            //while (!string.IsNullOrEmpty(line = File.ReadAllText(@"..\..\input.txt")))
            while (!string.IsNullOrEmpty(line = Console.ReadLine()))
            {
                var moves = line.Trim().Split(',');
                var startPos = new Pos(0, 0, 0);
                var allPos = new List<Pos>();
                var endPos = moves.Aggregate(startPos, (p, m) =>
                {
                    var pos = p.Move(m);
                    if (!allPos.Contains(pos))
                    {
                        allPos.Add(pos);
                    }
                    return pos;
                });
                var stepsEndPos = GetSteps(endPos);
                Console.WriteLine(stepsEndPos);
            }
        }

        private static readonly ConcurrentDictionary<Pos, int> Steps = new ConcurrentDictionary<Pos, int>();

        private static int GetSteps(Pos endPos)
        {
            return GetSteps(endPos, new Stack<Pos>());
        }

        private static int GetStepsCached(Pos endPos, Stack<Pos> visited)
        {
            return Steps.GetOrAdd(endPos, p => GetSteps(p, visited));
        }

        private static int GetSteps(Pos endPos, Stack<Pos> visited)
        {
            visited.Push(endPos);
            int retVal;
            var neighbours = Constants.Dirs.Select(endPos.Move).ToArray();
            var nearerUnvisitedNeighbours = neighbours.Where(n => n.IsNearerThan(endPos) && !visited.Any(v => v.Equals(n) || v.IsNearerThan(n))).ToArray();
            if (endPos.Equals(StartPos))
                retVal = 0;
            else
            {
                var neighbourDists = nearerUnvisitedNeighbours.Select(n => new {n, steps = GetStepsCached(n, visited)})
                    .ToArray();
                if (neighbourDists.Any())
                {
                    var pathNext = neighbourDists.OrderBy(s => s.steps).First();
                    retVal = pathNext.steps + 1;
                }
                else retVal = int.MaxValue;
            }
            visited.Pop();
            return retVal;
        }
    }

    class Pos
    {
        public readonly double X, Y, Dist;
        public readonly int Moved;
        public Pos(double x, double y, int moved)
        {
            X = x;
            Y = y;
            Moved = moved;
            Dist = X * X + Y * Y;
        }

        public bool IsNearerThan(Pos other)
        {
            return Dist < other.Dist;
            //return -Constants.Tolerance < other.Dist - Dist;
        }

        public override bool Equals(object obj)
        {
            return obj is Pos pos &&
                   Math.Abs(X - pos.X) < Constants.Tolerance &&
                   Math.Abs(Y - pos.Y) < Constants.Tolerance;
        }

        public override int GetHashCode()
        {
            var hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }

        public Pos Move(string dir)
        {
            var newx = X + Constants.Xd[dir];
            var newy = Y + Constants.Yd[dir];
            return new Pos(newx, newy, Moved+1);
        }
        
        public override string ToString()
        {
            return $"{X:0.00}, {Y:0.00}";
        }
    }

    class Constants
    {
        public static readonly string[] Dirs = {"ne", "n", "nw", "sw", "s", "se"};
        public static readonly Dictionary<string, double> Xd = new Dictionary<string, double>
        {
            {"ne", Math.Sin(Math.PI/3) },
            {"n", 0 },
            {"nw", -Math.Sin(Math.PI/3) },
            {"sw",  -Math.Sin(Math.PI/3)},
            {"s", 0 },
            {"se", Math.Sin(Math.PI/3) }
        };
        public static readonly Dictionary<string, double> Yd = new Dictionary<string, double>
        {
            {"ne", Math.Cos(Math.PI/3) },
            {"n", 1 },
            {"nw", Math.Cos(Math.PI/3) },
            {"sw",  -Math.Cos(Math.PI/3)},
            {"s", -1 },
            {"se", -Math.Cos(Math.PI/3) }
        };

        public const double Tolerance = 1e-6;
    }
}
