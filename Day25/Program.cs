using System;
using System.Collections.Concurrent;

namespace Day25
{
    class Program
    {
        static void Main(string[] args)
        {
            char state = 'A';
            var states = new States();
            var c = new ConcurrentDictionary<int, int>();
            int curpos = 0;
            for (int n = 0; n < 12425180; n++)
            {
                if (state == 'A')
                {
                    if (states[curpos] == 0)
                    {
                        states[curpos] = 1;
                        curpos++;
                        state = 'B';
                    }
                    else
                    {
                        states[curpos] = 0;
                        curpos++;
                        state = 'F';
                    }
                }
                else if (state == 'B')
                {
                    if (states[curpos] == 0)
                    {
                        states[curpos] = 0;
                        curpos--;
                        state = 'B';
                    }
                    else
                    {
                        states[curpos] = 1;
                        curpos--;
                        state = 'C';
                    }
                }
                else if (state == 'C')
                {
                    if (states[curpos] == 0)
                    {
                        states[curpos] = 1;
                        curpos--;
                        state = 'D';
                    }
                    else
                    {
                        states[curpos] = 0;
                        curpos++;
                        state = 'C';
                    }
                }
                else if (state == 'D')
                {
                    if (states[curpos] == 0)
                    {
                        states[curpos] = 1;
                        curpos--;
                        state = 'E';
                    }
                    else
                    {
                        states[curpos] = 1;
                        curpos++;
                        state = 'A';
                    }
                }
                else if (state == 'E')
                {
                    if (states[curpos] == 0)
                    {
                        states[curpos] = 1;
                        curpos--;
                        state = 'F';
                    }
                    else
                    {
                        states[curpos] = 0;
                        curpos--;
                        state = 'D';
                    }
                }
                else if (state == 'F')
                {
                    if (states[curpos] == 0)
                    {
                        states[curpos] = 1;
                        curpos++;
                        state = 'A';
                    }
                    else
                    {
                        states[curpos] = 0;
                        curpos--;
                        state = 'E';
                    }
                }
                else throw new NotImplementedException();
            }
            Console.WriteLine(states.Checksum);
        }
    }

    public class States
    {
        public int Checksum { get; private set; }
        private ConcurrentDictionary<int, int> values = new ConcurrentDictionary<int, int>();
        public int this[int i]
        {
            get { return values.GetOrAdd(i, 0); }
            set
            {
                Checksum += value - this[i];
                values[i] = value;
            }
        }
    }

}
