using System;
using System.Collections.Generic;
using Assets.Actors;
using Utils;
using Utils.Coordinates;

namespace Assets.Mazes
{
    static class PositionRoomsInMazeImpl
    {
        public const int NumAttemptTillGrowMaze = 10;
        public const int TilesToCheckIfRoomTooClose = 4;
        private const int TilesToCheckIfEdgeTooClose = 3;

        internal static Maze PositionRoomsInMaze(this Maze maze, IEnumerable<Room> rooms)
        {
            var mazeWithRooms = maze;

            foreach (var room in rooms)
            {
                var postitionAttempts = 0;
                var roomIsPositioned = false;

                var tilesChange = new List<(string Name, Coordinate coordinates)>();
                while (!roomIsPositioned && ++postitionAttempts < NumAttemptTillGrowMaze)
                {
                    var (topLeft, topRight, bottomLeft, bottomRight) = FindCoordindatesInsideMaze(room, mazeWithRooms);
                    if (IsTooCloseToEdgeOfMaze(mazeWithRooms, topLeft, topRight, bottomLeft, bottomRight)) continue;
                    if (IsTooCloseToAnotherRoom(mazeWithRooms, topLeft, topRight, bottomLeft, bottomRight)) continue;

                    var changes = PositionRoom(mazeWithRooms, room, topLeft, topRight, bottomLeft);
                    tilesChange.AddRange(changes);
                    roomIsPositioned = true;
                }

                var changedTileState = tilesChange.ToTilesState();
                mazeWithRooms = mazeWithRooms.Clone(changedTileState);
            }

            return mazeWithRooms;
        }

        private static bool IsSurroundedByRock(Coordinate coordinate, Maze maze, int tilesToCheck, int boundaryRow, int boundaryColumn)
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

                    var name = maze.TileName(checkCoordinate);
                    if (name.IsNullOrEmpty()) return false;

                    // really want isPaper and isSissors
                    var isRock = maze.IsTyleType<Rock>(checkCoordinate);
                    if (!isRock) return false;
                }
            }

            return true;
        }

        private static bool IsTooCloseToEdgeOfMaze(Maze tiles, Coordinate coordinates, int tilesToCheck)
        {
            if (coordinates.Row - tilesToCheck < 0) return true;
            if (coordinates.Column - tilesToCheck < 0) return true;

            var (maxRow, maxColumn) = tiles.MazeUpperBounds;
            if (coordinates.Row > maxRow) return true;
            if (coordinates.Column > maxColumn) return true;

            return false;
        }

        private static bool IsTooCloseToEdgeOfMaze(Maze maze, Coordinate topLeft, Coordinate topRight, Coordinate bottomLeft, Coordinate bottomRight)
        {
            if (IsTooCloseToEdgeOfMaze(maze, topLeft, TilesToCheckIfEdgeTooClose)) return true;
            if (IsTooCloseToEdgeOfMaze(maze, topRight, TilesToCheckIfEdgeTooClose)) return true;
            if (IsTooCloseToEdgeOfMaze(maze, bottomLeft, TilesToCheckIfEdgeTooClose)) return true;
            return IsTooCloseToEdgeOfMaze(maze, bottomRight, TilesToCheckIfEdgeTooClose);
        }

        private static bool IsTooCloseToAnotherRoom(Maze maze, Coordinate topLeft, Coordinate topRight, Coordinate bottomLeft, Coordinate bottomRight)
        {
            var (row, column) = maze.MazeUpperBounds;
            if (!IsSurroundedByRock(topLeft, maze, TilesToCheckIfRoomTooClose, row, column)) return true;
            if (!IsSurroundedByRock(topRight, maze, TilesToCheckIfRoomTooClose, row, column)) return true;
            if (!IsSurroundedByRock(bottomLeft, maze, TilesToCheckIfRoomTooClose, row, column)) return true;
            return !IsSurroundedByRock(bottomRight, maze, TilesToCheckIfRoomTooClose, row, column);
        }

        private static (Coordinate TopLeft, Coordinate TopRight, Coordinate BottomLeft, Coordinate BottomRight) FindCoordindatesInsideMaze(Room room, Maze maze)
        {
            var isInsideMaze = false;

            var topLeft = Coordinate.NotSet;
            var topRight = Coordinate.NotSet;
            var bottomLeft = Coordinate.NotSet;
            var bottomRight = Coordinate.NotSet;

            while (!isInsideMaze)
            {
                var start = maze.RandomRockTile();

                (topLeft, topRight, bottomLeft, bottomRight) = room.GetCorners();
                topLeft += start;
                topRight += start;
                bottomLeft += start;
                bottomRight += start;

                isInsideMaze =
                    maze.IsInsideMaze(topLeft) &&
                    maze.IsInsideMaze(topRight) &&
                    maze.IsInsideMaze(bottomLeft) &&
                    maze.IsInsideMaze(bottomRight);
            }

            return (topLeft, topRight, bottomLeft, bottomRight);
        }

        public static IList<(string Name, Coordinate coordinates)> PositionRoom(Maze maze, Room room, Coordinate topLeft, Coordinate topRight, Coordinate bottomLeft)
        {
            var rows = bottomLeft.Row - topLeft.Row;
            var columns = topRight.Column - topLeft.Column;

            var tileChanges = new List<(string Name, Coordinate coordinates)>();

            for (var row = 0; row <= rows; row++)
            {
                for (var column = 0; column <= columns; column++)
                {
                    var roomCoordindates = new Coordinate(row, column);
                    var roomTileName = room.Tiles[roomCoordindates];
                    var mazeCoordinates = topLeft + roomCoordindates;

                    (string Name, Coordinate Coordinates) tile = (null, mazeCoordinates);
                    if (!roomTileName.IsNullOrEmpty())
                    {
                        var roomTile = maze.GetDispatchee(roomTileName);
                        var state = Maze.FormatState(mazeCoordinates);
                        var mazeTile = roomTile.CloneDispatchee(state);
                        tile.Name = mazeTile.UniqueId;
                    }

                    tileChanges.Add(tile);
                }
            }

            return tileChanges;
        }

    }
}
