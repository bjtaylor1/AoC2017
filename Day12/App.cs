using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickGraph;

namespace Day12
{
    class App
    {
        static void Main(string[] args)
        {
            string line;
            var connections = new HashSet<Connection>();
            while (!string.IsNullOrEmpty(line = Console.ReadLine()))
            {
                var parts = line.Split(new[] {"<->"}, StringSplitOptions.RemoveEmptyEntries);
                var from = int.Parse(parts[0]);
                var tos = parts[1].Split(new[] {", "}, StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .ToArray();
                foreach (var to in tos)
                {
                    connections.Add(new Connection(from, to));
                    connections.Add(new Connection(to, from));
                }
            }
            HashSet<int> connectedToZero = new HashSet<int>();
            Count(connections, connectedToZero, 0);
            Console.WriteLine(connectedToZero.Count);
        }

        static void Count(HashSet<Connection> allConnections, HashSet<int> connectedTo, int n)
        {
            foreach (var connection in allConnections.Where(c => c.From == n).ToArray())
            {
                if (connectedTo.Add(connection.To))
                {
                    Count(allConnections, connectedTo, connection.To);
                }
            }
        }
    }

    class Connection
    {
        public readonly int From;
        public readonly int To;

        public Connection(int from, int to)
        {
            From = from;
            To = to;
        }

        public override bool Equals(object obj)
        {
            return obj is Connection connection &&
                   From == connection.From &&
                   To == connection.To;
        }

        public override int GetHashCode()
        {
            var hashCode = -1781160927;
            hashCode = hashCode * -1521134295 + From.GetHashCode();
            hashCode = hashCode * -1521134295 + To.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return $"{From} -> {To}";
        }
    }

    public static class Extensions
    {
        public static void AddRange<T>(this HashSet<T> hashSet, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                hashSet.Add(item);
            }
        }
    }
}
