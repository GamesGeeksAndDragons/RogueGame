#nullable enable
using Assets.Rooms;
using Assets.Tiles;
using Utils;
using Utils.Coordinates;
using TileChanges = System.Collections.Generic.List<(string UniqueId, Utils.Coordinates.Coordinate Coordinates)>;

namespace Assets.Mazes
{
    internal static class PlaceRoomInMaze
    {
        public const int NumAttemptTillGrowMaze = 3;
        public const int TooCloseToRoom = 4;
        public const int TooCloseToEdge = 3;

        internal static string[] PositionRoomsInMaze(this IMaze maze, IEnumerable<Room> rooms)
        {
            var removedTiles = new List<string>();

            foreach (var room in rooms)
            {
                var roomIsPositioned = false;

                var tilesChange = new List<(string Name, Coordinate coordinates)>();
                while (!roomIsPositioned)
                {
                    var (topLeft, topRight, bottomLeft, bottomRight) = AttemptToPositionRoomInsideTiles(maze, room);

                    var bigEnough = AreTilesBigEnough(topLeft, topRight, bottomLeft, bottomRight);
                    if (!bigEnough)
                    {
                        maze.Grow();
                        continue;
                    }

                    if (IsTooCloseToEdge(maze, topLeft, topRight, bottomLeft, bottomRight)) continue;
                    if (IsTooCloseToARoom(maze, topLeft, topRight, bottomLeft, bottomRight)) continue;

                    var changes = PositionRoom(room, topLeft);
                    tilesChange.AddRange(changes);
                    roomIsPositioned = true;
                }

                var removed = maze.Replace(tilesChange);
                removedTiles.AddRange(removed);
            }

            return removedTiles.ToArray();
        }

        private static bool AreTilesBigEnough(Coordinate topLeft, Coordinate topRight, Coordinate bottomLeft, Coordinate bottomRight)
        {
            return topLeft != topRight && topRight != bottomLeft && bottomLeft != bottomRight && bottomRight != Coordinate.NotSet;
        }

        private static bool IsSurroundedByRock(Coordinate coordinate, IMaze maze, int tilesToCheck, int boundaryRow, int boundaryColumn)
        {
            var minRow = Math.Max(coordinate.Row - tilesToCheck, 0);
            var maxRow = Math.Min(coordinate.Row + tilesToCheck, boundaryRow);
            var minColumn = Math.Max(coordinate.Column - tilesToCheck, 0);
            var maxColumn = Math.Min(coordinate.Column + tilesToCheck, boundaryColumn);

            for (var row = minRow; row <= maxRow; row++)
            {
                for (var column = minColumn; column < maxColumn; column++)
                {
                    var checkCoordinate = new Coordinate(row, column);

                    var name = maze[checkCoordinate];
                    if (name.IsNullOrEmpty()) return false;

                    // really want isPaper and isScissors
                    var isRock = maze.IsTileType<Rock>(checkCoordinate);
                    if (!isRock) return false;
                }
            }

            return true;
        }

        private static bool IsTooCloseToEdge(IMaze maze, Coordinate coordinates, int tilesToCheck)
        {
            if (coordinates.Row - tilesToCheck < 0) return true;
            if (coordinates.Column - tilesToCheck < 0) return true;

            var (maxRow, maxColumn) = maze.UpperBounds;
            if (coordinates.Row > maxRow) return true;
            if (coordinates.Column > maxColumn) return true;

            return false;
        }

        private static bool IsTooCloseToEdge(IMaze maze, Coordinate topLeft, Coordinate topRight,
            Coordinate bottomLeft, Coordinate bottomRight)
        {
            if (IsTooCloseToEdge(maze, topLeft, TooCloseToEdge)) return true;
            if (IsTooCloseToEdge(maze, topRight, TooCloseToEdge)) return true;
            if (IsTooCloseToEdge(maze, bottomLeft, TooCloseToEdge)) return true;
            return IsTooCloseToEdge(maze, bottomRight, TooCloseToEdge);
        }

        private static bool IsTooCloseToARoom(IMaze maze, Coordinate topLeft, Coordinate topRight, Coordinate bottomLeft, Coordinate bottomRight)
        {
            var (row, column) = maze.UpperBounds;
            if (!IsSurroundedByRock(topLeft, maze, TooCloseToRoom, row, column)) return true;
            if (!IsSurroundedByRock(topRight, maze, TooCloseToRoom, row, column)) return true;
            if (!IsSurroundedByRock(bottomLeft, maze, TooCloseToRoom, row, column)) return true;
            return !IsSurroundedByRock(bottomRight, maze, TooCloseToRoom, row, column);
        }

        private static (Coordinate TopLeft, Coordinate TopRight, Coordinate BottomLeft, Coordinate BottomRight)
            AttemptToPositionRoomInsideTiles(IMaze maze, Room room)
        {
            for (var attempts = 0; attempts < NumAttemptTillGrowMaze; ++attempts)
            {
                var start = maze.RandomRockTile().Coordinates;

                var (topLeft, topRight, bottomLeft, bottomRight) = room.GetSize();
                topLeft += start;
                topRight += start;
                bottomLeft += start;
                bottomRight += start;

                var isInsideMaze =
                    maze.IsInside(topLeft) &&
                    maze.IsInside(topRight) &&
                    maze.IsInside(bottomLeft) &&
                    maze.IsInside(bottomRight);

                if (isInsideMaze)
                {
                    return (topLeft, topRight, bottomLeft, bottomRight);
                }
            }

            return (Coordinate.NotSet, Coordinate.NotSet, Coordinate.NotSet, Coordinate.NotSet);
        }

        private static TileChanges PositionRoom(Room room, Coordinate topLeft)
        {
            var (rows, columns) = room.UpperBounds;

            var tileChanges = new List<(string Name, Coordinate coordinates)>();

            for (var row = 0; row <= rows; row++)
            {
                for (var column = 0; column <= columns; column++)
                {
                    var change = GetTileChange(row, column);
                    tileChanges.Add(change);
                }
            }

            return tileChanges;

            (string UniqueId, Coordinate Coordinates) GetTileChange(int row, int column)
            {
                var roomCoordinates = new Coordinate(row, column);
                var uniqueId = room[roomCoordinates];

                var mazeCoordinates = topLeft + roomCoordinates;
                return (uniqueId, mazeCoordinates);
            }
        }
    }
}

