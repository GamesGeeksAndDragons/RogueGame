using System;
using Assets.Actors;
using Assets.Deeds;
using Assets.Messaging;
using AssetsTests.Fakes;
using AssetsTests.Helpers;
using AssetsTests.RoomTests;
using Utils;
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

        internal static int GetLevel(int testNum)
        {
            switch (testNum)
            {
                case 1:
                case 2:
                    return 1;

                default: throw new ArgumentException($"Didn't have Generator for [{testNum}]");
            }
        }

        internal static string GetExpectation(int testNum)
        {
            switch (testNum)
            {
                case 1:
                    return @"
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
20|███████████║        ║█████████|20
21|███████████║ @      ║█████████|21
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
                case 2:
                    return @"
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
20|███████████║@       ║█████████|20
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
                default: throw new ArgumentException($"Didn't have Generator for [{testNum}]");
            }
        }

        [Fact]
        public void Move_Me_CanMoveIntoEmptySpace()
        {
            var testNum = 1;
            var dispatchRegistry = new DispatchRegistry();
            var actionRegistry = new ActionRegistry();
            var dispatcher = new Dispatcher(dispatchRegistry);
            var actorBuilder = new ActorBuilder(dispatchRegistry, actionRegistry);
            var fakeRandomNumbers = new DieBuilder();
            var fakeLogger = new FakeLogger(_output);

            var builder = new LevelBuilder(fakeRandomNumbers, fakeLogger, dispatcher, dispatchRegistry, actionRegistry, actorBuilder);
            var maze = builder.Build(GetLevel(testNum));

            var me = actorBuilder.Build<Me>();
            dispatcher.EnqueueTeleport(me);
            dispatcher.Dispatch();

            var before = maze.Tiles.Print(dispatchRegistry);
            _output.WriteLine(RoomTestHelpers.Divider + " before " + RoomTestHelpers.Divider);
            _output.WriteLine(before);

            // t+1
            dispatcher.EnqueueMove(me, Compass8Points.South);
            dispatcher.Dispatch();

            var expected = GetExpectation(testNum).Trim(CharHelpers.EndOfLine);
            var actual = maze.Tiles.Print(dispatchRegistry);

            _output.WriteLine(expected);
            _output.WriteLine('='.ToPaddedString(10));
            _output.WriteLine(actual);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Move_Me_CantMoveIntoWallsButCanMoveIntoEmptySpace()
        {
            var testNum = 2;

            var dispatchRegistry = new DispatchRegistry();
            var actionRegistry = new ActionRegistry();
            var dispatcher = new Dispatcher(dispatchRegistry);
            var actorBuilder = new ActorBuilder(dispatchRegistry, actionRegistry);
            var fakeRandomNumbers = new DieBuilder();
            var fakeLogger = new FakeLogger(_output);

            var builder = new LevelBuilder(fakeRandomNumbers, fakeLogger, dispatcher, dispatchRegistry, actionRegistry, actorBuilder);
            var maze = builder.Build(GetLevel(testNum));

            var me = actorBuilder.Build<Me>();
            dispatcher.EnqueueTeleport(me);
            dispatcher.Dispatch();

            var before = maze.Tiles.Print(dispatchRegistry);
            _output.WriteLine(RoomTestHelpers.Divider + " before " + RoomTestHelpers.Divider);
            _output.WriteLine(before);

            // t+1
            dispatcher.EnqueueMove(me, Compass8Points.West);
            dispatcher.EnqueueMove(me, Compass8Points.West);
            dispatcher.EnqueueMove(me, Compass8Points.West);
            dispatcher.EnqueueMove(me, Compass8Points.West);
            dispatcher.EnqueueMove(me, Compass8Points.West);
            dispatcher.EnqueueMove(me, Compass8Points.West);
            dispatcher.EnqueueMove(me, Compass8Points.North);
            dispatcher.EnqueueMove(me, Compass8Points.North);
            dispatcher.EnqueueMove(me, Compass8Points.North);
            dispatcher.EnqueueMove(me, Compass8Points.North);
            dispatcher.Dispatch();

            var expected = GetExpectation(testNum).Trim(CharHelpers.EndOfLine);
            var actual = maze.Tiles.Print(dispatchRegistry);

            _output.WriteLine(expected);
            _output.WriteLine('='.ToPaddedString(10));
            _output.WriteLine(actual);

            Assert.Equal(expected, actual);
        }
    }
}
