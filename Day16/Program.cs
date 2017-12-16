using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day16
{
    class Program
    {
        private const string startPos = "abcdefghijklmnop";

        static void Main(string[] args)
        {
            string line;
            while(!string.IsNullOrEmpty(line = Console.ReadLine()))
            {
                string[] moves = line.Split(',');
                var programs = startPos.ToArray();
                const int N = (int)1e9;
                for (int n = 0; n < N; )
                {
                    foreach (var m in moves)
                    {
                        void DoExchange(int a, int b) { var mem = programs[a]; programs[a] = programs[b]; programs[b] = mem; }
                        if (IsSpin(m, out int x))
                        {
                            programs = Enumerable.Range(0, programs.Length).Select(i => programs[(i - x + programs.Length) % programs.Length]).ToArray();
                        }
                        else if (IsExchange(m, out int a, out int b))
                        {
                            DoExchange(a, b);
                        }
                        else if (IsPartner(m, out char _a, out char _b))
                        {
                            DoExchange(Array.IndexOf(programs, _a), Array.IndexOf(programs, _b));
                        }
                        else throw new InvalidOperationException($"Invalid move {m}");
                    }
                    n++;
                    if (new string(programs) == startPos)
                    {
                        n *= (N / n);
                    }
                }
                Console.WriteLine($"\n{new string(programs)}");
            }
        }

        private static bool IsPartner(string m, out char a, out char b)
        {
            var match = Regex.Match(m, @"p(\w)\/(\w+)");
            a = match.Groups[1]?.Value?.Single() ?? ' ';
            b = match.Groups[2]?.Value?.Single() ?? ' ';
            return match.Success;
        }

        private static bool IsExchange(string m, out int a, out int b)
        {
            var match = Regex.Match(m, @"x(\d+)\/(\d+)");
            a = int.Parse($"0{match.Groups[1]?.Value}");
            b = int.Parse($"0{match.Groups[2]?.Value}");
            return match.Success;
        }

        private static bool IsSpin(string m, out int x)
        {
            var match = Regex.Match(m, @"s(\d+)");
            x = int.Parse($"0{match.Groups[1]?.Value}");
            return match.Success;
        }
    }
}
