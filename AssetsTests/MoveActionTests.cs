using System;
using Assets.Actors;
using Assets.Deeds;
using Assets.Messaging;
using AssetsTests.Fakes;
using Utils;
using Utils.Coordinates;
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

        internal static IDieBuilder GetGenerator(int testNum)
        {
            var generator = new DieBuilder();

            //switch (testNum)
            //{
            //    case 1:
            //    case 2:
            //        generator.PopulateEnum(Compass4Points.South, Compass4Points.North);
            //        generator.PopulateDice(1, 0, 1, 1, 1);
            //        break;

            //    default: throw new ArgumentException($"Didn't have Generator for [{testNum}]");
            //}

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
            var dispatchRegistry = new DispatchRegistry();
            var actionRegistry = new ActionRegistry();
            var dispatcher = new Dispatcher(dispatchRegistry);

            var fakeRandomNumbers = GetGenerator(testNum);
            var fakeLogger = new FakeLogger(_output);

            var builder = new LevelBuilder(fakeRandomNumbers, fakeLogger, dispatcher, dispatchRegistry, actionRegistry);
            builder.Build(GetLevel(testNum));
            var coordinates = new Coordinate(10, 10);
            var me = ActorBuilder.Build<Me>(coordinates, dispatchRegistry, actionRegistry, "");
            dispatcher.EnqueueTeleport(me);
            dispatcher.Dispatch();

            // t+1
            dispatcher.EnqueueMove(me, Compass8Points.South);
            dispatcher.Dispatch();

            var expected = GetExpectation(testNum);
            var actual = dispatchRegistry.GetDispatchee("Maze1").ToString();

            _output.WriteLine(expected);
            _output.WriteLine('='.ToPaddedString(10));
            _output.WriteLine(actual);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Move_Me_CantMoveIntoWallsButCanMoveIntoEmptySpace()
        {
            var testNum = 2;
            var dispatchRegistry = new DispatchRegistry();
            var actionRegistry = new ActionRegistry();
            var dispatcher = new Dispatcher(dispatchRegistry);

            var fakeRandomNumbers = GetGenerator(testNum);
            var fakeLogger = new FakeLogger(_output);

            var builder = new LevelBuilder(fakeRandomNumbers, fakeLogger, dispatcher, dispatchRegistry, actionRegistry);
            builder.Build(GetLevel(testNum));
            var me = ActorBuilder.Build<Me>(Coordinate.NotSet, dispatchRegistry, actionRegistry, new Coordinate(10, 10).ToString());
            dispatcher.EnqueueTeleport(me);
            dispatcher.Dispatch();

            dispatcher.EnqueueMove(me, Compass8Points.West);
            dispatcher.EnqueueMove(me, Compass8Points.North);
            dispatcher.EnqueueMove(me, Compass8Points.SouthWest);
            dispatcher.EnqueueMove(me, Compass8Points.SouthEast);

            dispatcher.Dispatch();

            var expected = GetExpectation(testNum);
            var actual = dispatchRegistry.GetDispatchee("Maze1").ToString();

            _output.WriteLine(expected);
            _output.WriteLine('='.ToPaddedString(10));
            _output.WriteLine(actual);

            Assert.Equal(expected, actual);
        }
    }
}
