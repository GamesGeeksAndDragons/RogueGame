#nullable enable
using Assets.Actors;
using Assets.Tiles;
using Utils.Coordinates;
using Utils.Dispatching;

namespace Assets.Deeds
{
    internal class TeleportAction : Action
    {
        public override void Act(IDispatched dispatched, string actionValue)
        {
            if (Tiles == null) throw new ArgumentException($"Tried to Teleport when Tiles is null");

            var floorTile = RandomEmptyFloorTile();

            var moved = Tiles.MoveOnto(dispatched.UniqueId, floorTile.Floor);

            if (moved)
            {
                var character = (ICharacter)dispatched;
                character.Position = floorTile.Coordinates;
            }

            (IFloor Floor, Coordinate Coordinates) RandomEmptyFloorTile()
            {
                var tile = Tiles.RandomFloorTile(false);

                return ((IFloor) tile.Dispatched, tile.Coordinates);
            }
        }
    }
}
