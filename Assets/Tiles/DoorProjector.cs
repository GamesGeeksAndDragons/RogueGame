﻿using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Actors;
using Utils.Coordinates;
using Utils.Dispatching;
using Utils.Enums;
using Utils.Random;
using Line = System.Collections.Generic.List<(string Id, Utils.Coordinates.Coordinate Coordinates)>;
using ILine = System.Collections.Generic.IList<(string Id, Utils.Coordinates.Coordinate Coordinates)>;

namespace Assets.Tiles
{
    internal class DoorProjector
    {
        private readonly IDispatchRegistry _registry;
        private readonly IDieBuilder _dieBuilder;
        private readonly ITiles _tiles;

        public ILine FindProjection(Door start, Compass4Points startDirection, Door target, Compass4Points targetDirection)
        {
            var startProjection = ProjectDirectionForDoor(start.Coordinates, startDirection);
            if (HasCoordinates(startProjection, target.Coordinates))
            {
                return startProjection;
            }

            var targetProjection = ProjectDirectionForDoor(target.Coordinates, targetDirection);

            if (HasIntersection(startProjection, targetProjection))
            {
                return Join();
            }

            return Search(startProjection, startDirection, targetProjection, targetDirection);

            Line Join()
            {
                var intersectionTile = FindIntersection(startProjection, targetProjection);

                var projection = TakeUpToCoordinates(startProjection, intersectionTile.Coordinates);
                projection.Add(intersectionTile);
                projection.AddRange(TakeUpToCoordinates(targetProjection, intersectionTile.Coordinates));

                return projection;
            }

            Line ProjectDirectionForDoor(Coordinate coordinate, Compass4Points direction)
            {
                var startingTile = coordinate.Move(direction);
                var projection = _tiles.ProjectLine(startingTile, _registry, direction);
                return OrderProjection(projection, direction);
            }
        }

        public DoorProjector(ITiles tiles, IDispatchRegistry registry, IDieBuilder dieBuilder)
        {
            _tiles = tiles;

            _registry = registry;
            _dieBuilder = dieBuilder;

        }

        private Line TakeUpToCoordinates(Line line, Coordinate stopCoordinates)
        {
            return line.TakeWhile(startTile => startTile.Coordinates != stopCoordinates).ToList();
        }

        private Line TakeBetweenCoordinates(Line line, Coordinate startCoordinates, Coordinate stopCoordinates, Compass4Points direction)
        {
            var startCheck = GetDirectionCheck(direction);
            var truncated = line.Where(tile => startCheck(tile, startCoordinates)).ToList();

            var targetCheck = GetDirectionCheck(direction.Rotate(2));
            truncated = truncated.Where(tile => targetCheck(tile, stopCoordinates)).ToList();
            
            return truncated;

            bool IsNorthOf((string Id, Coordinate Coordinates) tile, Coordinate coordinates) => tile.Coordinates.Row < coordinates.Row;
            bool IsSouthOf((string Id, Coordinate Coordinates) tile, Coordinate coordinates) => tile.Coordinates.Row > coordinates.Row;
            bool IsEastOf((string Id, Coordinate Coordinates) tile, Coordinate coordinates) => tile.Coordinates.Column > coordinates.Column;
            bool IsWestOf((string Id, Coordinate Coordinates) tile, Coordinate coordinates) => tile.Coordinates.Column < coordinates.Column;

            Func<(string Id, Coordinate Coordinates), Coordinate, bool> GetDirectionCheck(Compass4Points check)
            {
                switch (check)
                {
                    case Compass4Points.North: return IsNorthOf;
                    case Compass4Points.South: return IsSouthOf;
                    case Compass4Points.East: return IsEastOf;
                    case Compass4Points.West: return IsWestOf;
                    default: throw new ArgumentException($"Unable to GetDirectionCheck for {check}");
                }
            }
        }

        private (string Id, Coordinate Coordinates) FindIntersection(Line line1, Line line2)
        {
            return line1.Single(tile =>
                line2.Any(targetTile => tile.Coordinates == targetTile.Coordinates));
        }

