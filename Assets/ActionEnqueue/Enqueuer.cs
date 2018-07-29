namespace Assets.ActionEnqueue
{
    public interface IEnqueuer
    {
        string Name { get; }
        void Enqueue();
    }

    internal abstract class Enqueuer<T> where T : Enqueuer<T>, IEnqueuer
    {
        public string Name => ActionName;
        public static string ActionName = typeof(T).Name;

        public abstract void Enqueue();
    }
}