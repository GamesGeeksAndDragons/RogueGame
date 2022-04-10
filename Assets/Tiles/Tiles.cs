using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Actors;
using Assets.Deeds;
using Utils;
using Utils.Coordinates;
using Utils.Dispatching;
using Utils.Enums;
using Utils.Random;
using TileChanges = System.Collections.Generic.List<(string Name, Utils.Coordinates.Coordinate Coordinates)>;

namespace Assets.Tiles
{
    public interface ITiles
    {
        (int Row, int Column) UpperBounds { get; }
        bool IsInside(Coordinate coordinate);
        IDispatchee RandomTile(Predicate<IDispatchee> tileCondition);
        bool TileExists(string name);
        IList<string> GetTilesOfType<TTileType>();
        string this[Coordinate point] { get; set; }
        void Replace(TileChanges state);
        bool MoveOnto(string name, Coordinate coordinates);
    }

    internal class Tiles : ITiles
    {
        protected internal readonly IDispatchRegistry DispatchRegistry;
        protected internal readonly IActionRegistry ActionRegistry;
        protected internal readonly IDieBuilder DieBuilder;

        protected internal string[,] TilesRegistryOfDispatcheeNames;

        public (int Row, int Column) UpperBounds => TilesRegistryOfDispatcheeNames.UpperBounds();

        internal Tiles(int maxRows, int maxColumns, IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, IDieBuilder dieBuilder)
        {
            dieBuilder.ThrowIfNull(nameof(dieBuilder));
            dispatchRegistry.ThrowIfNull(nameof(dispatchRegistry));
            actionRegistry.ThrowIfNull(nameof(actionRegistry));
            maxRows.ThrowIfBelow(0, nameof(maxRows));
            maxColumns.ThrowIfBelow(0, nameof(maxColumns));

            DispatchRegistry = dispatchRegistry;
            ActionRegistry = actionRegistry;
            DieBuilder = dieBuilder;

            TilesRegistryOfDispatcheeNames = new string[maxRows, maxColumns];
            DefaultTilesToRock(maxRows, maxColumns);
        }

        internal Tiles(string[,] registryOfDispatcheeNames, IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, IDieBuilder dieBuilder)
        {
            dieBuilder.ThrowIfNull(nameof(dieBuilder));
            dispatchRegistry.ThrowIfNull(nameof(dispatchRegistry));
            registryOfDispatcheeNames.RowUpperBound().ThrowIfBelow(1, nameof(registryOfDispatcheeNames));
            registryOfDispatcheeNames.ColumnUpperBound().ThrowIfBelow(1, nameof(registryOfDispatcheeNames));

            DispatchRegistry = dispatchRegistry;
            ActionRegistry = actionRegistry;
            DieBuilder = dieBuilder;
            TilesRegistryOfDispatcheeNames = registryOfDispatcheeNames;
        }

        internal Tiles(ITiles toCopy)
        {
            var tiles = (Tiles)toCopy;
            DispatchRegistry = tiles.DispatchRegistry;
            ActionRegistry = tiles.ActionRegistry;
            DieBuilder = tiles.DieBuilder;
            TilesRegistryOfDispatcheeNames = tiles.TilesRegistryOfDispatcheeNames.CloneStrings();
        }

        public void Replace(TileChanges state)
        {
            foreach (var (name, coordinates) in state)
            {
                this[coordinates] = name;
            }
        }

        public bool MoveOnto(string name, Coordinate coordinates)
        {
            var floorName = this[coordinates];
            var floor = (Floor) DispatchRegistry.GetDispatchee(floorName);
            if (floor.Contains != null) return false;

            var mover = DispatchRegistry.GetDispatchee(name);
            floor.Contains = mover;
            return true;
        }

