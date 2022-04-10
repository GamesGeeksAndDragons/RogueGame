using System;
using System.Collections.Generic;
using Assets.Actors;
using Assets.Messaging;
using Assets.Rooms;
using Utils;
using Utils.Coordinates;
using Utils.Dispatching;
using TileChanges = System.Collections.Generic.List<(string Name, Utils.Coordinates.Coordinate Coordinates)>;

namespace Assets.Tiles
{
    internal static class PlaceRoomInTiles
    {
        public const int NumAttemptTillGrowMaze = 3;
        public const int TilesToCheckIfRoomTooClose = 4;
        public const int TilesToCheckIfEdgeTooClose = 3;

        internal static ITiles PositionRoomsInTiles(this ITiles tiles, IEnumerable<Room> rooms)
        {
            var tilesWithRooms = new Tiles(tiles);

            foreach (var room in rooms)
            {
                var roomIsPositioned = false;

                var tilesChange = new List<(string Name, Coordinate coordinates)>();
                while (!roomIsPositioned)
                {
                    var (topLeft, topRight, bottomLeft, bottomRight) = AttemptToPositionRoomInsideTiles(tilesWithRooms, room);

                    var bigEnough = AreTilesBigEnough(topLeft, topRight, bottomLeft, bottomRight);
                    if (!bigEnough)
                    {
                        tilesWithRooms = Tiles.Grow(tilesWithRooms);
                        continue;
                    }

                    if (IsTooCloseToEdge(tilesWithRooms, topLeft, topRight, bottomLeft, bottomRight)) continue;
                    if (IsTooCloseToARoom(tilesWithRooms, topLeft, topRight, bottomLeft, bottomRight)) continue;

                    var changes = PositionRoom(room, topLeft);
                    tilesChange.AddRange(changes);
                    roomIsPositioned = true;
                }

                tilesWithRooms.Replace(tilesChange);
            }

            return tilesWithRooms;
        }

        private static bool AreTilesBigEnough(Coordinate topLeft, Coordinate topRight, Coordinate bottomLeft, Coordinate bottomRight)
        {
            return topLeft != topRight && topRight != bottomLeft && bottomLeft != bottomRight && bottomRight != Coordinate.NotSet;
        }

        private static bool IsSurroundedByRock(Coordinate coordinate, Tiles tiles, int tilesToCheck, int boundaryRow, int boundaryColumn, IDispatchRegistry registry)
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
                    var isRock = tiles.IsTileType<Rock>(checkCoordinate, registry);
                    if (!isRock) return false;
                }
            }

            return true;
        }

        private static bool IsTooCloseToEdge(Tiles tiles, Coordinate coordinates, int tilesToCheck)
        {
            if (coordinates.Row - tilesToCheck < 0) return true;
            if (coordinates.Column - tilesToCheck < 0) return true;

            var (maxRow, maxColumn) = tiles.UpperBounds;
            if (coordinates.Row > maxRow) return true;
            if (coordinates.Column > maxColumn) return true;

            return false;
        }

        private static bool IsTooCloseToEdge(Tiles tiles, Coordinate topLeft, Coordinate topRight,
            Coordinate bottomLeft, Coordinate bottomRight)
        {
            if (IsTooCloseToEdge(tiles, topLeft, TilesToCheckIfEdgeTooClose)) return true;
            if (IsTooCloseToEdge(tiles, topRight, TilesToCheckIfEdgeTooClose)) return true;
            if (IsTooCloseToEdge(tiles, bottomLeft, TilesToCheckIfEdgeTooClose)) return true;
            return IsTooCloseToEdge(tiles, bottomRight, TilesToCheckIfEdgeTooClose);
        }

        private static bool IsTooCloseToARoom(Tiles tiles, Coordinate topLeft, Coordinate topRight, Coordinate bottomLeft, Coordinate bottomRight)
        {
            var dispatchRegistry = tiles.DispatchRegistry;
            var (row, column) = tiles.UpperBounds;
            if (!IsSurroundedByRock(topLeft, tiles, TilesToCheckIfRoomTooClose, row, column, dispatchRegistry)) return true;
            if (!IsSurroundedByRock(topRight, tiles, TilesToCheckIfRoomTooClose, row, column, dispatchRegistry)) return true;
            if (!IsSurroundedByRock(bottomLeft, tiles, TilesToCheckIfRoomTooClose, row, column, dispatchRegistry)) return true;
            return !IsSurroundedByRock(bottomRight, tiles, TilesToCheckIfRoomTooClose, row, column, dispatchRegistry);
        }

        private static (Coordinate TopLeft, Coordinate TopRight, Coordinate BottomLeft, Coordinate BottomRight)
            AttemptToPositionRoomInsideTiles(Tiles tiles, Room room)
        {
            for (var attempts = 0; attempts < NumAttemptTillGrowMaze; ++attempts)
            {
                var start = RandomRockTile(tiles).Coordinates;

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

            IDispatchee RandomRockTile(ITiles maze)
            {
                return maze.RandomTile(dispatchee => dispatchee.IsFloor());
            }
        }

        private static TileChanges PositionRoom(Room room, Coordinate topLeft)
        {
            var rows = room.NumberRows;
            var columns = room.NumberColumns;

            var tileChanges = new List<(string Name, Coordinate coordinates)>();

            for (var row = 0; row < rows; row++)
            {
                for (var column = 0; column < columns; column++)
                {
                    var change = GetTileChange(row, column);
                    tileChanges.Add(change);
                }
            }

            return tileChanges;

            (string Name, Coordinate Coordinates) GetTileChange(int row, int column)
            {
                var roomCoordinates = new Coordinate(row, column);
                var roomTile = room[roomCoordinates];

                var mazeCoordinates = topLeft + roomCoordinates;
                return (roomTile.UniqueId, mazeCoordinates);
            }
        }
    }
}

