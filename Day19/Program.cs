using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Day19
{
    class Program
    {
        static readonly List<string> Lines = new List<string>();
        static void Main(string[] args)
        {
            string line;
            while (!string.IsNullOrEmpty(line = Console.ReadLine()))
            {
                Lines.Add(line);
            }
            var currentDir = new Vector(0, 1);
            Vector currentPos = new Vector(Lines[0].IndexOf('|'), 0);
            char CharAt(Vector pos) => Lines[pos.Y][pos.X];
            bool IsOutOfBounds(Vector pos) =>
                pos.Y >= Lines.Count || 
                pos.X >= Lines[pos.Y].Length || 
                pos.Y < 0 || 
                pos.X < 0;
            Vector TurnLeft (Vector v) => new Vector(v.Y, v.X);
            Vector TurnRight (Vector v) => new Vector(-v.Y, -v.X);
            bool ContainsValidChar(Vector v) => !IsOutOfBounds(v) && CharAt(v) != ' ';

            var msg = new StringBuilder();
            do
            {
                currentPos += currentDir;
                char? currentChar = CharAt(currentPos);
                if (!new char?[] {'|', '-', '+'}.Contains(currentChar))
                {
                    msg.Append(currentChar.Value);
                }
                if (!ContainsValidChar(currentPos + currentDir))
                {
                    var turns = new[] {TurnLeft(currentDir), TurnRight(currentDir)};
                    currentDir = turns.FirstOrDefault(v => ContainsValidChar(currentPos + v));
                }
            } while (currentDir != null);

            Console.WriteLine(msg.ToString());
        }

        public class Vector
        {
            public readonly int X;
            public readonly int Y;

            public Vector(int x, int y)
            {
                X = x;
                Y = y;
            }

            public override bool Equals(object obj)
            {
                if (!(obj is Vector))
                {
                    return false;
                }

                var vector = (Vector)obj;
                return X == vector.X &&
                       Y == vector.Y;
            }

            public override int GetHashCode()
            {
                var hashCode = 1861411795;
                hashCode = hashCode * -1521134295 + base.GetHashCode();
                hashCode = hashCode * -1521134295 + X.GetHashCode();
                hashCode = hashCode * -1521134295 + Y.GetHashCode();
                return hashCode;
            }

            public override string ToString()
            {
                return $"{X}, {Y}";
            }

            public static Vector operator+(Vector lhs, Vector rhs)
            {
                return new Vector(lhs.X + rhs.X, lhs.Y + rhs.Y);
            }
        }
    }
}
