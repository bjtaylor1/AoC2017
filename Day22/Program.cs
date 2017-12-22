using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day22
{
    class Program
    {
        static void Main(string[] args)
        {
            string line;
            var lines = new List<string>();
            while (!string.IsNullOrEmpty(line = Console.ReadLine()))
            {
                lines.Add(line);
            }
            (int X, int Y)[] dirs =
            {
                (0,-1),
                (1, 0),
                (0, 1),
                (-1, 0)
            };
            var nodes = new ConcurrentDictionary<(int X, int Y), char>(
                lines.SelectMany((l, Y) => l.Select((C, X) => new KeyValuePair<(int X, int Y), char>((X, Y), C))));

            (int X, int Y) curPos = (nodes.Keys.Max(n => n.X) / 2, nodes.Keys.Max(n => n.Y / 2));
            int curDir = 0;
            const int itMax =
#if DEBUG
                70;
#else
                10000;
#endif
            int infections = 0;
            for(int it = 0; it < itMax; it++)
            {
                char curNode = nodes.GetOrAdd(curPos, '.');
                var dDir = curNode == '#' ? 1 : 3;
                curDir = (curDir + dDir) % 4;
                if (curNode == '#')
                    nodes[curPos] = '.';
                else
                {
                    nodes[curPos] = '#';
                    infections++;
                }
                nodes[curPos] = curNode == '#' ? '.' : '#';
                curPos = (curPos.X + dirs[curDir].X, curPos.Y + dirs[curDir].Y);


#if DEBUG
                for(int y = nodes.Keys.Min(n => n.Y); y <= nodes.Keys.Max(n => n.Y); y++)
                {
                    for (int x = nodes.Keys.Min(n => n.X); x <= nodes.Keys.Max(n => n.X); x++)
                    {
                        char c;
                        if (!nodes.TryGetValue((x, y), out c)) c = ' ';
                        string s = Equals(curPos, (x, y)) ? $"[{c}]" : $" {c} ";
                        Console.Write(s);
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
#endif
            }

            Console.WriteLine(infections);

        }


    }
}
