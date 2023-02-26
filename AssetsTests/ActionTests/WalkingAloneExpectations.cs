#nullable enable
using AssetsTests.Helpers;
using Utils.Display;

namespace AssetsTests.ActionTests;

public enum WalkAloneTest
{
    North = 0,
    South,
    East,
    West,
    NorthEast,
    SouthEast,
    MultipleDirections
}

static class WalkingAloneExpectations
{
    class WalkNorthExpectations : MazeExpectations
    {
        public WalkNorthExpectations()
        {
            StartingMaze = CommonMaze.EmptyMaze;

            ExpectedMaze = @"
  |0123456789012345678901|  
----------------------------
0 |██████████████████████|0 
1 |██████████████████████|1 
2 |████╔═══════════╗█████|2 
3 |████║¹¹¹¹¹@¹¹¹¹¹║█████|3 
4 |████║¹¹¹¹¹¹¹¹¹¹¹║█████|4 
5 |████║¹¹¹¹¹¹¹¹¹¹¹║█████|5 
6 |████║¹¹¹¹¹¹¹¹¹¹¹║█████|6 
7 |████║¹¹¹¹¹¹¹¹¹¹¹║█████|7 
8 |████║¹¹¹¹¹¹¹¹¹¹¹║█████|8 
9 |████║¹¹¹¹¹¹¹¹¹¹¹║█████|9 
10|████║¹¹¹¹¹¹¹¹¹¹¹║█████|10
11|████╚═══════════╝█████|11
12|██████████████████████|12
13|██████████████████████|13
----------------------------
  |0123456789012345678901|  
";
            string me = ActorCoordinates(CharacterDisplay.Me, 10, 10);
            AddCharacter(me);
        }
}

    class WalkSouthExpectations : MazeExpectations
    {
        public WalkSouthExpectations()
        {
            StartingMaze = CommonMaze.EmptyMaze;

            ExpectedMaze = @"
  |0123456789012345678901|  
----------------------------
0 |██████████████████████|0 
1 |██████████████████████|1 
2 |████╔═══════════╗█████|2 
3 |████║¹¹¹¹¹¹¹¹¹¹¹║█████|3 
4 |████║¹¹¹¹¹¹¹¹¹¹¹║█████|4 
5 |████║¹¹¹¹¹¹¹¹¹¹¹║█████|5 
6 |████║¹¹¹¹¹¹¹¹¹¹¹║█████|6 
7 |████║¹¹¹¹¹¹¹¹¹¹¹║█████|7 
8 |████║¹¹¹¹¹¹¹¹¹¹¹║█████|8 
9 |████║¹¹¹¹¹¹¹¹¹¹¹║█████|9 
10|████║¹¹¹¹¹¹@¹¹¹¹║█████|10
11|████╚═══════════╝█████|11
12|██████████████████████|12
13|██████████████████████|13
----------------------------
  |0123456789012345678901|  
";
            string me = ActorCoordinates(CharacterDisplay.Me, 3, 11);
            AddCharacter(me);
        }
    }

    class WalkEastExpectations : MazeExpectations
    {
        public WalkEastExpectations()
        {
            StartingMaze = CommonMaze.EmptyMaze;

            ExpectedMaze = @"
  |0123456789012345678901|  
----------------------------
0 |██████████████████████|0 
1 |██████████████████████|1 
2 |████╔═══════════╗█████|2 
3 |████║¹¹¹¹¹¹¹¹¹¹¹║█████|3 
4 |████║¹¹¹¹¹¹¹¹¹¹¹║█████|4 
5 |████║¹¹¹¹¹¹¹¹¹¹¹║█████|5 
6 |████║¹¹¹¹¹¹¹¹¹¹¹║█████|6 
7 |████║¹¹¹¹¹¹¹¹¹¹@║█████|7 
8 |████║¹¹¹¹¹¹¹¹¹¹¹║█████|8 
9 |████║¹¹¹¹¹¹¹¹¹¹¹║█████|9 
10|████║¹¹¹¹¹¹¹¹¹¹¹║█████|10
11|████╚═══════════╝█████|11
12|██████████████████████|12
13|██████████████████████|13
----------------------------
  |0123456789012345678901|  
";

            string me = ActorCoordinates(CharacterDisplay.Me, 7, 5);
            AddCharacter(me);
        }
    }

    class WalkWestExpectations : MazeExpectations
    {
        public WalkWestExpectations()
        {
            StartingMaze = CommonMaze.EmptyMaze;

            ExpectedMaze = @"
  |0123456789012345678901|  
----------------------------
0 |██████████████████████|0 
1 |██████████████████████|1 
2 |████╔═══════════╗█████|2 
3 |████║¹¹¹¹¹¹¹¹¹¹¹║█████|3 
4 |████║¹¹¹¹¹¹¹¹¹¹¹║█████|4 
5 |████║¹¹¹¹¹¹¹¹¹¹¹║█████|5 
6 |████║@¹¹¹¹¹¹¹¹¹¹║█████|6 
7 |████║¹¹¹¹¹¹¹¹¹¹¹║█████|7 
8 |████║¹¹¹¹¹¹¹¹¹¹¹║█████|8 
9 |████║¹¹¹¹¹¹¹¹¹¹¹║█████|9 
10|████║¹¹¹¹¹¹¹¹¹¹¹║█████|10
11|████╚═══════════╝█████|11
12|██████████████████████|12
13|██████████████████████|13
----------------------------
  |0123456789012345678901|  
";

            string me = ActorCoordinates(CharacterDisplay.Me, 6, 15);
            AddCharacter(me);
        }
    }

