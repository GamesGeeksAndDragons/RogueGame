using System;
using Assets.Actions;
using Assets.Random;

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

        internal static IRandomNumberGenerator GetGenerator(BlockNavigationTests.Test test)
        {
            var generator = new FakeRandomNumberGenerator();

            switch (test)
            {
                case BlockNavigationTests.Test.TopLeft:
                    generator.PopulateEnum(BlockDirection.Left, BlockDirection.Up, BlockDirection.Right, BlockDirection.Right, BlockDirection.Right);
                    generator.PopulateDice(0, 0);
                    break;
                case BlockNavigationTests.Test.TopRight:
                    generator.PopulateEnum(BlockDirection.Up, BlockDirection.Right, BlockDirection.Down, BlockDirection.Right, BlockDirection.Right);
                    generator.PopulateDice(1, 0);
                    break;
                case BlockNavigationTests.Test.BottomRight:
                    generator.PopulateEnum(BlockDirection.Right, BlockDirection.Down, BlockDirection.Left, BlockDirection.Right, BlockDirection.Right);
                    generator.PopulateDice(1, 1);
                    break;
                case BlockNavigationTests.Test.BottomLeft:
                    generator.PopulateEnum(BlockDirection.Down, BlockDirection.Left, BlockDirection.Up, BlockDirection.Right, BlockDirection.Right);
                    generator.PopulateDice(0, 1);
                    break;
                case BlockNavigationTests.Test.Cornered:
                    generator.PopulateEnum(BlockDirection.Left, BlockDirection.Down, BlockDirection.Right, BlockDirection.Right, BlockDirection.Right);
                    generator.PopulateDice(4, 3, 0, 0, 1, 1, 2, 4);
                    break;

                default: throw new ArgumentException($"Didn't have Generator for [{test}]");
            }

            return generator;
        }
    }
}
