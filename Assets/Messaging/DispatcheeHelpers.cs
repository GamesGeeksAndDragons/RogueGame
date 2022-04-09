using Assets.Actors;
using Utils;
using Utils.Dispatching;

namespace Assets.Messaging
{
    public static class DispatcheeHelpers
    {
        internal static bool IsTypeof<T>(this IDispatchee dispatchee) 
            where T : Dispatchee<T>
        {
            if (dispatchee == null) return false;
            return dispatchee.Name == Dispatchee<T>.DispatcheeName;
        }

        internal static bool IsWall(this IDispatchee dispatchee)
        {
            return dispatchee.IsTypeof<Wall>();
        }

        internal static bool IsRock(this IDispatchee dispatchee)
        {
            return dispatchee.IsTypeof<Rock>();
        }

        internal static bool IsFloor(this IDispatchee dispatchee)
        {
            return dispatchee.IsTypeof<Floor>();
        }

        internal static bool IsDoor(this IDispatchee dispatchee)
        {
            return dispatchee.IsTypeof<Door>();
        }

        internal static bool IsTile(this IDispatchee dispatchee)
        {
            return dispatchee.IsRock() || dispatchee.IsWall() || dispatchee.IsFloor() || dispatchee.IsDoor();
        }

        internal static string[,] ExtractTilesRegistry(this IDispatchee[,] dispatchees)
        {
            var maxRows = dispatchees.RowUpperBound() + 1;
            var maxColumns = dispatchees.ColumnUpperBound() + 1;

            var registry = new string[maxRows, maxColumns];

            for (int rowIndex = 0; rowIndex < maxRows; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < maxColumns; columnIndex++)
                {
                    var dispatchee = dispatchees[rowIndex, columnIndex];
                    if (!dispatchee.IsTile()) continue;

                    registry[rowIndex, columnIndex] = dispatchee.UniqueId;
                }
            }

            return registry;
        }
    }
}