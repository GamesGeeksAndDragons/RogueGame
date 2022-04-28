﻿using System;
using Assets.Actors;
using Assets.Deeds;
using Assets.Messaging;
using Assets.Tiles;
using AssetsTests.Fakes;
using AssetsTests.Helpers;
using AssetsTests.RoomTests;
using Utils;
using Utils.Random;
using Xunit;
using Xunit.Abstractions;

namespace AssetsTests.DispatcherTests
{
    public class LevelBuilderTests
    {
        private readonly ITestOutputHelper _output;

        public LevelBuilderTests(ITestOutputHelper output)
        {
            _output = output;
        }

        static class LevelExpectedResults
        {
            public const string Level1 = @"
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
16|███████████╔════════╗█████████|16
17|███████████║        ║█████████|17
18|███████████║        ║█████████|18
19|███████████║        ║█████████|19
20|███████████║ @      ║█████████|20
21|███████████║        ║█████████|21
22|███████████║        ║█████████|22
23|███████████║        ║█████████|23
24|███████████║        ║█████████|24
25|███████████╚════════╝█████████|25
26|██████████████████████████████|26
27|██████████████████████████████|27
28|██████████████████████████████|28
29|██████████████████████████████|29
------------------------------------
  |012345678901234567890123456789|  
";

            public const string Level2 = @"
  |012345678901234567890123456789012345|  
------------------------------------------
0 |██████████████████         █████████|0 
1 |██████████████████ █████         ███|1 
2 |██████████████████ █████ █ █████ ███|2 
3 |██████████████████ █████ █ █████ ███|3 
4 |██████████████████ █████ █ █████ ███|4 
5 |██████████████████ █████ █ █████ ███|5 
6 |██████████████████ █████ █ █████ ███|6 
7 |██████████████████ █████ █ █████ ███|7 
8 |██████████████████ █████ █ █████ ███|8 
9 |██████████████████ █████ ╔2═════1═╗█|9 
10|██████████████████ █████ ║        ║█|10
11|██████████████████ █████ ║        ║█|11
12|██████████████████ █████ ║        ║█|12
13|██████████████████ █████ ║        ║█|13
14|██████████████████ █████ ║        ║█|14
15|██████████████████ █████ ║        ║█|15
16|██████████████████ █████ ║        ║█|16
17|██████████████████ █████ ║        ║█|17
18|██████████████████ █████ ╚════════╝█|18
19|██████████████████ █████ ███████████|19
20|██████████████████ █████ ███████████|20
21|██████████████████ █████ ███████████|21
22|██████████████████ █████ ███████████|22
23|██████████████████ █████ ███████████|23
24|██████████████████ █████ ███████████|24
25|██████████████████ █████ ███████████|25
26|█████████████████╔2═════1═╗█████████|26
27|█████████████████║     @  ║█████████|27
28|█████████████████║        ║█████████|28
29|█████████████████║        ║█████████|29
30|█████████████████║        ║█████████|30
31|█████████████████║        ║█████████|31
32|█████████████████║        ║█████████|32
33|█████████████████║        ║█████████|33
34|█████████████████║        ║█████████|34
35|█████████████████╚════════╝█████████|35
------------------------------------------
  |012345678901234567890123456789012345|  
";

