#nullable enable
using Assets.Characters;
using Assets.Mazes;
using Utils.Coordinates;
using Utils.Dispatching;
using IFloor = Assets.Tiles.IFloor;

namespace Assets.Deeds
{
    internal class TeleportAction : Action
    {
        public override void Act(IDispatchRegistry dispatchRegistry, IDispatched dispatched, string actionValue)
        {
            var maze = (IMaze)dispatchRegistry.GetDispatched(Maze.DispatchedName);

            var checkedTiles = new List<string>();
            var floorTile = RandomEmptyFloorTile();

            var moved = maze.MoveOnto(dispatched.UniqueId, floorTile.Floor);

            if (moved)
            {
                var character = (ICharacter)dispatched;
                character.Position = floorTile.Coordinates;
            }

            (IFloor Floor, Coordinate Coordinates) RandomEmptyFloorTile()
            {
                var tile = maze.RandomFloorTile(checkedTiles, false, false);

                return ((IFloor) tile.Dispatched, tile.Coordinates);
            }
        }
    }
}
