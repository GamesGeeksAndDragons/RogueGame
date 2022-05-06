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
        private readonly List<int> _buffer = new List<int>();
        private int _index = -1;
        public FakeDie(int max, params int[] random)
        {
            Name = "Fake." + Die.NameFormat(1, max);
            _buffer.AddRange(random);
        }

        public string Name { get; }

        public int Random
        {
            get
            {
                if (++_index == _buffer.Count) _index = 0;

                return _buffer[_index];
            }
        }
    }
}
