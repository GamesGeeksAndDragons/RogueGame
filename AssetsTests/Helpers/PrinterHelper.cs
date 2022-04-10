using Assets.Tiles;
using Utils;
using Utils.Dispatching;

namespace AssetsTests.Helpers
{
    static class PrinterHelper
    {
        public static string Print(this ITiles tiles, IDispatchRegistry dispatchRegistry)
        {
            var t = (Tiles)tiles;

            return t.TilesRegistryOfDispatcheeNames.Print(uniqueId =>
            {
                if (uniqueId == null) return "";

                var dispatchee = dispatchRegistry.GetDispatchee(uniqueId);

                return dispatchee.ToString();
            });
        }
    }
}
