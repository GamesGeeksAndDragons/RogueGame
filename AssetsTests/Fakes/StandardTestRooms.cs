using System;
using System.Collections.Generic;
using System.Text;
using Utils.Enums;

namespace AssetsTests.Fakes
{
    static class StandardTestRooms
    {
        internal static FakeRandomNumberGenerator AddTestRoom(this FakeRandomNumberGenerator generator, int testNum)
        {
            switch (testNum)
            {
                case 1:
                    generator.PopulateEnum(Compass4Points.South);
                    generator.PopulateDice(0, 1);
                    break;
                case 2:
                    generator.PopulateEnum(Compass4Points.West);
                    generator.PopulateDice(0, 1);
                    break;
                case 3:
                    generator.PopulateEnum(Compass4Points.South, Compass4Points.East);
                    generator.PopulateDice(0, 1);
                    break;
                case 4:
                    generator.PopulateEnum(Compass4Points.East, Compass4Points.South);
                    generator.PopulateDice(0, 1);
                    break;
                case 5:
                    generator.PopulateEnum(Compass4Points.West, Compass4Points.South);
                    generator.PopulateDice(0, 1);
                    break;
                case 6:
                    generator.PopulateEnum(Compass4Points.South, Compass4Points.West);
                    generator.PopulateDice(0, 1);
                    break;

                default: throw new ArgumentException($"Didn't have Generator for [{testNum}]");
            }

            return generator;
        }

        internal static FakeRandomNumberGenerator AddRoomCount(this FakeRandomNumberGenerator generator, int roomCount)
        {
            generator.PopulateDice(roomCount);
            return generator;
        }

        internal static FakeRandomNumberGenerator AddCoordindates(this FakeRandomNumberGenerator generator, int row, int column)
        {
            generator.PopulateDice(row, column);
            return generator;
        }

        internal static FakeRandomNumberGenerator AddRandomNumbers(this FakeRandomNumberGenerator generator, params int[] numbers)
        {
            generator.PopulateDice(numbers);
            return generator;
        }

        internal static FakeRandomNumberGenerator AddDirection(this FakeRandomNumberGenerator generator, params Compass8Points[] directions)
        {
            generator.PopulateEnum(directions);
            return generator;
        }
    }
}
