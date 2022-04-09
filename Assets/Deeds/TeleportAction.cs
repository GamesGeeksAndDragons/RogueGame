using Assets.Actors;
using Assets.Mazes;
using Utils.Dispatching;
using Parameters = System.Collections.Generic.IReadOnlyList<(string Name, string Value)>;

namespace Assets.Deeds
{
    internal class TeleportAction : IAction
    {
        internal Maze Maze;
        private readonly IDispatchRegistry _dispatchRegistry;

        public TeleportAction()
        {
        }

        public void TeleportImpl(Parameters parameters)
        {
        }

        public void Act(IDispatchee dispatchee, string actionValue)
        {
            var floorTile = RandomFloorTile();
            while (! IsEmpty(floorTile))
            {
                floorTile = RandomFloorTile();
            }

            //PlaceInMaze(dispatchee, coordinates);

            bool IsEmpty(Floor floor)
            {
                return floor.OnFloor == null;
            }

            Floor RandomFloorTile()
            {
                return (Floor) Maze.RandomFloorTile();
            }
        }
    }
}
