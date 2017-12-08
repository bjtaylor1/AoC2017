using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp;

namespace Day08
{
    class Program
    {
        static void Main(string[] args)
        {
            var csc = new CSharpCodeProvider(new Dictionary<string, string>());

            var parameters = new CompilerParameters(new[] { "mscorlib.dll", "System.Core.dll", Assembly.GetExecutingAssembly().Location }, "day08dynamic.dll", true);
            
            var result = csc.CompileAssemblyFromSource(parameters,
                @"using Day08;
                    public static class Test
                    {
                        public static Registers GetMessage()
                        {
                            return new Registers(""Hello from dynamic!"");
                        }
                    }");
            if(result.NativeCompilerReturnValue != 0) throw new InvalidOperationException("Compiler error!");

            var testClass = result.CompiledAssembly.GetType("Test");
            var obj = testClass.GetMethod("GetMessage")?.Invoke(null, new object[] {});
            var registers = (Registers) obj;
            Console.Out.WriteLine(registers?.Message);
        }
    }

    public class Registers
    {
        public string Message { get; }

        public Registers(string message)
        {
            Message = message;
        }
    }

    
}
