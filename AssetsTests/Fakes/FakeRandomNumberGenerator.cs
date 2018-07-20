using System;
using System.Collections.Generic;
using Utils;
using Utils.Random;

namespace AssetsTests.Fakes
{
    internal class FakeRandomNumberGenerator 
    {
        private readonly Queue<bool> _fakeBools = new Queue<bool>();
        public void PopulateBooleans(params bool[] rolls)
        {
            foreach (var roll in rolls)
            {
                _fakeBools.Enqueue(roll);
            }
        }
        public bool Boolean => _fakeBools.Dequeue();

        private readonly Queue<int> _fakeDice = new Queue<int>();
        public void PopulateDice(params int[] rolls)
        {
            foreach (var roll in rolls)
            {
                _fakeDice.Enqueue(roll);
            }
        }
        public int Dice(int points)
        {
            return _fakeDice.Dequeue();
        }

        private readonly Queue<int> _fakeEnums = new Queue<int>();
        public void PopulateEnum<T>(params T[] rolls) where T : struct
        {
            foreach (var roll in rolls)
            {
                _fakeEnums.Enqueue(Convert.ToInt32(roll));
            }
        }

        public T Enum<T>() where T : struct
        {
            return _fakeEnums.Dequeue().ToEnum<T>();
        }
    }
}
