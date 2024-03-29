﻿using AssetsTests.Helpers;

namespace AssetsTests.MazeTests.Helpers
{
    static class ThreeConnectingLinesTestDefinitions
    {
        class Test1 : MazeExpectations
        {
            public Test1()
            {
                StartingMaze = @"
██████████████████
██████████████████
██████████████████
███╔═╗██████╔═╗███
███║ρ1██████║π║███
███╚═╝██████╚═╝███
██████████████████
██████████████████
██████████████████
███╔═╗██████╔═╗███
███║ο║██████1ξ║███
███╚═╝██████╚═╝███
██████████████████
██████████████████
██████████████████
";

                ExpectedMaze = @"
  |012345678901234567|  
------------------------
0 |██████████████████|0 
1 |██████████████████|1 
2 |██████████████████|2 
3 |███╔═╗██████╔═╗███|3 
4 |███║ρ1⁰⁰████║π║███|4 
5 |███╚═╝█⁰████╚═╝███|5 
6 |███████⁰██████████|6 
7 |███████⁰██████████|7 
8 |███████⁰██████████|8 
9 |███╔═╗█⁰████╔═╗███|9 
10|███║ο║█⁰⁰⁰⁰⁰1ξ║███|10
11|███╚═╝██████╚═╝███|11
12|██████████████████|12
13|██████████████████|13
14|██████████████████|14
------------------------
  |012345678901234567|  
";
            }
    }

        class Test2 : MazeExpectations
        {
            public Test2()
            {
                StartingMaze = @"
██████████████████
██████████████████
██████████████████
███╔═╗██████╔═╗███
███║ν║██████1μ║███
███╚═╝██████╚═╝███
██████████████████
██████████████████
██████████████████
███╔═╗██████╔═╗███
███║λ1██████║κ║███
███╚═╝██████╚═╝███
██████████████████
██████████████████
██████████████████
";

                ExpectedMaze = @"
  |012345678901234567|  
------------------------
0 |██████████████████|0 
1 |██████████████████|1 
2 |██████████████████|2 
3 |███╔═╗██████╔═╗███|3 
4 |███║ν║████⁰⁰1μ║███|4 
5 |███╚═╝████⁰█╚═╝███|5 
6 |██████████⁰███████|6 
7 |██████████⁰███████|7 
8 |██████████⁰███████|8 
9 |███╔═╗████⁰█╔═╗███|9 
10|███║λ1⁰⁰⁰⁰⁰█║κ║███|10
11|███╚═╝██████╚═╝███|11
12|██████████████████|12
13|██████████████████|13
14|██████████████████|14
------------------------
  |012345678901234567|  
";
            }
}

        class Test3 : MazeExpectations
        {
            public Test3()
            {
                StartingMaze = @"
██████████████████
██████████████████
██████████████████
███╔═╗██████╔═╗███
███║ι2██████1θ║███
███╚═╝██████╚═╝███
██████████████████
██████████████████
██████████████████
███╔═╗██████╔═╗███
███║η1██████2ζ║███
███╚═╝██████╚═╝███
██████████████████
██████████████████
██████████████████
";

                ExpectedMaze = @"
  |012345678901234567|  
------------------------
0 |██████████████████|0 
1 |██████████████████|1 
2 |██████████████████|2 
3 |███╔═╗██████╔═╗███|3 
4 |███║ι2⁰⁰██⁰⁰1θ║███|4 
5 |███╚═╝█⁰██⁰█╚═╝███|5 
6 |███████⁰██⁰███████|6 
7 |███████⁰██⁰███████|7 
8 |███████⁰██⁰███████|8 
9 |███╔═╗█⁰██⁰█╔═╗███|9 
10|███║η1⁰⁰⁰⁰⁰⁰2ζ║███|10
11|███╚═╝██████╚═╝███|11
12|██████████████████|12
13|██████████████████|13
14|██████████████████|14
------------------------
  |012345678901234567|  
";
            }
        }

        class Test4 : MazeExpectations
        {
            public Test4()
            {
                StartingMaze = @"
█████████████
█████████████
█████████████
███╔═╗███████
███2ε1███████
███╚═╝███████
█████████████
█████████████
█████████████
███████╔═╗███
███████2δ1███
███████╚═╝███
█████████████
█████████████
█████████████
";

                ExpectedMaze = @"
  |0123456789012|  
-------------------
0 |█████████████|0 
1 |█████████████|1 
2 |█████████████|2 
3 |███╔═╗███████|3 
4 |█⁰⁰2ε1⁰⁰⁰⁰⁰⁰█|4 
5 |█⁰█╚═╝█████⁰█|5 
6 |█⁰█████████⁰█|6 
7 |█⁰█████████⁰█|7 
8 |█⁰█████████⁰█|8 
9 |█⁰█████╔═╗█⁰█|9 
10|█⁰⁰⁰⁰⁰⁰2δ1⁰⁰█|10
11|███████╚═╝███|11
12|█████████████|12
13|█████████████|13
14|█████████████|14
-------------------
  |0123456789012|  
";
            }
        }

        internal static IMazeExpectations GetExpectations(int testNumber)
        {
            switch (testNumber)
            {
                case 1: return new Test1();
                case 2: return new Test2();
                case 3: return new Test3();
                case 4: return new Test4();
            }

            MazeTestHelpers.ThrowUnknownTest(testNumber);
            return null;
        }
    }
}
