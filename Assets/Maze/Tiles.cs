#nullable enable
using Assets.Actors;
using Assets.Deeds;
using Assets.Messaging;
using Utils;
using Utils.Coordinates;
using Utils.Dispatching;
using Utils.Random;
using TileChanges = System.Collections.Generic.List<(string UniqueId, Utils.Coordinates.Coordinate Coordinates)>;

namespace Assets.Maze
{
    public interface ITiles
    {
        bool IsInside(Coordinate coordinate);
        (IDispatched Dispatched, Coordinate Coordinates) RandomTile(Predicate<IDispatched> tileCondition);
        TileChanges GetTilesOfType<TTileType>();
        string[] Replace(TileChanges state);
        bool MoveOnto(string name, IFloor floor);
        void Grow();

        (int Row, int Column) UpperBounds { get; }
        string this[Coordinate point] { get; set; }
        Coordinate this[string uniqueId] { get; }
        IDispatched GetDispatched(Coordinate point);
    }

    internal class Tiles : Dispatched<Tiles>, ITiles
    {
        internal IDieBuilder DieBuilder { get; }
        internal IActorBuilder ActorBuilder { get; }

        protected internal string[,] TilesRegistry;

        public (int Row, int Column) UpperBounds => TilesRegistry.UpperBounds();

        internal Tiles(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, IDieBuilder dieBuilder, IActorBuilder actorBuilder, string[,] tiles)
            : base(dispatchRegistry, actionRegistry, ActorDisplay.Tiles)
        {
            dieBuilder.ThrowIfNull(nameof(dieBuilder));
            actorBuilder.ThrowIfNull(nameof(actorBuilder));

            DieBuilder = dieBuilder;
            ActorBuilder = actorBuilder;

            TilesRegistry = tiles.CloneStrings();
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
            return TilesRegistry.IsInside(coordinate);
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

            TilesRegistry.DefaultTiles(ActorBuilder.RockBuilder());

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
