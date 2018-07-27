using Utils.Coordinates;

namespace Assets.Actors
{
    internal class Me : Actor
    {
        public Me(Coordinate coordinates) : base(coordinates)
        {
        }

        public override string Name => "ME";
        public override string UniqueId { get; internal set; }

        public override Actor Clone()
        {
            return new Me(Coordinates);
        }

        public override string ToString()
        {
            return "@";
        }
    }
}
