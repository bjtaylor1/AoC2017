using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
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
            var parameters = new CompilerParameters(new[] { "mscorlib.dll", "System.Core.dll" }, "day08dynamic.dll", true);
            var result = csc.CompileAssemblyFromSource(parameters,
                @"public class Test
                    {
                        public static string GetMessage()
                        {
                            return ""Hello from dynamic!"";
                        }
                    }");
            if(result.NativeCompilerReturnValue != 0) throw new InvalidOperationException("Compiler error!");

            var testClass = result.CompiledAssembly.GetType("Test");
            var testInst = Activator.CreateInstance(testClass);
            var message = testClass.GetMethod("GetMessage")?.Invoke(testInst, new object[] {});

            Console.Out.WriteLine(message);
        }


    }

    
}
