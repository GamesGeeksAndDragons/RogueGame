using System;
using Utils.Enums;

namespace AssetsTests.Fakes
{
    public enum StandardTestRoom
    {
        First,
        Second,
        Third,
        Fourth,
        Fifth,
        Sixth
    }

    static class StandardTestRooms
    {
        internal static FakeRandomNumberGenerator PopulateRandomForTestRoom(this FakeRandomNumberGenerator generator, StandardTestRoom testRoom)
        {
            switch (testRoom)
            {
                case StandardTestRoom.First:
                    generator.PopulateEnum(Compass4Points.South);
                    generator.PopulateDice(0, 1);
                    break;
                case StandardTestRoom.Second:
                    generator.PopulateEnum(Compass4Points.West);
                    generator.PopulateDice(0, 1);
                    break;
                case StandardTestRoom.Third:
                    generator.PopulateEnum(Compass4Points.South, Compass4Points.East);
                    generator.PopulateDice(0, 1);
                    break;
                case StandardTestRoom.Fourth:
                    generator.PopulateEnum(Compass4Points.East, Compass4Points.South);
                    generator.PopulateDice(0, 1);
                    break;
                case StandardTestRoom.Fifth:
                    generator.PopulateEnum(Compass4Points.West, Compass4Points.South);
                    generator.PopulateDice(0, 1);
                    break;
                case StandardTestRoom.Sixth:
                    generator.PopulateEnum(Compass4Points.South, Compass4Points.West);
                    generator.PopulateDice(0, 1);
                    break;

                default: throw new ArgumentException($"Didn't have Generator for [{testRoom}]");
            }

            return generator;
        }

        internal static string GetExpectation(StandardTestRoom testRoom)
        {
            switch (testRoom)
            {
                case StandardTestRoom.First:
                    return " |012345" + Environment.NewLine +
                           "--------" + Environment.NewLine +
                           "0|╔════╗" + Environment.NewLine +
                           "1|║    ║" + Environment.NewLine +
                           "2|║    ║" + Environment.NewLine +
                           "3|║    ║" + Environment.NewLine +
                           "4|║    ║" + Environment.NewLine +
                           "5|║    ║" + Environment.NewLine +
                           "6|║    ║" + Environment.NewLine +
                           "7|║    ║" + Environment.NewLine +
                           "8|║    ║" + Environment.NewLine +
                           "9|╚════╝";
                case StandardTestRoom.Second:
                    return " |0123456789" + Environment.NewLine +
                           "------------" + Environment.NewLine +
                           "0|╔════════╗" + Environment.NewLine +
                           "1|║        ║" + Environment.NewLine +
                           "2|║        ║" + Environment.NewLine +
                           "3|║        ║" + Environment.NewLine +
                           "4|║        ║" + Environment.NewLine +
                           "5|╚════════╝";
                case StandardTestRoom.Third:
                    return " |0123456789" + Environment.NewLine +
                           "------------" + Environment.NewLine +
                           "0|╔════╗████" + Environment.NewLine +
                           "1|║    ║████" + Environment.NewLine +
                           "2|║    ║████" + Environment.NewLine +
                           "3|║    ║████" + Environment.NewLine +
                           "4|║    ╚═══╗" + Environment.NewLine +
                           "5|║        ║" + Environment.NewLine +
                           "6|║        ║" + Environment.NewLine +
                           "7|║        ║" + Environment.NewLine +
                           "8|║        ║" + Environment.NewLine +
                           "9|╚════════╝";
                case StandardTestRoom.Fourth:
                    return " |0123456789" + Environment.NewLine +
                           "------------" + Environment.NewLine +
                           "0|╔════════╗" + Environment.NewLine +
                           "1|║        ║" + Environment.NewLine +
                           "2|║        ║" + Environment.NewLine +
                           "3|║        ║" + Environment.NewLine +
                           "4|║        ║" + Environment.NewLine +
                           "5|╚═══╗    ║" + Environment.NewLine +
                           "6|████║    ║" + Environment.NewLine +
                           "7|████║    ║" + Environment.NewLine +
                           "8|████║    ║" + Environment.NewLine +
                           "9|████╚════╝";
                case StandardTestRoom.Fifth:
                    return " |0123456789" + Environment.NewLine +
                           "------------" + Environment.NewLine +
                           "0|╔════════╗" + Environment.NewLine +
                           "1|║        ║" + Environment.NewLine +
                           "2|║        ║" + Environment.NewLine +
                           "3|║        ║" + Environment.NewLine +
                           "4|║        ║" + Environment.NewLine +
                           "5|║    ╔═══╝" + Environment.NewLine +
                           "6|║    ║████" + Environment.NewLine +
                           "7|║    ║████" + Environment.NewLine +
                           "8|║    ║████" + Environment.NewLine +
                           "9|╚════╝████";
                case StandardTestRoom.Sixth:
                    return " |0123456789" + Environment.NewLine +
                           "------------" + Environment.NewLine +
                           "0|████╔════╗" + Environment.NewLine +
                           "1|████║    ║" + Environment.NewLine +
                           "2|████║    ║" + Environment.NewLine +
                           "3|████║    ║" + Environment.NewLine +
                           "4|╔═══╝    ║" + Environment.NewLine +
                           "5|║        ║" + Environment.NewLine +
                           "6|║        ║" + Environment.NewLine +
                           "7|║        ║" + Environment.NewLine +
                           "8|║        ║" + Environment.NewLine +
                           "9|╚════════╝";

                default: throw new ArgumentException($"Didn't have Generator for [{testRoom}]");
            }
        }

        internal static FakeRandomNumberGenerator PopulateRandomForRoomCount(this FakeRandomNumberGenerator generator, int roomCount)
        {
            generator.PopulateDice(roomCount);
            return generator;
        }

        internal static FakeRandomNumberGenerator AddCoordinates(this FakeRandomNumberGenerator generator, int row, int column)
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
