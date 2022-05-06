using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Random;

namespace AssetsTests.Fakes
{
    internal sealed class FakeDieBuilder : DieBuilder
    {
        public FakeDieBuilder(int dieNumber, params int[] fakeRandomNumbers)
        {
            var die = new FakeDie(dieNumber, fakeRandomNumbers);

            switch (dieNumber)
            {
                case 2: D2 = die; break;
                case 3: D3 = die; break;
                case 4: D4 = die; break;
                case 5: D5 = die; break;
                case 6: D6 = die; break;
                case 7: D7 = die; break;
                case 8: D8 = die; break;
                case 9: D9 = die; break;
                case 10: D10 = die; break;
                case 12: D12 = die; break;
                case 20: D20 = die; break;
            }
        }
    }
}
