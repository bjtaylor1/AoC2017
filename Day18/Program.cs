using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Day18
{
    class Program
    {
        static void Main(string[] args)
        {
            string line;
            var lines = new List<string>();
            while (!string.IsNullOrEmpty(line = Console.ReadLine()))
            {
                lines.Add(line);
            }
            var registers = new ConcurrentDictionary<string, long>();

            long? lastSound = null;
            
            long ParamVal(string p) => long.TryParse(p, out var intVal) ? intVal : registers.GetOrAdd(p, 0);
            long i = 0;
            var commands = new Dictionary<string, Action<string, string>>
            {
                {"snd", (p1, p2) => lastSound = ParamVal(p1)},
                {"set", (p1, p2) => registers[p1] = ParamVal(p2)},
                {"add", (p1, p2) => registers[p1] = ParamVal(p1) + ParamVal(p2)},
                {"mul", (p1, p2) => registers[p1] = ParamVal(p1) * ParamVal(p2)},
                {"mod", (p1, p2) => registers[p1] = ParamVal(p1) % ParamVal(p2)},
                {"rcv", (p1, p2) => { if (ParamVal(p1) != 0) throw new FinishedException(); } },
                { "jgz", (p1, p2) => { if (ParamVal(p1) > 0) i += ParamVal(p2) - 1; } }
            };
            try
            {
                for (; i < lines.Count; i++)
                {
                    var parts = lines[(int)i].Split(' ');
                    Array.Resize(ref parts, 3);
                    var cmd = commands[parts[0]];

                    Debug.WriteLine($"{i+1}: {lines[(int)i]}  = {parts[0]} {string.Join(" " , parts.Skip(1).Where(s => s != null).Select(ParamVal))}");
                    cmd(parts[1], parts[2]);
                }
                throw new InvalidOperationException("Reached end!");
            }
            catch (FinishedException)
            {
                Console.WriteLine(lastSound);
            }
        }

/*
 snd X plays a sound with a frequency equal to the value of X.
set X Y sets register X to the value of Y.
add X Y increases register X by the value of Y.
mul X Y sets register X to the result of multiplying the value contained in register X by the value of Y.
mod X Y sets register X to the remainder of dividing the value contained in register X by the value of Y (that is, it sets X to the result of X modulo Y).
rcv X recovers the frequency of the last sound played, but only when the value of X is not zero. (If it is zero, the command does nothing.)
jgz X Y jumps with an offset of the value of Y, but only if the value of X is greater than zero. (An offset of 2 skips the next instruction, an offset of -1 jumps to the previous instruction, and so on.)
 * */
    }

    internal class FinishedException : Exception
    {
    }
}
