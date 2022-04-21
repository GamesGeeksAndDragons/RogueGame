using System.Linq;
using Assets.Actors;
using Utils;
using Utils.Dispatching;

namespace Assets.Messaging
{
    public static class DispatchRegistryHelpers
    {
        internal static IDispatchee[,] Register(this IDispatchRegistry dispatchRegistry, IActorBuilder builder, string[] dispatchees)
        {
            var noRows = dispatchees.Length;
            var noColumns = dispatchees.Max(row => row.Length);

            var tiles = new IDispatchee[noRows, noColumns];

            for (int rowIndex = 0; rowIndex < noRows; rowIndex++)
            {
                var row = dispatchees[rowIndex];

                for (int colIndex = 0; colIndex < noColumns; colIndex++)
                {
                    var actor = row[colIndex].ToString();
                    tiles[rowIndex, colIndex] = builder.Build(actor);
                }
            }

            return tiles;
        }

        internal static IDispatchee[,] Register(this IDispatchRegistry dispatchRegistry, IActorBuilder builder, string dispatchees)
        {
            var dispatcheeArray = dispatchees.SplitIntoLines();

            return dispatchRegistry.Register(builder, dispatcheeArray);
        }
    }
}