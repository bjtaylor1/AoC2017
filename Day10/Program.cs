﻿using System;
using System.Linq;
using System.Text;

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
                byte[] list = Enumerable.Range(0, count).Select(i => (byte)i).ToArray();
                int curPos = 0;
                int skipSize = 0;
                int[] lengths = line.Trim().Select(c => (int)c).Concat(new[] { 17, 31, 73, 47, 23 }).ToArray();
                for (int round = 0; round < 64; round++)
                {
                    foreach (var length in lengths)
                    {
                        byte[] valuesToReverse = new byte[length];
                        for (var i = 0; i < length; i++)
                        {
                            valuesToReverse[i] = list[(curPos + i) % list.Length];
                        }
                        for (var i = 0; i < length; i++)
                        {
                            list[(curPos + i) % list.Length] = valuesToReverse[length - i - 1];
                        }
                        curPos += (length + skipSize++);
                    }
                }
                var denseHash = list.Select((b, i) => new { b, i })
                    .GroupBy(a => a.i / 16)
                    .Select(g => g.Select(a => a.b).Aggregate((b1, b2) => (byte)(b1 ^ b2))).ToArray();

                var knotHash = denseHash.Aggregate(new StringBuilder(), (sb, b) => sb.Append($"{b:x}".PadLeft(2, '0'))).ToString();
                Console.WriteLine(knotHash);
            }
        }

        
    }
}
