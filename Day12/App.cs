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
                var programs = parts.Select(int.Parse).Select(i => allPrograms.GetOrAdd(i, new Program(i))).ToArray();
                programs.First().ConnectTo(programs.Skip(1));
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
        
        public void ConnectTo(IEnumerable<Program> others)
        {
            foreach (var program in others)
            {
                program.ConnectedTo.Add(this);
                ConnectedTo.Add(program);
            }
        }

        public override string ToString()
        {
            return $"{Id} ({string.Join(", ", ConnectedTo)})";
        }
    }
}
