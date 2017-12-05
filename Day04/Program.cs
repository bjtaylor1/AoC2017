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
                    valid++;
                }
            }
            Console.WriteLine(valid);
        }

        static bool IsValid(string line)
        {
            var words = Regex.Split(line.Trim(), @"\s+");
            var isValid = words.Distinct().Count() == words.Length;
            return isValid;
        }
    }
}
