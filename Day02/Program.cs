using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day02
{
    class Program
    {
        static void Main(string[] args)
        {
            string line;
            int tot = 0;
            while(!string.IsNullOrEmpty(line = Console.ReadLine()))
            {
                string[] parts = Regex.Split(line.Trim(), @"\s+");
                int[] numbers = parts.Select(int.Parse).ToArray();
#if DAY2
                int checksum = GetChecksum(numbers);
#else
                int checksum = numbers.Max() - numbers.Min();
#endif
                tot += checksum;
            }
            Console.WriteLine(tot);
        }

        static int GetChecksum(int[] numbers)
        {
            foreach(var number in numbers)
            {
                for(int factor = number * 2; factor <= numbers.Max(); factor += number)
                {
                    if(numbers.Contains(factor))
                    {
                        return factor / number;
                    }
                }
            }
            throw new InvalidOperationException("No divisable pairs!");
        }
    }
}
