using Utils.Coordinates;

namespace Assets.Actors
{
    public abstract class Actor
    {
        protected Actor(Coordinate coordinate)
        {
            Coordinates = coordinate;
        }

        public abstract string Name { get; }

        public abstract string UniqueId { get; internal set; }

        public abstract Actor Clone();

        public Coordinate Coordinates { get; }
    }
}