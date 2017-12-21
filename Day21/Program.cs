using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Day21
{
    class Program
    {
        static void Main(string[] args)
        {
            var rules = new List<(PixelGroup Source, PixelGroup Target)>();
#if DEBUG
            foreach(var line in File.ReadAllLines(@"..\..\input.txt"))
#else
            string line;
            while (!string.IsNullOrEmpty(line = Console.ReadLine()))
#endif
            {
                var parts = line.Split(new[] {"=>"}, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim().Split('/'))
                    .Select(PixelGroup.FromPattern)
                    .ToArray();
                rules.Add((parts[0], parts[1]));
            }

            foreach (var rule in rules)
            {
                var length = rule.Source.SideLength;
                Console.Out.WriteLine($"{length}\t{rule.Source.CountOn}\t{rule.Target.CountOn}");
            }
            
            var pattern = new[]
            {
                ".#.",
                "..#",
                "###"
            };
            
            var pic = PixelGroup.FromPattern(pattern);
            var sw = Stopwatch.StartNew();
            var ruleCache = new ConcurrentDictionary<PixelGroup, (PixelGroup Source, PixelGroup Target)>();
            for (int it = 0; it < 18; it++)
            {
                var sideLength = pic.SideLength;
                var side = new[] {2, 3}.First(i => sideLength % i == 0);
                var split = pic.Split(side);
                var output = split.Select(p => (p.Position, ruleCache.GetOrAdd(p.Segment, p1 => rules.First(r => p1.Match(r.Source))).Target)).ToArray();
                pic = PixelGroup.Combine(output);
                Console.Out.WriteLine($"{it}, {pic.SideLength}");
            }
            Console.Out.WriteLine(pic.CountOn);
            sw.Stop();
            Console.Out.WriteLine(sw.Elapsed);
        }
    }

    public class PixelGroup
    {
        public static PixelGroup FromPattern(IEnumerable<string> pattern)
        {
            return new PixelGroup(pattern.SelectMany((l, y) => l.Select((c, x) => new Pixel(c, new Position(x, y)))));
        }

        public PixelGroup(IEnumerable<Pixel> pixels)
        {
            Pixels = pixels.OrderBy(p => p.Position.Y).ThenBy(p => p.Position.X).ToArray();
        }

        public int SideLength => (int) Math.Sqrt(Pixels.Length);
        public Pixel[] Pixels { get; }

        public int CountOn => Pixels.Count(p => p.Fill == '#');

        public PixelGroup Rotate(int times)
        {
            if (times == 0) return this;
            var newPixels = Pixels.Select(p => new Pixel(p.Fill, p.Position.Rotate(times))).ToArray();
            var retval = new PixelGroup(newPixels).Normalize();
            return retval;
        }

        public PixelGroup Flip(bool flip)
        {
            return flip
                ? this
                : new PixelGroup(Pixels.Select(p => new Pixel(p.Fill, p.Position.Flip())).ToArray()).Normalize();
        }

        public PixelGroup Normalize()
        {
            var offset = new Position(Pixels.Min(p => p.Position.X), Pixels.Min(p => p.Position.Y));
            var newPixels = Pixels.Select(p => new Pixel(p.Fill, p.Position - offset)).ToArray();
            var retval = new PixelGroup(newPixels);
            return retval;
        }

        public Pixel[][] Sort()
        {
            var sortedPixels = Pixels.GroupBy(p => p.Position.Y).OrderBy(g => g.Key).Select(row => row.OrderBy(p => p.Position.X).ToArray()).ToArray();
            return sortedPixels;
        }

        public override string ToString()
        {
            var sort = Sort();
            var join = string.Join(Environment.NewLine,
                sort.Select(line => new string(line.Select(p => p.Fill).ToArray())));
            return join;
        }

        public (Position Position, PixelGroup Segment)[] Split(int n)
        {
            var groups = Pixels.GroupBy(p => new Position(p.Position.X / n, p.Position.Y / n))
                .Select(g => (g.Key, new PixelGroup(g.ToArray()).Normalize())).ToArray();
            return groups;
        }

        public static PixelGroup Combine((Position Position, PixelGroup PixelGroup)[] segments)
        {
            var segmentSize = segments.Select(s => s.PixelGroup.Pixels.Length).Distinct().Single();
            var segmentLength = (int)Math.Sqrt(segmentSize);
            var newPixels = segments.SelectMany(seg =>
                seg.PixelGroup.Normalize().Pixels.Select(p => new Pixel(p.Fill, p.Position + seg.Position * segmentLength))).ToArray();
            var newGroup = new PixelGroup(newPixels).Normalize();
            return newGroup;
        }

        public bool Match(PixelGroup rule)
        {
            if (Pixels.Length != rule.Pixels.Length) return false;
            foreach (var flip in new[] {false, true})
            {
                foreach (var rotate in new[] {0, 1, 2, 3})
                {
                    var transformed = Flip(flip).Rotate(rotate).Normalize();
                    if (transformed.Equals(rule))
                        return true;
                }
            }
            return false;
        }

        public override bool Equals(object obj)
        {
            return obj is PixelGroup group &&
                   SideLength == group.SideLength &&
                   Pixels.SequenceEqual(group.Pixels);
        }

        public override int GetHashCode()
        {
            var hashCode = -1469460586;
            hashCode = hashCode * -1521134295 + SideLength.GetHashCode();
            hashCode = Pixels.Aggregate(hashCode, (h, p) => h * -1521134295 + p.GetHashCode());
            return hashCode;
        }
    }

    public class Pixel
    {
        public Pixel(char fill, Position position)
        {
            Fill = fill;
            Position = position;
        }

        public char Fill { get; }
        public Position Position { get; }

        protected bool Equals(Pixel other)
        {
            return Fill == other.Fill && Equals(Position, other.Position);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Pixel) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Fill.GetHashCode() * 397) ^ (Position != null ? Position.GetHashCode() : 0);
            }
        }
    }

    public class Position
    {
        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; }
        public int Y { get; }

        protected bool Equals(Position other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Position) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }

        public override string ToString()
        {
            return $"{X}, {Y}";
        }

        private readonly int[][] rotationMatrices =
        {
            new[] {0, 0, 0, 0},
            new[] {0, -1, 1, 0},
            new[] {-1, 0, 0, -1},
            new[] {0, 1, -1, 0}
        };
        public Position Rotate(int times)
        {
            var matrix = rotationMatrices[times];
            var newPos = new Position(matrix[0] * X + matrix[1] * Y, matrix[2] * X + matrix[3] * Y);
            return newPos;
        }

        public Position Flip()
        {
            var flipped = new Position(-X, Y);
            return flipped;
        }

        public static Position operator -(Position pos, Position other)
        {
            return new Position(pos.X - other.X, pos.Y - other.Y);
        }

        public static Position operator +(Position pos, Position other)
        {
            return new Position(pos.X + other.X, pos.Y + other.Y);
        }

        public static Position operator *(Position pos, Position other)
        {
            return new Position(pos.X * other.X, pos.Y * other.Y);
        }

        public static Position operator *(Position pos, int other)
        {
            return new Position(pos.X * other, pos.Y * other);
        }        
    }
}
