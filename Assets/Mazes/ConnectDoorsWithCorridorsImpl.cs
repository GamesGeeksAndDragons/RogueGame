using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Actors;
using Assets.Messaging;
using Utils;
using Utils.Coordinates;
using Utils.Enums;
using Utils.Exceptions;
using Utils.Random;

namespace Assets.Mazes
{
    static class ConnectDoorsWithCorridorsImpl
    {
        internal static Maze ConnectDoorsWithCorridors(this Maze maze)
        {
            var doors = FindDoorsInMaze(maze);
            while (doors.Count != 0)
            {
                var (door1, door2) = FindDoorsToConnect(doors);
                maze = ConnectDoors(maze, door1, door2);

                doors.Remove(door1);
                doors.Remove(door2);
            }

            return maze;
        }

        private static IList<Door> FindDoorsInMaze(Maze maze)
        {
            var tiles = maze.GetTiles<Door>();

            var doors = tiles.Select(maze.GetDispatchee)
                .Cast<Door>()
                .ToList();

            return doors;
        }

        private static (Door, Door) FindDoorsToConnect(IList<Door> doors)
        {
            var door1 = doors.First();
            var door2 = doors
                .Skip(1)
                .First(door => door.DoorId == door1.DoorId);

            door1.ThrowIfSameInstance(door2, nameof(door1), nameof(door2));

            return (door1, door2);
        }

        private static Maze ConnectDoors(Maze maze, Door door1, Door door2)
        {
            var startDirection = GetPosition(maze.Registry, maze, door1);
            var start = door1.Coordinates.Move(startDirection, 2);
            var targetDirection = GetPosition(maze.Registry, maze, door2);
            var target = door2.Coordinates.Move(targetDirection);

            var tileChanges = new List<(string Name, Coordinate Coordinates)>();

            Coordinate current = Coordinate.NotSet;
            (current, maze) = Tunnel(maze, tileChanges, door1.Coordinates, start.Move(startDirection));

            //while (current != target)
            //{
            //    tileChanges.Clear();
            //    current = Tunnel(maze, tileChanges, current, target);
            //    maze = maze.Clone(stateChange);
            //}

            return maze;
        }
        
        private static (Coordinate next, Maze maze) Tunnel(Maze maze, IList<(string Name, Coordinate Coordinates)> tileChanges, Coordinate current, Coordinate target)
        {
            var direction = GetNextDirection(current, target);
            var numTiles = GetMaxNumTilesToMove(maze, target - current, direction);

            for (int i = 0; i < numTiles && current != target; i++)
            {
                switch (direction)
                {
                    case Compass4Points.West:
                        maze = TunnelWest(maze, current, numTiles);
                        break;
                    case Compass4Points.East:
                        TunnelEast(maze, tileChanges, current, numTiles);
                        break;
                    case Compass4Points.South:
                        TunnelSouth(maze, tileChanges, current, numTiles);
                        break;
                }

                current = current.Move(direction);
            }

            return (current, maze);
        }

        private static void TunnelSouth(Maze maze, IList<(string, Coordinate)> tileChanges, Coordinate coordindates, int numTiles)
        {
            //TunnelTiles(maze, tileChanges, coordindates, Compass8Points.South);
            //TunnelTiles(maze, tileChanges, coordindates, Compass8Points.North, numTiles, WallDirection.Horizontal);
            //TunnelTiles(maze, tileChanges, coordindates, Compass8Points.NorthWest, numTiles, WallDirection.TopLeftCorner);
            //TunnelTiles(maze, tileChanges, coordindates, Compass8Points.West, numTiles, WallDirection.Vertical);
        }

        private static void TunnelEast(Maze maze, IList<(string Name, Coordinate Coordinates)> tileChanges, Coordinate coordindates, int numTiles)
        {
            //TunnelTiles(maze, tileChanges, coordindates, Compass8Points.East);
            //TunnelTiles(maze, tileChanges, coordindates, Compass8Points.North, numTiles, WallDirection.Horizontal);
            //TunnelTiles(maze, tileChanges, coordindates, Compass8Points.NorthWest, numTiles, WallDirection.TopLeftCorner);
            //TunnelTiles(maze, tileChanges, coordindates, Compass8Points.West, numTiles, WallDirection.Vertical);
        }

