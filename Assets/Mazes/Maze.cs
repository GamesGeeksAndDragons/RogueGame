﻿using System;
using System.Collections.Generic;
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
        IDispatchee this[Coordinate coordinate] { get; }
        string Name { get; }
        string UniqueId { get; }
        (IDispatchee Dispatchee, Coordinate Coordinates) RandomTile(Predicate<IDispatchee> condition);
        void Update(TileChanges state);
        bool IsInMaze(string uniqueId);
    }

    internal class Maze : Dispatchee<Maze>, IMaze
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

        public IDispatchee this[Coordinate coordinate]
        {
            get
            {
                var name = Tiles[coordinate];
                return DispatchRegistry.GetDispatchee(name);
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

        public (IDispatchee Dispatchee, Coordinate Coordinates) RandomTile(Predicate<IDispatchee> condition)
        {
            return Tiles.RandomTile(condition);
        }
    }
}
