using System;
using System.Collections.Generic;
using Assets.Actions;
using Assets.Coordinates;
using Assets.Random;
using Assets.Tiles;
using log4net;

namespace Assets.Rooms
{
    /*
     * Basic Room unit
     *  ****
     *  ****
     *  ****
     *  Four verticies
     * 
     * Complicated rooms are multiples of the basic unit
     *  
     * */
    public class RandomRoomBuilder
    {
        private readonly IRandomNumberGenerator _randomNumberGenerator;
        private readonly ILog _logger;

        public RandomRoomBuilder(IRandomNumberGenerator randomNumberGenerator, ILog logger)
        {
            _randomNumberGenerator = randomNumberGenerator;
            _logger = logger;
        }

        internal ITile[,] BuildBlock(int x, int y)
        {
            ITile[,] tiles = new ITile[4, 4];

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    tiles[i, j] = Tile.Create(i + x, j + y);
                }
            }

            return tiles;
        }

        internal void BuildRoom(int numBlocks)
        {
            var blocks = new bool[numBlocks + 1, numBlocks + 1];
            blocks[1, 1] = true;

            int x = 1, y = 1;

            int max = --numBlocks;
            while (numBlocks > 0)
            {
                var randomBlock = _randomNumberGenerator.Enum<BlockDirection>();
                switch (randomBlock)
                {
                    case BlockDirection.Up:
                        y++;
                        break;
                    case BlockDirection.Down:
                        y--;
                        break;
                    case BlockDirection.Right:
                        x++;
                        break;
                    case BlockDirection.Left:
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
            if(numBlocks < 2) throw new ArgumentException($"Expect more than 3 blocks, got [{numBlocks}]", nameof(numBlocks));

            var blocks = new RoomBlocks(numBlocks);

            var point = RandomCoordinates(blocks);
            blocks[point] = true;
            --numBlocks;

            while (numBlocks > 0)
            {
                if (! blocks[point] && blocks.IsTouchingAnyBlock(point))
                {
                    blocks[point] = true;
                    if(--numBlocks == 0) continue;
                }

                if (blocks.IsCornered(point) || ! blocks.IsTouchingAnyBlock(point))
                {
                    point = RandomCoordinates(blocks);
                }
                else
                {
                    point = RandomWalkCoordinates(blocks, point);
                }
            }

            return blocks;
        }

        private Coordinate RandomWalkCoordinates(RoomBlocks blocks, Coordinate point)
        {
            var nextPoint = point;

            BlockDirection randomDirection;
            do
            {
                randomDirection = _randomNumberGenerator.Enum<BlockDirection>();
                switch (randomDirection)
                {
                    case BlockDirection.Up:
                        nextPoint = point.North();
                        break;
                    case BlockDirection.Down:
                        nextPoint = point.South();
                        break;
                    case BlockDirection.Right:
                        nextPoint = point.East();
                        break;
                    case BlockDirection.Left:
                        nextPoint = point.West();
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
                point = new Coordinate(
                    _randomNumberGenerator.Dice(blocks.UpperBound),
                    _randomNumberGenerator.Dice(blocks.UpperBound)
                );
            } while (!blocks.IsInsideBounds(point));

            return point;
        }
    }
}