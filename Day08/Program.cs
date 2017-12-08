using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.CSharp;

namespace Day08
{
    class Program
    {
        static void Main(string[] args)
        {
            string line;
            var codeLines = new List<string>();
            while(!string.IsNullOrEmpty(line = Console.ReadLine()))
            {
                codeLines.Add(TranslateCode(line));
            }
            var dynamicInstructions = string.Join("\r\n", codeLines);
            var csc = new CSharpCodeProvider(new Dictionary<string, string>());
            var parameters = new CompilerParameters(new[] { "mscorlib.dll", "System.Core.dll", Assembly.GetExecutingAssembly().Location }, "day08dynamic.dll", true);
            var code  = @"using System;
                    using System.Collections.Concurrent;
                    public static class Calculator
                    {
                        public static int MaxEver = int.MinValue;
                        public static ConcurrentDictionary<string,int> ComputeRegisters()
                        {
                            var registers = new ConcurrentDictionary<string,int>();
                            " + dynamicInstructions + @"
                            return registers;
                        }
                    }";
            var result = csc.CompileAssemblyFromSource(parameters, code);
            if(result.NativeCompilerReturnValue != 0) throw new InvalidOperationException("Compiler error!");
            var testClass = result.CompiledAssembly.GetType("Calculator");
            var registers = (ConcurrentDictionary<string,int>)testClass.GetMethod("ComputeRegisters")?.Invoke(null, new object[] {});
            if(registers == null) throw new InvalidOperationException("It returned null");
            var maxRegisterValue = registers.OrderByDescending(k => k.Value).First();
            Console.WriteLine($"Max: {maxRegisterValue.Key} = {maxRegisterValue.Value}");

            var maxEver = testClass.GetField("MaxEver").GetValue(null);
            Console.WriteLine($"MaxEver: {maxEver}");
        }

        private static readonly Dictionary<string, string> Operators = new Dictionary<string, string> { { "inc", "+=" }, { "dec", "-=" } };

        private static string TranslateCode(string line)
        {
            var match = Regex.Match(line, @"^(\w+) (inc|dec) (-?\d+) if (\w+) (.+)$");
            if (!match.Success) throw new InvalidOperationException("Not matched!");
            var register = match.Groups[1].Value;
            var operation = match.Groups[2].Value;
            var amount = match.Groups[3].Value;
            var conditionRegister = match.Groups[4].Value;
            var condition = match.Groups[5].Value;
            var conditionText = $"registers.GetOrAdd(\"{conditionRegister}\", 0) {condition}";
            var op = Operators[operation];
            return $"if({conditionText}) {{ registers.GetOrAdd(\"{register}\", 0); registers[\"{register}\"] {op} {amount}; MaxEver = Math.Max(MaxEver, registers[\"{register}\"]); }}";
        }

    }
}
