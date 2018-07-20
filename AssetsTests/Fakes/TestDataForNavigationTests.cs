using System;
using Utils.Enums;
using Utils.Random;

namespace AssetsTests.Fakes
{
    public static class TestDataForNavigationTests
    {
        public static int GetNumBlocks(BlockNavigationTests.Test test)
        {
            switch (test)
            {
                case BlockNavigationTests.Test.TopLeft:
                case BlockNavigationTests.Test.TopRight:
                case BlockNavigationTests.Test.BottomRight:
                case BlockNavigationTests.Test.BottomLeft:
                    return 2;
                case BlockNavigationTests.Test.Cornered:
                    return 5;
            }

            throw new ArgumentException($"Didn't have Blocks for [{test}]");
        }

        public static string GetExpected(BlockNavigationTests.Test test)
        {
            switch (test)
            {
                case (BlockNavigationTests.Test)(-1):
                    return
                        "..|" + Environment.NewLine +
                        "..|" + Environment.NewLine;
                case BlockNavigationTests.Test.TopLeft:
                    return
                        "**|" + Environment.NewLine +
                        "..|" + Environment.NewLine;
                case BlockNavigationTests.Test.TopRight:
                    return
                        ".*|" + Environment.NewLine +
                        ".*|" + Environment.NewLine;
                case BlockNavigationTests.Test.BottomRight:
                    return
                        "..|" + Environment.NewLine +
                        "**|" + Environment.NewLine;
                case BlockNavigationTests.Test.BottomLeft:
                    return
                        "*.|" + Environment.NewLine +
                        "*.|" + Environment.NewLine;
                case BlockNavigationTests.Test.Cornered:
                    return
                        ".....|" + Environment.NewLine +
                        ".....|" + Environment.NewLine +
                        ".....|" + Environment.NewLine +
                        "...**|" + Environment.NewLine +
                        "..***|" + Environment.NewLine;
            }

            throw new ArgumentException($"Didn't have Expected Layout for [{test}]");
        }

        //TODO reconsider
        /*internal static IRandomNumberGenerator GetGenerator(BlockNavigationTests.Test test)
        {
            var generator = new FakeRandomNumberGenerator();

            switch (test)
            {
                case BlockNavigationTests.Test.TopLeft:
                    generator.PopulateEnum(Compass4Points.West, Compass4Points.North, Compass4Points.East, Compass4Points.East, Compass4Points.East);
                    generator.PopulateDice(0, 0);
                    break;
                case BlockNavigationTests.Test.TopRight:
                    generator.PopulateEnum(Compass4Points.North, Compass4Points.East, Compass4Points.South, Compass4Points.East, Compass4Points.East);
                    generator.PopulateDice(1, 0);
                    break;
                case BlockNavigationTests.Test.BottomRight:
                    generator.PopulateEnum(Compass4Points.East, Compass4Points.South, Compass4Points.West, Compass4Points.East, Compass4Points.East);
                    generator.PopulateDice(1, 1);
                    break;
                case BlockNavigationTests.Test.BottomLeft:
                    generator.PopulateEnum(Compass4Points.South, Compass4Points.West, Compass4Points.North, Compass4Points.East, Compass4Points.East);
                    generator.PopulateDice(0, 1);
                    break;
                case BlockNavigationTests.Test.Cornered:
                    generator.PopulateEnum(Compass4Points.West, Compass4Points.South, Compass4Points.East, Compass4Points.East, Compass4Points.East);
                    generator.PopulateDice(4, 3, 0, 0, 1, 1, 2, 4);
                    break;

                default: throw new ArgumentException($"Didn't have Generator for [{test}]");
            }

            return generator;
        }*/
    }
}
