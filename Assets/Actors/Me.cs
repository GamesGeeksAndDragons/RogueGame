using System;
using System.Collections.Generic;
using System.Text;

namespace Assets.Actors
{
    internal class Me : Actor
    {
        public override string Name => "Me";

        public override Actor Clone()
        {
            return new Me();
        }
    }
}
