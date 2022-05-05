using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Enums;
using Utils.Random;

namespace AssetsTests.Fakes
{
    class FakeDie : IDice
    {
        public string Name { get; internal set; }
        public int Random { get; internal set; }
    }
}
