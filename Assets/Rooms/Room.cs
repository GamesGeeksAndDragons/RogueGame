using Assets.Actors;
using Assets.Messaging;
using Utils.Coordinates;
using RoomTiles = Assets.Tiles.Tiles;

namespace Assets.Rooms
{
    internal class Room : Actor
    {
        public override string Name => "ROOM";
        public override string UniqueId { get; internal set; }

        private readonly ActorRegistry _registry;
        public RoomTiles Tiles;

        internal Room(RoomBlocks blocks, ActorRegistry registry) : base(Coordinate.NotSet)
        {
            _registry = registry;

            Tiles = new RoomTiles(blocks.RowUpperBound, blocks.ColumnUpperBound, registry);
        }

        private Room(Room rhs) : this(rhs, rhs.Tiles)
        {
        }

        private Room(Room rhs, RoomTiles tiles) : base(rhs.Coordinates)
        {
            _registry = rhs._registry;
            Tiles = tiles.Clone();
        }

        public override Actor Clone()
        {
            return new Room(this);
        }

        internal Room PopulateWithTiles(RoomBlocks blocks)
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

            return new Room(this, tiles);
        }

        internal Room PopulateWithWalls()
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

            return new Room(this, tiles);
        }

        public override string ToString()
        {
            return Tiles.ToString();
        }
    }
}
