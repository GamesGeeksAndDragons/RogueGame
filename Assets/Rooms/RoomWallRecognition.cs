#nullable enable
using Utils.Enums;

namespace Assets.Rooms
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
