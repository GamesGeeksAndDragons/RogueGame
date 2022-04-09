using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Actors;
using Assets.Messaging;
using Utils;
using Utils.Coordinates;
using Utils.Enums;
using Utils.Exceptions;
using Line = System.Collections.Generic.List<(string Id, Utils.Coordinates.Coordinate Coordinates)>;
using ILine = System.Collections.Generic.IList<(string Id, Utils.Coordinates.Coordinate Coordinates)>;

namespace Assets.Tiles
{
    class LineProjector
    {
        private readonly DispatchRegistry _registry;
        private readonly (Compass4Points first, Compass4Points second) _searchDirections;
        private readonly Coordinate _start;
        private readonly Coordinate _target;

        public ILine FoundProjection { get; private set; }

        private List<LineProjector> _children;

        public Tiles Tiles { get; }

        public LineProjector(Line searchedLine, Coordinate start, Coordinate target, (Compass4Points first, Compass4Points second) searchDirections, Tiles tiles, DispatchRegistry registry)
        {
            Tiles = tiles;

            _start = start;
            _target = target;
            _searchDirections = searchDirections;
            _registry = registry;

            FoundProjection = null;
            _children = new List<LineProjector>();

            FoundProjection = ProjectChildren(searchedLine);

            var succeededProjection = _children.FirstOrDefault(projection => projection.FoundProjection != null);
            if (succeededProjection != null)
            {
                FoundProjection = succeededProjection.FoundProjection;
            }
        }

        private ILine ProjectChildren(Line searchedLine)
        {
            var firstStart = _start.Move(_searchDirections.first);
            var firstSearch = Tiles.ProjectLine(firstStart, _registry, _searchDirections.first);
            if (LineHasTarget(firstSearch))
            {
                return searchedLine.JoinLines(firstSearch);
            }

            var secondStart = _start.Move(_searchDirections.second);
            var secondSearch = Tiles.ProjectLine(secondStart, _registry, _searchDirections.second);
            if (LineHasTarget(secondSearch))
            {
                return searchedLine.JoinLines(secondSearch);
            }

            var searched = Search(firstSearch);
            return searched ?? Search(secondSearch);

            ILine Search(Line toSearchLine)
            {
                var numTiles = toSearchLine.Count;
                for (var i = 0; i < numTiles; i++)
                {
                    var toSearchTile = toSearchLine[i];
                    var toSearchSegment = toSearchLine.Take(i + 1).ToList();

                    var fullPath = searchedLine.JoinLines(toSearchSegment);
                    var newSearchDirections = _searchDirections;//.SwapAxis();
                    var child = new LineProjector(fullPath, toSearchTile.Coordinates, _target, newSearchDirections, Tiles, _registry);

                   if (child.FoundProjection != null) return child.FoundProjection;
                }

                return null;
            }
        }

        private (Compass4Points first, Compass4Points second) GetDirectionsToProject(Coordinate start)
        {
            var east = Tiles.GetDispatchee(start.Move(Compass4Points.East), _registry);
            var west = Tiles.GetDispatchee(start.Move(Compass4Points.West), _registry);

            if (east.IsWall() && west.IsWall())
                return (Compass4Points.North, Compass4Points.South);

            var north = Tiles.GetDispatchee(start.Move(Compass4Points.North), _registry);
            var south = Tiles.GetDispatchee(start.Move(Compass4Points.South), _registry);

            if (north.IsWall() && south.IsWall())
                return (Compass4Points.East, Compass4Points.West);

            throw new UnexpectedTileException($"Attempting to find the direction to project for door:coordinates [{start}]");
        }

        bool LineHasTarget(IList<(string Id, Coordinate Coordinates)> line)
        {
            return line.Any(tile => tile.Coordinates == _target);
        }


        private IList<(string Id, Coordinate Coordinates)>[] ProjectionsConnectsToDoor(IList<(string Id, Coordinate Coordinates)>[] projections)
        {
            var connectedProjections = new List<IList<(string Id, Coordinate Coordinates)>>();

            foreach (var line in projections)
            {
                if (LineHasTarget(line))
                {
                    connectedProjections.Add(line);
                }
            }

            return connectedProjections.ToArray();
        }
    }
}