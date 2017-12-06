using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day01
{
    class Program
    {
        static void Main(string[] args)
        {
            string input;
            while((input = Console.ReadLine()) != null)
            {
                Process(input);
            }
        }

        private static void Process(string input)
        {
            int[] digits = input.Select(i => int.Parse(i.ToString())).ToArray();
            int[] digitsThatMatchNext = digits.Where((d, i) => d == digits[(i + (digits.Length / 2)) % digits.Length]).ToArray();
            int sumOfDigitsThatMatchNext = digitsThatMatchNext.Sum();
            Console.WriteLine(sumOfDigitsThatMatchNext);
        }
    }
}
