using Assets.Messaging;
using Utils;
using Utils.Coordinates;
using Utils.Dispatching;

namespace Assets.Mazes
{
    static class MazeHelpers
    {
        public static IDispatchee RandomFloorTile(this Maze maze)
        {
            return maze.RandomTile(IsFloor);

            bool IsFloor(Coordinate coordinate)
            {
                var tile = maze[coordinate];
                return tile.IsFloor();
            }
        }

        public static Maze PlaceInMaze(this Maze maze, IDispatchee dispatchee, Coordinate coordinates)
        {
            var cloner = (ICloner<Maze>)dispatchee;

            var state = coordinates.FormatParameter();
            var actor = cloner.Clone(state);

            var newState = coordinates.ToParameter(actor.UniqueId);

            return maze.Clone(newState);
        }


    }
}
