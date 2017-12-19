using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Day18
{
    class Program
    {

        static void Main(string[] args)
        {

            string line;
            var lines = new List<string>();
            //lines = File.ReadAllLines(@"..\..\input.txt").ToList();
            while (!string.IsNullOrEmpty(line = Console.ReadLine()))
            {
                lines.Add(line);
            }

            var queues = Enumerable.Range(0,2).Select(n => new BlockingCollection<long?>()).ToArray();
            var deadlockCheck = new object();

            long ParamVal(ConcurrentDictionary<string,long> registers, string p) => long.TryParse(p, out var intVal) ? intVal : registers.GetOrAdd(p, 0);
            long timesP1SentValue = 0;
            ManualResetEvent stop = new ManualResetEvent(false);
            var commands = new Dictionary<string, Action<CommandParameters>>
                {
                    {"snd", p =>
                        {
                            p.QOut.Add(ParamVal(p.Registers, p.P1));
                            if (p.ProgNum == 1) timesP1SentValue++;
                        }
                    },
                    {"set", p => p.Registers[p.P1] = ParamVal(p.Registers, p.P2)},
                    {"add", p => p.Registers[p.P1] = ParamVal(p.Registers, p.P1) + ParamVal(p.Registers, p.P2)},
                    {"mul", p => p.Registers[p.P1] = ParamVal(p.Registers, p.P1) * ParamVal(p.Registers, p.P2)},
                    {"mod", p => p.Registers[p.P1] = ParamVal(p.Registers, p.P1) % ParamVal(p.Registers, p.P2)},
                    {
                        "rcv", p =>
                        {
                            void DoReceive()
                            {
                                var nextVal = p.QIn.Take();
                                Console.Out.WriteLine($"{p.ProgNum} received {nextVal}");
                                if(nextVal == null) throw new FinishedException(); //the other thread has detected deadlock, so we should also terminate.
                                p.Registers[p.P1] = nextVal.Value;
                                
                            }
                            bool doneReceive = false;
                            if (p.QIn.Count == 0) lock (p.QIn) if (p.QIn.Count == 0) //are we going to have to wait?
                            {
                                bool monitorEntered = false;
                                try
                                {
                                    // waiting, so we need to check the other  isn't waiting as well.
                                    if (!(monitorEntered = Monitor.TryEnter(deadlockCheck)))
                                    {
                                        p.QOut.Add(null); //give the other one a value which it will be waiting for.
                                        p.QOut.CompleteAdding();
                                        throw new DeadlockException();
                                    }
                                    DoReceive();
                                    doneReceive = true;
                                }
                                finally
                                {
                                    if(monitorEntered) Monitor.Exit(deadlockCheck);
                                }
                            }
                            if (!doneReceive) DoReceive(); //not waiting, so don't need a lock.
                        }
                    },
                    {
                        "jgz", p =>
                        {
                            if (ParamVal(p.Registers, p.P1) > 0) p.Jump = ParamVal(p.Registers, p.P2) - 1;
                        }
                    }
                };
            var tasks = Enumerable.Range(0, 2).Select(n => Task.Run(() =>
            {
                try
                {
                    var registers = new ConcurrentDictionary<string, long> {["p"] = n};
                    var qout = queues[n];
                    var qin = queues[1 - n];
                    for (long i = 0; i < lines.Count; i++)
                    {
                        if (stop.WaitOne(0))
                        {
                            Console.Out.WriteLine($"{n} asked to stop");
                            return;
                        }
                        var parts = lines[(int) i].Split(' ');
                        Array.Resize(ref parts, 3);
                        var cmd = commands[parts[0]];
                        var cmdParams = new CommandParameters(n, parts[0], parts[1], parts[2], qin, qout, registers);
                        cmd(cmdParams);
                        i += cmdParams.Jump;
                    }
                }
                catch (FinishedException e)
                {
                    Console.WriteLine($"{n} threw {e.GetType().Name}");
                    stop.Set();
                }
            })).ToArray();
            Task.WaitAll(tasks);
            Console.WriteLine(timesP1SentValue);
        }

/*
 snd X plays a sound with a frequency equal to the value of X.
set X Y sets register X to the value of Y.
add X Y increases register X by the value of Y.
mul X Y sets register X to the result of multiplying the value contained in register X by the value of Y.
mod X Y sets register X to the remainder of dividing the value contained in register X by the value of Y (that is, it sets X to the result of X modulo Y).
rcv X recovers the frequency of the last sound played, but only when the value of X is not zero. (If it is zero, the command does nothing.)
jgz X Y jumps with an offset of the value of Y, but only if the value of X is greater than zero. (An offset of 2 skips the next instruction, an offset of -1 jumps to the previous instruction, and so on.)
 * */
    }

    public class CommandParameters
    {
        public int ProgNum { get; }
        public string Instruction { get;  }
        public string P1 { get; }
        public string P2 { get; }
        public BlockingCollection<long?> QIn { get; }
        public BlockingCollection<long?> QOut { get; }
        public ConcurrentDictionary<string, long> Registers { get; set; }
        public long Jump { get; set; }

        public CommandParameters(int progNum, string instruction, string p1, string p2, BlockingCollection<long?> qIn, BlockingCollection<long?> qOut, ConcurrentDictionary<string, long> registers)
        {
            ProgNum = progNum;
            Instruction = instruction;
            P1 = p1;
            P2 = p2;
            QIn = qIn;
            QOut = qOut;
            Registers = registers;
        }
    }

    internal class DeadlockException : FinishedException
    {
    }

    internal class FinishedException : Exception
    {
    }
}
