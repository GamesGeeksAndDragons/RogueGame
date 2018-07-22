namespace Assets.Actors
{
    public abstract class Actor
    {
        public abstract string Name { get; }

        public abstract Actor Clone();
    }
}