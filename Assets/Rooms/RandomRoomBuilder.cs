using Assets.Messaging;
using log4net;
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
        private readonly ActorRegistry _registry;

        public RandomRoomBuilder(IRandomNumberGenerator randomNumberGenerator, ILog logger, ActorRegistry registry)
        {
            _randomNumberGenerator = randomNumberGenerator;
            _logger = logger;
            _registry = registry;
        }

        internal Room BuildRoom(int numBlocks)
        {
            var blocks = DecideLayout(numBlocks);
            blocks = blocks.ReduceLayout();

            var roomOfRocks = new Room(blocks, _registry, _randomNumberGenerator);

            var room = roomOfRocks.PopulateWithTiles(blocks);
            room = room.PopulateWithWalls();

            _registry.Deregister(roomOfRocks);
            _registry.Register(room);

            return room;
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
            } while (! blocks.IsInside(nextPoint));

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