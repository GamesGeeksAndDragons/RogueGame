#nullable enable
using Assets.Deeds;
using Utils;
using Utils.Dispatching;
using Utils.Enums;
using ActorMethodType= System.Func<Utils.Dispatching.IDispatchRegistry, Assets.Deeds.IActionRegistry, string, string, Utils.Dispatching.IDispatched>;

namespace Assets.Actors
{
    public interface IActorBuilder
    {
        IDispatched Build(string actor, string state = "");
        Func<IDispatched> RockBuilder(string state = "");
        Func<IDispatched> WallBuilder(string roomNumber, string state = "");
    }

    internal class ActorBuilder : IActorBuilder
    {
        private readonly IDispatchRegistry _dispatchRegistry;
        private readonly IActionRegistry _actionRegistry;
        private readonly Dictionary<string, ActorMethodType> _actorBuilderMethods;

        public ActorBuilder(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry)
        {
            _dispatchRegistry = dispatchRegistry;
            _actionRegistry = actionRegistry;

            _actorBuilderMethods = new Dictionary<string, ActorMethodType>
            {
                {ActorDisplay.Rock,  BuilderMethods.BuildRock},
                {ActorDisplay.Null, BuilderMethods.BuildNull},
                {ActorDisplay.Me, BuilderMethods.BuildMe},
            };

            foreach (var wall in ActorDisplay.Walls)
            {
                _actorBuilderMethods.Add(wall, BuilderMethods.BuildWall);
            }

            foreach (var door in ActorDisplay.Doors)
            {
                _actorBuilderMethods.Add(door,  BuilderMethods.BuildDoor);
            }

            foreach (var roomNumberFloorIn in ActorDisplay.RoomNumberOfFloor)
            {
                _actorBuilderMethods.Add(roomNumberFloorIn, BuilderMethods.BuildFloor);
            }
        }

        internal static class BuilderMethods
        {
            internal static IDispatched BuildFloor(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string actor, string state)
            {
                return new Floor(dispatchRegistry, actionRegistry, actor, state);
            }

            internal static IDispatched BuildDoor(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string actor, string state)
            {
                return new Door(dispatchRegistry, actionRegistry, actor, state);
            }

            internal static IDispatched BuildRock(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string actor, string state)
            {
                return new Rock(dispatchRegistry, actionRegistry, actor, state);
            }

            internal static IDispatched BuildWall(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string actor, string state)
            {
                var direction = WallDirectionHelpers.ToWallDirection(actor);

                return new Wall(dispatchRegistry, actionRegistry, actor, direction.ToString());
            }

            public static IDispatched BuildNull(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string actor, string state)
            {
                return new Null(dispatchRegistry, actionRegistry, actor);
            }

            public static IDispatched BuildMe(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string actor, string state)
            {
                return new Me(dispatchRegistry, actionRegistry, actor, state);
            }
        }

        public IDispatched Build(string actor, string state="")
        {
            if (_actorBuilderMethods.TryGetValue(actor, out var builder))
            {
                return builder(_dispatchRegistry, _actionRegistry, actor, state);
            }

            throw new ArgumentException($"Unable to find Actor Builder for [{actor}]");
        }

        public Func<IDispatched> RockBuilder(string state = "") => () => Build(ActorDisplay.Rock, state);
        public Func<IDispatched> WallBuilder(string roomNumber, string state = "") => () => Build(roomNumber, state);
    }
}
