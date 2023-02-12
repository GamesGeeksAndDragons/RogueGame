using AssetsTests.Helpers;

namespace AssetsTests.ActionTests
{
    static class ActionTestsDefinitions
    {
        class TeleportWithOneValidSpace : MazeExpectations
        {
            public TeleportWithOneValidSpace()
            {
            StartingMaze = @"
████████████
████████████
████████████
████╔═╗█████
████║¹║█████
████╚═╝█████
████████████
████████████
████████████
";

            ExpectedMaze = @"
 |012345678901| 
----------------
0|████████████|0
1|████████████|1
2|████████████|2
3|████╔═╗█████|3
4|████║@║█████|4
5|████╚═╝█████|5
6|████████████|6
7|████████████|7
8|████████████|8
----------------
 |012345678901| 
";
            }
        }

        class TeleportWithSixValidSpaces : MazeExpectations
        {
            public TeleportWithSixValidSpaces()
            {
            StartingMaze = @"
████████████
████████████
████████████
████████████
████╔══╗████
████║²²║████
████║²²║████
████╚══╝████
████████████
";

            ExpectedMaze = @"
 |012345678901| 
----------------
0|████████████|0
1|████████████|1
2|████████████|2
3|████████████|3
4|████╔══╗████|4
5|████║M²║████|5
6|████║²@║████|6
7|████╚══╝████|7
8|████████████|8
----------------
 |012345678901| 
";
            }
        }

        class TeleportWithWithTwoSpacesAndTwoCharacters : MazeExpectations
        {
            public TeleportWithWithTwoSpacesAndTwoCharacters()
            {
            StartingMaze = @"
████████████
████████████
████████████
████╔══╗████
████║αα║████
████╚══╝████
████████████
████████████
████████████
";

            ExpectedMaze = @"
 |012345678901| 
----------------
0|████████████|0
1|████████████|1
2|████████████|2
3|████╔══╗████|3
4|████║@M║████|4
5|████╚══╝████|5
6|████████████|6
7|████████████|7
8|████████████|8
----------------
 |012345678901| 
";
            }
        }

        class TeleportWithWithSixSpacesAndTwoCharacters : MazeExpectations
        {
            public TeleportWithWithSixSpacesAndTwoCharacters()
            {
            StartingMaze = @"
████████████
████████████
████████████
████████████
████╔══╗████
████║ββ║████
████║ββ║████
████╚══╝████
████████████
";

    ExpectedMaze = @"
 |012345678901| 
----------------
0|████████████|0
1|████████████|1
2|████████████|2
3|████████████|3
4|████╔══╗████|4
5|████║Mβ║████|5
6|████║β@║████|6
7|████╚══╝████|7
8|████████████|8
----------------
 |012345678901| 
";
}
        }

        internal static IMazeExpectations GetExpectations(int testNumber)
        {
            switch (testNumber)
            {
                case 1: return new TeleportWithOneValidSpace();
                case 2: return new TeleportWithSixValidSpaces();
                case 3: return new TeleportWithWithTwoSpacesAndTwoCharacters();
                case 4: return new TeleportWithWithSixSpacesAndTwoCharacters();
            }

            MazeTestHelpers.ThrowUnknownTest(testNumber);
            return null;
        }
    }
}
