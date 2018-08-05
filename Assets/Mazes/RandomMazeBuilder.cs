using Assets.Messaging;
using log4net;
using Utils;
using Utils.Coordinates;
using Utils.Random;
using Utils.Enums;

namespace Assets.Mazes
{
    public class RandomMazeBuilder
    {
        private readonly IRandomNumberGenerator _randomNumberGenerator;
        private readonly IMazeDescriptor _descriptor;
        private readonly ILog _logger;
        private readonly DispatchRegistry _registry;

        public RandomMazeBuilder(IRandomNumberGenerator randomNumberGenerator, IMazeDescriptor descriptor, ILog logger, DispatchRegistry registry)
        {
            _randomNumberGenerator = randomNumberGenerator;
            _descriptor = descriptor;
            _logger = logger;
            _registry = registry;
        }

        internal Maze BuildMaze(int level)
        {
            var mazeDetail = _descriptor[level];

            var blocks = DecideLayout(mazeDetail.BlocksPerRoom);
            blocks = blocks.ReduceLayout();

            var mazeOfRocks = new Maze(blocks, _registry, _randomNumberGenerator);

            var mazeWithSpace = mazeOfRocks.PopulateWithTiles(blocks);
            var mazeWithWalls = mazeWithSpace.PopulateWithWalls();

            return mazeWithWalls;
        }

        internal MazeBlocks DecideLayout(int numBlocks)
        {
            numBlocks.ThrowIfBelow(2, nameof(numBlocks));

            var blocks = new MazeBlocks(numBlocks);

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

        private Coordinate RandomWalkCoordinates(MazeBlocks blocks, Coordinate point)
        {
            Coordinate nextPoint;

            Compass4Points randomDirection;
            do
            {
                randomDirection = _randomNumberGenerator.Enum<Compass4Points>();
                nextPoint = point.Move(randomDirection);
            } while (! blocks.IsInside(nextPoint));

            _logger.Debug($"From Point [{point}] go [{randomDirection.ToString()}] to [{nextPoint}]");

            return nextPoint;
        }

        private Coordinate RandomCoordinates(MazeBlocks blocks)
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