            public const string Level3 = @"
  |012345678901234567890123456789012345678901234567890123456789012345678901234567890|  
---------------------------------------------------------------------------------------
0 |█████████████████████████████████████████████████████████████████████████████████|0 
1 |█████████████████████████████████████████████████████████████████████████████████|1 
2 |█████████████████████████████████████████████████████████████████████████████████|2 
3 |█████████████████████████████████████████████████████████████████████████████████|3 
4 |███████                                                             █████████████|4 
5 |███████ ███████████████████████████████████████████████████████████ █████████████|5 
6 |███████ ███████████████████████████████████████████████████████████ █████████████|6 
7 |███████ ███████████████████████████████████████████████████████████ █████████████|7 
8 |███████ ███████████████████████████████████████████████████████████ █████████████|8 
9 |███████ ███████████████████████████████████████████████████████████ █████████████|9 
10|███████ ███████████████████████████████████████████████████████████ █████████████|10
11|███████ ███████████████████████████████████████████████████████████ █████████████|11
12|███████ ███████████████████████████████████████████████████████████ █████████████|12
13|███████ ███████████████████████████████████████████████████████████ █████████████|13
14|███████ ██████████████████████████                                   @    ███████|14
15|███████ ██████████████████████████ ████████████████████████████████ █████ ███████|15
16|███████ ██████████████████████████ ████████████████████████████████ █████ ███████|16
17|███████ ██████████████████████████ ████████████████████████████████ █████ ███████|17
18|███████ ██████████████████████████ ████████████████████████████████ █████ ███████|18
19|███████ ██████████████████████████ ████████████████████████████████ █████ ███████|19
20|███████ ██████████████████████████ ████████████████████████████████ █████ ███████|20
21|███████ ██████████████████████████ ████████████████████████████████ █████ ███████|21
22|███████ ██████████████████████████ ████████████████████████████████ █████ ███████|22
23|███████ ██████████████████████████ ████████████████████████████████ █████ ███████|23
24|███████ ██████████████████████████ ████████████████████████████████ █████ ███████|24
25|███████ ██████████████████████████ ████████████████████████████████ █████ ███████|25
26|███████ ██████████████████████████ ████████████████████████████████ █████ ███████|26
27|███████ ██████████████████████████ ████████████████████████████████ █████ ███████|27
28|███████ ██████████████████████████ ████████████████████████████████ █████ ███████|28
29|███████ ██████████████████████████ ████████████████████████████████ █████ ███████|29
30|███████ ██████████████████████████ ████████████████████████████████ █████ ███████|30
31|███████ ██████████████████████████ ████████████████████████████████ █████ ███████|31
32|███████ ██████████████████████████ ████████████████████████████████ █████ ███████|32
33|███████ ██████████████████████████ ████████████████████████████████ █████ ███████|33
34|███████ ██████████████████████████ ████████████████████████████████ █████ ███████|34
35|███████ ██████████████████████████ ████████████████████████████████ █████ ███████|35
36|███████ ██████████████████████████ ███████████████████████████████╔2═════1═╗█████|36
37|███████ ██████████████████████████ ███████████████████████████████║        ║█████|37
38|███████ ██████████████████████████ ███████████████████████████████║        ║█████|38
39|███████ ██████████████████████████ ███████████████████████████████║        ║█████|39
40|███████ ██████████████████████████ ███████████████████████████████║        ║█████|40
41|███████ ████████████████████                                      3        ║█████|41
42|███████ ████████████████████ █████ ███████████████████████████████║        ║█████|42
43|███████ ████████████████████ █████ ███████████████████████████████║        ║█████|43
44|███████ ████████████████████ █████ ███████████████████████████████║        ║█████|44
45|███████ ████████████████████ █████ ███████████████████████████████╚════════╝█████|45
46|███████ ████████████████████ █████ ██████████████████████████████████████████████|46
47|███████ ████████████████████ █████ ██████████████████████████████████████████████|47
48|███████ ████████████████████ █████ ██████████████████████████████████████████████|48
49|██████╔2═══════╗████████████ █████ ██████████████████████████████████████████████|49
50|██████║        ║████████████ █████ ██████████████████████████████████████████████|50
51|██████║        ║████████████ █████ ██████████████████████████████████████████████|51
52|██████║        ║████████████ █████ ██████████████████████████████████████████████|52
53|██████║        ║████████████ █████ ██████████████████████████████████████████████|53
54|██████║        ║████████████ █████ ██████████████████████████████████████████████|54
55|██████║        ║████████████ █████ ██████████████████████████████████████████████|55
56|██████║        ║████████████ █████ ██████████████████████████████████████████████|56
57|██████║        ║████████████ █████ ██████████████████████████████████████████████|57
58|██████╚════════╝████████████ █████ ██████████████████████████████████████████████|58
59|████████████████████████████ █████ ██████████████████████████████████████████████|59
60|████████████████████████████ █████ ██████████████████████████████████████████████|60
61|████████████████████████████ █████ ██████████████████████████████████████████████|61
62|████████████████████████████ █████ ██████████████████████████████████████████████|62
63|███████████████████████████╔3═════1═╗████████████████████████████████████████████|63
64|███████████████████████████║        ║████████████████████████████████████████████|64
65|███████████████████████████║        ║████████████████████████████████████████████|65
66|███████████████████████████║        ║████████████████████████████████████████████|66
67|███████████████████████████║        ║████████████████████████████████████████████|67
68|███████████████████████████║        ║████████████████████████████████████████████|68
69|███████████████████████████║        ║████████████████████████████████████████████|69
70|███████████████████████████║        ║████████████████████████████████████████████|70
71|███████████████████████████║        ║████████████████████████████████████████████|71
72|███████████████████████████╚════════╝████████████████████████████████████████████|72
73|█████████████████████████████████████████████████████████████████████████████████|73
74|█████████████████████████████████████████████████████████████████████████████████|74
75|█████████████████████████████████████████████████████████████████████████████████|75
76|█████████████████████████████████████████████████████████████████████████████████|76
77|█████████████████████████████████████████████████████████████████████████████████|77
78|█████████████████████████████████████████████████████████████████████████████████|78
79|█████████████████████████████████████████████████████████████████████████████████|79
80|█████████████████████████████████████████████████████████████████████████████████|80
---------------------------------------------------------------------------------------
  |012345678901234567890123456789012345678901234567890123456789012345678901234567890|  
";

