using AssetsTests.Helpers;

namespace AssetsTests.ActionTests
{
    static class ActionTestsDefinitions
    {
        class TeleportWithOneValidSpace : MazeExpectations
        {
            protected override string Start => @"
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

            protected override string Expected => @"
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

        class TeleportWithSixValidSpaces : MazeExpectations
        {
            protected override string Start => @"
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

            protected override string Expected => @"
 |012345678901| 
----------------
0|████████████|0
1|████████████|1
2|████████████|2
3|████████████|3
4|████╔══╗████|4
5|████║²²║████|5
6|████║@²║████|6
7|████╚══╝████|7
8|████████████|8
----------------
 |012345678901| 
";
        }

        class TeleportWithWithTwoSpacesAndTwoCharacters : MazeExpectations
        {
            protected override string Start => @"
████████████
████████████
████████████
████╔══╗████
████║ᵏᵏ║████
████╚══╝████
████████████
████████████
████████████
";

            protected override string Expected => @"
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

        class TeleportWithWithSixSpacesAndTwoCharacters : MazeExpectations
        {
            protected override string Start => @"
████████████
████████████
████████████
████████████
████╔══╗████
████║ᵃᵃ║████
████║ᵃᵃ║████
████╚══╝████
████████████
";

            protected override string Expected => @"
 |012345678901| 
----------------
0|████████████|0
1|████████████|1
2|████████████|2
3|████████████|3
4|████╔══╗████|4
5|████║ᵃ@║████|5
6|████║Mᵃ║████|6
7|████╚══╝████|7
8|████████████|8
----------------
 |012345678901| 
";
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

            testNumber.ThrowUnknownTest();
            return null;
        }
    }
}
