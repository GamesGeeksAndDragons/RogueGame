#nullable enable
using Assets.Actors;
using Utils;
using Utils.Dispatching;

namespace Assets.Messaging
{
    public static class DispatchRegistryHelpers
    {
        internal static IDispatched[,] Register(this IDispatchRegistry dispatchRegistry, IActorBuilder builder, string[] dispatched)
        {
            var noRows = dispatched.Length;
            var noColumns = dispatched.Max(row => row.Length);

            var tiles = new IDispatched[noRows, noColumns];

            for (int rowIndex = 0; rowIndex < noRows; rowIndex++)
            {
                var row = dispatched[rowIndex];

                for (int colIndex = 0; colIndex < noColumns; colIndex++)
                {
                    var actor = row[colIndex].ToString();
                    tiles[rowIndex, colIndex] = builder.Build(actor);
                }
            }

            return tiles;
        }

        internal static IDispatched[,] Register(this IDispatchRegistry dispatchRegistry, IActorBuilder builder, string dispatched)
        {
            var dispatchedArray = dispatched.SplitIntoLines();

            return dispatchRegistry.Register(builder, dispatchedArray);
        }
    }
}