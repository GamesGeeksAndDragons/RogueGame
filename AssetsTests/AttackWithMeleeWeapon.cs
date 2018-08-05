using System;
using Assets.Actors;
using Assets.Messaging;
using AssetsTests.Fakes;
using Utils.Coordinates;
using Utils.Enums;
using Utils.Random;
using Xunit;
using Xunit.Abstractions;

// https://beej.us/moria/mmspoilers/items.html#weapons

namespace AssetsTests
{
    public class AttackWithMeleeWeapon
    {
        private readonly ITestOutputHelper _output;

        public AttackWithMeleeWeapon(ITestOutputHelper output)
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
                    generator.PopulateDice(0, 1, 1, 1, 1, 2, 12, 2);
                    break;

                default: throw new ArgumentException($"Didn't have Generator for [{testNum}]");
            }

            return generator;
        }

        [Fact]
        public void AttachingAMonstor_WillReduceItsHitPoints()
        {
            var state = MeleeWeapon.BuildState("Wooden Club", "Me1", 1, 12, -1, 1, 7.5, 0, 50);

            var testNum = 1;
            var fakeRandomNumbers = GetGenerator(testNum);
            var registry = new DispatchRegistry();
            var dispatcher = new Dispatcher(registry);
            var fakeLogger = new FakeLogger(_output);
            var mazeDescriptor = FakeMazeDescriptorBuilder.Build(1, 1, 4, 2);

            var builder = new LevelBuilder(fakeRandomNumbers, mazeDescriptor, fakeLogger, dispatcher, registry);
            builder.Build(testNum);

            var me = new Me(Me.CharacterState(10, 10), Coordinate.NotSet, registry);
            dispatcher.EnqueueTeleport(me);
            var monster = new Monster(Monster.CharacterState(10, 10), Coordinate.NotSet, registry);
            dispatcher.EnqueueTeleport(monster);
            dispatcher.Dispatch();

            var weapon = new MeleeWeapon(state, fakeRandomNumbers, registry, dispatcher);
            dispatcher.EnqueueUse(weapon, Compass8Points.East);
            dispatcher.Dispatch();

            monster = (Monster)registry.GetDispatchee(monster.UniqueId);
            var expected = "HitPoints [7], ArmourClass [10]";
            var actual = $"HitPoints [{monster.HitPoints}], ArmourClass [{monster.ArmourClass}]";

            Assert.Equal(expected, actual);
        }
    }
}
