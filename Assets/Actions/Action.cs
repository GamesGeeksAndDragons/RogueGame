namespace Assets.Actions
{
    public abstract class Action
    {
        public abstract string Name { get; }

        public abstract void Act();
    }
}