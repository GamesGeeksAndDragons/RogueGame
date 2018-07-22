using System;
using AssetsTests.Fakes;
using Utils.Enums;
using Utils.Random;

namespace AssetsTests.RoomTests
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
                        " |01" + Environment.NewLine +
                        "----" + Environment.NewLine +
                        "0|.." + Environment.NewLine +
                        "1|..";
                case BlockNavigationTests.Test.TopLeft:
                    return
                        " |01" + Environment.NewLine +
                        "----" + Environment.NewLine +
                        "0|**" + Environment.NewLine +
                        "1|..";
                case BlockNavigationTests.Test.TopRight:
                    return
                        " |01" + Environment.NewLine +
                        "----" + Environment.NewLine +
                        "0|.*" + Environment.NewLine +
                        "1|.*";
                case BlockNavigationTests.Test.BottomRight:
                    return
                        " |01" + Environment.NewLine +
                        "----" + Environment.NewLine +
                        "0|.." + Environment.NewLine +
                        "1|**";
                case BlockNavigationTests.Test.BottomLeft:
                    return
                        " |01" + Environment.NewLine +
                        "----" + Environment.NewLine +
                        "0|*." + Environment.NewLine +
                        "1|*.";
                case BlockNavigationTests.Test.Cornered:
                    return
                        " |01234" + Environment.NewLine +
                        "-------" + Environment.NewLine +
                        "0|....." + Environment.NewLine +
                        "1|....." + Environment.NewLine +
                        "2|....." + Environment.NewLine +
                        "3|...**" + Environment.NewLine +
                        "4|..***";
            }

            throw new ArgumentException($"Didn't have Expected Layout for [{test}]");
        }

        internal static IRandomNumberGenerator GetGenerator(BlockNavigationTests.Test test)
        {
            var generator = new FakeRandomNumberGenerator();

            switch (test)
            {
                case BlockNavigationTests.Test.TopLeft:
                    generator.PopulateEnum(Compass4Points.West, Compass4Points.North, Compass4Points.East, Compass4Points.East, Compass4Points.East);
                    generator.PopulateDice(0, 1);
                    break;
                case BlockNavigationTests.Test.TopRight:
                    generator.PopulateEnum(Compass4Points.North, Compass4Points.East, Compass4Points.South, Compass4Points.East, Compass4Points.East);
                    generator.PopulateDice(1, 1);
                    break;
                case BlockNavigationTests.Test.BottomRight:
                    generator.PopulateEnum(Compass4Points.South, Compass4Points.West, Compass4Points.East, Compass4Points.East, Compass4Points.East);
                    generator.PopulateDice(1, 0);
                    break;
                case BlockNavigationTests.Test.BottomLeft:
                    generator.PopulateEnum(Compass4Points.South, Compass4Points.West, Compass4Points.North, Compass4Points.East, Compass4Points.East);
                    generator.PopulateDice(0, 0);
                    break;
                case BlockNavigationTests.Test.Cornered:
                    generator.PopulateEnum(Compass4Points.West, Compass4Points.South, Compass4Points.East, Compass4Points.East, Compass4Points.East);
                    generator.PopulateDice(3, 4, 0, 0, 1, 1, 4, 2);
                    break;

                default: throw new ArgumentException($"Didn't have Generator for [{test}]");
            }

            return generator;
        }
    }
}
