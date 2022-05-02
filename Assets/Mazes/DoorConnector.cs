#nullable enable
using Assets.Deeds;
using Assets.Tiles;
using Utils;
using Utils.Dispatching;
using Utils.Random;
using TileChanges = System.Collections.Generic.List<(string UniqueId, Utils.Coordinates.Coordinate Coordinates)>;
using DoorsWithCoordinates = System.Collections.Generic.List<Assets.Tiles.Door>;

namespace Assets.Mazes
{
    public static class DoorConnector
    {
        public static TileChanges GetTunnelToConnectDoors(this IMaze maze, IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, IDieBuilder dieBuilder)
        {
            var tunnel = new TileChanges();

            var doorsAndCoordinates = GetDoors();
            while (doorsAndCoordinates.Count > 0)
            {
                var firstDoor = doorsAndCoordinates.First();
                var doorsToConnect = doorsAndCoordinates.Where(doorAndCoordinates => doorAndCoordinates.DoorId == firstDoor.DoorId).ToList();
                doorsToConnect.Count.ThrowIfNotEqual(2, "doorsToConnect.Count");

                var first = doorsToConnect[0];
                var second = doorsToConnect[1];

                var replaced = ConnectDoors(first, second);
                tunnel.AddRange(replaced);
                doorsToConnect.ForEach(door => doorsAndCoordinates.Remove(door));
            }

            return tunnel;

            TileChanges ConnectDoors(Door firstDoor, Door secondDoor)
            {
                var doorProjector = new DoorProjector(maze, dispatchRegistry, dieBuilder);
                return doorProjector.FindProjection(firstDoor, secondDoor);
            }

            DoorsWithCoordinates GetDoors()
            {
                var doors = maze.GetTiles<Door>();

                return doors
                    .Select(tile => (Door)dispatchRegistry.GetDispatched(tile.UniqueId))
                    .ToList();
            }
        }
    }
}
