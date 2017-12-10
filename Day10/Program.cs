using System;
using System.Linq;

namespace Day10
{
    class Program
    {
        static void Main(string[] args)
        {
            string line;
            int count = args.Length > 0 ? int.Parse(args[0]) : 256;
            while(!string.IsNullOrEmpty(line = Console.ReadLine()))
            {
                int[] list = Enumerable.Range(0, count).ToArray();
                int curPos = 0;
                int skipSize = 0;
                int[] lengths = line.Split(',').Select(s => int.Parse(s.Trim())).ToArray();
                foreach(var length in lengths)
                {
                    int[] valuesToReverse = new int[length];
                    for(var i = 0; i < length; i++)
                    {
                        valuesToReverse[i] = list[(curPos + i) % list.Length];
                    }
                    for(var i = 0; i < length; i++)
                    {
                        list[(curPos + i) % list.Length] = valuesToReverse[length - i - 1];
                    }
                    curPos += (length + skipSize++);
                }
                Console.WriteLine(list[0] * list[1]);
            }
        }
    }
}
