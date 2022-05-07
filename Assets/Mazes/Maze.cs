#nullable enable
using Assets.Deeds;
using Assets.Messaging;
using Assets.Resources;
using Assets.Tiles;
using Utils;
using Utils.Coordinates;
using Utils.Dispatching;
using Utils.Display;
using Utils.Random;
using TileChanges = System.Collections.Generic.List<(string UniqueId, Utils.Coordinates.Coordinate Coordinates)>;

namespace Assets.Mazes
{
    public interface IMaze
    {
        bool IsInside(Coordinate coordinate);
        (IDispatched Dispatched, Coordinate Coordinates) RandomTile(Predicate<IDispatched> tileCondition, IList<string> checkedTiles);
        TileChanges GetTiles<TTileType>();
        string[] Replace(TileChanges state);
        bool MoveOnto(string name, IFloor floor);
        void Grow();

        (int Row, int Column) UpperBounds { get; }
        string this[Coordinate point] { get; set; }
        Coordinate this[string uniqueId] { get; }
        IDispatched GetDispatched(Coordinate point);
    }

    internal class Maze : Dispatched<Maze>, IMaze
    {
        internal IDieBuilder DieBuilder { get; }
        internal IResourceBuilder ResourceBuilder { get; }

        protected internal string[,] Tiles;

        public (int Row, int Column) UpperBounds => Tiles.UpperBounds();

        internal Maze(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, IDieBuilder dieBuilder, IResourceBuilder resourceBuilder, string[,] tiles)
            : base(dispatchRegistry, actionRegistry, TilesDisplay.Maze, DispatchedName)
        {
            dieBuilder.ThrowIfNull(nameof(dieBuilder));
            resourceBuilder.ThrowIfNull(nameof(resourceBuilder));

            DieBuilder = dieBuilder;
            ResourceBuilder = resourceBuilder;

            Tiles = tiles.CloneStrings();
        }

        internal Maze(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, IDieBuilder dieBuilder, IResourceBuilder resourceBuilder, string maze)
            : base(dispatchRegistry, actionRegistry, TilesDisplay.Maze, DispatchedName)
        {
            dieBuilder.ThrowIfNull(nameof(dieBuilder));
            resourceBuilder.ThrowIfNull(nameof(resourceBuilder));

            DieBuilder = dieBuilder;
            ResourceBuilder = resourceBuilder;

            Tiles = BuildTiles();
            
            string[,] BuildTiles()
            {
                var tilesArray = maze.SplitIntoLines();
                var noRows = tilesArray.Length;
                var noColumns = tilesArray.Max(row => row.Length);

                var tiles = new string[noRows, noColumns];

                for (int rowIndex = 0; rowIndex < noRows; rowIndex++)
                {
                    var row = tilesArray[rowIndex];

                    for (int colIndex = 0; colIndex < noColumns; colIndex++)
                    {
                        var actor = row[colIndex].ToString();
                        tiles[rowIndex, colIndex] = ResourceBuilder.BuildResource(actor).UniqueId;
                    }
                }

                return tiles;
            }
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

        public bool IsInside(Coordinate coordinate)
        {
            return Tiles.IsInside(coordinate);
        }

        public (IDispatched Dispatched, Coordinate Coordinates) RandomTile(Predicate<IDispatched> tileCondition, IList<string> checkedTiles)
        {
            var (maxRows, maxColumns) = UpperBounds;

            var (tile, randomCoordinates) = GetRandomTile();

            while (! tileCondition(tile))
            {
                if (HasCheckedTile())
                {
                    (tile, randomCoordinates) = GetRandomTile();
                    continue;
                }

                checkedTiles.Add(tile.UniqueId);

                if(Tiles.Length == checkedTiles.Count) throw new Exception("Searched all tiles in RandomTile");

                (tile, randomCoordinates) = GetRandomTile();
            }

            checkedTiles.Add(tile.UniqueId);
            return (tile, randomCoordinates);

            bool HasCheckedTile()
            {
                return checkedTiles.Contains(tile.UniqueId);
            }

            (IDispatched dispatched, Coordinate randomCoordinates) GetRandomTile()
            {
                var coordinates = Tiles.RandomCoordinates(DieBuilder, maxRows, maxColumns);
                var uniqueId = this[coordinates];
                var dispatched = DispatchRegistry.GetDispatched(uniqueId);
                return (dispatched, coordinates);
            }
        }

        public TileChanges GetTiles<TTileType>()
        {
            return Tiles.GetTiles<TTileType>(DispatchRegistry.GetDispatched);
        }

        public string this[Coordinate point]
        {
            get => Tiles[point.Row, point.Column];
            set => Tiles[point.Row, point.Column] = value;
        }
        public Coordinate this[string uniqueId] => Tiles.Locate(name => name.IsSame(uniqueId));

        public IDispatched GetDispatched(Coordinate point) => DispatchRegistry.GetDispatched(this[point]);

        public void Grow()
        {
            var (currentRows, currentColumns) = UpperBounds;

            var (grownRows, grownColumns) = (currentRows * 2, currentColumns * 2);

            var newTiles = new string[grownRows, grownColumns];

            CopyExistingIntoNew();

            Tiles = newTiles;

            Tiles.DefaultTiles(ResourceBuilder.DefaultRockBuilder());

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
