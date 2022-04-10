using System;
using System.Collections.Generic;
using Assets.Deeds;
using Utils.Coordinates;
using Utils.Dispatching;

namespace Assets.Actors
{
    internal static class ActorBuilder
    {
        private static readonly Dictionary<Type, Func<IDispatchee, IDispatchee>> CloneBuilderMethods;
        private static readonly Dictionary<Type, Func<Coordinate, IDispatchRegistry, IActionRegistry, string, IDispatchee>> BuilderMethods;
        private static readonly Dictionary<string, Func<string, Coordinate, IDispatchRegistry, IActionRegistry, IDispatchee>> ActorBuilderMethods;

        static ActorBuilder()
        {
            CloneBuilderMethods = new Dictionary<Type, Func<IDispatchee, IDispatchee>>
            {
                {typeof(Door), CloneBuilder.BuildDoor},
                {typeof(Rock), CloneBuilder.BuildRock},
                {typeof(Wall), CloneBuilder.BuildWall},
                {typeof(Floor), CloneBuilder.BuildFloor},
                {typeof(Null), CloneBuilder.BuildNull},
            };

            BuilderMethods = new Dictionary<Type, Func<Coordinate, IDispatchRegistry, IActionRegistry, string, IDispatchee>>
            {
                {typeof(Door), Builder.BuildDoor},
                {typeof(Rock), Builder.BuildRock},
                {typeof(Wall), Builder.BuildWall},
                {typeof(Floor), Builder.BuildFloor},
                {typeof(Null), Builder.BuildNull},
            };

            ActorBuilderMethods = new Dictionary<string, Func<string, Coordinate, IDispatchRegistry, IActionRegistry, IDispatchee>>
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

        private static void CheckType(this IDispatchee dispatchee, string expectedDispatcheeType)
        {
            if (dispatchee.Name != expectedDispatcheeType)
            {
                throw new ArgumentException($"Expected a type of [{expectedDispatcheeType}] and got [{dispatchee.Name}]");
            }
        }

        internal static class CloneBuilder
        {
            internal static IDispatchee BuildWall(IDispatchee wall)
            {
                wall.CheckType(Wall.DispatcheeName);
                return new Wall((Wall)wall);
            }

            internal static IDispatchee BuildFloor(IDispatchee floor)
            {
                floor.CheckType(Floor.DispatcheeName);

                return new Floor((Floor)floor);
            }

            internal static IDispatchee BuildDoor(IDispatchee door)
            {
                door.CheckType(Door.DispatcheeName);

                return new Door((Door)door);
            }

            internal static IDispatchee BuildRock(IDispatchee rock)
            {
                rock.CheckType(Rock.DispatcheeName);

                return new Rock((Rock)rock);
            }

            public static IDispatchee BuildNull(IDispatchee tile)
            {
                tile.CheckType(Null.DispatcheeName);

                return new Null((Null)tile);
            }
        }

        internal static class Builder
        {
            internal static IDispatchee BuildWall(Coordinate coordinates, IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string state)
            {
                return new Wall(coordinates, dispatchRegistry, actionRegistry, state);
            }

            internal static IDispatchee BuildFloor(Coordinate coordinates, IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string state)
            {
                return new Floor(coordinates, dispatchRegistry, actionRegistry);
            }

            internal static IDispatchee BuildDoor(Coordinate coordinates, IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string state)
            {
                return new Door(coordinates, dispatchRegistry,actionRegistry, state);
            }

            internal static IDispatchee BuildRock(Coordinate coordinates, IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string _)
            {
                return new Rock(coordinates, dispatchRegistry, actionRegistry);
            }

            public static IDispatchee BuildNull(Coordinate coordinates, IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string _)
            {
                return new Null(coordinates, dispatchRegistry, actionRegistry);
            }
        }

        internal static class CharacterBuilder
        {
            internal static IDispatchee BuildFloor(string actor, Coordinate coordinates, IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry)
            {
                return new Floor(coordinates, dispatchRegistry, actionRegistry);
            }

            internal static IDispatchee BuildDoor(string actor, Coordinate coordinates, IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry)
            {
                return new Door(coordinates, dispatchRegistry, actionRegistry, actor);
            }

            internal static IDispatchee BuildRock(string actor, Coordinate coordinates, IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry)
            {
                return new Rock(coordinates, dispatchRegistry, actionRegistry);
            }

            internal static IDispatchee BuildWall(string actor, Coordinate coordinates, IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry)
            {
                var direction = Wall.GetDirection(actor);

                return new Wall(coordinates, dispatchRegistry, actionRegistry, direction.ToString());
            }

            public static IDispatchee BuildNull(string actor, Coordinate coordinates, IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry)
            {
                return new Null(coordinates, dispatchRegistry, actionRegistry);
            }
        }

        public static T Build<T>(Type type, Coordinate coordinate, IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string state)
            where T : IDispatchee
        {
            if (BuilderMethods.TryGetValue(type, out var builder))
            {
                return (T)builder(coordinate, dispatchRegistry, actionRegistry, state);
            }

            throw new TypeInitializationException(typeof(T).FullName, null);
        }

        public static T Build<T>(Coordinate coordinate, IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string state)
            where T : IDispatchee
        {
            var type = typeof(T);

            return Build<T>(type, coordinate, dispatchRegistry, actionRegistry, state);
        }

        public static IDispatchee Build(string actor, Coordinate coordinate, IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry)
        {
            if (ActorBuilderMethods.TryGetValue(actor, out var builder))
            {
                return builder(actor, coordinate, dispatchRegistry, actionRegistry);
            }

            throw new ArgumentException($"Unable to find Actor Builder for [{actor}]");
        }

        public static T Build<T>(T t)
            where T : IDispatchee
        {
            var type = typeof(T);
            if (CloneBuilderMethods.TryGetValue(type, out var builder))
            {
                return (T)builder(t);
            }

            throw new TypeInitializationException(typeof(T).FullName, null);
        }
    }
}
