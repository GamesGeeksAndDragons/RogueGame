using System;
using System.Collections.Generic;
using Assets.Actors;
using Assets.Deeds;
using Utils;
using Utils.Coordinates;
using Utils.Dispatching;
using Utils.Enums;
using Utils.Random;
using TileChanges = System.Collections.Generic.List<(string UniqueId, Utils.Coordinates.Coordinate Coordinates)>;

namespace Assets.Tiles
{
    public interface ITiles
    {
        bool IsInside(Coordinate coordinate);
        (IDispatchee Dispatchee, Coordinate Coordinates) RandomTile(Predicate<IDispatchee> tileCondition);
        bool TileExists(string name);
        TileChanges GetTilesOfType<TTileType>();
        string[] Replace(TileChanges state);
        bool MoveOnto(string name, IFloor floor);
        void Grow();

        (int Row, int Column) UpperBounds { get; }
        string this[Coordinate point] { get; set; }
        Coordinate this[string uniqueId] { get; }
        IDispatchee GetDispatchee(Coordinate point);
    }

    internal class Tiles : ITiles
    {
        private readonly IDispatchRegistry _dispatchRegistry;
        private readonly IActionRegistry _actionRegistry;
        private readonly IDieBuilder _dieBuilder;
        private readonly IActorBuilder _actorBuilder;

        protected internal string[,] TilesRegistryOfDispatcheeNames;

        public (int Row, int Column) UpperBounds => TilesRegistryOfDispatcheeNames.UpperBounds();

        internal Tiles(int maxRows, int maxColumns, IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, IDieBuilder dieBuilder, IActorBuilder actorBuilder)
        {
            dieBuilder.ThrowIfNull(nameof(dieBuilder));
            dispatchRegistry.ThrowIfNull(nameof(dispatchRegistry));
            actionRegistry.ThrowIfNull(nameof(actionRegistry));
            maxRows.ThrowIfBelow(0, nameof(maxRows));
            maxColumns.ThrowIfBelow(0, nameof(maxColumns));

            _dispatchRegistry = dispatchRegistry;
            _actionRegistry = actionRegistry;
            _dieBuilder = dieBuilder;
            _actorBuilder = actorBuilder;

            TilesRegistryOfDispatcheeNames = new string[maxRows, maxColumns];
            DefaultTilesToRock(maxRows, maxColumns);
        }

        internal Tiles(string[,] registryOfDispatcheeNames, IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, IDieBuilder dieBuilder)
        {
            dieBuilder.ThrowIfNull(nameof(dieBuilder));
            dispatchRegistry.ThrowIfNull(nameof(dispatchRegistry));
            registryOfDispatcheeNames.RowUpperBound().ThrowIfBelow(1, nameof(registryOfDispatcheeNames));
            registryOfDispatcheeNames.ColumnUpperBound().ThrowIfBelow(1, nameof(registryOfDispatcheeNames));

            _dispatchRegistry = dispatchRegistry;
            _actionRegistry = actionRegistry;
            _dieBuilder = dieBuilder;
            TilesRegistryOfDispatcheeNames = registryOfDispatcheeNames;
        }

        internal Tiles(ITiles toCopy)
        {
            var tiles = (Tiles)toCopy;
            _dispatchRegistry = tiles._dispatchRegistry;
            _actionRegistry = tiles._actionRegistry;
            _dieBuilder = tiles._dieBuilder;
            TilesRegistryOfDispatcheeNames = tiles.TilesRegistryOfDispatcheeNames.CloneStrings();
        }

        public string[] Replace(TileChanges state)
        {
            var replaced = new List<string>();

            foreach (var (replacement, coordinates) in state)
            {
                var current = this[coordinates];
                replaced.Add(current);
                this[coordinates] = replacement;
            }

            return replaced.ToArray();
        }

        public bool MoveOnto(string name, IFloor floor)
        {
            if (floor.Contains != null) return false;

            var mover = _dispatchRegistry.GetDispatchee(name);
            floor.Contains = mover;

            return true;
        }

        private void DefaultTilesToRock(int maxRows, int maxColumns)
        {
            for (var row = 0; row < maxRows; row++)
            {
                for (var column = 0; column < maxColumns; column++)
                {
                    var coordinates = new Coordinate(row, column);
                    TilesRegistryOfDispatcheeNames.ThrowIfOutsideBounds(coordinates, nameof(TilesRegistryOfDispatcheeNames));

                    var tile = this[coordinates];
                    if (tile.IsNullOrEmptyOrWhiteSpace())
                    {
                        var rock = _actorBuilder.Build<Rock>("");
                        this[coordinates] = rock.UniqueId;
                    }
                }
            }
        }

        internal Wall CreateWall(Coordinate coordinates, WallDirection direction)
        {
            this[coordinates].ThrowIfNull($"TilesRegistryOfDispatcheeNames[{coordinates}]");

            return _actorBuilder.Build<Wall>(direction.ToString());
        }

        public bool IsInside(Coordinate coordinate)
        {
            return TilesRegistryOfDispatcheeNames.IsInside(coordinate);
        }

        public Coordinate Locate(string uniqueId)
        {
            return TilesRegistryOfDispatcheeNames.Locate(id => uniqueId.IsSame(id));
        }

        public (IDispatchee Dispatchee, Coordinate Coordinates) RandomTile(Predicate<IDispatchee> tileCondition)
        {
            var (maxRows, maxColumns) = UpperBounds;

            IDispatchee tile = null;
            Coordinate randomCoordinates = Coordinate.NotSet;

            while (tile == null || ! tileCondition(tile))
            {
                randomCoordinates = TilesRegistryOfDispatcheeNames.RandomCoordinates(_dieBuilder, maxRows, maxColumns);
                var uniqueId = this[randomCoordinates];
                tile = _dispatchRegistry.GetDispatchee(uniqueId);
            }

            return (tile, randomCoordinates);
        }

        public bool TileExists(string name)
        {
            var (rowMax, colMax) = TilesRegistryOfDispatcheeNames.UpperBounds();
            for (var row = 0; row <= rowMax; row++)
            {
                for (var col = 0; col <= colMax; col++)
                {
                    var tile = TilesRegistryOfDispatcheeNames[row, col];
                    if (tile == name) return true;
                }
            }

            return false;
        }

        public TileChanges GetTilesOfType<TTileType>()
        {
            return TilesRegistryOfDispatcheeNames.GetTilesOfType<TTileType>(_dispatchRegistry.GetDispatchee);
        }

        public string this[Coordinate point]
        {
            get => TilesRegistryOfDispatcheeNames[point.Row, point.Column];
            set => TilesRegistryOfDispatcheeNames[point.Row, point.Column] = value;
        }
        public Coordinate this[string uniqueId] => TilesRegistryOfDispatcheeNames.Locate(name => name.IsSame(uniqueId));

        public IDispatchee GetDispatchee(Coordinate point) => _dispatchRegistry.GetDispatchee(this[point]);

        public void Grow()
        {
            var (currentRows, currentColumns) = UpperBounds;

            var (grownRows, grownColumns) = (currentRows * 2, currentColumns * 2);

            var newTiles = new string[grownRows, grownColumns];

            CopyExistingIntoNew();

            TilesRegistryOfDispatcheeNames = newTiles;

            DefaultTilesToRock(grownRows, grownColumns);

            void CopyExistingIntoNew()
            {
                for (var row = 0; row < currentRows; row++)
                {
                    for (var column = 0; column < currentColumns; column++)
                    {
                        var coordinate = new Coordinate(row, column);
                        newTiles[row, column] = this[coordinate];
                    }
                }
            }
        }
    }
}
