using Assets.Tiles;
using Utils;
using Utils.Dispatching;

namespace AssetsTests.Helpers
{
    static class PrinterHelper
    {
        public static string Print(this ITiles tiles, IDispatchRegistry dispatchRegistry)
        {
            var tileMatrix = (Tiles)tiles;

            return tileMatrix.TilesRegistry.Print(dispatchRegistry);
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
