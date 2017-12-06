using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day06
{
    class Program
    {
        static void Main(string[] args)
        {
            string line;
            while (!string.IsNullOrEmpty(line = Console.ReadLine()))
            {
                var configCurrent = new Config(line);
                var configs = new HashSet<Config>();
                int cycles = 0;
                do
                {
                    configs.Add(configCurrent);
                    configCurrent = configCurrent.Redistribute();
                    cycles++;
                    if (args.Contains("-v"))
                    {
                        Console.WriteLine(configCurrent);
                    }
                } while (!configs.Contains(configCurrent));
                Console.WriteLine(cycles);
            }
        }
    }

    class Config
    {
        public IReadOnlyCollection<byte> Memory { get; }

        public Config(string line) : this(Regex.Split(line.Trim(), @"\s+").Select(byte.Parse).ToArray())
        {
        }

        private Config(byte[] memory)
        {
            Memory = memory;
        }

        public Config Redistribute()
        {
            var memory = Memory.ToArray();
            var blocksToDistribute = memory.Max();
            var indexWithMostBlocks = Array.FindIndex(memory.ToArray(), b => b == blocksToDistribute);
            memory[indexWithMostBlocks] = 0;
            for (int i = 1; i <= blocksToDistribute; i++)
            {
                var pos = (indexWithMostBlocks + i) % Memory.Count;
                memory[pos]++;
            }
            return new Config(memory);
        }

        public override string ToString()
        {
            return string.Join(" ", Memory);
        }

        public override bool Equals(object obj)
        {
            return obj is Config config && Memory.SequenceEqual(config.Memory);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                if (Memory == null)
                {
                    return 0;
                }
                var hashCode = Memory.Aggregate(17, (current, element) => current * 31 + element.GetHashCode());
                return hashCode;
            }
        }
    }
    
}
