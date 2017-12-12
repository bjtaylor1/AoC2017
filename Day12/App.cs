using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day12
{
    class App
    {
        static void Main(string[] args)
        {
            var allPrograms = new ConcurrentDictionary<int, Program>();
            string line;
            while (!string.IsNullOrEmpty(line = Console.ReadLine()))
            {
                var parts = line.Split(new[] {"<->", ", "}, StringSplitOptions.RemoveEmptyEntries);
                var programs = parts
                    .Select(int.Parse)
                    .Select(i => allPrograms.GetOrAdd(i, new Program(i)))
                    .OrderByDescending(p => p.Id)
                    .ToArray();
                programs.First().ConnectTo(
                    programs.Concat(programs.SelectMany(p => p.Network())).Distinct().ToArray());
                Console.WriteLine();
            }
            Console.WriteLine(allPrograms[0].ConnectedTo.Count);
        }
    }

    public class Program
    {
        public int Id { get; set; }

        public HashSet<Program> ConnectedTo = new HashSet<Program>();

        public Program(int id)
        {
            Id = id;
        }

        public Program[] Network()
        {
            return ConnectedTo
                .Concat(ConnectedTo.Where(c => c.Id < Id).SelectMany(c => c.Network()))
                .Distinct().ToArray();
        }
        
        public void ConnectTo(Program[] others)
        {
            foreach (var program in others)
            {
                ConnectedTo.Add(program);
                program.ConnectedTo.Add(this);
                if (program.Id < Id)
                {
                    program.ConnectTo(others);
                }
            }
        }

        public override string ToString()
        {
            return $"{Id} ({string.Join(", ", ConnectedTo.Select(c => c.Id.ToString()))})";
        }

        public override bool Equals(object obj)
        {
            var program = obj as Program;
            return program != null &&
                   Id == program.Id;
        }

        public override int GetHashCode()
        {
            return 2108858624 + Id.GetHashCode();
        }
    }
}
