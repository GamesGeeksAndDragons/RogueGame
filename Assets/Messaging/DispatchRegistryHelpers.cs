#nullable enable
using Assets.Resources;
using Assets.Tiles;
using Utils;
using Utils.Dispatching;

namespace Assets.Messaging
{
    public static class DispatchRegistryHelpers
    {
        internal static string[,] Register(this IDispatchRegistry dispatchRegistry, IResourceBuilder builder, string[] tilesArray)
        {
            var noRows = tilesArray.Length;
            var noColumns = tilesArray.Max(row => row.Length);

            var tiles = new string[noRows, noColumns];

            for (int rowIndex = 0; rowIndex < noRows; rowIndex++)
            {
                var row = tilesArray[rowIndex];

                for (int colIndex = 0; colIndex < noColumns; colIndex++)
                {
                    var actor = row[colIndex].ToString();
                    tiles[rowIndex, colIndex] = builder.BuildResource(actor).UniqueId;
                }
            }

            return tiles;
        }

        internal static string[,] Register(this IDispatchRegistry dispatchRegistry, IResourceBuilder builder, string tiles)
        {
            var tilesArray = tiles.SplitIntoLines();

            return dispatchRegistry.Register(builder, tilesArray);
        }
    }
}