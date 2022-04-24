#nullable enable
namespace Utils.Enums
{
    public static class CompassHelpers
    {
        public static Compass4Points Rotate(this Compass4Points compassPoint)
        {
            switch (compassPoint)
            {
                case Compass4Points.North: return Compass4Points.East;
                case Compass4Points.East: return Compass4Points.South;
                case Compass4Points.South: return Compass4Points.West;
                case Compass4Points.West: return Compass4Points.North;
                default: throw new ArgumentException($"Unable to swap axis for {compassPoint}");
            }
        }

        public static Compass4Points Rotate(this Compass4Points compassPoint, int times)
        {
            for (int i = 0; i < times; i++)
            {
                compassPoint = compassPoint.Rotate();
            }

            return compassPoint;
        }

        public static readonly IReadOnlyCollection<Compass4Points> AllCompass4Points = EnumHelpers.Values<Compass4Points>().ToList();
        public static readonly IReadOnlyCollection<Compass4Points> ValidCompass4Points = AllCompass4Points.Where(direction => direction != Compass4Points.Undefined).ToList();
    }
}
