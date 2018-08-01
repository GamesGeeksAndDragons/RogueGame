using System;
using System.Collections.Generic;
using System.Text;
using Assets.Messaging;
using Utils.Coordinates;

namespace Assets.Actors
{
    internal class Rock : Actor<Rock>, IActor
    {
        public Rock(Coordinate coordinate, ActorRegistry registry) : base(coordinate, registry)
        {
        }

        private Rock(Rock rhs) : base(rhs)
        {
        }

        public override IActor Clone(string parameters=null)
        {
            return new Rock(this);
        }

        public override string ToString()
        {
            return "■";
        }
    }
}
