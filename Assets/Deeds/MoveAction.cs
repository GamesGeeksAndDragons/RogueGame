#nullable enable
using Assets.Actors;
using Assets.Maze;
using Assets.Mazes;
using Assets.Messaging;
using Utils;
using Utils.Coordinates;
using Utils.Dispatching;
using Utils.Enums;

namespace Assets.Deeds
{
    internal abstract class Action : IAction
    {
        public void SetMaze(IMaze maze) => Maze = maze;

        protected IMaze? Maze { get; private set; }

        public abstract void Act(IDispatched dispatched, string actionValue);
    }

    class MoveAction : Action
    {
        public override void Act(IDispatched dispatched, string actionValue)
        {
            var character = (ICharacter)dispatched;

            var oldCoordinates = character.Position;
            var direction = actionValue.ToEnum<Compass8Points>();
            var newCoordinates = oldCoordinates.Move(direction);

            if (Maze == null) throw new ArgumentException($"Tried to Move when Maze is null");

            var newTile = Maze.GetDispatched(newCoordinates);
            if (!newTile.IsFloor()) return;
            var to = (Floor)newTile;

            var moved = Maze.MoveOnto(dispatched.UniqueId, to);
            if (!moved) return;

            character.Position = newCoordinates;
            var from = (Floor)Maze.GetDispatched(oldCoordinates);
            from.SetEmpty();
        }
    }
}
