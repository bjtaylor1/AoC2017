using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Day07
{
    class App
    {
        static void Main(string[] args)
        {
            string line;
            var programs = new ConcurrentDictionary<string, Program>();
            var pattern = new Regex(@"^(\w+) \((\d+)\)(?: -> (.+))?$");
            while (!string.IsNullOrEmpty(line = Console.ReadLine()))
            {
                var match = pattern.Match(line);
                if (!match.Success) throw new InvalidOperationException("Not matched");
                var progName = match.Groups[1].Value;
                var weight = int.Parse(match.Groups[2].Value);
                var prog = programs.GetOrAdd(progName, s => new Program(s));
                prog.Weight = weight;
                if (match.Groups[3].Success)
                {
                    var progsAboveString = match.Groups[3].Value;
                    var progsAbove = progsAboveString.Split(',')
                        .Select(s => s.Trim())
                        .Select(s => new Program(s))
                        .ToArray();
                    foreach (var progAbove in progsAbove)
                    {
                        programs.GetOrAdd(progAbove.Name, progAbove).IsOnTopOfAnother = true;
                    }
                }
            }
            var atBottom = programs.Values.Single(p => !p.IsOnTopOfAnother).Name.ToLower();
            Console.WriteLine(atBottom);
        }
    }

    class Program
    {
        public Program(string name)
        {
            Name = name;
        }

        public int? Weight { get; set; }
        public string Name { get; }

        public override bool Equals(object obj)
        {
            return obj is Program program &&
                   Name == program.Name;
        }

        public override int GetHashCode()
        {
            return 539060726 + EqualityComparer<string>.Default.GetHashCode(Name);
        }

        public bool IsOnTopOfAnother { get; set; }

        public override string ToString()
        {
            return $"{(IsOnTopOfAnother ? Name : Name.ToUpper())} ({Weight})";
        }
    }
}
