using Assets.Level;
using Assets.Personas;
using Assets.Rooms;
using Utils;
using Utils.Coordinates;
using Utils.Dispatching;

namespace AssetsTests.Helpers
{
    static class PrinterHelper
    {
        public static string Print(this IGameLevel level, IDispatchRegistry dispatchRegistry)
        {
            var maze = (Maze)level.Maze;
            ICharacter CharacterGetter(Coordinate position) => level[position];

            return maze.Tiles.Print((Func<Coordinate, ICharacter>)CharacterGetter, dispatchRegistry);
        }

        public static string Print(this IRoom room, IDispatchRegistry dispatchRegistry)
        {
            var maze = (Maze)((Room)(room)).Maze;
            ICharacter CharacterGetter (Coordinate _) => null;

            return maze.Tiles.Print(CharacterGetter, dispatchRegistry);
        }

        private static string Print(this string[,] tiles, Func<Coordinate, ICharacter> characterGetter, IDispatchRegistry dispatchRegistry)
        {
            return tiles.Print(position =>
            {
                IDispatched LookupTile()
                {
                    var uniqueId = tiles[position.Row, position.Column];
                    return dispatchRegistry.GetDispatched(uniqueId);
                }

                var dispatched = (IDispatched)characterGetter(position);
                dispatched ??= LookupTile();

                return dispatched.ToString();
            });
        }
    }
}
