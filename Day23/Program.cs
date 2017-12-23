using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

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
            int i = 0;
            var registers = new ConcurrentDictionary<string, int>();
            int ParamVal(string param) => int.TryParse(param, out int intVal) ? intVal : registers.GetOrAdd(param, 0);
            int muls = 0;
            var commands = new Dictionary<string, Action<string, string>>
            {
                {"set", (p1, p2) => registers[p1] = ParamVal(p2)},
                {"sub", (p1, p2) => registers[p1] = ParamVal(p1) - ParamVal(p2) },
                {"mul", (p1, p2) => { registers[p1] = ParamVal(p1) * ParamVal(p2); muls++; } },
                {"jnz", (p1, p2) => { if(ParamVal(p1) != 0) i += (ParamVal(p2) - 1); } }
            };

            for(i = 0; i < lines.Count; i++)
            {
                var parts = lines[i].Split(' ');
                commands[parts[0]](parts[1], parts[2]);
            }
            Console.WriteLine(muls);
        }
    }
}
