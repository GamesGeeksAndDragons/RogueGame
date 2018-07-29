using Assets.Actors;
using Assets.Messaging;
using Utils.Coordinates;
using Utils.Random;
using RoomTiles = Assets.Rooms.Tiles;

namespace Assets.Rooms
{
    internal class Room : Actor<Room>, IActor
    {
        private readonly IRandomNumberGenerator _randomNumbers;
        public RoomTiles Tiles;

        internal Room(RoomBlocks blocks, ActorRegistry registry, IRandomNumberGenerator randomNumbers) 
            : base(Coordinate.NotSet, registry)
        {
            _randomNumbers = randomNumbers;

            Tiles = new RoomTiles(blocks.RowUpperBound, blocks.ColumnUpperBound, Registry, _randomNumbers);
        }

        private Room(Room rhs) : this(rhs, rhs.Tiles)
        {
        }

        private Room(Room rhs, RoomTiles tiles) : base(rhs)
        {
            Tiles = tiles.Clone();
        }

        public override IActor Clone()
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

            var (rowMax, colMax) = tiles.UpperBounds;
            for (var row = 0; row <= rowMax; row++)
            {
                for (var col = 0; col <= colMax; col++)
                {
                    var coordinate = new Coordinate(row, col);
                    if (tiles[coordinate] != null)
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

        public Room Place(IActor actor, Coordinate coordinates)
        {
            var tiles = Tiles.Clone();
            tiles[coordinates] = actor.UniqueId;

            return new Room(this, tiles);
        }
    }
}