            public static string GetExpectation(int level)
            {
                switch (level)
                {
                    case 1: return Level1;
                    case 2: return Level2;
                    case 3: return Level3;
                }

                throw new ArgumentException($"Error getting expectation for unknown level [{level}]");
            }
        }

        internal void AssertTest(Tiles maze, int level)
        {
            var actual = maze.Print(maze.DispatchRegistry);
            var expected = LevelExpectedResults.GetExpectation(level).Trim(CharHelpers.EndOfLine);

            _output.WriteLine(RoomTestHelpers.Divider + " expected " + RoomTestHelpers.Divider);
            _output.WriteLine(expected);
            _output.WriteLine(RoomTestHelpers.Divider + " expected " + RoomTestHelpers.Divider);
            _output.WriteLine(RoomTestHelpers.Divider + " actual " + RoomTestHelpers.Divider);
            _output.WriteLine(actual);
            _output.WriteLine(RoomTestHelpers.Divider + " actual " + RoomTestHelpers.Divider);

            Assert.Equal(expected, actual);
        }


        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void WhenBuiltDispatcher_ShouldHaveMeInMaze(int level)
        {
            var dispatchRegistry = new DispatchRegistry();
            var actionRegistry = new ActionRegistry();
            var dispatcher = new Dispatcher(dispatchRegistry);
            var fakeRandomNumbers = new DieBuilder();
            var fakeLogger = new FakeLogger(_output);
            var actorBuilder = new ActorBuilder(dispatchRegistry, actionRegistry);

            var builder = new LevelBuilder(fakeRandomNumbers, fakeLogger, dispatcher, dispatchRegistry, actionRegistry, actorBuilder);
            var maze = builder.Build(level);

            var before = maze.Print(dispatchRegistry);
            _output.WriteLine(RoomTestHelpers.Divider + " before " + RoomTestHelpers.Divider);
            _output.WriteLine(before);

            var me = new Me(dispatchRegistry, actionRegistry, "");
            dispatcher.EnqueueTeleport(me);
            dispatcher.Dispatch();

            AssertTest(maze, level);
        }
    }
}