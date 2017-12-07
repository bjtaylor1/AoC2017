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
                        var existingProgAbove = programs.GetOrAdd(progAbove.Name, progAbove);
                        existingProgAbove.IsOnTopOfAnother = true;
                        prog.ProgramsAbove.Add(existingProgAbove);
                        existingProgAbove.Parent = prog;
                    }
                }
            }

            var firstUnbalanced = programs.Values.OrderByDescending(p => p.Height)
                .First(p => p.IsUnbalanced());
            var unbalancers = firstUnbalanced.ProgramsAbove
                .GroupBy(p => p.WeightAbove).ToArray();
            var programWithIncorrectWeight = unbalancers
                .First(g => g.Count() == 1).Single();
            var correctWeightedStack = unbalancers.First(g => g.Key != programWithIncorrectWeight.WeightAbove).First();
            var weightItShouldBe = programWithIncorrectWeight.Weight + 
                                    correctWeightedStack.WeightAbove -
                                   programWithIncorrectWeight.WeightAbove;
            Console.WriteLine($"{programWithIncorrectWeight} should be {weightItShouldBe}");
        }
    }

    class Program
    {
        public Program(string name)
        {
            Name = name;
            weightAbove = new Lazy<int>(GetTotalWeightAbove);
            height = new Lazy<int>(GetHeight);
        }

        private int GetHeight()
        {
            return Parent?.GetHeight() + 1 ?? 0;
        }

        private readonly Lazy<int> weightAbove;
        private readonly Lazy<int> height;
        public int WeightAbove => weightAbove.Value;
        public int Height => height.Value;

        private int GetTotalWeightAbove()
        {
            var totalWeightAbove = ProgramsAbove.Sum(p => p.WeightAbove ) + Weight ?? 0;
            return totalWeightAbove;
        }

        public int? Weight { get; set; }
        public string Name { get; }
        public List<Program> ProgramsAbove { get; } = new List<Program>();

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
        public Program Parent { get; set; }

        public override string ToString()
        {
            return $"{(IsOnTopOfAnother ? Name : Name.ToUpper())} ({WeightAbove})";
        }

        public bool IsUnbalanced()
        {
            var isUnbalanced = ProgramsAbove.Select(p => p.WeightAbove).Distinct().Count() > 1;
            return isUnbalanced;
        }
    }
}
