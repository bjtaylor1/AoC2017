using System;
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

            //while (!string.IsNullOrEmpty(line = Console.ReadLine()))
            while (!string.IsNullOrEmpty(line = File.ReadAllText(@"..\..\input.txt")))
            {
                var moves = line.Trim().Split(',');
                var allPos = new List<Pos>();
                var endPos = moves.Aggregate(StartPos, (p, m) =>
                {
                    var pos = p.Move(m);
                    var absPos = new Pos(Math.Abs(pos.X), Math.Abs(pos.Y), 0);
                    if (!allPos.Contains(absPos)) allPos.Add(absPos);
                    return pos;
                });
                var curPos = new Pos(endPos.X, endPos.Y, 0);

                int closestMoved = GetStepsF(curPos);
                //int furthestEver = 0;
                //for (var index = 0; index < allPos.Count; index++)
                //{
                //    if(index % 10 == 0) Console.WriteLine($"\r{(double)index / allPos.Count:P}");
                //    var pos = allPos[index];
                //    var steps = GetSteps(pos);
                //    if (steps > furthestEver) furthestEver = steps;
                //}
                Console.WriteLine(closestMoved);
            }
        }

        private static int GetStepsF(Pos curPos)
        {
            curPos.GetHighestFactor(out Pos factor, out int f);
            int stepsf = f * GetSteps(factor);
            return stepsf;
        }

        private static int GetSteps(Pos curPos)
        {
            var positions = new List<Pos> { curPos };
            Pos closest;
            while (!StartPos.Equals(closest = positions.OrderBy(p => p.Dist).First()))
            {
                var newPositions = Constants.Dirs.Select(p => closest.Move(p))
                    .Where(np => !positions.Contains(np))
                    .ToArray();
                positions.AddRange(newPositions);
            }
            var closestMoved = closest.Moved;
            return closestMoved;
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
            Dist = GetDist(x, y);
        }

        public void GetHighestFactor(out Pos factor, out int f)
        {
            factor = this;
            f = 1;
            for (int n = 1; n <= Dist; n++)
            {
                double xd = X / n, yd = Y / n;
                if (IsOnLattice(xd, yd))
                {
                    factor = new Pos(xd, yd, 0);
                    f = n;
                }
            }
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
            return $"{X}, {Y}";
        }

        public static bool IsOnLattice(double x, double y)
        {
            double n = y - x * Math.Tan(Math.PI / 6);
            bool isOnLattice = IsInt(n) && IsInt(GetDist(x, y - n));
            return isOnLattice;
        }

        private static double GetDist(double x, double y)
        {
            return Math.Sqrt(x * x + y * y);
        }

        public static bool IsInt(double x)
        {
            return Math.Abs(x - Math.Round(x)) < Constants.Tolerance;
        }
    }

    class Constants
    {
        public static readonly string[] Dirs = {"ne", "n", "nw", "sw", "s", "se"};
        public static readonly Dictionary<string, double> Xd = new Dictionary<string, double>
        {
            {"ne", Math.Cos(Math.PI/6) },
            {"n", 0 },
            {"nw", -Math.Cos(Math.PI/6) },
            {"sw",  -Math.Cos(Math.PI/6)},
            {"s", 0 },
            {"se", Math.Cos(Math.PI/6) }
        };
        public static readonly Dictionary<string, double> Yd = new Dictionary<string, double>
        {
            {"ne", Math.Sin(Math.PI/6) },
            {"n", 1 },
            {"nw", Math.Sin(Math.PI/6) },
            {"sw",  -Math.Sin(Math.PI/6)},
            {"s", -1 },
            {"se", -Math.Sin(Math.PI/6) }
        };

        public const double Tolerance = 1e-6;
    }
}
