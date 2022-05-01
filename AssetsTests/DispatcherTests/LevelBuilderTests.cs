﻿using System;
using Assets.Actors;
using Assets.Deeds;
using Assets.Maze;
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
17|███████████║¹¹¹¹¹¹¹¹║█████████|17
18|███████████║¹¹¹¹¹¹¹¹║█████████|18
19|███████████║¹¹¹¹¹¹¹¹║█████████|19
20|███████████║¹@¹¹¹¹¹¹║█████████|20
21|███████████║¹¹¹¹¹¹¹¹║█████████|21
22|███████████║¹¹¹¹¹¹¹¹║█████████|22
23|███████████║¹¹¹¹¹¹¹¹║█████████|23
24|███████████║¹¹¹¹¹¹¹¹║█████████|24
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
10|██████████████████ █████ ║²²²²²²²²║█|10
11|██████████████████ █████ ║²²²²²²²²║█|11
12|██████████████████ █████ ║²²²²²²²²║█|12
13|██████████████████ █████ ║²²²²²²²²║█|13
14|██████████████████ █████ ║²²²²²²²²║█|14
15|██████████████████ █████ ║²²²²²²²²║█|15
16|██████████████████ █████ ║²²²²²²²²║█|16
17|██████████████████ █████ ║²²²²²²²²║█|17
18|██████████████████ █████ ╚════════╝█|18
19|██████████████████ █████ ███████████|19
20|██████████████████ █████ ███████████|20
21|██████████████████ █████ ███████████|21
22|██████████████████ █████ ███████████|22
23|██████████████████ █████ ███████████|23
24|██████████████████ █████ ███████████|24
25|██████████████████ █████ ███████████|25
26|█████████████████╔2═════1═╗█████████|26
27|█████████████████║¹¹¹¹¹@¹¹║█████████|27
28|█████████████████║¹¹¹¹¹¹¹¹║█████████|28
29|█████████████████║¹¹¹¹¹¹¹¹║█████████|29
30|█████████████████║¹¹¹¹¹¹¹¹║█████████|30
31|█████████████████║¹¹¹¹¹¹¹¹║█████████|31
32|█████████████████║¹¹¹¹¹¹¹¹║█████████|32
33|█████████████████║¹¹¹¹¹¹¹¹║█████████|33
34|█████████████████║¹¹¹¹¹¹¹¹║█████████|34
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
14|███████ ██████████████████████████                                        ███████|14
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
37|███████ ██████████████████████████ ███████████████████████████████║@³³³³³³³║█████|37
38|███████ ██████████████████████████ ███████████████████████████████║³³³³³³³³║█████|38
39|███████ ██████████████████████████ ███████████████████████████████║³³³³³³³³║█████|39
40|███████ ██████████████████████████ ███████████████████████████████║³³³³³³³³║█████|40
41|███████ ████████████████████                                      3³³³³³³³³║█████|41
42|███████ ████████████████████ █████ ███████████████████████████████║³³³³³³³³║█████|42
43|███████ ████████████████████ █████ ███████████████████████████████║³³³³³³³³║█████|43
44|███████ ████████████████████ █████ ███████████████████████████████║³³³³³³³³║█████|44
45|███████ ████████████████████ █████ ███████████████████████████████╚════════╝█████|45
46|███████ ████████████████████ █████ ██████████████████████████████████████████████|46
47|███████ ████████████████████ █████ ██████████████████████████████████████████████|47
48|███████ ████████████████████ █████ ██████████████████████████████████████████████|48
49|██████╔2═══════╗████████████ █████ ██████████████████████████████████████████████|49
50|██████║²²²²²²²²║████████████ █████ ██████████████████████████████████████████████|50
51|██████║²²²²²²²²║████████████ █████ ██████████████████████████████████████████████|51
52|██████║²²²²²²²²║████████████ █████ ██████████████████████████████████████████████|52
53|██████║²²²²²²²²║████████████ █████ ██████████████████████████████████████████████|53
54|██████║²²²²²²²²║████████████ █████ ██████████████████████████████████████████████|54
55|██████║²²²²²²²²║████████████ █████ ██████████████████████████████████████████████|55
56|██████║²²²²²²²²║████████████ █████ ██████████████████████████████████████████████|56
57|██████║²²²²²²²²║████████████ █████ ██████████████████████████████████████████████|57
58|██████╚════════╝████████████ █████ ██████████████████████████████████████████████|58
59|████████████████████████████ █████ ██████████████████████████████████████████████|59
60|████████████████████████████ █████ ██████████████████████████████████████████████|60
61|████████████████████████████ █████ ██████████████████████████████████████████████|61
62|████████████████████████████ █████ ██████████████████████████████████████████████|62
63|███████████████████████████╔3═════1═╗████████████████████████████████████████████|63
64|███████████████████████████║¹¹¹¹¹¹¹¹║████████████████████████████████████████████|64
65|███████████████████████████║¹¹¹¹¹¹¹¹║████████████████████████████████████████████|65
66|███████████████████████████║¹¹¹¹¹¹¹¹║████████████████████████████████████████████|66
67|███████████████████████████║¹¹¹¹¹¹¹¹║████████████████████████████████████████████|67
68|███████████████████████████║¹¹¹¹¹¹¹¹║████████████████████████████████████████████|68
69|███████████████████████████║¹¹¹¹¹¹¹¹║████████████████████████████████████████████|69
70|███████████████████████████║¹¹¹¹¹¹¹¹║████████████████████████████████████████████|70
71|███████████████████████████║¹¹¹¹¹¹¹¹║████████████████████████████████████████████|71
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

            var me = new Me(dispatchRegistry, actionRegistry, ActorDisplay.Me, "");
            dispatcher.EnqueueTeleport(me);
            dispatcher.Dispatch();

            AssertTest(maze, level);
        }
    }
}