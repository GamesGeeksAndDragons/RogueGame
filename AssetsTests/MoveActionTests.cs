using System;
using Assets.ActionEnqueue;
using Assets.Messaging;
using AssetsTests.Fakes;
using Utils;
using Utils.Enums;
using Utils.Random;
using Xunit;
using Xunit.Abstractions;

namespace AssetsTests
{
    public class MoveActionTests
    {
        private readonly ITestOutputHelper _output;

        public MoveActionTests(ITestOutputHelper output)
        {
            _output = output;
        }

        internal static IRandomNumberGenerator GetGenerator(int testNum)
        {
            var generator = new FakeRandomNumberGenerator();

            switch (testNum)
            {
                case 1:
                case 2:
                    generator.PopulateEnum(Compass4Points.South);
                    generator.PopulateDice(0, 1, 1, 1);
                    break;

                default: throw new ArgumentException($"Didn't have Generator for [{testNum}]");
            }

            return generator;
        }

        internal static int GetLevel(int testNum)
        {
            switch (testNum)
            {
                case 1:
                case 2:
                    return 1;

                default: throw new ArgumentException($"Didn't have Generator for [{testNum}]");
            }
        }

        internal static string GetExpectation(int testNum)
        {
            switch (testNum)
            {
                case 1:
                    return " |012345" + Environment.NewLine +
                           "--------" + Environment.NewLine +
                           "0|╔════╗" + Environment.NewLine +
                           "1|║    ║" + Environment.NewLine +
                           "2|║@   ║" + Environment.NewLine +
                           "3|║    ║" + Environment.NewLine +
                           "4|║    ║" + Environment.NewLine +
                           "5|║    ║" + Environment.NewLine +
                           "6|║    ║" + Environment.NewLine +
                           "7|║    ║" + Environment.NewLine +
                           "8|║    ║" + Environment.NewLine +
                           "9|╚════╝";
                case 2:
                    return " |012345" + Environment.NewLine +
                           "--------" + Environment.NewLine +
                           "0|╔════╗" + Environment.NewLine +
                           "1|║    ║" + Environment.NewLine +
                           "2|║ @  ║" + Environment.NewLine +
                           "3|║    ║" + Environment.NewLine +
                           "4|║    ║" + Environment.NewLine +
                           "5|║    ║" + Environment.NewLine +
                           "6|║    ║" + Environment.NewLine +
                           "7|║    ║" + Environment.NewLine +
                           "8|║    ║" + Environment.NewLine +
                           "9|╚════╝";
                default: throw new ArgumentException($"Didn't have Generator for [{testNum}]");
            }
        }

        [Fact]
        public void Move_Me_CanMoveIntoEmptySpace()
        {
            var testNum = 1;
            var actorRegistry = new ActorRegistry();
            var dispatcher = new Dispatcher(actorRegistry);

            var fakeRandomNumbers = GetGenerator(testNum);
            var fakeLogger = new FakeLogger(_output);

            var builder = new LevelBuilder(fakeRandomNumbers, fakeLogger, dispatcher, actorRegistry);
            builder.Build(GetLevel(testNum));
            dispatcher.Dispatch();

            // t+1
            var move = new Move("Me1", Compass8Points.South.ToString(), dispatcher);
            move.Enqueue();
            dispatcher.Dispatch();

            var expected = GetExpectation(testNum);
            var actual = actorRegistry.GetActor("Room1").ToString();

            _output.WriteLine(expected);
            _output.WriteLine('='.ToPaddedString(10));
            _output.WriteLine(actual);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Move_Me_CantMoveIntoWallsButCanMoveIntoEmptySpace()
        {
            var testNum = 2;
            var actorRegistry = new ActorRegistry();
            var dispatcher = new Dispatcher(actorRegistry);

            var fakeRandomNumbers = GetGenerator(testNum);
            var fakeLogger = new FakeLogger(_output);

            var builder = new LevelBuilder(fakeRandomNumbers, fakeLogger, dispatcher, actorRegistry);
            builder.Build(GetLevel(testNum));
            dispatcher.Dispatch();

            var move = new Move("Me1", Compass8Points.West.ToString(), dispatcher);
            move.Enqueue();
            move = new Move("Me1", Compass8Points.North.ToString(), dispatcher);
            move.Enqueue();
            move = new Move("Me1", Compass8Points.SouthWest.ToString(), dispatcher);
            move.Enqueue();
            move = new Move("Me1", Compass8Points.SouthEast.ToString(), dispatcher);
            move.Enqueue();

            dispatcher.Dispatch();

            var expected = GetExpectation(testNum);
            var actual = actorRegistry.GetActor("Room1").ToString();

            _output.WriteLine(expected);
            _output.WriteLine('='.ToPaddedString(10));
            _output.WriteLine(actual);

            Assert.Equal(expected, actual);
        }
    }
}
