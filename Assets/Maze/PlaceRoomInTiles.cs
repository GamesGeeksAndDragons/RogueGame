#nullable enable
using Assets.Actors;
using Assets.Rooms;
using Utils;
using Utils.Coordinates;
using TileChanges = System.Collections.Generic.List<(string UniqueId, Utils.Coordinates.Coordinate Coordinates)>;

namespace Assets.Maze
{
    internal static class PlaceRoomInTiles
    {
        public const int NumAttemptTillGrowMaze = 3;
        public const int TilesToCheckIfRoomTooClose = 4;
        public const int TilesToCheckIfEdgeTooClose = 3;

        internal static string[] PositionRoomsInTiles(this ITiles tiles, IEnumerable<Room> rooms)
        {
            var removedTiles = new List<string>();

            foreach (var room in rooms)
            {
                var roomIsPositioned = false;

                var tilesChange = new List<(string Name, Coordinate coordinates)>();
                while (!roomIsPositioned)
                {
                    var (topLeft, topRight, bottomLeft, bottomRight) = AttemptToPositionRoomInsideTiles(tiles, room);

                    var bigEnough = AreTilesBigEnough(topLeft, topRight, bottomLeft, bottomRight);
                    if (!bigEnough)
                    {
                        tiles.Grow();
                        continue;
                    }

                    if (IsTooCloseToEdge(tiles, topLeft, topRight, bottomLeft, bottomRight)) continue;
                    if (IsTooCloseToARoom(tiles, topLeft, topRight, bottomLeft, bottomRight)) continue;

                    var changes = PositionRoom(room, topLeft);
                    tilesChange.AddRange(changes);
                    roomIsPositioned = true;
                }

                var removed = tiles.Replace(tilesChange);
                removedTiles.AddRange(removed);
            }

            return removedTiles.ToArray();
        }

        private static bool AreTilesBigEnough(Coordinate topLeft, Coordinate topRight, Coordinate bottomLeft, Coordinate bottomRight)
        {
            return topLeft != topRight && topRight != bottomLeft && bottomLeft != bottomRight && bottomRight != Coordinate.NotSet;
        }

        private static bool IsSurroundedByRock(Coordinate coordinate, ITiles tiles, int tilesToCheck, int boundaryRow, int boundaryColumn)
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

                    var name = tiles[checkCoordinate];
                    if (name.IsNullOrEmpty()) return false;

                    // really want isPaper and isScissors
                    var isRock = tiles.IsTileType<Rock>(checkCoordinate);
                    if (!isRock) return false;
                }
            }

            return true;
        }

        private static bool IsTooCloseToEdge(ITiles tiles, Coordinate coordinates, int tilesToCheck)
        {
            if (coordinates.Row - tilesToCheck < 0) return true;
            if (coordinates.Column - tilesToCheck < 0) return true;

            var (maxRow, maxColumn) = tiles.UpperBounds;
            if (coordinates.Row > maxRow) return true;
            if (coordinates.Column > maxColumn) return true;

            return false;
        }

        private static bool IsTooCloseToEdge(ITiles tiles, Coordinate topLeft, Coordinate topRight,
            Coordinate bottomLeft, Coordinate bottomRight)
        {
            if (IsTooCloseToEdge(tiles, topLeft, TilesToCheckIfEdgeTooClose)) return true;
            if (IsTooCloseToEdge(tiles, topRight, TilesToCheckIfEdgeTooClose)) return true;
            if (IsTooCloseToEdge(tiles, bottomLeft, TilesToCheckIfEdgeTooClose)) return true;
            return IsTooCloseToEdge(tiles, bottomRight, TilesToCheckIfEdgeTooClose);
        }

        private static bool IsTooCloseToARoom(ITiles tiles, Coordinate topLeft, Coordinate topRight, Coordinate bottomLeft, Coordinate bottomRight)
        {
            var (row, column) = tiles.UpperBounds;
            if (!IsSurroundedByRock(topLeft, tiles, TilesToCheckIfRoomTooClose, row, column)) return true;
            if (!IsSurroundedByRock(topRight, tiles, TilesToCheckIfRoomTooClose, row, column)) return true;
            if (!IsSurroundedByRock(bottomLeft, tiles, TilesToCheckIfRoomTooClose, row, column)) return true;
            return !IsSurroundedByRock(bottomRight, tiles, TilesToCheckIfRoomTooClose, row, column);
        }

        private static (Coordinate TopLeft, Coordinate TopRight, Coordinate BottomLeft, Coordinate BottomRight)
            AttemptToPositionRoomInsideTiles(ITiles tiles, Room room)
        {
            for (var attempts = 0; attempts < NumAttemptTillGrowMaze; ++attempts)
            {
                var start = tiles.RandomRockTile().Coordinates;

                var (topLeft, topRight, bottomLeft, bottomRight) = room.GetSize();
                topLeft += start;
                topRight += start;
                bottomLeft += start;
                bottomRight += start;

                var isInsideMaze =
                    tiles.IsInside(topLeft) &&
                    tiles.IsInside(topRight) &&
                    tiles.IsInside(bottomLeft) &&
                    tiles.IsInside(bottomRight);

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

