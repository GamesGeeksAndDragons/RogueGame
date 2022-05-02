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
        public override void Act(IDispatched dispatched, string actionValue)
        {
            if (Maze == null) throw new ArgumentException($"Tried to Teleport when Maze is null");

            var floorTile = RandomEmptyFloorTile();

            var moved = Maze.MoveOnto(dispatched.UniqueId, floorTile.Floor);

            if (moved)
            {
                var character = (ICharacter)dispatched;
                character.Position = floorTile.Coordinates;
            }

            (IFloor Floor, Coordinate Coordinates) RandomEmptyFloorTile()
            {
                var tile = Maze.RandomFloorTile(false, false);

                return ((IFloor) tile.Dispatched, tile.Coordinates);
            }
        }
    }
}
