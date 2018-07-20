using System;
using Assets.Coordinates;
using Assets.Tiles;
using log4net;
using Utils.Enums;
using Utils.Random;

namespace Assets.Rooms
{
    public class RandomRoomBuilder
    {
        private readonly ILog _logger;
        private readonly Func<int, Coordinate> _coordinateGeneration;

        public RandomRoomBuilder(Func<int, Coordinate> generation, ILog logger)
        {
            _coordinateGeneration = generation;
            _logger = logger;
        }

        internal ITile[,] BuildBlock(int x, int y)
        {
            var tiles = new ITile[4, 4];

            for (var i = 0; i < 4; i++)
            for (var j = 0; j < 4; j++)
                tiles[i, j] = Tile.Create(i + x, j + y);

            return tiles;
        }

        internal void BuildRoom(int numBlocks)
        {
            var blocks = new bool[numBlocks + 1, numBlocks + 1];
            blocks[1, 1] = true;

            int x = 1, y = 1;

            var max = --numBlocks;
            while (numBlocks > 0)
            {
                var randomBlock = RandomNumberGenerator.Enum<Compass4Points>();
                switch (randomBlock)
                {
                    case Compass4Points.North:
                        y++;
                        break;
                    case Compass4Points.South:
                        y--;
                        break;
                    case Compass4Points.East:
                        x++;
                        break;
                    case Compass4Points.West:
                        x--;
                        break;
                }

                if (blocks[x, y] == false)
                {
                    blocks[x, y] = true;
                    numBlocks--;
                }
            }
        }

        internal RoomBlocks DecideLayout(int numBlocks)
        {
            if (numBlocks < 2)
                throw new ArgumentException($"Expect more than 3 blocks, got [{numBlocks}]", nameof(numBlocks));

            var blocks = new RoomBlocks(numBlocks);

            var point = RandomCoordinates(blocks);
            blocks[point] = true;
            --numBlocks;

            while (numBlocks > 0)
            {
                if (!blocks[point] && blocks.IsTouchingAnyBlock(point))
                {
                    blocks[point] = true;
                    if (--numBlocks == 0) continue;
                }

                if (blocks.IsCornered(point) || !blocks.IsTouchingAnyBlock(point))
                    point = RandomCoordinates(blocks);
                else
                    point = RandomWalkCoordinates(blocks, point);
            }

            return blocks;
        }

        private Coordinate RandomWalkCoordinates(RoomBlocks blocks, Coordinate point)
        {
            var nextPoint = point;

            Compass4Points randomDirection;
            do
            {
                randomDirection = RandomNumberGenerator.Enum<Compass4Points>();
                switch (randomDirection)
                {
                    case Compass4Points.North:
                        nextPoint = point.GoNorth();
                        break;
                    case Compass4Points.South:
                        nextPoint = point.GoSouth();
                        break;
                    case Compass4Points.East:
                        nextPoint = point.GoEast();
                        break;
                    case Compass4Points.West:
                        nextPoint = point.GoWest();
                        break;
                    default:
                        var message = $"Unrecognised direction [{randomDirection}]";
                        _logger.Fatal(message);
                        throw new ArgumentException(message);
                }
            } while (!blocks.IsInsideBounds(nextPoint));

            _logger.Debug($"From Point [{point}] go [{randomDirection.ToString()}] to [{nextPoint}]");

            return nextPoint;
        }

        private Coordinate RandomCoordinates(RoomBlocks blocks)
        {
            Coordinate point;

            do
            {
                point = _coordinateGeneration.Invoke(blocks.UpperBound);
            } while (!blocks.IsInsideBounds(point));

            return point;
        }
    }
}

public static class CoordinateGenerationStrategies
{
    public static readonly Func<int, Coordinate> RandomStrategy = upperBound => 
        new Coordinate(
            RandomNumberGenerator.Dice(upperBound),
            RandomNumberGenerator.Dice(upperBound));
}