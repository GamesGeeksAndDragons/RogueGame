#nullable enable
using Assets.Deeds;
using Utils.Dispatching;
using ActorMethodType= System.Func<Utils.Dispatching.IDispatchRegistry, Assets.Deeds.IActionRegistry, string, string, Utils.Dispatching.IDispatched>;

namespace Assets.Actors
{
    public interface IActorBuilder
    {
        IDispatched Build(string actor, string state = "");
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
                {ActorDisplay.Floor, BuilderMethods.BuildFloor},
                {ActorDisplay.WallHorizontal, BuilderMethods.BuildWall},
                {ActorDisplay.WallVertical, BuilderMethods.BuildWall},
                {ActorDisplay.WallTopLeftCorner, BuilderMethods.BuildWall},
                {ActorDisplay.WallTopRightCorner, BuilderMethods.BuildWall},
                {ActorDisplay.WallBottomLeftCorner, BuilderMethods.BuildWall},
                {ActorDisplay.WallBottomRightCorner, BuilderMethods.BuildWall},
                {ActorDisplay.Null, BuilderMethods.BuildNull},
                {ActorDisplay.Me, BuilderMethods.BuildMe},
            };

            foreach (var door in ActorDisplay.Doors)
            {
                _actorBuilderMethods.Add(door,  BuilderMethods.BuildDoor);
            }
        }

        internal static class BuilderMethods
        {
            internal static IDispatched BuildFloor(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string actor, string state)
            {
                return new Floor(dispatchRegistry, actionRegistry);
            }

            internal static IDispatched BuildDoor(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string actor, string state)
            {
                return new Door(dispatchRegistry, actionRegistry, actor);
            }

            internal static IDispatched BuildRock(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string actor, string state)
            {
                return new Rock(dispatchRegistry, actionRegistry);
            }

            internal static IDispatched BuildWall(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string actor, string state)
            {
                var direction = Wall.GetDirection(actor);

                return new Wall(dispatchRegistry, actionRegistry, direction.ToString());
            }

            public static IDispatched BuildNull(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string actor, string state)
            {
                return new Null(dispatchRegistry, actionRegistry);
            }

            public static IDispatched BuildMe(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string actor, string state)
            {
                return new Me(dispatchRegistry, actionRegistry, state);
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
    }
}
