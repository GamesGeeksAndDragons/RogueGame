#nullable enable
using Assets.Actors;
using Utils;
using Utils.Dispatching;

namespace Assets.Messaging
{
    public static class DispatcheeHelpers
    {
        internal static bool IsTypeof<T>(this IDispatched? dispatchee) 
            where T : Dispatched<T>
        {
            if (dispatchee == null) return false;
            return dispatchee.Name == Dispatched<T>.DispatcheeName;
        }

        internal static bool IsWall(this IDispatched dispatched)
        {
            return dispatched.IsTypeof<Wall>();
        }

        internal static bool IsRock(this IDispatched dispatched)
        {
            return dispatched.IsTypeof<Rock>();
        }

        internal static bool IsFloor(this IDispatched dispatched)
        {
            return dispatched.IsTypeof<Floor>();
        }

        internal static bool IsDoor(this IDispatched dispatched)
        {
            return dispatched.IsTypeof<Door>();
        }

        internal static bool IsNull(this IDispatched dispatched)
        {
            return dispatched.IsTypeof<Null>();
        }

        internal static bool IsTile(this IDispatched dispatched)
        {
            return dispatched.IsRock() || dispatched.IsWall() || dispatched.IsFloor() || dispatched.IsDoor();
        }

        internal static string[,] ExtractTilesRegistry(this IDispatched[,] dispatchedTiles)
        {
            var maxRows = dispatchedTiles.RowUpperBound() + 1;
            var maxColumns = dispatchedTiles.ColumnUpperBound() + 1;

            var registry = new string[maxRows, maxColumns];

            for (int rowIndex = 0; rowIndex < maxRows; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < maxColumns; columnIndex++)
                {
                    var dispatched = dispatchedTiles[rowIndex, columnIndex];
                    if (!dispatched.IsTile()) continue;

                    registry[rowIndex, columnIndex] = dispatched.UniqueId;
                }
            }

            return registry;
        }
    }
}