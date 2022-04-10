using Assets.Actors;
using Assets.Mazes;
using Assets.Tiles;
using Utils.Dispatching;

namespace Assets.Deeds
{
    internal class TeleportAction : IAction
    {
        internal ITiles Tiles;

        public void Act(IDispatchee dispatchee, string actionValue)
        {
            var floorTile = RandomEmptyFloorTile();

            Tiles.MoveOnto(dispatchee.UniqueId, floorTile.Coordinates);

            Floor RandomEmptyFloorTile()
            {
                return (Floor) Tiles.RandomFloorTile(false);
            }
        }
    }
}
