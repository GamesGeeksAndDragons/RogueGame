#nullable enable
using Assets.Deeds;
using Utils.Dispatching;
using BuilderMethodType= System.Func<Utils.Dispatching.IDispatchRegistry, Assets.Deeds.IActionRegistry, string, Utils.Dispatching.IDispatched>;
using ActorMethodType= System.Func<string, Utils.Dispatching.IDispatchRegistry, Assets.Deeds.IActionRegistry, Utils.Dispatching.IDispatched>;

namespace Assets.Actors
{
    public interface IActorBuilder
    {
        T Build<T>(string state="") where T : IDispatched;

        IDispatched Build(string actor, string state = "");
    }

    internal class ActorBuilder : IActorBuilder
    {
        private readonly IDispatchRegistry _dispatchRegistry;
        private readonly IActionRegistry _actionRegistry;
        private readonly Dictionary<Type, BuilderMethodType> _builderMethods;
        private readonly Dictionary<string, ActorMethodType> _actorBuilderMethods;

        public ActorBuilder(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry)
        {
            _dispatchRegistry = dispatchRegistry;
            _actionRegistry = actionRegistry;

            _builderMethods = new Dictionary<Type, BuilderMethodType>
            {
                {typeof(Door), Builder.BuildDoor},
                {typeof(Rock), Builder.BuildRock},
                {typeof(Wall), Builder.BuildWall},
                {typeof(Floor), Builder.BuildFloor},
                {typeof(Null), Builder.BuildNull},
                {typeof(Me), Builder.BuildMe},
            };

            _actorBuilderMethods = new Dictionary<string, ActorMethodType>
            {
                {ActorDisplay.Rock,  CharacterBuilder.BuildRock},
                {ActorDisplay.Floor, CharacterBuilder.BuildFloor},
                {ActorDisplay.Door1, CharacterBuilder.BuildDoor},
                {ActorDisplay.Door2, CharacterBuilder.BuildDoor},
                {ActorDisplay.Door3, CharacterBuilder.BuildDoor},
                {ActorDisplay.Door4, CharacterBuilder.BuildDoor},
                {ActorDisplay.Door5, CharacterBuilder.BuildDoor},
                {ActorDisplay.Door6, CharacterBuilder.BuildDoor},
                {ActorDisplay.Door7, CharacterBuilder.BuildDoor},
                {ActorDisplay.Door8, CharacterBuilder.BuildDoor},
                {ActorDisplay.Door9, CharacterBuilder.BuildDoor},
                {ActorDisplay.Door10, CharacterBuilder.BuildDoor},
                {ActorDisplay.Door11, CharacterBuilder.BuildDoor},
                {ActorDisplay.Door12, CharacterBuilder.BuildDoor},
                {ActorDisplay.Door13, CharacterBuilder.BuildDoor},
                {ActorDisplay.Door14, CharacterBuilder.BuildDoor},
                {ActorDisplay.Door15, CharacterBuilder.BuildDoor},
                {ActorDisplay.WallHorizontal, CharacterBuilder.BuildWall},
                {ActorDisplay.WallVertical, CharacterBuilder.BuildWall},
                {ActorDisplay.WallTopLeftCorner, CharacterBuilder.BuildWall},
                {ActorDisplay.WallTopRightCorner, CharacterBuilder.BuildWall},
                {ActorDisplay.WallBottomLeftCorner, CharacterBuilder.BuildWall},
                {ActorDisplay.WallBottomRightCorner, CharacterBuilder.BuildWall},
                {ActorDisplay.Null, CharacterBuilder.BuildNull},
            };
        }

        private static class Builder
        {
            internal static IDispatched BuildWall(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string state)
            {
                return new Wall(dispatchRegistry, actionRegistry, state);
            }

            internal static IDispatched BuildFloor(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string state)
            {
                return new Floor(dispatchRegistry, actionRegistry);
            }

            internal static IDispatched BuildDoor(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string state)
            {
                return new Door(dispatchRegistry, actionRegistry, state);
            }

            internal static IDispatched BuildRock(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string _)
            {
                return new Rock(dispatchRegistry, actionRegistry);
            }

            public static IDispatched BuildNull(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string _)
            {
                return new Null(dispatchRegistry, actionRegistry);
            }

            internal static IDispatched BuildMe(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string state)
            {
                return new Me(dispatchRegistry, actionRegistry, state);
            }
        }

        internal static class CharacterBuilder
        {
            internal static IDispatched BuildFloor(string actor, IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry)
            {
                return new Floor(dispatchRegistry, actionRegistry);
            }

            internal static IDispatched BuildDoor(string actor, IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry)
            {
                return new Door(dispatchRegistry, actionRegistry, actor);
            }

            internal static IDispatched BuildRock(string actor, IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry)
            {
                return new Rock(dispatchRegistry, actionRegistry);
            }

            internal static IDispatched BuildWall(string actor, IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry)
            {
                var direction = Wall.GetDirection(actor);

                return new Wall(dispatchRegistry, actionRegistry, direction.ToString());
            }

            public static IDispatched BuildNull(string actor, IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry)
            {
                return new Null(dispatchRegistry, actionRegistry);
            }
        }

        public T Build<T>(string state="")
            where T : IDispatched
        {
            var type = typeof(T);

            if (_builderMethods.TryGetValue(type, out var builder))
            {
                return (T) builder(_dispatchRegistry, _actionRegistry, state);
            }

            throw new TypeInitializationException(typeof(T).FullName, null);
        }

        public IDispatched Build(string actor, string state="")
        {
            if (_actorBuilderMethods.TryGetValue(actor, out var builder))
            {
                return builder(actor, _dispatchRegistry, _actionRegistry);
            }

            throw new ArgumentException($"Unable to find Actor Builder for [{actor}]");
        }
    }
}
