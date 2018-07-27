using Assets.Actors;
using Assets.Messaging;
using Assets.Tiles;
using Utils.Enums;

namespace Assets.Actions
{
    public class MoveAction : Action
    {
        public MoveAction(string actor, string to, ActorRegistry registry)
        {
            
        }

        public override string Name => "MOVE";
        public override void Act()
        {
            throw new System.NotImplementedException();
        }
    }
}