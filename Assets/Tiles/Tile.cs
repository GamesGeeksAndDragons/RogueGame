using Assets.Actors;
using Utils.Coordinates;

namespace Assets.Tiles
{
    public class Tile : Actor
    {
        public override string Name => "TILE";
        public override Actor Clone()
        {
            return new Tile(Coordinates, Actor?.Clone());
        }

        public Tile(Coordinate coordinates, Actor actors = null)
        {
            Coordinates = coordinates;

            Actor = actors?.Clone();
        }

        public Coordinate Coordinates { get; }

        public Actor Actor;
    }
}