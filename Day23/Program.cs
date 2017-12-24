using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day23
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = new List<string>();
            string line;
            while(!string.IsNullOrEmpty(line = Console.ReadLine()))
            {
                lines.Add(line);
            }
            var registers = new Registers
            {
                ["a"] = 1
            };
            int ParamVal(string param) => int.TryParse(param, out int intVal) ? intVal : registers.GetOrAdd(param, 0);
            int muls = 0;
            int i = 0;
            var commands = new Dictionary<string, Func<string, string, string>>
            {
                {"set", (p1, p2) => $"{p1} = {p2};" },
                {"sub", (p1, p2) => $"{p1} -= {p2};" },
                {"mul", (p1, p2) => $"{p1} *= {p2};" },
                {"jnz", (p1, p2) => $"if({p1} != 0) goto line{(i + int.Parse(p2))};" }
            };
            int count = 0;
            int prevh = -1;
            File.Delete("debug.txt");
            for(i = 0; i < lines.Count; i++)
            {
                var parts = lines[i].Split(' ');
                Console.WriteLine($"line{i}: {commands[parts[0]](parts[1], parts[2])}");
            }
        }
    }

    public class Registers
    {
        private readonly ConcurrentDictionary<string, int> concurrentDictionary = new ConcurrentDictionary<string, int>();
        
        public int this[string s]
        {
            get { return concurrentDictionary[s]; }
            set
            {
                var existingVal = GetOrAdd(s, 0);
                concurrentDictionary[s] = value;
            }
        }

        public string Values => string.Join(",", concurrentDictionary.OrderBy(v => v.Key).Select(v => v.Value));

        public int GetOrAdd(string s, int val)
        {
            return concurrentDictionary.GetOrAdd(s, val);
        }
    }
}
