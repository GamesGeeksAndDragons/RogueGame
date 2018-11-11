using System;
using System.Collections.Generic;
using System.Text;
using Assets.Mazes;
using Assets.Messaging;
using AssetsTests.Fakes;
using Utils;
using Utils.Random;
using Xunit;
using Xunit.Abstractions;

namespace AssetsTests.MazeTests
{
    public class TilesBuilderTests
    {
        private readonly ITestOutputHelper _output;

        public TilesBuilderTests(ITestOutputHelper output)
        {
            _output = output;
        }

        private string GetInputString(int testNumber)
        {
            switch (testNumber)
            {
case 1: return @"
████
█  █
█  █
████
";
case 2: return @"
██████
█╔══╗█
█║  ║█
█║  ║█
█╚══╝█
██████
";
case 3: return @"
███████████
█╔═1═══2═╗█
█║ █████ ║█
█║ █╔═╗█ ║█
█║ █║ ║█ 3█
█8 █║ ║█ ║█
█║ █╚9╝█ ║█
█║ █████ 4█
█7   █   ║█
█╚═6═══5═╝█
███████████
";
            }

            throw new ArgumentException($"Unexpected test number [{testNumber}] for Input String");
        }

        private string GetExpectedTiles(int testNumber)
        {
            switch (testNumber)
{
case 1: return
@" |0123
------
0|████
1|█  █
2|█  █
3|████";
case 2: return
@" |012345
--------
0|██████
1|█╔══╗█
2|█║  ║█
3|█║  ║█
4|█╚══╝█
5|██████";
case 3: return
@"  |01234567890
--------------
0 |███████████
1 |█╔═1═══2═╗█
2 |█║ █████ ║█
3 |█║ █╔═╗█ ║█
4 |█║ █║ ║█ 3█
5 |█8 █║ ║█ ║█
6 |█║ █╚9╝█ ║█
7 |█║ █████ 4█
8 |█7   █   ║█
9 |█╚═6═══5═╝█
10|███████████";
}

            throw new ArgumentException($"Unexpected test number [{testNumber}] for Input String");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void BuildTilesFromInputString(int testNumber)
        {
            var input = GetInputString(testNumber);
            var registry = new DispatchRegistry();
            var fakeRandomNumbers = new FakeRandomNumberGenerator();
            var tiles = TilesBuilder.BuildTiles(input, registry, fakeRandomNumbers);

            var actual = tiles.ToString();
            var expected = GetExpectedTiles(testNumber);

            _output.WriteLine('='.ToPaddedString(10));
            _output.WriteLine(expected);
            _output.WriteLine('='.ToPaddedString(10));
            _output.WriteLine(actual);
            _output.WriteLine('='.ToPaddedString(10));

            Assert.Equal(expected, actual);
        }
    }
}