    class WalkNorthEastExpectations : MazeExpectations
    {
        public WalkNorthEastExpectations()
        {
            StartingMaze = CommonMaze.EmptyMaze;

            ExpectedMaze = @"
  |0123456789012345678901|  
----------------------------
0 |██████████████████████|0 
1 |██████████████████████|1 
2 |████╔═══════════╗█████|2 
3 |████║¹¹¹¹¹¹¹@¹¹¹║█████|3 
4 |████║¹¹¹¹¹¹¹¹¹¹¹║█████|4 
5 |████║¹¹¹¹¹¹¹¹¹¹¹║█████|5 
6 |████║¹¹¹¹¹¹¹¹¹¹¹║█████|6 
7 |████║¹¹¹¹¹¹¹¹¹¹¹║█████|7 
8 |████║¹¹¹¹¹¹¹¹¹¹¹║█████|8 
9 |████║¹¹¹¹¹¹¹¹¹¹¹║█████|9 
10|████║¹¹¹¹¹¹¹¹¹¹¹║█████|10
11|████╚═══════════╝█████|11
12|██████████████████████|12
13|██████████████████████|13
----------------------------
  |0123456789012345678901|  
";

            string me = ActorCoordinates(CharacterDisplay.Me, 10, 5);
            AddCharacter(me);
        }
    }

    class WalkSouthEastExpectations : MazeExpectations
    {
        public WalkSouthEastExpectations()
        {
            StartingMaze = CommonMaze.EmptyMaze;

            ExpectedMaze = @"
  |0123456789012345678901|  
----------------------------
0 |██████████████████████|0 
1 |██████████████████████|1 
2 |████╔═══════════╗█████|2 
3 |████║¹¹¹¹¹¹¹¹¹¹¹║█████|3 
4 |████║¹¹¹¹¹¹¹¹¹¹¹║█████|4 
5 |████║¹¹¹¹¹¹¹¹¹¹¹║█████|5 
6 |████║¹¹¹¹¹¹¹¹¹¹¹║█████|6 
7 |████║¹¹¹¹¹¹¹¹¹¹¹║█████|7 
8 |████║¹¹¹¹¹¹¹¹¹¹¹║█████|8 
9 |████║¹¹¹¹¹¹¹¹¹¹¹║█████|9 
10|████║¹¹¹¹¹¹¹@¹¹¹║█████|10
11|████╚═══════════╝█████|11
12|██████████████████████|12
13|██████████████████████|13
----------------------------
  |0123456789012345678901|  
";

            var me = ActorCoordinates(CharacterDisplay.Me, 3, 5);
            AddCharacter(me);
        }
    }

    class WalkMultipleDirectionsExpectations : MazeExpectations
    {
        public WalkMultipleDirectionsExpectations()
        {
            StartingMaze = CommonMaze.EmptyMaze;

            ExpectedMaze = @"
  |0123456789012345678901|  
----------------------------
0 |██████████████████████|0 
1 |██████████████████████|1 
2 |████╔═══════════╗█████|2 
3 |████║¹¹¹¹¹¹¹¹¹¹¹║█████|3 
4 |████║¹¹¹¹¹¹¹¹¹¹¹║█████|4 
5 |████║¹¹¹¹¹¹¹¹¹¹¹║█████|5 
6 |████║@¹¹¹¹¹¹¹¹¹¹║█████|6 
7 |████║¹¹¹¹¹¹¹¹¹¹¹║█████|7 
8 |████║¹¹¹¹¹¹¹¹¹¹¹║█████|8 
9 |████║¹¹¹¹¹¹¹¹¹¹¹║█████|9 
10|████║¹¹¹¹¹¹¹¹¹¹¹║█████|10
11|████╚═══════════╝█████|11
12|██████████████████████|12
13|██████████████████████|13
----------------------------
  |0123456789012345678901|  
";

            string me = ActorCoordinates(CharacterDisplay.Me, 3, 5);
            AddCharacter(me);
        }
    }

    public static IMazeExpectations GetExpectations(this WalkAloneTest test)
    {
        switch (test)
        {
            case WalkAloneTest.North: return new WalkNorthExpectations();
            case WalkAloneTest.South: return new WalkSouthExpectations();
            case WalkAloneTest.East: return new WalkEastExpectations();
            case WalkAloneTest.West: return new WalkWestExpectations();
            case WalkAloneTest.NorthEast: return new WalkNorthEastExpectations();
            case WalkAloneTest.SouthEast: return new WalkSouthEastExpectations();
            case WalkAloneTest.MultipleDirections: return new WalkMultipleDirectionsExpectations();
        }

        MazeTestHelpers.ThrowUnknownTest((int)test); ;
        return null!;
    }
}