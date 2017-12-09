using System;
using System.IO;

namespace Day09
{
    class Program
    {
        static void Main(string[] args)
        {
            string line;
            while(!string.IsNullOrEmpty(line = Console.ReadLine()))
            {
                using (var stringReader = new StringReader(line))
                {
                    int group = 0;
                    int groups = 0;
                    int totalScore = 0;
                    int charInt;
                    bool inGarbage = false;
                    int garbageChars = 0;
                    while((charInt = stringReader.Read()) != -1)
                    {
                        char nextChar = (char)charInt;
                        if (nextChar == '!')
                            stringReader.Read();
                        else if (inGarbage)
                        {
                            if (nextChar == '>') inGarbage = false;
                            else garbageChars++;
                        }
                        else if (nextChar == '<') inGarbage = true;
                        else if(nextChar == '{')
                        {
                            group++;
                            groups++;
                            totalScore += group;
                        }
                        else if(nextChar == '}')
                        {
                            if (--group < 0) throw new InvalidOperationException("Closed more groups than are open!");
                        }
                    }
                    Console.WriteLine($"Groups: {groups}, score: {totalScore}, garbage: {garbageChars}");
                }
            }
        }
    }
}
