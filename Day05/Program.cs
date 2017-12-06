using System;
using System.Collections.Generic;
using System.Linq;

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
                if (instructionsList[currentInstruction] >= 3)
                {
                    instructionsList[currentInstruction]--;
                }
                else
                {
                    instructionsList[currentInstruction]++;
                }
                currentInstruction += nextInstruction;
                steps++;
                if(args.Contains("-v"))
                {
                    Console.WriteLine(string.Join(" ", instructionsList.Select((i,n) => n == currentInstruction ? $"({i})" : $" {i} ").ToArray()));
                }
            }

            Console.WriteLine(steps);
        }
    }
}
