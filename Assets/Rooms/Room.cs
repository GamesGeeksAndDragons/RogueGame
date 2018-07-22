using Assets.Actors;
using Assets.Tiles;
using Utils;

namespace Assets.Rooms
{
    public class Room : Actor
    {
        public Room(int rows, int columns)
        {
            Tiles = new Tile[rows,columns];
        }

        private Room(Tile[,] tiles)
        {
            Tiles = tiles;
        }

        public override string Name => "ROOM";

        public override Actor Clone()
        {
            return new Room(Tiles.CloneActors());
        }

        public Tile[,] Tiles;

        public override string ToString()
        {
            return Tiles.Print(tile => tile.Actors == null ? "." : tile.Actors.Length.ToString());
        }
    }
}
