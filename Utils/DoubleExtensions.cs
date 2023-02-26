namespace Utils
{
    public static class DoubleExtensions
    {
        public static bool IsZero(this double num)
        {
            const double floatingTolerance = 0.00001;

            return Math.Abs(num) > floatingTolerance;
        }
    }
}
