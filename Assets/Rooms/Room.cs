using Assets.Messaging;
using Utils;
using Utils.Coordinates;
using Utils.Enums;
using Utils.Random;
using RoomTiles = Assets.Rooms.Tiles;
using ExtractedParameters = System.Collections.Generic.IReadOnlyList<(string name, string value)>;

namespace Assets.Rooms
{
    internal class Room : Dispatchee<Room>
    {
        private readonly IRandomNumberGenerator _randomNumbers;
        public RoomTiles Tiles;

        internal Room(RoomBlocks blocks, DispatchRegistry registry, IRandomNumberGenerator randomNumbers) 
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

        public override IDispatchee Clone(string parameters=null)
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

        private void PlaceInRoom(IDispatchee dispatchee, Coordinate coordinates)
        {
            var parameters = $"Coordinates [{coordinates}]";
            var newDispatchee = dispatchee.Clone(parameters);

            var tiles = Tiles.Clone();
            if (tiles.IsInside(dispatchee.Coordinates))
            {
                tiles[dispatchee.Coordinates] = null;
            }

            tiles[newDispatchee.Coordinates] = newDispatchee.UniqueId;

            Registry.Swap(dispatchee, newDispatchee);

            var newRoom = new Room(this, tiles);

            Registry.Swap(this, newRoom);
        }

        protected internal override void RegisterActions()
        {
            RegisterAction(Actions.Teleport, TeleportImpl);
            RegisterAction(Actions.Move, MoveImpl);
        }

        public void TeleportImpl(ExtractedParameters parameters)
        {
            var dispatchee = parameters.GetDispatchee("Dispatchee", Registry);
            var coordinates = Tiles.RandomEmptyTile();

            PlaceInRoom(dispatchee, coordinates);
        }

        public void MoveImpl(ExtractedParameters parameters)
        {
            var dispatchee = parameters.GetDispatchee("Dispatchee", Registry);
            var direction = parameters.ToValue<Compass8Points>("Direction");
            var newCoordindates = dispatchee.Coordinates.Move(direction);

            if (!Tiles.IsInside(newCoordindates)) return;
            if (!Tiles[newCoordindates].IsNullOrEmpty()) return;

            PlaceInRoom(dispatchee, newCoordindates);
        }
    }
}
