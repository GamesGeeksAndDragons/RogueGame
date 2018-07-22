using System;
using log4net;
using Assets.Tiles;
using Utils;
using Utils.Coordinates;
using Utils.Random;
using Utils.Enums;

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
        private const int TilesPerBlock = 4;

        public RandomRoomBuilder(IRandomNumberGenerator randomNumberGenerator, ILog logger)
        {
            _randomNumberGenerator = randomNumberGenerator;
            _logger = logger;
        }

        internal Tile[,] PopulateBlock(Tile[,] tiles, int rowOffset, int colOffset)
        {
            for (var row = 0; row < TilesPerBlock; row++)
            {
                for (var column = 0; column < TilesPerBlock; column++)
                {
                    var coordindates = new Coordinate(row + rowOffset, column + colOffset);
                    tiles[row, column] = new Tile(coordindates);
                }
            }

            return tiles;
        }

        internal void BuildRoom(int numBlocks)
        {
            var blocks = new RoomBlocks(numBlocks);
            blocks = blocks.ReduceLayout();

            var maxRows = blocks.RowCount + 2;
            var maxCols = blocks.ColumnCount + 2;

            var room = new Room(maxRows, maxCols);

            for (var row = 0; row <= maxRows; row++)
            {
                for (var column = 0; column <= maxCols; column++)
                {
                    var rowOffset = row * TilesPerBlock + 1;
                    var colOffset = column * TilesPerBlock + 1;

                    PopulateBlock(room.Tiles, rowOffset, colOffset);
                }
            }
        }

        internal RoomBlocks DecideLayout(int numBlocks)
        {
            numBlocks.ThrowIfBelow(2, nameof(numBlocks));

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
            Coordinate nextPoint;

            Compass4Points randomDirection;
            do
            {
                randomDirection = _randomNumberGenerator.Enum<Compass4Points>();
                nextPoint = point.Move(randomDirection);
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
                    _randomNumberGenerator.Dice(blocks.RowCount),
                    _randomNumberGenerator.Dice(blocks.ColumnCount)
                );
            } while (!blocks.IsInsideBounds(point));

            return point;
        }
    }
}