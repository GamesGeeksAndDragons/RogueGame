using System;
using System.Collections.Generic;
using System.Text;
using Assets.Messaging;
using Utils;
using Utils.Coordinates;
using Utils.Exceptions;
using Utils.Random;

namespace Assets.Mazes
{
    class MazeTiles : Tiles
    {
        public MazeTiles(int maxRows, int maxColumns, DispatchRegistry registry, IRandomNumberGenerator randomNumbers) 
            : base(maxRows, maxColumns, registry, randomNumbers)
        {
        }

        public MazeTiles(MazeTiles tiles) : base(tiles)
        {
        }

        public override Tiles Clone(string stateChange = null)
        {
            var clone = Create();
            if (stateChange.IsNullOrEmpty()) return clone;

            var state = stateChange.ToParameters();
            UpdateState(clone, state);

            return clone;
        }

        public override Tiles Create()
        {
            return new MazeTiles(this);
        }
    }
}
