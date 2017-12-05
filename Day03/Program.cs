using System;
using System.Collections.Generic;
using System.Linq;

namespace Day03
{
    class Program
    {
        static void Main(string[] args)
        {
            string line;
            while (!string.IsNullOrEmpty(line = Console.ReadLine()))
            {
                var result = GetResult(line);
                Console.WriteLine(result);
            }
        }

        private static long GetResult(string line)
        {
            long N = long.Parse(line);

            long[] xs = { 1, 0, -1, 0 };
            long[] ys = { 0, 1, 0, -1 };
            long dir = 0;
            long x = 0, y = 0;
            long step = 1;
            long n = 1;
            while (n < N)
            {
                long dx = xs[dir], dy = ys[dir];
                long length = Math.Min(step / 2, N - n);
                n += length;
                x += length * dx;
                y += length * dy;
                step++;
                dir = (dir + 1) % 4;
            }
            long m = Math.Abs(x) + Math.Abs(y);
            return m;
        }
    }

    public struct Point
    {
        public long x;
        public long y;
        public override string ToString()
        {
            return $"{x},{y}";
        }

        public bool IsAdjacent(Point rhs)
        {
            var isAdjacent = Math.Abs(rhs.x - x) <= 1 && Math.Abs(rhs.y - y) <= 1;
            return isAdjacent;
        }
    }
}
