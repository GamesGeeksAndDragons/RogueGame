using Assets.Actors;
using Assets.Tiles;
using Utils.Coordinates;
using Utils.Dispatching;

namespace Assets.Deeds
{
    internal class TeleportAction : Action
    {
        public override void Act(IDispatchee dispatchee, string actionValue)
        {
            var floorTile = RandomEmptyFloorTile();

            var moved = Tiles.MoveOnto(dispatchee.UniqueId, floorTile.Floor);

            if (moved)
            {
                var character = (ICharacter)dispatchee;
                character.Position = floorTile.Coordinates;
            }

            (IFloor Floor, Coordinate Coordinates) RandomEmptyFloorTile()
            {
                var tile = Tiles.RandomFloorTile(false);

                return ((IFloor) tile.Dispatchee, tile.Coordinates);
            }
        }
    }
}
