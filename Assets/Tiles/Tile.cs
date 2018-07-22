using Assets.Actors;
using Utils.Coordinates;

namespace Assets.Tiles
{
    public class Tile : Actor
    {
        public override string Name => "TILE";
        public override Actor Clone()
        {
            var actorClones = Actors.CloneActors();
            return new Tile(Coordinates, actorClones);
        }

        public Tile(Coordinate coordinates, Actor[] actors = null)
        {
            Coordinates = coordinates;

            Actors = actors?.CloneActors();
        }

        public Coordinate Coordinates { get; }

        public Actor[] Actors;
    }
}