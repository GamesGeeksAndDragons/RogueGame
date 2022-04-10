using System.Collections.Generic;
using System.Linq;
using Assets.Actors;
using Assets.Deeds;
using Assets.Messaging;
using Utils;
using Utils.Coordinates;
using Utils.Dispatching;
using Utils.Enums;
using Utils.Exceptions;
using Utils.Random;

namespace Assets.Tiles
{
    public static class DoorConnector
    {
        public static ITiles ConnectDoors(this ITiles tilesWithUnconnectedDoors, IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, IDieBuilder dieBuilder)
        {
            IList<Door> GetDoors()
            {
                var doorNames = tilesWithUnconnectedDoors.GetTilesOfType<Door>();
                return doorNames.Select(dispatchRegistry.GetDispatchee)
                    .Cast<Door>()
                    .ToList();
            }

            var doors = GetDoors();

            var doorConnector = new DoorConnectorImpl(tilesWithUnconnectedDoors, dispatchRegistry, actionRegistry, dieBuilder);

            while (doors.Count > 0)
            {
                var firstDoor = doors.First();
                var doorsToConnect = doors.Where(door => door.DoorId == firstDoor.DoorId).ToList();
                doorsToConnect.Count.ThrowIfNotEqual(2, "doorsToConnect.Count");

                var first = doorsToConnect[0];
                var second = doorsToConnect[1];
                var firstDirection = GetDirectionToProject(first);
                var secondDirection = GetDirectionToProject(second);

                doorConnector.ConnectDoors(first, firstDirection, second, secondDirection);
                doorsToConnect.ForEach(door => doors.Remove(door));
            }

            return doorConnector.Tiles;

            Compass4Points GetDirectionToProject(Door door)
            {
                foreach (var point in CompassHelpers.ValidCompass4Points)
                {
                    var projectionDirection = SearchDirection(point);
                    if (projectionDirection != Compass4Points.Undefined) return projectionDirection;
                }

                throw new UnexpectedTileException($"Attempting to find the direction to project for door id:[{door.DoorId}] :coordinates [{door.Coordinates}]");

                Compass4Points SearchDirection(Compass4Points direction)
                {
                    var tile = tilesWithUnconnectedDoors.GetDispatchee(door.Coordinates.Move(direction), dispatchRegistry);
                    return tile.IsRock() ? direction : Compass4Points.Undefined;
                }
            }
        }
    }
}
