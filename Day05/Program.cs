using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day05
{
    class Program
    {
        static void Main(string[] args)
        {
            string line;
            var instructionsList = new List<int>();
            while (!string.IsNullOrEmpty(line = Console.ReadLine()))
            {
                instructionsList.Add(int.Parse(line.Trim()));
            }

            int currentInstruction = 0;
            int steps = 0;
            while (currentInstruction >= 0 && currentInstruction < instructionsList.Count)
            {
                var nextInstruction = instructionsList[currentInstruction];
                instructionsList[currentInstruction]++;
                currentInstruction += nextInstruction;
                steps++;
                //Console.WriteLine(string.Join(" ", instructionsList.Select((i,n) => n == currentInstruction ? $"({i})" : $" {i} ").ToArray()));
            }

            Console.WriteLine(steps);
        }

        
    }
}
