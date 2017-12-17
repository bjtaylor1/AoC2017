using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day17
{
    class Program
    {
        static void Main(string[] args)
        {
            string line;
            while (!string.IsNullOrEmpty(line = Console.ReadLine()))
            {
                int N = int.Parse(line);
                var buffer = new List<int>() { 0 };
                int curPos = 0;
                int indexOfTarget;
                for(int i = 1; -1 == (indexOfTarget = buffer.IndexOf(2017)); i++)
                {
                    curPos = (curPos + N) % buffer.Count;
                    buffer.Insert(++curPos, i);
                    Debug.WriteLine(string.Join(" ", buffer.Select((b, j) => j == curPos ? $"({b})" : $" {b} ")));
                }
                Console.Out.WriteLine(buffer[(indexOfTarget + 1) % buffer.Count]);
            }
        }
    }
}
