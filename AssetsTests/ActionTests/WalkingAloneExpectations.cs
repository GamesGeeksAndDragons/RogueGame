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
    static readonly string CommonStartingMaze = @"
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
    class WalkNorthExpectations : MazeAndCharacterExpectations
    {
        public WalkNorthExpectations()
        {
            StartingMaze = CommonStartingMaze;

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
        }

        public override string Me => ActorCoordinates(CharacterDisplay.Me, 10, 10);
    }

    class WalkSouthExpectations : MazeAndCharacterExpectations
    {
        public WalkSouthExpectations()
        {
            StartingMaze = CommonStartingMaze;

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
        }
        public override string Me => ActorCoordinates(CharacterDisplay.Me, 3, 11);
    }

    class WalkEastExpectations : MazeAndCharacterExpectations
    {
        public WalkEastExpectations()
        {
            StartingMaze = CommonStartingMaze;

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
        }

        public override string Me => ActorCoordinates(CharacterDisplay.Me, 7, 5);
    }

    class WalkWestExpectations : MazeAndCharacterExpectations
    {
        public WalkWestExpectations()
        {
            StartingMaze = CommonStartingMaze;

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
        }

        public override string Me => ActorCoordinates(CharacterDisplay.Me, 6, 15);
    }

    class WalkNorthEastExpectations : MazeAndCharacterExpectations
    {
        public WalkNorthEastExpectations()
        {
            StartingMaze = CommonStartingMaze;

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
        }

        public override string Me => ActorCoordinates(CharacterDisplay.Me, 10, 5);
    }

    class WalkSouthEastExpectations : MazeAndCharacterExpectations
    {
        public WalkSouthEastExpectations()
        {
            StartingMaze = CommonStartingMaze;

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
        }

        public override string Me => ActorCoordinates(CharacterDisplay.Me, 3, 5);
    }

    class WalkMultipleDirectionsExpectations : MazeAndCharacterExpectations
    {
        public WalkMultipleDirectionsExpectations()
        {
            StartingMaze = CommonStartingMaze;

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
        }

        public override string Me => ActorCoordinates(CharacterDisplay.Me, 3, 5);
    }

    public static ICharacterExpectations GetExpectations(this WalkAloneTest test)
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