        Line GetRandomLine(List<Line> lines)
        {
            var random = _dieBuilder.Between(1, lines.Count).Random - 1;
            return lines[random];
        }

        Line GetLongestLine(List<Line> lines)
        {
            var longest = lines.Max(line => line.Count);
            var longestLines = lines.Where(line => line.Count == longest).ToList();
            return GetRandomLine(longestLines);
        }

        private List<Line> GetIntersections(List<Line> startIntersections, List<Line> endIntersections)
        {
            return startIntersections.Where(startLine => endIntersections.Any(endLine => HasIntersection(startLine, endLine))).ToList();
        }

        private ILine Search(Line start, Compass4Points startDirection, Line target, Compass4Points targetDirection)
        {
            var startProjections = BuildProjections(start, startDirection);

            Line projectionFromStart;
            var startIntersections = GetIntersections(startProjections, target).ToList();
            if (DoThreeLinesIntersect(startIntersections))
            {
                projectionFromStart = GetRandomLine(startIntersections);

                return Join(start, projectionFromStart, target);
            }

            var targetProjections = BuildProjections(target, targetDirection);
            var targetIntersections = GetIntersections(targetProjections, startProjections).ToList();
            startIntersections = GetIntersections(startProjections, targetProjections).ToList();

            Line projectionToTarget;
            if (DoFourLinesIntersect(startIntersections, targetIntersections))
            {
                projectionFromStart = GetRandomLine(startIntersections);
                projectionToTarget = GetRandomLine(targetIntersections);

                return Join(start, projectionFromStart, projectionToTarget, target);
            }

            projectionFromStart = GetLongestLine(startProjections);
            projectionToTarget = GetLongestLine(targetProjections);

            return Search(start, startDirection, projectionFromStart, projectionToTarget, target); ;
        }

        bool DoThreeLinesIntersect(List<Line> intersections) => intersections.Count != 0;
        bool DoFourLinesIntersect(List<Line> intersections1, List<Line> intersections2) => intersections1.Count != 0 && intersections2.Count != 0;

        List<Line> GetIntersections(List<Line> intersections, Line target)
        {
            return intersections.Where(line => HasIntersection(line, target)).ToList();
        }

        private ILine Search(Line start, Compass4Points startDirection, Line projectionFromStart, Line projectionFromTarget, Line target)
        {
            var projectionFromStartDirection = startDirection.Rotate();
            var startProjections = BuildProjections(projectionFromStart, projectionFromStartDirection);

            var startIntersections = GetIntersections(startProjections, projectionFromTarget).ToList();
            if (! DoThreeLinesIntersect(startIntersections))
            {
                throw new NotImplementedException($"Not implemented finding with six projections");
            }

            var connectingProjection = GetRandomLine(startIntersections);

            return Join(start, projectionFromStart, connectingProjection, projectionFromTarget, target);
        }

        private List<Line> BuildProjections(Line line, Compass4Points direction)
        {
            direction = direction.Rotate();
            var oppositeDirection = direction.Rotate(2);

            var newProjections = new List<Line>();

            foreach (var tile in line)
            {
                var projection = ProjectLine(tile);
                newProjections.Add(projection);
            }

            return newProjections;

            Line ProjectLine((string Id, Coordinate Coordinates) tile)
            {
                var projection = _tiles.ProjectLine(tile.Coordinates, _registry, direction);

                var oppositeProjection = _tiles.ProjectLine(tile.Coordinates, _registry, oppositeDirection);
                oppositeProjection.Remove(tile);

                projection.AddRange(oppositeProjection);

                return projection;
            }
        }

        private Compass4Points GetIntersectionDirection(Coordinate start, Coordinate end)
        {
            if (start == end) return Compass4Points.Undefined;
            if (start.Row == end.Row) return start.Column < end.Column ? Compass4Points.East : Compass4Points.West;
            if (start.Column == end.Column) return start.Row < end.Row ? Compass4Points.South : Compass4Points.North;

            throw new ArgumentException($"Unable to find a direction for start {start} and end {end}. Expected either the row or column to be the same.");
        }

