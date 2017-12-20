using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day20
{
    class Program
    {
        static void Main(string[] args)
        {
            string line;
            var particles = new List<Particle>();
#if DEBUG
            particles.AddRange(File.ReadAllLines(@"..\..\input.txt").Select((l, i) => new Particle(l, i)));
#else
            long particleId = 0;
            while (!string.IsNullOrEmpty(line = Console.ReadLine()))
            {
                particles.Add(new Particle(line, particleId++));
            }
#endif
            var minAccel = particles.Min(p => p.Acceleration.Magnitude);
            //particles.RemoveAll(p => p.Acceleration.Magnitude != minAccel);
            long answer = -1;
            bool achievedQuiscence = false;
            bool reportedQuiescence = false;
            while (answer == -1)
            {
                
                if (achievedQuiscence || (achievedQuiscence = particles.All(p => p.IsQuiescent)))
                {
                    if (!reportedQuiescence)
                    {
                        Console.WriteLine("achieved quiescence");
                        reportedQuiescence = true;
                    }

                    var nearest = particles.OrderBy(p => p.Position.Magnitude).First();
                    var lowestVelocity = particles.OrderBy(p => p.Velocity.Magnitude).First();
                    if(nearest.Id == lowestVelocity.Id) answer = nearest.Id;
                }
                particles.ForEach(p => p.Move());
                var collisions = particles.GroupBy(p => p.Position).Where(g => g.Count() > 1);
                var colliderIds = collisions.SelectMany(c => c.Select(p => p.Id)).ToArray();
                int removed = particles.RemoveAll(p => colliderIds.Contains(p.Id));
                if(removed > 0) Console.WriteLine($"Removed {removed}");

            }
            Console.WriteLine(particles.Count);
        }
    }

    public class Particle
    {
        public long Id { get; }

        public Particle(string line, long id)
        {
            Id = id;
            line = line.Replace(" ", "");
            var parts = Regex.Matches(line, @"\w=<[^>]+>").Cast<Match>().Select(m => m.Value).ToArray();
            if(parts.Length != 3) throw new InvalidOperationException($"Invalid spec: {line}");
            Position = new Vector(parts.Single(p => p.StartsWith("p")));
            Velocity = new Vector(parts.Single(p => p.StartsWith("v")));
            Acceleration = new Vector(parts.Single(p => p.StartsWith("a")));
        }
        public Vector Position { get; set; }
        public Vector Velocity { get; set; }
        public Vector Acceleration { get; set; }
        public override string ToString()
        {
            return $"p=<{Position}>, v=<{Velocity}>, a=<{Acceleration}>";
        }

        private static bool AllSameExceptZero<T>(params T[] values)
        {
            return values.Where(v => !Equals(0, v)).Distinct().Count() <= 1;
        }

        public bool IsQuiescent =>
            AllSameExceptZero(Math.Sign(Position.X), Math.Sign(Velocity.X), Math.Sign(Acceleration.X)) &&
            AllSameExceptZero(Math.Sign(Position.Y), Math.Sign(Velocity.Y), Math.Sign(Acceleration.Y)) &&
            AllSameExceptZero(Math.Sign(Position.Z), Math.Sign(Velocity.Z), Math.Sign(Acceleration.Z));


        public bool Overflown => Position.IsOverflown;
        public void Move()
        {
            Velocity += Acceleration;
            Position += Velocity;
        }
    }

    public class Vector
    {
        public bool IsOverflown => Math.Abs(X) > int.MaxValue || Math.Abs(Y) > int.MaxValue || Math.Abs(Z) > int.MaxValue;
        public long Magnitude => Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z);

        public Vector(string spec)
        {
            spec = spec.Replace(" ", "");
            var match = Regex.Match(spec, @"^\w=<(-?\d+),(-?\d+),(-?\d+)>$");
            if(!match.Success) throw new InvalidOperationException($"Invalid spec: {spec}");
            X = long.Parse(match.Groups[1].Value);
            Y = long.Parse(match.Groups[2].Value);
            Z = long.Parse(match.Groups[3].Value);
        }

        public readonly long X;
        public readonly long Y;
        public readonly long Z;
        public bool Overflown { get; private set; }

        public Vector(long x, long y, long z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Vector))
            {
                return false;
            }

            var vector = (Vector)obj;
            return X == vector.X &&
                   Y == vector.Y &&
                   Z == vector.Z;
        }

        public override int GetHashCode()
        {
            var hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            hashCode = hashCode * -1521134295 + Z.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return $"{X}, {Y}, {Z}";
        }

        public static Vector operator +(Vector lhs, Vector rhs)
        {
            return new Vector(lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z + rhs.Z);
        }
    }

}
