﻿using System;
using Assets.Deeds;
using Assets.Level;
using Assets.Messaging;
using Assets.Resources;
using Assets.Tiles;
using AssetsTests.Fakes;
using AssetsTests.Helpers;
using AssetsTests.RoomTests;
using Utils;
using Utils.Display;
using Utils.Enums;
using Utils.Random;
using Xunit;
using Xunit.Abstractions;

namespace AssetsTests.ActionTests
{
    public class MoveActionTests
    {
        private readonly ITestOutputHelper _output;

        public MoveActionTests(ITestOutputHelper output)
        {
            _output = output;
        }

        internal static string GetExpectation(int testNum)
        {
            switch (testNum)
            {
                case 1:return @"
  |012345678901234567890123456789|  
------------------------------------
0 |██████████████████████████████|0 
1 |██████████████████████████████|1 
2 |██████████████████████████████|2 
3 |██████████████████████████████|3 
4 |██████████████████████████████|4 
5 |██████████████████████████████|5 
6 |██████████████████████████████|6 
7 |██████████████████████████████|7 
8 |██████████████████████████████|8 
9 |██████████████████████████████|9 
10|██████████████████████████████|10
11|██████████████████████████████|11
12|██████████████████████████████|12
13|██████████████████████████████|13
14|██████████████████████████████|14
15|██████████████████████████████|15
16|███████╔════════╗█████████████|16
17|███████║¹¹¹¹¹¹¹¹║█████████████|17
18|███████║¹¹¹¹¹¹@¹║█████████████|18
19|███████║¹¹¹¹¹¹¹¹║█████████████|19
20|███████║¹¹¹¹¹¹¹¹║█████████████|20
21|███████║¹¹¹¹¹¹¹¹║█████████████|21
22|███████║¹¹¹¹¹¹¹¹║█████████████|22
23|███████║¹¹¹¹¹¹¹¹║█████████████|23
24|███████║M¹¹¹¹¹¹¹║█████████████|24
25|███████╚════════╝█████████████|25
26|██████████████████████████████|26
27|██████████████████████████████|27
28|██████████████████████████████|28
29|██████████████████████████████|29
------------------------------------
  |012345678901234567890123456789|  
";
                case 2:return @"
  |012345678901234567890123456789|  
------------------------------------
0 |██████████████████████████████|0 
1 |██████████████████████████████|1 
2 |██████████████████████████████|2 
3 |██████████████████████████████|3 
4 |██████████████████████████████|4 
5 |██████████████████████████████|5 
6 |██████████████████████████████|6 
7 |██████████████████████████████|7 
8 |██████████████████████████████|8 
9 |██████████████████████████████|9 
10|██████████████████████████████|10
11|██████████████████████████████|11
12|██████████████████████████████|12
13|██████████████████████████████|13
14|██████████████████████████████|14
15|██████████████████████████████|15
16|███████╔════════╗█████████████|16
17|███████║¹¹¹¹¹¹¹¹║█████████████|17
18|███████║¹¹¹¹¹¹¹¹║█████████████|18
19|███████║¹¹¹¹¹¹¹¹║█████████████|19
20|███████║¹¹¹¹¹¹¹¹║█████████████|20
21|███████║¹¹¹¹¹¹¹¹║█████████████|21
22|███████║¹¹¹¹¹¹¹¹║█████████████|22
23|███████║¹¹¹¹¹¹¹¹║█████████████|23
24|███████║M¹¹¹¹¹@¹║█████████████|24
25|███████╚════════╝█████████████|25
26|██████████████████████████████|26
27|██████████████████████████████|27
28|██████████████████████████████|28
29|██████████████████████████████|29
------------------------------------
  |012345678901234567890123456789|  
";
                case 3:return @"
  |012345678901234567890123456789|  
------------------------------------
0 |██████████████████████████████|0 
1 |██████████████████████████████|1 
2 |██████████████████████████████|2 
3 |██████████████████████████████|3 
4 |██████████████████████████████|4 
5 |██████████████████████████████|5 
6 |██████████████████████████████|6 
7 |██████████████████████████████|7 
8 |██████████████████████████████|8 
9 |██████████████████████████████|9 
10|██████████████████████████████|10
11|██████████████████████████████|11
12|██████████████████████████████|12
13|██████████████████████████████|13
14|██████████████████████████████|14
15|██████████████████████████████|15
16|███████╔════════╗█████████████|16
17|███████║¹¹¹¹¹¹¹¹║█████████████|17
18|███████║¹¹¹¹¹¹¹¹║█████████████|18
19|███████║¹¹¹¹¹¹¹¹║█████████████|19
20|███████║¹¹¹¹¹¹¹¹║█████████████|20
21|███████║¹¹¹¹¹¹¹¹║█████████████|21
22|███████║¹¹¹¹¹¹¹¹║█████████████|22
23|███████║¹¹¹¹¹¹¹@║█████████████|23
24|███████║M¹¹¹¹¹¹¹║█████████████|24
25|███████╚════════╝█████████████|25
26|██████████████████████████████|26
27|██████████████████████████████|27
28|██████████████████████████████|28
29|██████████████████████████████|29
------------------------------------
  |012345678901234567890123456789|  
";
                case 4: return @"
  |012345678901234567890123456789|  
------------------------------------
0 |██████████████████████████████|0 
1 |██████████████████████████████|1 
2 |██████████████████████████████|2 
3 |██████████████████████████████|3 
4 |██████████████████████████████|4 
5 |██████████████████████████████|5 
6 |██████████████████████████████|6 
7 |██████████████████████████████|7 
8 |██████████████████████████████|8 
9 |██████████████████████████████|9 
10|██████████████████████████████|10
11|██████████████████████████████|11
12|██████████████████████████████|12
13|██████████████████████████████|13
14|██████████████████████████████|14
15|██████████████████████████████|15
16|███████╔════════╗█████████████|16
17|███████║¹¹¹¹¹¹¹¹║█████████████|17
18|███████║¹¹¹¹¹¹¹¹║█████████████|18
19|███████║¹¹¹¹¹¹¹¹║█████████████|19
20|███████║¹¹¹¹¹¹¹¹║█████████████|20
21|███████║¹¹¹¹¹¹¹¹║█████████████|21
22|███████║¹¹¹¹¹¹¹¹║█████████████|22
23|███████║@¹¹¹¹¹¹¹║█████████████|23
24|███████║M¹¹¹¹¹¹¹║█████████████|24
25|███████╚════════╝█████████████|25
26|██████████████████████████████|26
27|██████████████████████████████|27
28|██████████████████████████████|28
29|██████████████████████████████|29
------------------------------------
  |012345678901234567890123456789|  
";
                case 5: return @"
  |012345678901234567890123456789|  
------------------------------------
0 |██████████████████████████████|0 
1 |██████████████████████████████|1 
2 |██████████████████████████████|2 
3 |██████████████████████████████|3 
4 |██████████████████████████████|4 
5 |██████████████████████████████|5 
6 |██████████████████████████████|6 
7 |██████████████████████████████|7 
8 |██████████████████████████████|8 
9 |██████████████████████████████|9 
10|██████████████████████████████|10
11|██████████████████████████████|11
12|██████████████████████████████|12
13|██████████████████████████████|13
14|██████████████████████████████|14
15|██████████████████████████████|15
16|███████╔════════╗█████████████|16
17|███████║¹¹¹¹¹¹¹¹║█████████████|17
18|███████║¹¹¹¹¹¹¹¹║█████████████|18
19|███████║¹¹¹¹¹¹¹¹║█████████████|19
20|███████║¹¹¹¹¹¹¹¹║█████████████|20
21|███████║¹¹¹¹¹¹¹¹║█████████████|21
22|███████║¹¹¹¹¹¹¹@║█████████████|22
23|███████║¹¹¹¹¹¹¹¹║█████████████|23
24|███████║M¹¹¹¹¹¹¹║█████████████|24
25|███████╚════════╝█████████████|25
26|██████████████████████████████|26
27|██████████████████████████████|27
28|██████████████████████████████|28
29|██████████████████████████████|29
------------------------------------
  |012345678901234567890123456789|  
";
                case 6: return @"
  |012345678901234567890123456789|  
------------------------------------
0 |██████████████████████████████|0 
1 |██████████████████████████████|1 
2 |██████████████████████████████|2 
3 |██████████████████████████████|3 
4 |██████████████████████████████|4 
5 |██████████████████████████████|5 
6 |██████████████████████████████|6 
7 |██████████████████████████████|7 
8 |██████████████████████████████|8 
9 |██████████████████████████████|9 
10|██████████████████████████████|10
11|██████████████████████████████|11
12|██████████████████████████████|12
13|██████████████████████████████|13
14|██████████████████████████████|14
15|██████████████████████████████|15
16|███████╔════════╗█████████████|16
17|███████║¹¹¹¹¹¹¹¹║█████████████|17
18|███████║¹¹¹¹¹¹¹¹║█████████████|18
19|███████║¹¹¹¹¹¹¹¹║█████████████|19
20|███████║¹¹¹¹¹¹¹¹║█████████████|20
21|███████║¹¹¹¹¹¹¹¹║█████████████|21
22|███████║¹¹¹¹¹¹¹¹║█████████████|22
23|███████║¹¹¹¹¹¹¹¹║█████████████|23
24|███████║M¹¹¹¹¹¹@║█████████████|24
25|███████╚════════╝█████████████|25
26|██████████████████████████████|26
27|██████████████████████████████|27
28|██████████████████████████████|28
29|██████████████████████████████|29
------------------------------------
  |012345678901234567890123456789|  
";
                case 7: return @"
  |012345678901234567890123456789|  
------------------------------------
0 |██████████████████████████████|0 
1 |██████████████████████████████|1 
2 |██████████████████████████████|2 
3 |██████████████████████████████|3 
4 |██████████████████████████████|4 
5 |██████████████████████████████|5 
6 |██████████████████████████████|6 
7 |██████████████████████████████|7 
8 |██████████████████████████████|8 
9 |██████████████████████████████|9 
10|██████████████████████████████|10
11|██████████████████████████████|11
12|██████████████████████████████|12
13|██████████████████████████████|13
14|██████████████████████████████|14
15|██████████████████████████████|15
16|███████╔════════╗█████████████|16
17|███████║¹¹¹¹¹¹¹¹║█████████████|17
18|███████║¹¹¹¹¹¹¹¹║█████████████|18
19|███████║¹¹¹¹¹¹¹¹║█████████████|19
20|███████║¹¹¹¹¹¹¹¹║█████████████|20
21|███████║¹¹¹¹¹¹¹¹║█████████████|21
22|███████║¹¹¹@¹¹¹¹║█████████████|22
23|███████║¹¹¹¹¹¹¹¹║█████████████|23
24|███████║M¹¹¹¹¹¹¹║█████████████|24
25|███████╚════════╝█████████████|25
26|██████████████████████████████|26
27|██████████████████████████████|27
28|██████████████████████████████|28
29|██████████████████████████████|29
------------------------------------
  |012345678901234567890123456789|  
";

                default: throw new ArgumentException($"Didn't have Generator for [{testNum}]");
            }
        }

