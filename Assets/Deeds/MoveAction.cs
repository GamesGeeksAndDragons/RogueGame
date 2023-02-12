#nullable enable
using Assets.Level;
using Assets.Messaging;
using Assets.Personas;
using Utils;
using Utils.Coordinates;
using Utils.Enums;

namespace Assets.Deeds
{
    class MoveAction : Action
    {
        public override void Act(IGameLevel level, ICharacter who, string actionValue)
        {
            var oldCoordinates = who.Coordinates;
            var direction = actionValue.ToEnum<Compass8Points>();
            var newCoordinates = oldCoordinates.Move(direction);

            var tile = level.Maze.GetDispatched(newCoordinates);
            var canWalkTo = tile.IsFloor() || tile.IsDoor();
            if (!canWalkTo) return;

            var atNewCoordinates = level[newCoordinates];
            if (atNewCoordinates != null) return;

            who.Coordinates = newCoordinates;
        }
    }
}
