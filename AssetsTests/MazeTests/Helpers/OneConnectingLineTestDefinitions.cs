﻿using AssetsTests.Helpers;

namespace AssetsTests.MazeTests.Helpers;

internal static class OneConnectingLineTestDefinitions
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
███║ω║██████║ψ║███
███╚1╝██████╚2╝███
██████████████████
██████████████████
██████████████████
███╔1╗██████╔2╗███
███║χ║██████║φ║███
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
4 |███║ω║██████║ψ║███|4 
5 |███╚1╝██████╚2╝███|5 
6 |████⁰████████⁰████|6 
7 |████⁰████████⁰████|7 
8 |████⁰████████⁰████|8 
9 |███╔1╗██████╔2╗███|9 
10|███║χ║██████║φ║███|10
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
███║υ1██████1τ║███
███╚═╝██████╚═╝███
██████████████████
██████████████████
██████████████████
███╔═╗██████╔═╗███
███║σ2██████2ς║███
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
4 |███║υ1⁰⁰⁰⁰⁰⁰1τ║███|4 
5 |███╚═╝██████╚═╝███|5 
6 |██████████████████|6 
7 |██████████████████|7 
8 |██████████████████|8 
9 |███╔═╗██████╔═╗███|9 
10|███║σ2⁰⁰⁰⁰⁰⁰2ς║███|10
11|███╚═╝██████╚═╝███|11
12|██████████████████|12
13|██████████████████|13
14|██████████████████|14
------------------------
  |012345678901234567|  
";
        }
    }

    internal static IMazeExpectations GetExpectations(int testNumber)
    {
        switch (testNumber)
        {
            case 1: return new Test1();
            case 2: return new Test2();
        }

        MazeTestHelpers.ThrowUnknownTest(testNumber);
        return null;
    }
}