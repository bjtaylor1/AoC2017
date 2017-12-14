using System;
using System.Linq;
using System.Text;

namespace Helpers
{
    public class Helper
    {
        public static string GetKnotHash(string input)
        {
            byte[] knotHashBytes = GetKnotHashBytes(input);
            var knotHash = knotHashBytes.Aggregate(new StringBuilder(), (sb, b) => sb.Append($"{b:x}".PadLeft(2, '0'))).ToString();
            return knotHash;
        }

        public static byte[] GetKnotHashBytes(string input)
        {
            const int count = 256;
            byte[] list = Enumerable.Range(0, count).Select(i => (byte)i).ToArray();
            int curPos = 0;
            int skipSize = 0;
            int[] lengths = input.Trim().Select(c => (int)c).Concat(new[] { 17, 31, 73, 47, 23 }).ToArray();
            for (int round = 0; round < 64; round++)
            {
                foreach (var length in lengths)
                {
                    byte[] valuesToReverse = Enumerable.Range(0, length)
                        .Select(i => list[(curPos + i) % list.Length]).Reverse().ToArray();
                    Array.Copy(valuesToReverse, 0, list, curPos, Math.Min(length, list.Length - curPos));
                    if (list.Length - curPos < length)
                    {
                        Array.Copy(valuesToReverse, list.Length - curPos, list, 0, length - (list.Length - curPos));
                    }
                    curPos = (curPos + length + skipSize++) % list.Length;
                }
            }
            var denseHash = list.Select((b, i) => new { b, i })
                .GroupBy(a => a.i / 16)
                .Select(g => g.Select(a => a.b).Aggregate((b1, b2) => (byte)(b1 ^ b2))).ToArray();
            return denseHash;
        }
    }
}