        private void DefaultTilesToRock(int maxRows, int maxColumns)
        {
            for (var row = 0; row < maxRows; row++)
            {
                for (var column = 0; column < maxColumns; column++)
                {
                    var rockCoordinates = new Coordinate(row, column);
                    TilesRegistryOfDispatcheeNames.ThrowIfOutsideBounds(rockCoordinates, nameof(TilesRegistryOfDispatcheeNames));

                    var rock = ActorBuilder.Build<Rock>(rockCoordinates, DispatchRegistry, ActionRegistry, "");
                    this[rock.Coordinates] = rock.UniqueId;
                }
            }
        }

        internal Wall CreateWall(Coordinate coordinates, WallDirection direction)
        {
            this[coordinates].ThrowIfNull($"TilesRegistryOfDispatcheeNames[{coordinates}]");

            return ActorBuilder.Build<Wall>(coordinates, DispatchRegistry, ActionRegistry, direction.ToString());
        }

        public bool IsInside(Coordinate coordinate)
        {
            return TilesRegistryOfDispatcheeNames.IsInside(coordinate);
        }

        //public Coordinate RandomTile(Predicate<Coordinate> tileCondition)
        //{
        //    var (maxRows, maxColumns) = UpperBounds;

        //    Coordinate coordinates;

        //    bool found;

        //    do
        //    {
        //        var row = DieBuilder.Dice(maxRows).Random;
        //        var col = DieBuilder.Dice(maxColumns).Random;

        //        coordinates = new Coordinate(row, col);
        //        found = TilesRegistryOfDispatcheeNames.IsInside(coordinates);
        //        if (found)
        //        {
        //            found = tileCondition(coordinates);
        //        }
        //    } while (! found);

        //    return coordinates;
        //}

        public IDispatchee RandomTile(Predicate<IDispatchee> tileCondition)
        {
            var floorTiles = DispatchRegistry.Dispatchees.Where(dispatchee => tileCondition(dispatchee)).ToList();
            var randomIndex = DieBuilder.Dice(floorTiles.Count).Random - 1;
            return floorTiles[randomIndex];
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

        public IList<string> GetTilesOfType<TTileType>()
        {
            var tiles = TilesRegistryOfDispatcheeNames.GetTilesOfType<TTileType>(DispatchRegistry.GetDispatchee);
            return tiles;
        }

        private (Coordinate TopLeft, Coordinate TopRight, Coordinate BottomLeft, Coordinate BottomRight) GetCorners()
        {
            var (maxRow, maxColumn) = TilesRegistryOfDispatcheeNames.UpperBounds();

            var topLeft  = DispatchRegistry.GetDispatchee(TilesRegistryOfDispatcheeNames[0, 0]);
            var topRight = DispatchRegistry.GetDispatchee(TilesRegistryOfDispatcheeNames[0, maxColumn]);
            var bottomLeft = DispatchRegistry.GetDispatchee(TilesRegistryOfDispatcheeNames[maxRow, 0]);
            var bottomRight = DispatchRegistry.GetDispatchee(TilesRegistryOfDispatcheeNames[maxRow, maxColumn]);

            return (topLeft.Coordinates, topRight.Coordinates, bottomLeft.Coordinates, bottomRight.Coordinates);
        }

        public string this[Coordinate point]
        {
            get => TilesRegistryOfDispatcheeNames[point.Row, point.Column];
            set => TilesRegistryOfDispatcheeNames[point.Row, point.Column] = value;
        }

        internal static Tiles Grow(Tiles tiles)
        {
            var (maxRow, maxColumn) = tiles.UpperBounds;
            var grown = new Tiles(maxRow * 2, maxColumn * 2, tiles.DispatchRegistry, tiles.ActionRegistry, tiles.DieBuilder);

            for (var row = 0; row <= maxRow; row++)
            {
                for (var column = 0; column <= maxColumn; column++)
                {
                    var coordinate = new Coordinate(row, column);
                    grown[coordinate] = tiles[coordinate];
                }
            }

            return grown;
        }
    }
}
