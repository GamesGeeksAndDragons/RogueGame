#nullable enable
using Assets.Deeds;
using Utils.Dispatching;
using Utils.Display;
using Utils.Enums;
using BuilderMethodType= System.Func<Utils.Dispatching.IDispatchRegistry, Assets.Deeds.IActionRegistry, string, string, Utils.Dispatching.IDispatched>;

namespace Assets.Resources
{
    public interface IResourceBuilder
    {
        Func<IDispatched> DefaultRockBuilder();
        Func<string, string, IDispatched> RockBuilder();
        Func<string, string, IDispatched> FloorBuilder();
        Func<string, string, IDispatched> DoorBuilder();
        Func<string, string, IDispatched> NullBuilder();
        Func<string, string, IDispatched> WallBuilder();
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

        private IDispatched BuildResource(string actor, string state="")
        {
            if (_builderMethods.TryGetValue(actor, out var builder))
            {
                return builder(_dispatchRegistry, _actionRegistry, actor, state);
            }

            throw new ArgumentException($"Unable to find Actor Builder for [{actor}]");
        }

        public Func<IDispatched> DefaultRockBuilder() => () => BuildResource(TilesDisplay.Rock, "");
        public Func<string, string, IDispatched> RockBuilder() => BuildResource;
        public Func<string, string, IDispatched> FloorBuilder() => BuildResource;
        public Func<string, string, IDispatched> DoorBuilder() => BuildResource;
        public Func<string, string, IDispatched> NullBuilder() => BuildResource;
        public Func<string, string, IDispatched> WallBuilder() => BuildResource;
    }
}
