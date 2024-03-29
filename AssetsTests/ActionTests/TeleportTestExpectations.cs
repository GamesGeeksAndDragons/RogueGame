﻿#nullable enable
using AssetsTests.Helpers;
using Utils.Display;

namespace AssetsTests.ActionTests
{
    static class TeleportTestExpectations
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

                var me = ActorCoordinates(CharacterDisplay.Me);
                AddCharacter(me);
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
5|████║@²║████|5
6|████║²M║████|6
7|████╚══╝████|7
8|████████████|8
----------------
 |012345678901| 
";
                AddCharacter(ActorCoordinates(CharacterDisplay.Me));
                AddCharacter(ActorCoordinates(CharacterDisplay.DebugMonster));
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
4|████║M@║████|4
5|████╚══╝████|5
6|████████████|6
7|████████████|7
8|████████████|8
----------------
 |012345678901| 
";

                AddCharacter(ActorCoordinates(CharacterDisplay.Me));
                AddCharacter(ActorCoordinates(CharacterDisplay.DebugMonster));
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
5|████║@β║████|5
6|████║βM║████|6
7|████╚══╝████|7
8|████████████|8
----------------
 |012345678901| 
";
            AddCharacter(ActorCoordinates(CharacterDisplay.Me));
            AddCharacter(ActorCoordinates(CharacterDisplay.DebugMonster));
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
            return null!;
        }
    }
}
