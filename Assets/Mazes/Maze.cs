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
        (IDispatched Dispatched, Coordinate Coordinates) RandomTile(Predicate<IDispatched> tileCondition);
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

        public (IDispatched Dispatched, Coordinate Coordinates) RandomTile(Predicate<IDispatched> tileCondition)
        {
            var (maxRows, maxColumns) = UpperBounds;

            IDispatched? tile = null;
            Coordinate randomCoordinates = Coordinate.NotSet;

            while (tile == null || ! tileCondition(tile))
            {
                randomCoordinates = Tiles.RandomCoordinates(DieBuilder, maxRows, maxColumns);
                var uniqueId = this[randomCoordinates];
                tile = DispatchRegistry.GetDispatched(uniqueId);
            }

            return (tile, randomCoordinates);
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
