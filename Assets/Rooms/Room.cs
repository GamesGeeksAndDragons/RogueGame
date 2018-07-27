using Assets.Actors;
using Utils.Coordinates;
using RoomTiles = Assets.Tiles.Tiles;

namespace Assets.Rooms
{
    public class Room : Actor
    {
        public override string Name => "ROOM";
        public RoomTiles Tiles;

        public Room(RoomBlocks blocks)
        {
            Tiles = new RoomTiles(blocks.RowUpperBound, blocks.ColumnUpperBound);
        }

        private Room(RoomTiles tiles)
        {
            Tiles = tiles;
        }

        public override Actor Clone()
        {
            return new Room(Tiles.Clone());
        }

        public Room PopulateWithTiles(RoomBlocks blocks)
        {
            var tiles = new RoomTiles(Tiles);

            for (var blockRow = 0; blockRow <= blocks.RowUpperBound; blockRow++)
            {
                for (var blockCol = 0; blockCol <= blocks.ColumnUpperBound; blockCol++)
                {
                    var coordinate = new Coordinate(blockRow, blockCol);
                    if(blocks[coordinate])
                    {
                        tiles.PopulateBlock(blockRow, blockCol);
                    }
                }
            }

            return new Room(tiles);
        }

        public Room PopulateWithWalls()
        {
            var tiles = new RoomTiles(Tiles);

            for (var row = 0; row <= tiles.RowUpperBound; row++)
            {
                for (var col = 0; col <= tiles.ColumnUpperBound; col++)
                {
                    var coordinate = new Coordinate(row, col);
                    if (tiles[coordinate] == null)
                    {
                        tiles.PopulateWall(coordinate);
                    }
                }
            }

            return new Room(tiles);
        }

        public override string ToString()
        {
            return Tiles.ToString();
        }
    }
}
