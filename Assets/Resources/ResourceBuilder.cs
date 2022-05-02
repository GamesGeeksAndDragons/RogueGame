#nullable enable
using Assets.Deeds;
using Utils;
using Utils.Dispatching;
using Utils.Display;
using Utils.Enums;
using BuilderMethodType= System.Func<Utils.Dispatching.IDispatchRegistry, Assets.Deeds.IActionRegistry, string, string, Utils.Dispatching.IDispatched>;

namespace Assets.Resources
{
    public interface IResourceBuilder
    {
        IDispatched BuildTile(string actor, string state = "");
        Func<string, IDispatched> RockBuilder();
        Func<IDispatched> DefaultRockBuilder();
        Func<int, string, IDispatched> FloorBuilder();
        Func<int, string, IDispatched> DoorBuilder();
        Func<WallDirection, string, IDispatched> WallBuilder();
        Func<string, IDispatched> MeBuilder();
        Func<string, IDispatched> MonsterBuilder();
    }

    internal class ResourceBuilder : IResourceBuilder
    {
        private readonly IDispatchRegistry _dispatchRegistry;
        private readonly IActionRegistry _actionRegistry;
        private readonly Dictionary<string, BuilderMethodType> _builderMethods;

        public ResourceBuilder(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry)
        {
            _dispatchRegistry = dispatchRegistry;
            _actionRegistry = actionRegistry;

            _builderMethods = new Dictionary<string, BuilderMethodType>
            {
                {TilesDisplay.Rock,  TilesBuilderMethods.BuildRock},
                {TilesDisplay.Null, TilesBuilderMethods.BuildNull},
                {CharacterDisplay.Me, CharacterBuilderMethods.BuildMe},
                {CharacterDisplay.DebugMonster, CharacterBuilderMethods.BuildMonster},
            };

            foreach (var wall in TilesDisplay.Walls)
            {
                _builderMethods.Add(wall, TilesBuilderMethods.BuildWall);
            }

            foreach (var door in TilesDisplay.Doors)
            {
                _builderMethods.Add(door,  TilesBuilderMethods.BuildDoor);
            }

            foreach (var roomNumberFloorIn in TilesDisplay.RoomNumberOfFloor)
            {
                _builderMethods.Add(roomNumberFloorIn, TilesBuilderMethods.BuildFloor);
            }
        }

        public IDispatched BuildTile(string actor, string state="")
        {
            if (_builderMethods.TryGetValue(actor, out var builder))
            {
                return builder(_dispatchRegistry, _actionRegistry, actor, state);
            }

            throw new ArgumentException($"Unable to find Actor Builder for [{actor}]");
        }

        public Func<string, IDispatched> RockBuilder() => (state) => BuildTile(TilesDisplay.Rock, state);
        public Func<IDispatched> DefaultRockBuilder() => () => BuildTile(TilesDisplay.Rock, "");
        public Func<int, string, IDispatched> FloorBuilder() => (roomNumber, state) => BuildTile(roomNumber.ToRoomNumberString(), state);
        public Func<int, string, IDispatched> DoorBuilder() => (doorNumber, state) => BuildTile(doorNumber.ToDoorNumberString(), state);
        public Func<WallDirection, string, IDispatched> WallBuilder() => (wall, state) => BuildTile(wall.ToString(), state);
        public Func<string, IDispatched> MeBuilder() => (state) => BuildTile(CharacterDisplay.Me, state);
        public Func<string, IDispatched> MonsterBuilder() => (state) => BuildTile(CharacterDisplay.DebugMonster, state);
    }
}
