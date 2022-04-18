using Assets.Actors;
using Assets.Mazes;
using Assets.Tiles;
using Utils.Coordinates;
using Utils.Dispatching;

namespace Assets.Deeds
{
    internal class TeleportAction : IAction
    {
        internal ITiles Tiles;

        public void Act(IDispatchee dispatchee, string actionValue)
        {
            var floorTile = RandomEmptyFloorTile();

            Tiles.MoveOnto(dispatchee.UniqueId, floorTile.Floor);

            (IFloor Floor, Coordinate Coordinates) RandomEmptyFloorTile()
            {
                var tile = Tiles.RandomFloorTile(false);

                return ((IFloor) tile.Dispatchee, tile.Coordinates);
            }
        }
    }
}
