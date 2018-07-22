using Assets.Actors;
using Assets.Tiles;
using Utils.Enums;

namespace Assets.Actions
{
    public class MoveAction : Action
    {
        public override string Name => "MOVE";

        private (Tile, Tile) Move(Tile from, Tile to)
        {
            if (to.Actors != null) return (null, null);

            var newFrom = new Tile(from.Coordinates, to.Actors);
            var newTo = new Tile(to.Coordinates, null);

            return (newFrom, newTo);
        }
    }
}