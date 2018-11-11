using System;
using System.Collections.Generic;
using Assets.Messaging;
using Utils.Coordinates;

namespace Assets.Actors
{
    internal static class ActorBuilder
    {
        private static readonly Dictionary<Type, Func<IDispatchee, IDispatchee>> ThisBuilderMethods;
        private static readonly Dictionary<Type, Func<Coordinate, DispatchRegistry, string, IDispatchee>> TypeBuilderMethods;
        private static readonly Dictionary<char, Func<char, Coordinate, DispatchRegistry, IDispatchee>> ActorBuilderMethods;

        static ActorBuilder()
        {
            ThisBuilderMethods = new Dictionary<Type, Func<IDispatchee, IDispatchee>>
            {
                {typeof(Door), BuildDoorFromThis},
                {typeof(Rock), BuildRockFromThis},
                {typeof(Wall), BuildWallFromThis},
            };
            TypeBuilderMethods = new Dictionary<Type, Func<Coordinate, DispatchRegistry, string, IDispatchee>>
            {
                {typeof(Door), BuildDoorFromType},
                {typeof(Rock), BuildRockFromType},
                {typeof(Wall), BuildWallFromType},
            };

            ActorBuilderMethods = new Dictionary<char, Func<char, Coordinate, DispatchRegistry, IDispatchee>>
            {
                {ActorDisplay.Rock,  BuildRockFromType},
                {ActorDisplay.Empty, BuildNothing},
                {ActorDisplay.Door1, BuildDoorFromActor},
                {ActorDisplay.Door2, BuildDoorFromActor},
                {ActorDisplay.Door3, BuildDoorFromActor},
                {ActorDisplay.Door4, BuildDoorFromActor},
                {ActorDisplay.Door5, BuildDoorFromActor},
                {ActorDisplay.Door6, BuildDoorFromActor},
                {ActorDisplay.Door7, BuildDoorFromActor},
                {ActorDisplay.Door8, BuildDoorFromActor},
                {ActorDisplay.Door9, BuildDoorFromActor},
                {ActorDisplay.WallHorizontal, BuildWallFromActor},
                {ActorDisplay.WallVertical, BuildWallFromActor},
                {ActorDisplay.WallTopLeftCorner, BuildWallFromActor},
                {ActorDisplay.WallTopRightCorner, BuildWallFromActor},
                {ActorDisplay.WallBottomLeftCorner, BuildWallFromActor},
                {ActorDisplay.WallBottomRightCorner, BuildWallFromActor},
            };
        }

        private static IDispatchee BuildNothing(char _, Coordinate __, DispatchRegistry ___)
        {
            return null;
        }

        private static void CheckType(this IDispatchee dispatchee, string expectedDispatcheeType)
        {
            if (dispatchee.Name != expectedDispatcheeType)
            {
                throw new ArgumentException($"Expected a type of [{expectedDispatcheeType}] and got [{dispatchee.Name}]");
            }
        }

        private static IDispatchee BuildDoorFromThis(IDispatchee door)
        {
            door.CheckType(Door.DispatcheeName);

            return new Door((Door)door);
        }

        private static IDispatchee BuildDoorFromType(Coordinate coordinates, DispatchRegistry registry, string state)
        {
            return new Door(coordinates, registry, state);
        }

        private static IDispatchee BuildDoorFromActor(char actor, Coordinate coordinates, DispatchRegistry registry)
        {
            var state = actor.ToString();
            return new Door(coordinates, registry, state);
        }

        private static IDispatchee BuildRockFromType(Coordinate coordinates, DispatchRegistry registry, string state)
        {
            return new Rock(coordinates, registry, state);
        }

        private static IDispatchee BuildRockFromThis(IDispatchee rock)
        {
            rock.CheckType(Rock.DispatcheeName);
            return new Rock((Rock)rock);
        }

        private static IDispatchee BuildRockFromType(char actor, Coordinate coordinates, DispatchRegistry registry)
        {
            return new Rock(coordinates, registry, "");
        }

        private static IDispatchee BuildWallFromType(Coordinate coordinates, DispatchRegistry registry, string state)
        {
            return new Wall(coordinates, registry, state);
        }

        private static IDispatchee BuildWallFromActor(char actor, Coordinate coordinates, DispatchRegistry registry)
        {
            var direction = Wall.GetDirection(actor);

            return new Wall(coordinates, registry, direction.ToString());
        }

        private static IDispatchee BuildWallFromThis(IDispatchee wall)
        {
            wall.CheckType(Wall.DispatcheeName);
            return new Wall((Wall)wall);
        }

        public static T Build<T>(Type type, Coordinate coordinate, DispatchRegistry registry, string state)
            where T : IDispatchee, ICloner<T>
        {
            if (TypeBuilderMethods.TryGetValue(type, out var builder))
            {
                return (T)builder(coordinate, registry, state);
            }

            throw new TypeInitializationException(typeof(T).FullName, null);
        }

        public static T Build<T>(Coordinate coordinate, DispatchRegistry registry, string state)
            where T : IDispatchee, ICloner<T>
        {
            var type = typeof(T);

            return Build<T>(type, coordinate, registry, state);
        }

        public static IDispatchee Build(char actor, Coordinate coordinate, DispatchRegistry registry)
        {
            if (ActorBuilderMethods.TryGetValue(actor, out var builder))
            {
                return builder(actor, coordinate, registry);
            }

            throw new ArgumentException($"Unable to find Actor Builder for [{actor}]");
        }

        public static T Build<T>(T t)
            where T : IDispatchee, ICloner<T>
        {
            var type = typeof(T);
            if (ThisBuilderMethods.TryGetValue(type, out var builder))
            {
                return (T)builder(t);
            }

            throw new TypeInitializationException(typeof(T).FullName, null);
        }
    }
}
