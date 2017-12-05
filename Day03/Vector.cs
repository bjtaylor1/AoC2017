namespace Day03
{
    public class Vector
    {
        private readonly int dir;
        private static readonly long[] Xs = { 1, 0, -1, 0 };
        private static readonly long[] Ys = { 0, 1, 0, -1 };

        public long X;
        public long Y;

        public Vector(int dir)
        {
            this.dir = dir;
            X = Xs[dir];
            Y = Ys[dir];
        }

        public Vector Left()
        {
            return new Vector((dir + 1) % 4);
        }
    }
}