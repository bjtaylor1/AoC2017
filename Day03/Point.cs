using System;

namespace Day03
{
    public class Point
    {
        public readonly long x;
        public readonly long y;

        public Point(long x, long y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return $"{x},{y}";
        }

        public bool IsAdjacent(Point rhs)
        {
            var isAdjacent = Math.Abs(rhs.x - x) <= 1 && Math.Abs(rhs.y - y) <= 1;
            return isAdjacent;
        }

        public override bool Equals(object obj)
        {
            return obj is Point point &&
                   x == point.x &&
                   y == point.y;
        }

        public override int GetHashCode()
        {
            var hashCode = 1502939027;
            hashCode = hashCode * -1521134295 + x.GetHashCode();
            hashCode = hashCode * -1521134295 + y.GetHashCode();
            return hashCode;
        }

        public static Point operator +(Point p, Vector v)
        {
            return new Point(p.x + v.X, p.y + v.Y);
        }
    }
}