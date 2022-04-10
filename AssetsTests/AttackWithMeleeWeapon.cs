using Assets.Actors;
using Assets.Deeds;
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

        internal static IDieBuilder GetGenerator(int testNum)
        {
            var name = $"{nameof(AttackWithMeleeWeapon)}_{testNum}";
            return new DieBuilder(name);
        }

        [Fact]
        public void AttachingAMonstor_WillReduceItsHitPoints()
        {
            //var state = MeleeWeapon.FormatState("Wooden Club", "Me1", 1, 12, -1, 1, 7.5, 0, 50);

            //var testNum = 1;
            //var fakeRandomNumbers = GetGenerator(testNum);
            //var dispatchRegistry = new DispatchRegistry();
            //var actionRegistry = new ActionRegistry();
            //var dispatcher = new Dispatcher(dispatchRegistry);
            //var fakeLogger = new FakeLogger(_output);

            //var builder = new LevelBuilder(fakeRandomNumbers, fakeLogger, dispatcher, dispatchRegistry, actionRegistry);
            //builder.Build(testNum);

            //var me = ActorBuilder.Build<Me>(Coordinate.NotSet, dispatchRegistry, actionRegistry, Me.FormatState(10, 10));
            //dispatcher.EnqueueTeleport(me);
            //var monster = ActorBuilder.Build<Monster>(Coordinate.NotSet, dispatchRegistry, actionRegistry,  Monster.FormatState(10, 10));
            //dispatcher.EnqueueTeleport(monster);
            //dispatcher.Dispatch();

            //var weapon = new MeleeWeapon(fakeRandomNumbers, dispatchRegistry, actionRegistry, dispatcher, state);
            //dispatcher.EnqueueUse(weapon, Compass8Points.East);
            //dispatcher.Dispatch();

            //monster = (Monster)dispatchRegistry.GetDispatchee(monster.UniqueId);
            //var expected = "HitPoints [7], ArmourClass [10]";
            //var actual = $"HitPoints [{monster.HitPoints}], ArmourClass [{monster.ArmourClass}]";

            //Assert.Equal(expected, actual);
        }
    }
}
