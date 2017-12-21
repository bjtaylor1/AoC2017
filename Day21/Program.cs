using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day21
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = new List<string>();
            string line;
            while (!string.IsNullOrEmpty(line = Console.ReadLine()))
            {
                lines.Add(line);
            }
            var pixels = lines.SelectMany((l, y) => l.Select((c, x) => new Pixel(c, new Position(x, y)))).ToArray();
            var initialGroup = new PixelGroup(pixels);
            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine();
                Console.WriteLine(initialGroup.Rotate(i));
            }
        }
    }

    public class PixelGroup
    {
        public PixelGroup(Pixel[] pixels)
        {
            Pixels = pixels;
        }

        public Pixel[] Pixels { get; }

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
            return string.Join(Environment.NewLine,
                Sort().Select(line => new string(line.Select(p => p.Fill).ToArray())));
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
    }
}