        private static Maze TunnelWest(Maze maze, Coordinate coordindates, int numTiles)
        {
            var tileChanges = new List<(string Name, Coordinate Coordinates)>();

            coordindates = coordindates.Move(Compass8Points.West);
            TunnelTiles(maze, tileChanges, coordindates);
            TunnelTiles(maze, tileChanges, coordindates.Move(Compass8Points.East));
            //TunnelTiles(maze, tileChanges, coordindates.Move(Compass8Points.NorthEast), WallDirection.BottomRightCorner);
            //TunnelTiles(maze, tileChanges, coordindates.Move(Compass8Points.North),     WallDirection.Horizontal);
            //TunnelTiles(maze, tileChanges, coordindates.Move(Compass8Points.NorthWest), WallDirection.TopLeftCorner);
            //TunnelTiles(maze, tileChanges, coordindates.Move(Compass8Points.West),      WallDirection.Vertical);
            //TunnelTiles(maze, tileChanges, coordindates.Move(Compass8Points.SouthWest), WallDirection.BottomLeftCorner);
            //TunnelTiles(maze, tileChanges, coordindates.Move(Compass8Points.South),     WallDirection.Horizontal);
            //TunnelTiles(maze, tileChanges, coordindates.Move(Compass8Points.SouthEast), WallDirection.TopRightCorner);

            return maze.Clone(tileChanges.ToTilesState());
        }

        private static int GetMaxNumTilesToMove(Maze maze, Coordinate different, Compass4Points direction )
        {
            int max = 1;
            switch (direction)
            {
                case Compass4Points.North:
                    max = Math.Abs(different.Row);
                    break;
                case Compass4Points.South: 
                    max = Math.Abs(different.Row);
                    break;
                case Compass4Points.East:
                    max = Math.Abs(different.Column);
                    break;
                case Compass4Points.West:
                    max = Math.Abs(different.Column);
                    break;
            }

            return maze.RandomBetween(max);
        }

        private static void TunnelTiles(Maze maze, IList<(string, Coordinate)> tileChanges, Coordinate moveCoordinates, WallDirection? wallType=null)
        {
            (string Name, Coordinate Coordinates) tile = (null, moveCoordinates);
            if (wallType.HasValue)
            {
                var wall = new Wall(moveCoordinates, maze.Registry, wallType.Value);
                tile.Name = wall.UniqueId;
            }

            tileChanges.Add(tile);
        }

        private static string TileName(DispatchRegistry registry, Maze maze, Coordinate coordinates, Compass4Points direction)
        {
            var tileCoordinates = coordinates.Move(direction);
            if (!maze.IsInsideMaze(tileCoordinates)) return string.Empty;

            var dispatcheeName = maze.TileName(tileCoordinates);
            if (dispatcheeName.IsNullOrEmpty()) return null;

            var name = registry.GetDispatchee(dispatcheeName).Name;
            return name;
        }

        private static Compass4Points GetPosition(DispatchRegistry registry, Maze maze, Door door)
        {
            var coordinates = door.Coordinates;
            var north = TileName(registry, maze, coordinates, Compass4Points.North);
            var south = TileName(registry, maze, coordinates, Compass4Points.South);
            var east  = TileName(registry, maze, coordinates, Compass4Points.East);
            var west  = TileName(registry, maze, coordinates, Compass4Points.West);

            if (north == south && south == Wall.DispatcheeName && west == Rock.DispatcheeName && east.IsNullOrEmpty())
                return Compass4Points.West;

            if (north == south && south == Wall.DispatcheeName && east == Rock.DispatcheeName && west.IsNullOrEmpty())
                return Compass4Points.East;

            if (east == west && west == Wall.DispatcheeName && north == Rock.DispatcheeName && south.IsNullOrEmpty())
                return Compass4Points.North;

            if (east == west && west == Wall.DispatcheeName && south == Rock.DispatcheeName && north.IsNullOrEmpty())
                return Compass4Points.South;

            throw new StartingPointNotFoundException($"Unable to find starting point for corridor for door number [{door.DoorId}] and id [{door.UniqueId}]");
        }

        private static Compass4Points GetNextDirection(Coordinate source, Coordinate target)
        {
            var difference = target - source;

            if (difference.Row > 0) return Compass4Points.South;
            if (difference.Row < 0) return Compass4Points.North;
            if (difference.Column > 0) return Compass4Points.East;
            if (difference.Column < 0) return Compass4Points.West;

            throw new DirectionNotFoundException($"Unable to find a direction for a source of [{source}] and target of [{target}]");
        }
    }
}
