using System.Collections.Generic;
using Assets.Actors;

namespace Assets.Tiles
{
    class Tile : ITile
    {
        internal Tile(int x, int y)
        {
            _x = x;
            _y = y;
            _style = TyleStyle.Undefined;
        }

        private readonly IActor _actor;
        private readonly int _x;
        private readonly int _y;
        private readonly TyleStyle _style;

        public static Tile Create(int x, int y)
        {
            return new Tile(x, y);
        }


        public (int x, int y) Corrdinates => (_x, _y);
        public bool IsEmpty => _actor == null;
        public bool HasFloor { get; internal set; }
    }
}
