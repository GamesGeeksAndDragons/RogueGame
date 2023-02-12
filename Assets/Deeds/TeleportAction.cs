#nullable enable
using Assets.Level;
using Assets.Mazes;
using Assets.Personas;
using Assets.Tiles;
using Utils.Coordinates;

namespace Assets.Deeds
{
    internal class TeleportAction : Action
    {
        public override void Act(IGameLevel level, ICharacter who, string actionValue)
        {
            var maze = level.Maze;

            var checkedTiles = new List<string>();
            var floorTile = RandomEmptyFloorTile();

            who.Coordinates = floorTile.Coordinates;

            (IFloor Floor, Coordinate Coordinates) RandomEmptyFloorTile()
            {
                var tile = maze.RandomFloorTile(level.DieBuilder, checkedTiles, false);
                while (level[tile.Coordinates] != null)
                {
                    tile = maze.RandomFloorTile(level.DieBuilder, checkedTiles, false);
                }

                return ((IFloor) tile.Tile, tile.Coordinates);
            }
        }
    }
}
