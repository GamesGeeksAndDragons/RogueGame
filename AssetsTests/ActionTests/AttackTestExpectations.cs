using AssetsTests.Helpers;
using Utils.Display;

namespace AssetsTests.ActionTests;

public enum AttackTest
{
    MeAttacksOneMonster
}

internal static class AttackTestExpectations
{
    class MeAndOneMonster : MazeExpectations
    {
        public MeAndOneMonster()
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
6 |████║¹¹¹¹¹@¹¹¹¹¹║█████|6 
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
            string me = ActorCoordinates(CharacterDisplay.Me, 6, 10);
            AddCharacter(me);
        }
    }
}
