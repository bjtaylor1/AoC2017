using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Day24
{
    class Program
    {
        static void Main(string[] args)
        {
            var components = new List<Component>();
            string line;
            while(!string.IsNullOrEmpty(line= Console.ReadLine()))
            {
                components.Add(new Component(line.Split('/').Select(int.Parse).ToArray()));
            }

            var bridge = new Stack<int[]>();
            int maxStrength = 0;
            var maxStrengthByLength = new Dictionary<int, int>();
            void AddToBridge()
            {
                var connectionRequired = bridge.FirstOrDefault()?.Last() ?? 0;
                foreach(var component in components.Where(p => p.Contains(connectionRequired)).ToArray())
                {
                    components.Remove(component);
                    bridge.Push(component.Value.OrderByDescending(p => p == connectionRequired).ToArray());
                    var totalStrength = bridge.Sum(b => b.Sum());
                    var length = bridge.Count();
                    if (!maxStrengthByLength.TryGetValue(length, out int maxLength)) maxLength = 0;
                    maxStrengthByLength[length] = Math.Max(maxLength, totalStrength);
                    //var maxPossibleStrength = totalStrength + components.Sum(c => c.Value.Sum());
                    Debug.WriteLine($"{string.Join("--", bridge.Reverse().Select(b => string.Join("/", b)))} = {totalStrength}");
                    //if (maxPossibleStrength > maxStrength)
                    {
                        AddToBridge();
                    }
                    bridge.Pop();
                    components.Add(component);
                }
            }
            AddToBridge();
            Console.WriteLine(maxStrengthByLength[maxStrengthByLength.Keys.Max()]);
        }
        
    }
    
    public class Component
    {
        public int[] Value { get; }
        public bool Contains(int i) => Value.Contains(i);
        public Component(int[] v)
        {
            this.Value = v;
        }
        public override string ToString()
        {
            return string.Join("/", Value);
        }
    }
}