        private ((string Id, Coordinate Coordinates) Start, (string Id, Coordinate Coordinates) End) GetIntersection(Line startProjection, Line joiningProjection, Line endProjection)
        {
            var intersectionStart = FindIntersection(startProjection, joiningProjection);
            var intersectionEnd = FindIntersection(endProjection, joiningProjection);

            return (intersectionStart, intersectionEnd);
        }

        private (((string Id, Coordinate Coordinates) Start, (string Id, Coordinate Coordinates) End) intersection, Line projection) Populate(Line start, Line connecting, Line target)
        {
            var intersection = GetIntersection(start, connecting, target);

            var projection = TakeUpToCoordinates(start, intersection.Start.Coordinates);
            projection.Add(intersection.Start);
            projection.Add(intersection.End);

            var truncated = TruncateOutsideOfIntersection(connecting, intersection);
            projection.AddRange(truncated);

            return (intersection, projection);
        }

        private Line TruncateOutsideOfIntersection(Line connecting, ((string Id, Coordinate Coordinates) Start, (string Id, Coordinate Coordinates) End) intersection)
        {
            var intersectionDirection = GetIntersectionDirection(intersection.Start.Coordinates, intersection.End.Coordinates);

            var orderedProjection = OrderProjection(connecting, intersectionDirection);
            return TakeBetweenCoordinates(orderedProjection, intersection.Start.Coordinates,
                intersection.End.Coordinates, intersectionDirection);
        }

        private Line Join(Line start, Line connecting, Line target)
        {
            var (intersection, projection) = Populate(start, connecting, target);

            projection.AddRange(TakeUpToCoordinates(target, intersection.End.Coordinates));

            return projection;
        }

        private Line Join(Line start, Line connectingStart, Line connectingTarget, Line target)
        {
            var (_, projection) = Populate(start, connectingStart, connectingTarget);
            var (_, targetProjection) = Populate(target, connectingTarget, connectingStart);

            projection.AddRange(targetProjection);

            return projection;
        }

        private Line Join(Line start, Line connectingStart, Line connectingBetween, Line connectingTarget, Line target)
        {
            var (_, projection) = Populate(start, connectingStart, connectingBetween);
            var (_, targetProjection) = Populate(target, connectingTarget, connectingBetween);

            var intersection = GetIntersection(connectingStart, connectingBetween, connectingTarget);
            var truncated = TruncateOutsideOfIntersection(connectingBetween, intersection);
            projection.AddRange(truncated);

            projection.AddRange(targetProjection);

            return projection;
        }

        private Line OrderProjection(Line projection, Compass4Points projectionDirection)
        {
            var orderBy = GetOrderBy();
            return projection.OrderBy(tile => orderBy(tile)).ToList();

            int NorthOrder((string Id, Coordinate Coordinates) tile) => tile.Coordinates.Row * -1;
            int SouthOrder((string Id, Coordinate Coordinates) tile) => tile.Coordinates.Row;
            int EastOrder((string Id, Coordinate Coordinates) tile) => tile.Coordinates.Column;
            int WestOrder((string Id, Coordinate Coordinates) tile) => tile.Coordinates.Column * -1;

            Func<(string, Coordinate), int> GetOrderBy()
            {
                switch (projectionDirection)
                {
                    case Compass4Points.North: return NorthOrder;
                    case Compass4Points.South: return SouthOrder;
                    case Compass4Points.East: return EastOrder;
                    case Compass4Points.West: return WestOrder;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(projectionDirection), projectionDirection, null);
                }
            }
        }

        bool HasCoordinates(ILine line, Coordinate coordinate)
        {
            return line.Any(tile => tile.Coordinates == coordinate);
        }

        bool HasIntersection(ILine line1, ILine line2)
        {
            return line1.Any(tile => HasCoordinates(line2, tile.Coordinates));
        }
    }
}