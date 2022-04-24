#nullable enable
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
        (IDispatched Dispatched, Coordinate Coordinates) RandomTile(Predicate<IDispatched> tileCondition);
        bool TileExists(string name);
        TileChanges GetTilesOfType<TTileType>();
        string[] Replace(TileChanges state);
        bool MoveOnto(string name, IFloor floor);
        void Grow();

        (int Row, int Column) UpperBounds { get; }
        string this[Coordinate point] { get; set; }
        Coordinate this[string uniqueId] { get; }
        IDispatched GetDispatched(Coordinate point);
    }

    internal class Tiles : ITiles
    {
        internal IDispatchRegistry DispatchRegistry;
        internal IActionRegistry ActionRegistry;
        internal IDieBuilder DieBuilder;
        internal IActorBuilder ActorBuilder;

        protected internal string[,] TilesRegistry;

        public (int Row, int Column) UpperBounds => TilesRegistry.UpperBounds();

        internal Tiles(int maxRows, int maxColumns, IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, IDieBuilder dieBuilder, IActorBuilder actorBuilder)
        {
            dieBuilder.ThrowIfNull(nameof(dieBuilder));
            dispatchRegistry.ThrowIfNull(nameof(dispatchRegistry));
            actionRegistry.ThrowIfNull(nameof(actionRegistry));
            actorBuilder.ThrowIfNull(nameof(actorBuilder));
            maxRows.ThrowIfBelow(0, nameof(maxRows));
            maxColumns.ThrowIfBelow(0, nameof(maxColumns));

            DispatchRegistry = dispatchRegistry;
            ActionRegistry = actionRegistry;
            DieBuilder = dieBuilder;
            ActorBuilder = actorBuilder;
            TilesRegistry = new string[maxRows, maxColumns];

            DefaultTilesToRock(maxRows, maxColumns);
        }

        internal Tiles(string[,] tiles, IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, IDieBuilder dieBuilder, IActorBuilder actorBuilder)
        {
            dieBuilder.ThrowIfNull(nameof(dieBuilder));
            dispatchRegistry.ThrowIfNull(nameof(dispatchRegistry));
            actionRegistry.ThrowIfNull(nameof(actionRegistry));
            actorBuilder.ThrowIfNull(nameof(actorBuilder));

            DispatchRegistry = dispatchRegistry;
            ActionRegistry = actionRegistry;
            DieBuilder = dieBuilder;
            ActorBuilder = actorBuilder;

            TilesRegistry = tiles.CloneStrings();
        }

        internal Tiles(ITiles toCopy, string[,]? tilesRegistry = null)
        {
            var tiles = (Tiles)toCopy;
            DispatchRegistry = tiles.DispatchRegistry;
            ActionRegistry = tiles.ActionRegistry;
            DieBuilder = tiles.DieBuilder;
            ActorBuilder = tiles.ActorBuilder;

            TilesRegistry = tilesRegistry??tiles.TilesRegistry.CloneStrings();
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
            var mover = DispatchRegistry.GetDispatched(name);
            floor.Contains(mover);

            return true;
        }

        private void DefaultTilesToRock(int maxRows, int maxColumns)
        {
            for (var row = 0; row < maxRows; row++)
            {
                for (var column = 0; column < maxColumns; column++)
                {
                    var coordinates = new Coordinate(row, column);
                    TilesRegistry.ThrowIfOutsideBounds(coordinates, nameof(TilesRegistry));

                    var tile = this[coordinates];
                    if (!tile.IsNullOrEmptyOrWhiteSpace()) continue;

                    var rock = ActorBuilder.Build<Rock>();
                    this[coordinates] = rock.UniqueId;
                }
            }
        }

        internal Wall CreateWall(Coordinate coordinates, WallDirection direction)
        {
            this[coordinates].ThrowIfNull($"TilesRegistry[{coordinates}]");

            return ActorBuilder.Build<Wall>(direction.ToString());
        }

        public bool IsInside(Coordinate coordinate)
        {
            return TilesRegistry.IsInside(coordinate);
        }

        public Coordinate Locate(string uniqueId)
        {
            return TilesRegistry.Locate(id => uniqueId.IsSame(id));
        }

        public (IDispatched Dispatched, Coordinate Coordinates) RandomTile(Predicate<IDispatched> tileCondition)
        {
            var (maxRows, maxColumns) = UpperBounds;

            IDispatched? tile = null;
            Coordinate randomCoordinates = Coordinate.NotSet;

            while (tile == null || ! tileCondition(tile))
            {
                randomCoordinates = TilesRegistry.RandomCoordinates(DieBuilder, maxRows, maxColumns);
                var uniqueId = this[randomCoordinates];
                tile = DispatchRegistry.GetDispatched(uniqueId);
            }

            return (tile, randomCoordinates);
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

        public TileChanges GetTilesOfType<TTileType>()
        {
            return TilesRegistry.GetTilesOfType<TTileType>(DispatchRegistry.GetDispatched);
        }

        public string this[Coordinate point]
        {
            get => TilesRegistry[point.Row, point.Column];
            set => TilesRegistry[point.Row, point.Column] = value;
        }
        public Coordinate this[string uniqueId] => TilesRegistry.Locate(name => name.IsSame(uniqueId));

        public IDispatched GetDispatched(Coordinate point) => DispatchRegistry.GetDispatched(this[point]);

        public void Grow()
        {
            var (currentRows, currentColumns) = UpperBounds;

            var (grownRows, grownColumns) = (currentRows * 2, currentColumns * 2);

            var newTiles = new string[grownRows, grownColumns];

            CopyExistingIntoNew();

            TilesRegistry = newTiles;

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
