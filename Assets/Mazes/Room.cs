using System.Collections.Generic;
using Assets.Actors;
using Assets.Messaging;
using Utils;
using Utils.Coordinates;
using Utils.Enums;
using Utils.Random;
using ExtractedParameters = System.Collections.Generic.IReadOnlyList<(string name, string value)>;

namespace Assets.Mazes
{
    internal class Room : Dispatchee<Room>
    {
        private IRandomNumberGenerator _randomNumbers;
        public RoomTiles Tiles;

        internal Room(RoomBlocks blocks, DispatchRegistry registry, IRandomNumberGenerator randomNumbers, int tilesPerBlock)
            : base(Coordinate.NotSet, registry)
        {
            blocks.ThrowIfNull(nameof(blocks));
            randomNumbers.ThrowIfNull(nameof(randomNumbers));

            _randomNumbers = randomNumbers;

            Tiles = new RoomTiles(blocks.RowUpperBound, blocks.ColumnUpperBound, tilesPerBlock, Registry, _randomNumbers);
        }

        private Room(Room room) : base(room.Coordinates, room.Registry)
        {
            _randomNumbers = room._randomNumbers;

            Tiles = (RoomTiles)room.Tiles.Clone();
        }

        public override Room Create()
        {
            return new Room(this);
        }

        public override void UpdateState(Room door, ExtractedParameters state)
        {
            if (state.HasValue(nameof(Tiles)))
            {
                var tiles = state.ToString(nameof(Tiles));
                var tilesChanged = tiles.ToTiles();

                foreach (var tileChange in tilesChanged)
                {
                    door.Tiles[tileChange.Coordinates] = tileChange.Name;
                }
            }

            base.UpdateState(door, state);
        }

        internal Room PopulateWithSpace(RoomBlocks blocks)
        {
            var emptyTiles = new List<(string Name, Coordinate Coordinates)>();

            for (var blockRow = 0; blockRow <= blocks.RowUpperBound; blockRow++)
            {
                for (var blockCol = 0; blockCol <= blocks.ColumnUpperBound; blockCol++)
                {
                    var coordinate = new Coordinate(blockRow, blockCol);
                    if (blocks[coordinate])
                    {
                        var blockOfEmptyTiles = Tiles.PopulateBlock(blockRow, blockCol);
                        emptyTiles.AddRange(blockOfEmptyTiles);
                    }
                }
            }

            return Clone(emptyTiles.ToTilesState());
        }

        internal Room PopulateWithWalls()
        {
            var tilesWithWalls = new List<(string Name, Coordinate Coordinates)>();

            var (rowMax, colMax) = Tiles.UpperBounds;
            for (var row = 0; row <= rowMax; row++)
            {
                for (var col = 0; col <= colMax; col++)
                {
                    var coordinate = new Coordinate(row, col);
                    if (Tiles[coordinate] != null)
                    {
                        Tiles.PopulateWall(coordinate, tilesWithWalls);
                    }
                }
            }

            return Clone(tilesWithWalls.ToTilesState());
        }

        internal bool HasDoors => Tiles.HasDoors;

        internal Room AddDoor(int doorNumber)
        {
            IDispatchee tile = null;
            while(tile == null)
            {
                var tileName = FindNonEmptyTile();
                tile = Registry.GetDispatchee(tileName);

                if (! (tile is Wall))
                {
                    tile = null;
                }
            }

            var door = new Door(tile.Coordinates, Registry, doorNumber);
            var newState = door.Coordinates.ToTileState(door.UniqueId);

            return Clone(newState);
        }

        private string FindNonEmptyTile()
        {
            var coordinates = Tiles.RandomEmptyTile();

            coordinates = WalkUntilAWallIsFound(coordinates);

            return Tiles[coordinates];
        }

        private Coordinate WalkUntilAWallIsFound(Coordinate coordinates)
        {
            var direction = _randomNumbers.Enum<Compass4Points>();

            while (Tiles[coordinates].IsNullOrEmpty())
            {
                coordinates = coordinates.Move(direction);
            }

            return coordinates;
        }

        public override string ToString()
        {
            return Tiles.ToString();
        }

        public (Coordinate TopLeft, Coordinate TopRight, Coordinate BottomLeft, Coordinate BottomRight) GetCorners()
        {
            return Tiles.GetCorners();
        }
    }
}
