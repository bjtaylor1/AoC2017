using System;
using System.Linq;

namespace Day14
{
    class Program
    {
        static void Main(string[] args)
        {
            string line;
            bool[] BitsSet(int i) => Enumerable.Range(0,4).Reverse().Select(m => (i >> m)%2 > 0).ToArray();
            var bitsSet = Enumerable.Range(0, 0x10).ToDictionary(i => $"{i:x}"[0], BitsSet);
            while (!string.IsNullOrEmpty(line = Console.ReadLine()))
            {
                var keyStrings = Enumerable.Range(0, 128).Select(i => $"{line}-{i}").ToArray();
                var hashes = keyStrings.Select(Day10.Program.GetKnotHash).ToArray();
                var squares = hashes.Select(h => h.SelectMany(c => bitsSet[c]).ToArray()).ToArray();

                bool FindRegionStart(out int i, out int j)
                {
                    for (i = 0; i < squares.Length; i++)
                    {
                        for (j = 0; j < squares[i].Length; j++)
                        {
                            if (squares[i][j]) return true;
                        }
                    }
                    j = -1;
                    return false;
                }

                void ConsumeRegion(int i, int j)
                {
                    squares[i][j] = false;
                    var moves = new (int di, int dj)[] {(i + 1, j), (i - 1, j), (i, j - 1), (i, j + 1)}
                        .Where(t => t.di >= 0 && t.di < squares.Length &&
                                    t.dj >= 0 && t.dj < squares.Length).ToArray();
                    foreach (var move in moves)
                    {
                        if (squares[move.di][move.dj])
                        {
                            ConsumeRegion(move.di, move.dj);
                        }
                    }
                }

                int numRegions = 0;
                while (FindRegionStart(out int i, out int j))
                {
                    ConsumeRegion(i, j);
                }

                Console.Out.WriteLine($"\n{numRegions}");
            }
        }
    }
}
