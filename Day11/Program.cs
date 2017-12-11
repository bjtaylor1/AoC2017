using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Day11
{
    class Program
    {
        static void Main(string[] args)
        {
            string line;
            while (!string.IsNullOrEmpty(line = Console.ReadLine()))
            {
                var moves = line.Trim().Split(',');
                var startPos = new Pos(0, 0, 0);
                var endPos = moves.Aggregate(startPos, (p, m) => p.Move(m));
                var curPos = new Pos(endPos.X, endPos.Y, 0);
                var positions = new List<Pos> {curPos};
                Pos closest;
                while (!startPos.Equals(closest = positions.OrderBy(p => p.Dist).First()))
                {
                    var newPositions = Constants.Dirs.Select(p => closest.Move(p))
                        .Where(np => !positions.Contains(np))
                        .ToArray();
                    positions.AddRange(newPositions);
                }
                Console.WriteLine(closest.Moved);
            }
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
