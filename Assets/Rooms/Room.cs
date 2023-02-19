#nullable enable
using Assets.Deeds;
using Assets.Mazes;
using Assets.Messaging;
using Assets.Resources;
using Assets.Tiles;
using Utils.Coordinates;
using Utils.Dispatching;
using Utils.Display;
using Utils.Enums;
using Utils.Random;

namespace Assets.Rooms;

public interface IRoom : IDispatched
{
    (int Row, int Column) UpperBounds { get; }
    void AddDoor(int doorNumber);
    (Coordinate topLeft, Coordinate topRight, Coordinate bottomLeft, Coordinate bottomRight) GetSize();
    IMaze Maze { get; }
}

internal class Room : Dispatched<Room>, IRoom
{
    internal Room(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, IDieBuilder dieBuilder, IResourceBuilder resourceBuilder, IMaze maze)
    : base(dispatchRegistry, actionRegistry, TilesDisplay.Room)
    {
        _dieBuilder = dieBuilder;
        _resourceBuilder = resourceBuilder;
        _doorBuilder = _resourceBuilder.DoorBuilder();

        Maze = maze;
    }

    internal Room(Room room, IMaze maze)
        : this(room.DispatchRegistry, room.ActionRegistry, room._dieBuilder, room._resourceBuilder, maze)
    {
    }

    private readonly IDieBuilder _dieBuilder;
    private readonly IResourceBuilder _resourceBuilder;

    public (int Row, int Column) UpperBounds => Maze.UpperBounds;

    public IMaze Maze { get; }
    private readonly Func<string, string, IDispatched> _doorBuilder;

    public void AddDoor(int doorNumber)
    {
        var newDoor = _doorBuilder(doorNumber.ToDoorNumberString(), "");

        var walls = FindWallsWhichCanHoldDoors();
        if (walls.Count == 0) return;
        var random = _dieBuilder.Between(1, walls.Count).Random - 1;
        var toReplaceWithDoor = walls[random];

        Maze[toReplaceWithDoor.Coordinates] = newDoor.UniqueId;
        DispatchRegistry.Unregister(toReplaceWithDoor.Wall.UniqueId);

        List<(Wall Wall, Coordinate Coordinates)> FindWallsWhichCanHoldDoors()
        {
            var wallTiles = new List<(Wall Wall, Coordinate Coordinates)>();
            var (maxRow, maxColumn) = Maze.UpperBounds;

            foreach (var (uniqueId, coordinates) in Maze.GetTiles<Wall>())
            {
                if (!IsOutsideWall(coordinates, maxRow, maxColumn)) continue;
                if (IsNextToDoor(coordinates)) continue;

                var wall = (Wall)Maze.GetDispatched(coordinates);
                var isCorner = wall.WallType.HasDirection(WallDirection.Corner);
                if (isCorner) continue;

                wallTiles.Add((wall, coordinates));
            }

            return wallTiles;
        }

        bool IsOutsideWall(Coordinate coordinates, int maxRow, int maxColumn)
        {
            return coordinates.Row == 0 ||
                   coordinates.Row == maxRow ||
                   coordinates.Column == 0 ||
                   coordinates.Column == maxColumn;
        }

        bool IsNextToDoor(Coordinate coordinates)
        {
            var neighbouringCoordinates = coordinates.Surrounding4Coordinates();

            return neighbouringCoordinates.Any(neighbouring =>
            {
                if (!Maze.IsInside(neighbouring)) return false;

                var neighbouringId = Maze[neighbouring];
                var dispatched = DispatchRegistry.GetDispatched(neighbouringId);
                return dispatched.IsDoor();
            });
        }
    }

    public (Coordinate topLeft, Coordinate topRight, Coordinate bottomLeft, Coordinate bottomRight) GetSize()
    {
        var (maxRow, maxColumn) = Maze.UpperBounds;

        var topLeft = new Coordinate(0, 0);
        var topRight = new Coordinate(0, maxColumn - 1);
        var bottomLeft = new Coordinate(maxRow - 1, 0);
        var bottomRight = new Coordinate(maxRow - 1, maxColumn - 1);

        return (topLeft, topRight, bottomLeft, bottomRight);
    }
}
