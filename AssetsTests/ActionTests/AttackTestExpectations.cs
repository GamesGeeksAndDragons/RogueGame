using AssetsTests.Helpers;
using Utils.Display;

namespace AssetsTests.ActionTests;

enum AttackTest
{
    AttackMonster
}

internal static class AttackTestExpectations
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
}
