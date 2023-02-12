﻿using Assets.Level;
using AssetsTests.Fakes;
using AssetsTests.Helpers;
using Utils.Enums;

namespace AssetsTests.ActionTests
{
    public enum MoveActionTest
    {
        MoveNorth = 0,
        MoveSouth,
        MoveEast,
        MoveWest,
        MoveNorthEast,
        MoveSouthEast
    }

    static class MoveActionExpectations
    {
        class MoveNorthExpectations : MazeExpectations
        {
            public MoveNorthExpectations()
            {
                StartingMaze = @"
██████████████████████
██████████████████████
████╔═══════════╗█████
████║¹¹¹¹¹¹¹¹¹¹¹║█████
████║¹¹¹¹¹¹¹¹¹¹¹║█████
████║¹¹¹¹¹¹¹¹¹¹¹║█████
████║¹¹¹¹¹¹¹¹¹¹¹║█████
████║¹¹¹¹¹¹¹¹¹¹¹║█████
████║¹¹¹¹¹¹¹¹¹¹¹║█████
████║¹¹¹¹¹¹¹¹¹¹¹║█████
████║¹¹¹¹¹¹¹¹¹¹¹║█████
████╚═══════════╝█████
██████████████████████
██████████████████████
";
                ExpectedMaze = @"
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
------------------------------------
  |012345678901234567890123456789|  
";

            }
        }

        class MoveSouthExpectations : MazeExpectations
        {
            public MoveSouthExpectations()
            {
                ExpectedMaze = @"
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
17|███████║¹¹¹M¹¹¹¹║█████████████|17
18|███████║¹¹¹¹¹¹¹¹║█████████████|18
19|███████║¹¹¹¹¹¹¹¹║█████████████|19
20|███████║¹¹¹¹¹¹¹¹║█████████████|20
21|███████║¹¹¹¹¹¹¹¹║█████████████|21
22|███████║¹¹¹¹¹¹¹¹║█████████████|22
23|███████║¹¹¹¹¹¹¹¹║█████████████|23
24|███████║¹¹¹¹@¹¹¹║█████████████|24
25|███████╚════════╝█████████████|25
26|██████████████████████████████|26
27|██████████████████████████████|27
28|██████████████████████████████|28
29|██████████████████████████████|29
------------------------------------
  |012345678901234567890123456789|  
";

            }
        }

        class MoveEastExpectations : MazeExpectations
        {
            public MoveEastExpectations()
            {
                ExpectedMaze = @"
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
17|███████║¹¹¹M¹¹¹¹║█████████████|17
18|███████║¹¹¹¹¹¹¹¹║█████████████|18
19|███████║¹¹¹¹¹¹¹@║█████████████|19
20|███████║¹¹¹¹¹¹¹¹║█████████████|20
21|███████║¹¹¹¹¹¹¹¹║█████████████|21
22|███████║¹¹¹¹¹¹¹¹║█████████████|22
23|███████║¹¹¹¹¹¹¹¹║█████████████|23
24|███████║¹¹¹¹¹¹¹¹║█████████████|24
25|███████╚════════╝█████████████|25
26|██████████████████████████████|26
27|██████████████████████████████|27
28|██████████████████████████████|28
29|██████████████████████████████|29
------------------------------------
  |012345678901234567890123456789|  
";

            }
        }

        class MoveWestExpectations : MazeExpectations
        {
            public MoveWestExpectations()
            {
                ExpectedMaze = @"
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
17|███████║¹¹¹M¹¹¹¹║█████████████|17
18|███████║¹¹¹¹¹¹¹¹║█████████████|18
19|███████║@¹¹¹¹¹¹¹║█████████████|19
20|███████║¹¹¹¹¹¹¹¹║█████████████|20
21|███████║¹¹¹¹¹¹¹¹║█████████████|21
22|███████║¹¹¹¹¹¹¹¹║█████████████|22
23|███████║¹¹¹¹¹¹¹¹║█████████████|23
24|███████║¹¹¹¹¹¹¹¹║█████████████|24
25|███████╚════════╝█████████████|25
26|██████████████████████████████|26
27|██████████████████████████████|27
28|██████████████████████████████|28
29|██████████████████████████████|29
------------------------------------
  |012345678901234567890123456789|  
";
            }
        }

        class MoveNorthEastExpectations : MazeExpectations
        {
            public MoveNorthEastExpectations()
            {
                ExpectedMaze = @"
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
17|███████║¹¹¹M¹¹@¹║█████████████|17
18|███████║¹¹¹¹¹¹¹¹║█████████████|18
19|███████║¹¹¹¹¹¹¹¹║█████████████|19
20|███████║¹¹¹¹¹¹¹¹║█████████████|20
21|███████║¹¹¹¹¹¹¹¹║█████████████|21
22|███████║¹¹¹¹¹¹¹¹║█████████████|22
23|███████║¹¹¹¹¹¹¹¹║█████████████|23
24|███████║¹¹¹¹¹¹¹¹║█████████████|24
25|███████╚════════╝█████████████|25
26|██████████████████████████████|26
27|██████████████████████████████|27
28|██████████████████████████████|28
29|██████████████████████████████|29
------------------------------------
  |012345678901234567890123456789|  
";

            }
        }

