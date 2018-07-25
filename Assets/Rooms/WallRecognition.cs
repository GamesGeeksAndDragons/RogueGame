using System.Collections.Generic;
using System.Linq;
using Utils.Coordinates;
using Utils.Enums;
using RoomTiles= Assets.Tiles.Tiles;

namespace Assets.Rooms
{
    static class WallRecognition
    {
        private static readonly Dictionary<Compass8Points, WallDirection> CompassPointsToWall = new Dictionary<Compass8Points, WallDirection>
        {
            {Compass8Points.NorthEast, WallDirection.BottomLeftCorner},
            {Compass8Points.NorthWest, WallDirection.BottomRightCorner},
            {Compass8Points.SouthEast, WallDirection.TopLeftCorner},
            {Compass8Points.SouthWest, WallDirection.TopRightCorner}
        };

        private static readonly List<Compass8Points> OnlySouthernTiles = new List<Compass8Points>
        {
            Compass8Points.South, Compass8Points.SouthEast, Compass8Points.SouthWest
        };

        private static readonly List<Compass8Points> OnlyNorthernTiles = new List<Compass8Points>
        {
            Compass8Points.North, Compass8Points.NorthEast, Compass8Points.NorthWest
        };

        private static readonly List<Compass8Points> OnlyEasternTiles = new List<Compass8Points>
        {
            Compass8Points.East, Compass8Points.NorthEast, Compass8Points.SouthEast
        };

        private static readonly List<Compass8Points> OnlyWesternTiles = new List<Compass8Points>
        {
            Compass8Points.West, Compass8Points.NorthWest, Compass8Points.SouthWest
        };

        private static bool ShouldBeWall(this RoomTiles tiles, Coordinate coordinate)
        {
            if (!tiles.IsInside(coordinate)) return false;

            return tiles.IsTile(coordinate) && ! tiles.HasActor(coordinate);
        }

        public static List<Compass8Points> ExamineSurroundingTiles(this RoomTiles tiles, Coordinate coordinate)
        {
            var surroundingTiles = new List<Compass8Points>();

            var below = tiles.ShouldBeWall(coordinate.Down());
            if (below) surroundingTiles.Add(Compass8Points.South);

            var above = tiles.ShouldBeWall(coordinate.Up());
            if (above) surroundingTiles.Add(Compass8Points.North);

            var right = tiles.ShouldBeWall(coordinate.Right());
            if (right) surroundingTiles.Add(Compass8Points.East);

            var left = tiles.ShouldBeWall(coordinate.Left());
            if (left) surroundingTiles.Add(Compass8Points.West);

            var topLeft = tiles.ShouldBeWall(coordinate.Up().Left());
            if (topLeft) surroundingTiles.Add(Compass8Points.NorthWest);

            var topRight = tiles.ShouldBeWall(coordinate.Up().Right());
            if (topRight) surroundingTiles.Add(Compass8Points.NorthEast);

            var bottomLeft = tiles.ShouldBeWall(coordinate.Down().Left());
            if (bottomLeft) surroundingTiles.Add(Compass8Points.SouthWest);

            var bottomRight = tiles.ShouldBeWall(coordinate.Down().Right());
            if (bottomRight) surroundingTiles.Add(Compass8Points.SouthEast);

            return surroundingTiles;
        }

        public static bool IsCorner(this IReadOnlyCollection<Compass8Points> surroundingTiles)
        {
            if (surroundingTiles.Count != 1) return false;

            var compassPoint = surroundingTiles.Single();
            return CompassPointsToWall.ContainsKey(compassPoint);
        }

        public static WallDirection ToWallDirection(this Compass8Points compass)
        {
            return CompassPointsToWall[compass];
        }

        private static bool OnlyHas(this IReadOnlyCollection<Compass8Points> surroundingTiles, IReadOnlyCollection<Compass8Points> tiles)
        {
            var matches = surroundingTiles.Intersect(tiles);
            return matches.Count() == surroundingTiles.Count;
        }

        public static bool IsHorizontal(this IReadOnlyCollection<Compass8Points> surroundingTiles)
        {
            var onlySouthern = surroundingTiles.OnlyHas(OnlySouthernTiles);
            if (onlySouthern) return true;

            var onlyNorthern = surroundingTiles.OnlyHas(OnlyNorthernTiles);
            return onlyNorthern;
        }

        public static bool IsVertical(this IReadOnlyCollection<Compass8Points> surroundingTiles)
        {
            var onlyEastern = surroundingTiles.OnlyHas(OnlyEasternTiles);
            if (onlyEastern) return true;

            var onlyWestern = surroundingTiles.OnlyHas(OnlyWesternTiles);
            return onlyWestern;
        }

    }
}
