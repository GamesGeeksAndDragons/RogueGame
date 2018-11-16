﻿using System;
using Assets.Messaging;
using Assets.Tiles;
using AssetsTests.Fakes;
using Utils;
using Xunit;
using Xunit.Abstractions;

namespace AssetsTests.MazeTests
{
    public class ConnectDoorsInTiles
    {
        private readonly ITestOutputHelper _output;

        public ConnectDoorsInTiles(ITestOutputHelper output)
        {
            _output = output;
        }

        internal static string GetTestTiles(int testNum)
        {
switch (testNum)
{
case 0: return @"
██████████████████
██████████████████
██████████████████
███╔═╗██████╔═╗███
███║ ║██████║ ║███
███╚═╝██████╚═╝███
██████████████████
██████████████████
██████████████████
███╔═╗██████╔═╗███
███║ ║██████║ ║███
███╚═╝██████╚═╝███
██████████████████
██████████████████
██████████████████
";
case 1: return @"
██████████████████
██████████████████
██████████████████
███╔═╗██████╔═╗███
███║ ║██████║ ║███
███╚1╝██████╚2╝███
██████████████████
██████████████████
██████████████████
███╔1╗██████╔2╗███
███║ ║██████║ ║███
███╚═╝██████╚═╝███
██████████████████
██████████████████
██████████████████
";
case 2: return @"
██████████████████
██████████████████
██████████████████
███╔═╗██████╔═╗███
███║ 1██████1 ║███
███╚═╝██████╚═╝███
██████████████████
██████████████████
██████████████████
███╔═╗██████╔═╗███
███║ 2██████2 ║███
███╚═╝██████╚═╝███
██████████████████
██████████████████
██████████████████
";
case 3: return @"
██████████████████
██████████████████
██████████████████
███╔═╗██████╔═╗███
███║ 1██████║ ║███
███╚═╝██████╚═╝███
██████████████████
██████████████████
██████████████████
███╔═╗██████╔═╗███
███║ ║██████1 ║███
███╚═╝██████╚═╝███
██████████████████
██████████████████
██████████████████
";
}
            throw new ArgumentException($"Didn't have Generator for [{testNum}]");
        }

        internal static string GetExpectation(int testNum)
        {
            switch (testNum)
{
case 1: return
@"  |012345678901234567
---------------------
0 |██████████████████
1 |██████████████████
2 |██████████████████
3 |███╔═╗██████╔═╗███
4 |███║ ║██████║ ║███
5 |███╚ ╝██████╚ ╝███
6 |████ ████████ ████
7 |████ ████████ ████
8 |████ ████████ ████
9 |███╔ ╗██████╔ ╗███
10|███║ ║██████║ ║███
11|███╚═╝██████╚═╝███
12|██████████████████
13|██████████████████
14|██████████████████";
case 2: return
@"  |012345678901234567
---------------------
0 |██████████████████
1 |██████████████████
2 |██████████████████
3 |███╔═╗██████╔═╗███
4 |███║          ║███
5 |███╚═╝██████╚═╝███
6 |██████████████████
7 |██████████████████
8 |██████████████████
9 |███╔═╗██████╔═╗███
10|███║          ║███
11|███╚═╝██████╚═╝███
12|██████████████████
13|██████████████████
14|██████████████████";
case 3: return
@"  |012345678901234567
---------------------
0 |██████████████████
1 |██████████████████
2 |██████████████████
3 |███╔═╗██████╔═╗███
4 |███║     ███║ ║███
5 |███╚═╝██ ███╚═╝███
6 |████████ █████████
7 |████████ █████████
8 |████████ █████████
9 |███╔═╗██ ███╔═╗███
10|███║ ║   ███  ║███
11|███╚═╝██████╚═╝███
12|██████████████████
13|██████████████████
14|██████████████████";
}
            throw new ArgumentException($"Didn't have Generator for [{testNum}]");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        //[InlineData(3)]
        public void ConnectDoorTests(int testNumber)
        {
            var input = GetTestTiles(testNumber);
            var registry = new DispatchRegistry();
            var fakeRandomNumbers = new FakeRandomNumberGenerator();
            var tiles = TilesBuilder.BuildTiles(input, registry, fakeRandomNumbers);

            tiles = tiles.ConnectDoors(registry);

            var actual = tiles.ToString();
            var expected = GetExpectation(testNumber);

            _output.WriteLine('='.ToPaddedString(10));
            _output.WriteLine(expected);
            _output.WriteLine('='.ToPaddedString(10));
            _output.WriteLine(actual);
            _output.WriteLine('='.ToPaddedString(10));

            Assert.Equal(expected, actual);
        }
    }
}