        class MoveSouthEastExpectations : MazeExpectations
        {
            public MoveSouthEastExpectations()
            {
                ExpectedMaze = @"
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
17|███████║¹¹¹M¹¹¹¹║█████████████|17
18|███████║¹¹¹¹¹¹¹¹║█████████████|18
19|███████║¹¹¹¹¹¹¹¹║█████████████|19
20|███████║¹¹¹¹¹¹¹¹║█████████████|20
21|███████║¹¹¹¹¹¹¹¹║█████████████|21
22|███████║¹¹¹¹¹¹¹@║█████████████|22
23|███████║¹¹¹¹¹¹¹¹║█████████████|23
24|███████║¹¹¹¹¹¹¹¹║█████████████|24
25|███████╚════════╝█████████████|25
26|██████████████████████████████|26
27|██████████████████████████████|27
28|██████████████████████████████|28
29|██████████████████████████████|29
------------------------------------
  |012345678901234567890123456789|  
";


            }
        }

        public static IMazeExpectations GetExpectations(this MoveActionTest test)
        {
            switch (test)
            {
                case MoveActionTest.MoveNorth: return new MoveNorthExpectations();
                case MoveActionTest.MoveSouth: return new MoveSouthExpectations();
                case MoveActionTest.MoveEast: return new MoveEastExpectations();
                case MoveActionTest.MoveWest: return new MoveWestExpectations();
                case MoveActionTest.MoveNorthEast: return new MoveNorthEastExpectations();
                case MoveActionTest.MoveSouthEast: return new MoveSouthEastExpectations();
            }

            MazeTestHelpers.ThrowUnknownTest((int)test);;
            return null;
        }
    }

    public class MoveActionTests : MazeTestHelper
    {
        public MoveActionTests(ITestOutputHelper output)
        : base(output)
        {
        }

        internal static string GetExpectation(int testNum)
        {
            switch (testNum)
            {
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
17|███████║¹¹@M¹¹¹¹║█████████████|17
18|███████║¹¹¹¹¹¹¹¹║█████████████|18
19|███████║¹¹¹¹¹¹¹¹║█████████████|19
20|███████║¹¹¹¹¹¹¹¹║█████████████|20
21|███████║¹¹¹¹¹¹¹¹║█████████████|21
22|███████║¹¹¹¹¹¹¹¹║█████████████|22
23|███████║¹¹¹¹¹¹¹¹║█████████████|23
24|███████║¹¹¹¹¹¹¹¹║█████████████|24
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

        [Theory]
        [InlineData(MoveActionTest.MoveNorth, Compass8Points.North, Compass8Points.North, Compass8Points.North, Compass8Points.North, Compass8Points.North, Compass8Points.North, Compass8Points.North)]
        public void Move_Me_Test(MoveActionTest test, params Compass8Points[] directions)
        {
            var expectations = test.GetExpectations();
            base.TestArrange(expectations);
            ArrangeMovingMeCharacter();

            AssertTest(GameLevel, expectations);

            void ArrangeMovingMeCharacter()
            {
                GameLevel.Dispatcher.Dispatch();
                var start = GameLevel.Print(DispatchRegistry);
                Output.WriteLine(BuilderTestHelpers.Divider + " start " + BuilderTestHelpers.Divider);
                Output.WriteLine(start);

                foreach (var direction in directions)
                {
                    GameLevel.Dispatcher.EnqueueMove(GameLevel, GameLevel.Me, direction);
                }
            }
        }

        protected override void TestAct()
        {
            Dispatcher.Dispatch();
        }

        //[Fact]
        //public void Move_MeNorth_WillNotLeaveATrail()
        //{
        //    Move_Me_Test(MoveActionTest.MoveNorth, Compass8Points.North, Compass8Points.North, Compass8Points.North, Compass8Points.North, Compass8Points.North);
        //}

        //[Fact]
        //public void Move_MeSouth_WillNotLeaveATrail()
        //{
        //    Move_Me_Test(MoveActionTest.MoveSouth, Compass8Points.South, Compass8Points.South, Compass8Points.South, Compass8Points.South, Compass8Points.South);
        //}

        //[Fact]
        //public void Move_MeEast_WillNotLeaveATrail()
        //{
        //    Move_Me_Test(MoveActionTest.MoveEast, Compass8Points.East, Compass8Points.East, Compass8Points.East, Compass8Points.East, Compass8Points.East, Compass8Points.East);
        //}

        //[Fact]
        //public void Move_MeWest_WillNotLeaveATrail()
        //{
        //    Move_Me_Test(MoveActionTest.MoveWest, Compass8Points.West, Compass8Points.West, Compass8Points.West, Compass8Points.West, Compass8Points.West, Compass8Points.West);
        //}

        //[Fact]
        //public void Move_MeNorthEast_WillNotLeaveATrail()
        //{
        //    Move_Me_Test(MoveActionTest.MoveNorthEast, Compass8Points.NorthEast, Compass8Points.NorthEast, Compass8Points.NorthEast, Compass8Points.NorthEast, Compass8Points.NorthEast, Compass8Points.NorthEast);
        //}

        //[Fact]
        //public void Move_MeSouthEast_WillNotLeaveATrail()
        //{
        //    Move_Me_Test(MoveActionTest.MoveSouthEast, Compass8Points.SouthEast, Compass8Points.SouthEast, Compass8Points.SouthEast, Compass8Points.SouthEast, Compass8Points.SouthEast, Compass8Points.SouthEast);
        //}

        //[Fact]
        //public void Move_Me_AroundCharacterWillNotWalkThoughIt()
        //{
        //    Move_Me_Test(7, Compass8Points.North, Compass8Points.North, Compass8Points.West, Compass8Points.SouthWest, Compass8Points.North, Compass8Points.NorthWest, Compass8Points.East);
        //}
    }
}
