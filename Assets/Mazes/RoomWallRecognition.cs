﻿using System.Collections.Generic;
using Utils;
using Utils.Coordinates;
using Utils.Enums;

namespace Assets.Mazes
{
    static class RoomWallRecognition
    {
        static class OutsideCorders
        {
            public const Compass8Points BottomLeft = Compass8Points.NorthEast;
            public const Compass8Points BottomRight = Compass8Points.NorthWest;
            public const Compass8Points TopLeft = Compass8Points.SouthEast;
            public const Compass8Points TopRight = Compass8Points.SouthWest;
        }

        private static readonly Dictionary<Compass8Points, WallDirection> OutsideCorners = new Dictionary<Compass8Points, WallDirection>
        {
            {OutsideCorders.BottomLeft, WallDirection.BottomLeftCorner},
            {OutsideCorders.BottomRight, WallDirection.BottomRightCorner},
            {OutsideCorders.TopLeft, WallDirection.TopLeftCorner},
            {OutsideCorders.TopRight, WallDirection.TopRightCorner}
        };

        private static readonly Compass8Points InsideBottomLeft  = Compass8Points.South | Compass8Points.West      | Compass8Points.NorthWest | Compass8Points.SouthWest | Compass8Points.SouthEast;
        private static readonly Compass8Points InsideTopLeft     = Compass8Points.North | Compass8Points.NorthEast | Compass8Points.NorthWest | Compass8Points.West      | Compass8Points.SouthWest;
        private static readonly Compass8Points InsideBottomRight = Compass8Points.South | Compass8Points.East      | Compass8Points.SouthEast | Compass8Points.SouthWest | Compass8Points.NorthEast;
        private static readonly Compass8Points InsideTopRight    = Compass8Points.North | Compass8Points.NorthEast | Compass8Points.NorthWest | Compass8Points.East      | Compass8Points.SouthEast;

        private static readonly Dictionary<Compass8Points, WallDirection> InsideCorners = new Dictionary<Compass8Points, WallDirection>
        {
            {InsideBottomLeft, WallDirection.BottomLeftCorner},
            {InsideTopLeft, WallDirection.TopLeftCorner},
            {InsideBottomRight, WallDirection.BottomRightCorner},
            {InsideTopRight, WallDirection.TopRightCorner}
        };

        private static readonly Compass8Points AllDirections =
            Compass8Points.North | Compass8Points.South | Compass8Points.East | Compass8Points.West |
            Compass8Points.NorthWest | Compass8Points.SouthWest | Compass8Points.NorthEast | Compass8Points.SouthEast
            ;

        private static readonly Compass8Points SoutherlyDirection =
            Compass8Points.South | Compass8Points.SouthEast | Compass8Points.SouthWest;

        private static readonly Compass8Points ExcludeSoutherly = AllDirections ^ SoutherlyDirection;

        private static readonly Compass8Points NortherlyDirection = 
            Compass8Points.North | Compass8Points.NorthEast | Compass8Points.NorthWest;

        private static readonly Compass8Points ExcludeNortherly = AllDirections ^ NortherlyDirection;

        private const Compass8Points EasterlyDirection =
            Compass8Points.East | Compass8Points.NorthEast | Compass8Points.SouthEast;

        private static readonly Compass8Points ExcludeEasterly = AllDirections ^ EasterlyDirection;

        private static readonly Compass8Points WesterlyDirection =
            Compass8Points.West | Compass8Points.NorthWest | Compass8Points.SouthWest;

        private static readonly Compass8Points ExcludeWesterly = AllDirections ^ WesterlyDirection;

        private static bool CanConvertToWall(this RoomTiles tiles, Coordinate coordinate)
        {
            if (!tiles.IsInside(coordinate)) return false;

            return tiles[coordinate].IsNullOrEmpty();
        }

        public static Compass8Points DiscoverSurroundingSpace(this RoomTiles tiles, Coordinate coordinate)
        {
            var surroundingSpace = Compass8Points.Undefined;

            var below = CanConvertToWall(tiles, coordinate.Down());
            if (below) surroundingSpace |= Compass8Points.South;

            var above = CanConvertToWall(tiles, coordinate.Up());
            if (above) surroundingSpace |= Compass8Points.North;

            var right = CanConvertToWall(tiles, coordinate.Right());
            if (right) surroundingSpace |= Compass8Points.East;

            var left = CanConvertToWall(tiles, coordinate.Left());
            if (left) surroundingSpace |= Compass8Points.West;

            var topLeft = CanConvertToWall(tiles, coordinate.Up().Left());
            if (topLeft) surroundingSpace |= Compass8Points.NorthWest;

            var topRight = CanConvertToWall(tiles, coordinate.Up().Right());
            if (topRight) surroundingSpace |= Compass8Points.NorthEast;

            var bottomLeft = CanConvertToWall(tiles, coordinate.Down().Left());
            if (bottomLeft) surroundingSpace |= Compass8Points.SouthWest;

            var bottomRight = CanConvertToWall(tiles, coordinate.Down().Right());
            if (bottomRight) surroundingSpace |= Compass8Points.SouthEast;

            return surroundingSpace;
        }

        public static bool IsCorner(this Compass8Points surroundingTiles)
        {
            if (surroundingTiles == Compass8Points.Undefined) return false;

            return OutsideCorners.ContainsKey(surroundingTiles) || 
                   InsideCorners.ContainsKey(surroundingTiles);
        }

        public static WallDirection ToWallDirection(this Compass8Points direction)
        {
            if (InsideCorners.ContainsKey(direction)) return InsideCorners[direction];

            return OutsideCorners[direction];
        }

        private static bool DoesNotHaveTheseDirections(this Compass8Points source, Compass8Points compare)
        {
            return (source & compare) == Compass8Points.Undefined;
        }

        public static bool IsHorizontal(this Compass8Points surroundingTiles)
        {
            var isSoutherly = ExcludeSoutherly.DoesNotHaveTheseDirections(surroundingTiles);
            if (isSoutherly) return true;

            var isNortherly = ExcludeNortherly.DoesNotHaveTheseDirections(surroundingTiles);
            return isNortherly;
        }

        public static bool IsVertical(this Compass8Points surroundingTiles)
        {
            var isEasterly = ExcludeEasterly.DoesNotHaveTheseDirections(surroundingTiles);
            if (isEasterly) return true;

            var isWesterly = ExcludeWesterly.DoesNotHaveTheseDirections(surroundingTiles);
            return isWesterly;
      }
    }
}