        private void Move_Me_Test(int testNum, params Compass8Points[] directions)
        {
            var dispatchRegistry = new DispatchRegistry();
            var actionRegistry = new ActionRegistry();
            var dispatcher = new Dispatcher(dispatchRegistry, actionRegistry);
            var actorBuilder = new ResourceBuilder(dispatchRegistry, actionRegistry);
            var fakeRandomNumbers = new FakeDieBuilder(1);
            var fakeLogger = new FakeLogger(_output);

            var builder = new LevelBuilder(fakeRandomNumbers, fakeLogger, dispatcher, dispatchRegistry, actionRegistry, actorBuilder);
            var (maze, me) = builder.Build(1);

            var before = maze.Print(dispatchRegistry);
            _output.WriteLine(BuilderTestHelpers.Divider + " before " + BuilderTestHelpers.Divider);
            _output.WriteLine(before);

            // t+1
            foreach (var direction in directions)
            {
                dispatcher.EnqueueMove(me, direction);
            }
            dispatcher.Dispatch();

            var expected = GetExpectation(testNum).Trim(CharHelpers.EndOfLine);
            var actual = maze.Print(dispatchRegistry);

            _output.WriteLine(BuilderTestHelpers.Divider + " expected " + BuilderTestHelpers.Divider);
            _output.WriteLine(expected);
            _output.WriteLine(BuilderTestHelpers.Divider + " actual " + BuilderTestHelpers.Divider);
            _output.WriteLine(actual);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Move_MeNorth_WillNotLeaveATrail()
        {
            Move_Me_Test(1, Compass8Points.North, Compass8Points.North, Compass8Points.North, Compass8Points.North, Compass8Points.North);
        }

        [Fact]
        public void Move_MeSouth_WillNotLeaveATrail()
        {
            Move_Me_Test(2, Compass8Points.South, Compass8Points.South, Compass8Points.South, Compass8Points.South, Compass8Points.South);
        }

        [Fact]
        public void Move_MeEast_WillNotLeaveATrail()
        {
            Move_Me_Test(3, Compass8Points.East, Compass8Points.East, Compass8Points.East, Compass8Points.East, Compass8Points.East, Compass8Points.East);
        }

        [Fact]
        public void Move_MeWest_WillNotLeaveATrail()
        {
            Move_Me_Test(4, Compass8Points.West, Compass8Points.West, Compass8Points.West, Compass8Points.West, Compass8Points.West, Compass8Points.West);
        }

        [Fact]
        public void Move_MeNorthEast_WillNotLeaveATrail()
        {
            Move_Me_Test(5, Compass8Points.NorthEast, Compass8Points.NorthEast, Compass8Points.NorthEast, Compass8Points.NorthEast, Compass8Points.NorthEast, Compass8Points.NorthEast);
        }

        [Fact]
        public void Move_MeSouthEast_WillNotLeaveATrail()
        {
            Move_Me_Test(6, Compass8Points.SouthEast, Compass8Points.SouthEast, Compass8Points.SouthEast, Compass8Points.SouthEast, Compass8Points.SouthEast, Compass8Points.SouthEast);
        }

        [Fact]
        public void Move_Me_MultipleDirectionsWillNotLeaveATrail()
        {
            Move_Me_Test(7, Compass8Points.North, Compass8Points.NorthEast, Compass8Points.NorthWest, Compass8Points.NorthWest, Compass8Points.West, Compass8Points.West, Compass8Points.SouthEast, Compass8Points.SouthWest, Compass8Points.SouthEast, Compass8Points.West);
        }
    }
}
