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
            long n = long.Parse(line);
            Point currentPoint = new Point(0, 0);
            Vector currentDir = new Vector(0);
            var pointNums = new Dictionary<Point, int> { { currentPoint, 1 } };
            while(true)
            {
                currentPoint = currentPoint + currentDir;
                var num = pointNums.Where(k => k.Key.IsAdjacent(currentPoint)).Sum(k => k.Value);
                if (num > n)
                {
                    return num;
                }
                pointNums.Add(currentPoint, num);
                if (!pointNums.ContainsKey(currentPoint + currentDir.Left()))
                {
                    currentDir = currentDir.Left();
                }
            }
        }
    }
}
