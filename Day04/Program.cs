using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Day04
{
    class Program
    {
        static void Main(string[] args)
        {
            string line;
            int valid = 0;
            while (!string.IsNullOrEmpty(line = Console.ReadLine()))
            {
                if (IsValid(line))
                {
                    Console.WriteLine("...valid");
                    valid++;
                }
                else
                {
                    Console.WriteLine("...invalid");
                }
            }
            Console.WriteLine(valid);
        }

        static bool IsValid(string line)
        {
            var words = Regex.Split(line.Trim(), @"\s+");
            var isValid = words.Distinct(AnagramEqualityComparer.Instance).Count() == words.Length;
            return isValid;
        }
    }

    public class AnagramEqualityComparer : IEqualityComparer<string>
    {
        private AnagramEqualityComparer()
        {
            
        }
        public static AnagramEqualityComparer Instance = new AnagramEqualityComparer();
        public bool Equals(string x, string y)
        {
            return string.Equals(Order(x), Order(y));
        }

        public int GetHashCode(string obj)
        {
            return Order(obj).GetHashCode();
        }

        private static string Order(string input)
        {
            var ordered = new string(input.ToCharArray().OrderBy(c => c).ToArray());
            return ordered;
        }
    }
}
