﻿using System;
using System.Collections.Generic;
using Assets.Actors;
using Assets.Messaging;
using Utils;
using Utils.Coordinates;
using Utils.Enums;
using Utils.Random;
using ExtractedParameters = System.Collections.Generic.IReadOnlyList<(string name, string value)>;

namespace Assets.Tiles
{
    class Tiles : ICloner<Tiles>
    {
        protected readonly DispatchRegistry Registry;
        protected readonly IRandomNumberGenerator RandomNumbers;
        protected string[,] TilesRegistry;

        public (int Row, int Column) UpperBounds => TilesRegistry.UpperBounds();

        internal Tiles(int maxRows, int maxColumns, DispatchRegistry registry, IRandomNumberGenerator randomNumbers)
        {
            randomNumbers.ThrowIfNull(nameof(randomNumbers));
            registry.ThrowIfNull(nameof(registry));
            maxRows.ThrowIfBelow(0, nameof(maxRows));
            maxColumns.ThrowIfBelow(0, nameof(maxColumns));

            Registry = registry;
            RandomNumbers = randomNumbers;

            TilesRegistry = new string[maxRows, maxColumns];
            DefaultTilesToRock(maxRows, maxColumns);
        }

        protected Tiles(Tiles tiles)
        {
            Registry = tiles.Registry;
            RandomNumbers = tiles.RandomNumbers;
            TilesRegistry = tiles.TilesRegistry.CloneStrings();
        }

        private void DefaultTilesToRock(int maxRows, int maxColumns)
        {
            for (var row = 0; row < maxRows; row++)
            {
                for (var column = 0; column < maxColumns; column++)
                {
                    var rockCoordinates = new Coordinate(row, column);
                    TilesRegistry.ThrowIfOutsideBounds(rockCoordinates, nameof(TilesRegistry));

                    var rock = ActorBuilder.Build<Rock>(rockCoordinates, Registry, "");
                    this[rock.Coordinates] = rock.UniqueId;
                }
            }
        }

        protected Wall CreateWall(Coordinate coordinates, WallDirection direction)
        {
            this[coordinates].ThrowIfNull($"TilesRegistry[{coordinates}]");

            return ActorBuilder.Build<Wall>(coordinates, Registry, direction.ToString());
        }

        public bool IsInside(Coordinate coordinate)
        {
            return TilesRegistry.IsInside(coordinate);
        }

        public Coordinate RandomTile(Predicate<Coordinate> tileCondition)
        {
            var (maxRows, maxColumns) = UpperBounds;

            Coordinate coordinates;

            bool found;

            do
            {
                var row = RandomNumbers.Dice(maxRows);
                var col = RandomNumbers.Dice(maxColumns);

                coordinates = new Coordinate(row, col);
                found = TilesRegistry.IsInside(coordinates);
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
            var (rowMax, colMax) = TilesRegistry.UpperBounds();
            for (var row = 0; row <= rowMax; row++)
            {
                for (var col = 0; col <= colMax; col++)
                {
                    var tile = TilesRegistry[row, col];
                    if (tile == name) return true;
                }
            }

            return false;
        }

        public Coordinate RandomEmptyTile()
        {
            return RandomTile(IsEmptyTile);
        }

        public bool IsTileType<T>(Coordinate coordinates)
        {
            var name = this[coordinates];
            if (name.IsNullOrEmpty()) return false;

            var dispatchee = Registry.GetDispatchee(name);
            return dispatchee.Name == typeof(T).Name;
        }

        public Coordinate RandomRockTile()
        {
            return RandomTile(coordinates => !IsEmptyTile(coordinates) && IsTileType<Rock>(coordinates));
        }

        public IList<string> GetTilesOfType<TTileType>()
        {
            var tiles = TilesRegistry.GetTilesOfType<TTileType>(Registry.GetDispatchee);
            return tiles;
        }

        public (Coordinate TopLeft, Coordinate TopRight, Coordinate BottomLeft, Coordinate BottomRight) GetCorners()
        {
            var (maxRow, maxColumn) = TilesRegistry.UpperBounds();

            var topLeft  = Registry.GetDispatchee(TilesRegistry[0, 0]);
            var topRight = Registry.GetDispatchee(TilesRegistry[0, maxColumn]);
            var bottomLeft = Registry.GetDispatchee(TilesRegistry[maxRow, 0]);
            var bottomRight = Registry.GetDispatchee(TilesRegistry[maxRow, maxColumn]);

            return (topLeft.Coordinates, topRight.Coordinates, bottomLeft.Coordinates, bottomRight.Coordinates);
        }

        public string this[Coordinate point]
        {
            get => TilesRegistry[point.Row, point.Column];
            set => TilesRegistry[point.Row, point.Column] = value;
        }

        public override string ToString()
        {
            return TilesRegistry.Print(uniqueId =>
            {
                if (uniqueId == null) return "";

                var dispatchee = Registry.GetDispatchee(uniqueId);

                return dispatchee.ToString();
            });
        }

        internal static Tiles Grow(Tiles tiles)
        {
            var (maxRow, maxColumn) = tiles.UpperBounds;
            var grown = new Tiles(maxRow * 2, maxColumn * 2, tiles.Registry, tiles.RandomNumbers);

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
            if (state.HasValue(nameof(Tiles)))
            {
                var newTiles = state.ToString(nameof(Tiles));

                var tileValues = newTiles.ToTiles();
                foreach (var (name, coordinates) in tileValues)
                {
                    tiles[coordinates] = name;
                }
            }
        }
    }
}
