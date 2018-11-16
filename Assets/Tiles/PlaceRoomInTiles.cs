using System;
using System.Collections.Generic;
using Assets.Actors;
using Assets.Mazes;
using Assets.Messaging;
using Utils;
using Utils.Coordinates;

namespace Assets.Tiles
{
    internal static class PlaceRoomInTiles
    {
        public const int NumAttemptTillGrowMaze = 3;
        public const int TilesToCheckIfRoomTooClose = 4;
        public const int TilesToCheckIfEdgeTooClose = 3;

        internal static Tiles PositionRoomsInTiles(this Tiles tiles, DispatchRegistry registry, IEnumerable<Room> rooms)
        {
            var tilesWithRooms = tiles;

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

                    var changes = PositionRoom(room, registry, topLeft, topRight, bottomLeft);
                    tilesChange.AddRange(changes);
                    roomIsPositioned = true;
                }

                var changedTileState = tilesChange.ToTilesState();
                tilesWithRooms = tilesWithRooms.Clone(changedTileState);
            }

            return tilesWithRooms;
        }

        private static bool AreTilesBigEnough(Coordinate topLeft, Coordinate topRight, Coordinate bottomLeft, Coordinate bottomRight)
        {
            return topLeft != topRight && topRight != bottomLeft && bottomLeft != bottomRight && bottomRight != Coordinate.NotSet;
        }

        private static bool IsSurroundedByRock(Coordinate coordinate, Tiles tiles, int tilesToCheck, int boundaryRow, int boundaryColumn)
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

        private static bool IsTooCloseToARoom(Tiles tiles, Coordinate topLeft, Coordinate topRight,
            Coordinate bottomLeft, Coordinate bottomRight)
        {
            var (row, column) = tiles.UpperBounds;
            if (!IsSurroundedByRock(topLeft, tiles, TilesToCheckIfRoomTooClose, row, column)) return true;
            if (!IsSurroundedByRock(topRight, tiles, TilesToCheckIfRoomTooClose, row, column)) return true;
            if (!IsSurroundedByRock(bottomLeft, tiles, TilesToCheckIfRoomTooClose, row, column)) return true;
            return !IsSurroundedByRock(bottomRight, tiles, TilesToCheckIfRoomTooClose, row, column);
        }

        private static (Coordinate TopLeft, Coordinate TopRight, Coordinate BottomLeft, Coordinate BottomRight)
            AttemptToPositionRoomInsideTiles(Tiles tiles, Room room)
        {
            for (var attempts = 0; attempts < NumAttemptTillGrowMaze; ++attempts)
            {
                var start = tiles.RandomRockTile();

                var (topLeft, topRight, bottomLeft, bottomRight) = room.GetCorners();
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

        private static IList<(string Name, Coordinate coordinates)> PositionRoom(Room room, DispatchRegistry registry,
            Coordinate topLeft, Coordinate topRight, Coordinate bottomLeft)
        {
            var rows = bottomLeft.Row - topLeft.Row;
            var columns = topRight.Column - topLeft.Column;

            var tileChanges = new List<(string Name, Coordinate coordinates)>();

            for (var row = 0; row <= rows; row++)
            {
                for (var column = 0; column <= columns; column++)
                {
                    var roomCoordinates = new Coordinate(row, column);
                    var roomTileName = room.Tiles[roomCoordinates];
                    var mazeCoordinates = topLeft + roomCoordinates;

                    (string Name, Coordinate Coordinates) tile = (null, mazeCoordinates);
                    if (!roomTileName.IsNullOrEmpty())
                    {
                        var roomTile = registry.GetDispatchee(roomTileName);
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

