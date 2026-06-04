namespace Project.Code
{
    public struct Progress
    {
        public int Current;
        public int Max;

        public Progress(int current, int max)
        {
            Current = current;
            Max = max;
        }
    }
}