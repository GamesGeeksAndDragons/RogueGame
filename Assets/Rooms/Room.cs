using Assets.ActionEnqueue;
using Assets.Actors;
using Assets.Messaging;
using Utils;
using Utils.Coordinates;
using Utils.Enums;
using Utils.Random;
using RoomTiles = Assets.Rooms.Tiles;
using ExtractedParameters = System.Collections.Generic.IReadOnlyList<(string name, string value)>;

namespace Assets.Rooms
{
    internal class Room : Actor<Room>, IActor
    {
        private readonly IRandomNumberGenerator _randomNumbers;
        public RoomTiles Tiles;

        internal Room(RoomBlocks blocks, ActorRegistry actorRegistry, IRandomNumberGenerator randomNumbers) 
            : base(Coordinate.NotSet, actorRegistry)
        {
            _randomNumbers = randomNumbers;

            Tiles = new RoomTiles(blocks.RowUpperBound, blocks.ColumnUpperBound, ActorRegistry, _randomNumbers);
        }

        private Room(Room rhs) : this(rhs, rhs.Tiles)
        {
        }

        private Room(Room rhs, RoomTiles tiles) : base(rhs)
        {
            Tiles = tiles.Clone();
        }

        public override IActor Clone(string parameters=null)
        {
            return new Room(this);
        }

        internal Room PopulateWithTiles(RoomBlocks blocks)
        {
            var tiles = new RoomTiles(Tiles);

            for (var blockRow = 0; blockRow <= blocks.RowUpperBound; blockRow++)
            {
                for (var blockCol = 0; blockCol <= blocks.ColumnUpperBound; blockCol++)
                {
                    var coordinate = new Coordinate(blockRow, blockCol);
                    if(blocks[coordinate])
                    {
                        tiles.PopulateBlock(blockRow, blockCol);
                    }
                }
            }

            return new Room(this, tiles);
        }

        internal Room PopulateWithWalls()
        {
            var tiles = new RoomTiles(Tiles);

            var (rowMax, colMax) = tiles.UpperBounds;
            for (var row = 0; row <= rowMax; row++)
            {
                for (var col = 0; col <= colMax; col++)
                {
                    var coordinate = new Coordinate(row, col);
                    if (tiles[coordinate] != null)
                    {
                        tiles.PopulateWall(coordinate);
                    }
                }
            }

            return new Room(this, tiles);
        }

        public override string ToString()
        {
            return Tiles.ToString();
        }

        private void PlaceActorInRoom(IActor actor, Coordinate coordinates)
        {
            var actorParameters = $"Coordinates {coordinates}";
            var newActor = actor.Clone(actorParameters);

            var tiles = Tiles.Clone();
            if (tiles.IsInside(actor.Coordinates))
            {
                tiles[actor.Coordinates] = null;
            }

            tiles[newActor.Coordinates] = newActor.UniqueId;

            ActorRegistry.Deregister(actor);
            ActorRegistry.Register(newActor);

            var newRoom = new Room(this, tiles);

            ActorRegistry.Deregister(this);
            ActorRegistry.Register(newRoom);
        }

        protected internal override void RegisterActions()
        {
            RegisterAction(Teleport.ActionName, TeleportImpl);
            RegisterAction(Move.ActionName, MoveImpl);
        }

        public void TeleportImpl(ExtractedParameters parameters)
        {
            var actor = parameters.GetActor("Actor", ActorRegistry);
            var coordinates = Tiles.RandomEmptyTile();

            PlaceActorInRoom(actor, coordinates);
        }

        public void MoveImpl(ExtractedParameters parameters)
        {
            var actor = parameters.GetActor("Actor", ActorRegistry);
            var direction = parameters.GetParameter<Compass8Points>("Direction");
            var newCoordindates = actor.Coordinates.Move(direction);

            if (!Tiles.IsInside(newCoordindates)) return;
            if (!Tiles[newCoordindates].IsNullOrEmpty()) return;

            PlaceActorInRoom(actor, newCoordindates);
        }
    }
}
