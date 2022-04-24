#nullable enable
using Assets.Actors;
using Assets.Deeds;
using Assets.Messaging;
using Assets.Rooms;
using Assets.Tiles;
using Utils;
using Utils.Coordinates;
using Utils.Dispatching;
using Utils.Random;
using TileChanges = System.Collections.Generic.List<(string Name, Utils.Coordinates.Coordinate Coordinates)>;

namespace Assets.Mazes
{
    public interface IMaze
    {
        IDispatched this[Coordinate coordinate] { get; }
        string Name { get; }
        string UniqueId { get; }
        (IDispatched Dispatched, Coordinate Coordinates) RandomTile(Predicate<IDispatched> condition);
        void Update(TileChanges state);
        bool IsInMaze(string uniqueId);
    }

    internal class Maze : Dispatched<Maze>, IMaze
    {
        internal Maze(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, IDieBuilder dieBuilder, IActorBuilder actorBuilder, int rows, int columns) 
            : base(dispatchRegistry, actionRegistry)
        {
            dieBuilder.ThrowIfNull(nameof(dieBuilder));

            _dieBuilder = dieBuilder;
            _actorBuilder = actorBuilder;

            Tiles = new Tiles.Tiles(rows, columns, dispatchRegistry, actionRegistry, dieBuilder, actorBuilder);

            ActionRegistry.RegisterTiles(Tiles);
        }

        private readonly IDieBuilder _dieBuilder;
        private readonly IActorBuilder _actorBuilder;
        internal ITiles Tiles { get; }

        public IDispatched this[Coordinate coordinate]
        {
            get
            {
                var name = Tiles[coordinate];
                return DispatchRegistry.GetDispatched(name);
            }
        }

        public bool IsInMaze(string uniqueId) => Tiles.TileExists(uniqueId);

        protected internal override void RegisterActions()
        {
        }

        public void Update(TileChanges state)
        {
            Tiles.Replace(state);
        }

        public void PositionRoomsInMaze(IList<Room> roomsWithDoors)
        {
            var removed = Tiles.PositionRoomsInTiles(roomsWithDoors);
            DispatchRegistry.Unregister(removed);

            ActionRegistry.RegisterTiles(Tiles);
        }

        public void ConnectDoorsWithCorridors()
        {
            var changes = Tiles.GetTunnelToConnectDoors(DispatchRegistry, ActionRegistry, _dieBuilder);

            Tiles.ConnectDoorsWithCorridors(changes, DispatchRegistry, _actorBuilder);
        }

        public (IDispatched Dispatched, Coordinate Coordinates) RandomTile(Predicate<IDispatched> condition)
        {
            return Tiles.RandomTile(condition);
        }
    }
}
