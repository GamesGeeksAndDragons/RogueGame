using System;
using System.Collections.Generic;
using Assets.Actors;
using Assets.Deeds;
using Assets.Messaging;
using Utils;
using Utils.Coordinates;
using Utils.Dispatching;
using Utils.Enums;
using Utils.Random;
using ExtractedParameters = System.Collections.Generic.IReadOnlyList<(string Name, string Value)>;

namespace Assets.Tiles
{
    public class Tiles : ICloner<Tiles>
    {
        protected internal readonly DispatchRegistry DispatchRegistry;
        protected internal readonly ActionRegistry ActionRegistry;

        protected readonly IDieBuilder DieBuilder;
        protected string[,] TilesRegistryOfDispatcheeNames;

        public (int Row, int Column) UpperBounds => TilesRegistryOfDispatcheeNames.UpperBounds();

        internal Tiles(int maxRows, int maxColumns, DispatchRegistry dispatchRegistry, ActionRegistry actionRegistry, IDieBuilder dieBuilder)
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

        internal Tiles(string[,] registryOfDispatcheeNames, DispatchRegistry dispatchRegistry, ActionRegistry actionRegistry, IDieBuilder dieBuilder)
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

        protected Tiles(Tiles tiles)
        {
            DispatchRegistry = tiles.DispatchRegistry;
            ActionRegistry = tiles.ActionRegistry;
            DieBuilder = tiles.DieBuilder;
            TilesRegistryOfDispatcheeNames = tiles.TilesRegistryOfDispatcheeNames.CloneStrings();
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

        public Coordinate RandomTile(Predicate<Coordinate> tileCondition)
        {
            var (maxRows, maxColumns) = UpperBounds;

            Coordinate coordinates;

            bool found;

            do
            {
                var row = DieBuilder.Dice(maxRows).Random;
                var col = DieBuilder.Dice(maxColumns).Random;

                coordinates = new Coordinate(row, col);
                found = TilesRegistryOfDispatcheeNames.IsInside(coordinates);
                if (found)
                {
                    found = tileCondition(coordinates);
                }
            } while (! found);

            return coordinates;
        }

        public bool IsEmptyTile(Coordinate coordinates)
        {
            return this[coordinates].IsNullOrEmpty();
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

        public Coordinate RandomEmptyTile()
        {
            return RandomTile(IsEmptyTile);
        }

        public Coordinate RandomRockTile()
        {
            return RandomTile(coordinates => !IsEmptyTile(coordinates) && this.IsRock(coordinates, DispatchRegistry));
        }

        public IList<string> GetTilesOfType<TTileType>()
        {
            var tiles = TilesRegistryOfDispatcheeNames.GetTilesOfType<TTileType>(DispatchRegistry.GetDispatchee);
            return tiles;
        }

        public (Coordinate TopLeft, Coordinate TopRight, Coordinate BottomLeft, Coordinate BottomRight) GetCorners()
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

        public override string ToString()
        {
            return TilesRegistryOfDispatcheeNames.Print(uniqueId =>
            {
                if (uniqueId == null) return "";

                var dispatchee = DispatchRegistry.GetDispatchee(uniqueId);

                return dispatchee.ToString();
            });
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

        public Tiles Clone(string stateChange = null)
        {
            var clone = Create();
            if (stateChange.IsNullOrEmpty()) return clone;

            var state = stateChange.ToParameters();
            UpdateState(clone, state);

            return clone;
        }

        public virtual Tiles Create()
        {
            return new Tiles(this);
        }

        public virtual void UpdateState(Tiles tiles, ExtractedParameters state)
        {
            foreach (var (name, pos) in state)
            {
                var coordinates = pos.ToCoordinates();
                tiles[coordinates] = name;
            }
        }
    }
}
