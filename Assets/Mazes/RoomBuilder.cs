using Assets.Messaging;
using log4net;
using Utils;
using Utils.Coordinates;
using Utils.Enums;
using Utils.Random;

namespace Assets.Mazes
{
    public class RoomBuilder
    {
        private readonly IRandomNumberGenerator _randomNumberGenerator;
        private readonly ILog _logger;
        private readonly DispatchRegistry _registry;

        public RoomBuilder(IRandomNumberGenerator randomNumberGenerator, ILog logger, DispatchRegistry registry)
        {
            randomNumberGenerator.ThrowIfNull(nameof(randomNumberGenerator));
            logger.ThrowIfNull(nameof(logger));
            registry.ThrowIfNull(nameof(registry));

            _randomNumberGenerator = randomNumberGenerator;
            _logger = logger;
            _registry = registry;
        }

        internal Room BuildRoom(int numBlocks, int tilesPerBlock)
        {
            var blocks = DecideLayout(numBlocks);
            blocks = blocks.ReduceLayout();

            var roomOfRocks = new Room(blocks, _registry, _randomNumberGenerator, tilesPerBlock);

            var roomWithSpace = roomOfRocks.PopulateWithSpace(blocks);
            var roomWithWalls = roomWithSpace.PopulateWithWalls();

            return roomWithWalls;
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
                if (!blocks[point] && blocks.IsTouchingAnyBlock(point))
                {
                    blocks[point] = true;
                    if (--numBlocks == 0) continue;
                }

                if (blocks.IsCornered(point) || !blocks.IsTouchingAnyBlock(point))
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
            } while (!blocks.IsInside(nextPoint));

            _logger.Debug($"From Point [{point}] go [{randomDirection.ToString()}] to [{nextPoint}]");

            return nextPoint;
        }

        private Coordinate RandomCoordinates(RoomBlocks blocks)
        {
            Coordinate point;

            do
            {
                point = new Coordinate(
                    _randomNumberGenerator.Dice(blocks.RowUpperBound),
                    _randomNumberGenerator.Dice(blocks.ColumnUpperBound)
                );
            } while (!blocks.IsInside(point));

            return point;
        }
    }
}
