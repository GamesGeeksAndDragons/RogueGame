using Assets.Mazes;
using Utils;
using Utils.Dispatching;

namespace AssetsTests.Helpers
{
    static class PrinterHelper
    {
        public static string Print(this IMaze maze, IDispatchRegistry dispatchRegistry)
        {
            var tileMatrix = (Maze)maze;

            return tileMatrix.Tiles.Print(dispatchRegistry);
        }

        public static string Print(this string[,] tiles, IDispatchRegistry dispatchRegistry)
        {
            return tiles.Print(uniqueId =>
            {
                if (uniqueId == null) return "";

                var dispatchee = dispatchRegistry.GetDispatched(uniqueId);

                return dispatchee.ToString();
            });
        }
    }
}
