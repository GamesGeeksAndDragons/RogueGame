using Assets.Actors;
using Utils.Coordinates;

namespace Assets.Tiles
{
    public class Tile : Actor
    {
        public override string Name => "TILE";
        public override string UniqueId { get; internal set; }

        public override Actor Clone()
        {
            return new Tile(Coordinates);
        }

        public Tile(Coordinate coordinates) : base(coordinates)
        {
        }

        public override string ToString()
        {
            return ".";
        }
    }
}