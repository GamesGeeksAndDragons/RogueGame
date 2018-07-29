using System.Reflection.PortableExecutable;
using Assets.Messaging;
using Utils.Coordinates;

namespace Assets.Actors
{
    public interface IActor
    {
        string Name { get; }

        string UniqueId { get; }

        Coordinate Coordinates { get; }

        IActor Clone();
        void Dispatch(string actor, string parameters);
    }

    internal abstract class Actor<T> where T : Actor<T>, IActor
    {
        protected Actor(Coordinate coordinate, ActorRegistry registry)
        {
            Coordinates = coordinate;
            Registry = registry;
            UniqueId = registry.Register((IActor)this);
        }

        protected Actor(Actor<T> rhs)
        {
            Coordinates = rhs.Coordinates;
            Registry = rhs.Registry;
            UniqueId = rhs.UniqueId;
        }

        protected internal readonly ActorRegistry Registry;

        public string Name => ActorName;
        public static string ActorName => typeof(T).Name;

        public string UniqueId { get; }

        public abstract IActor Clone();

        public Coordinate Coordinates { get; private set; }

        public virtual void Dispatch(string actor, string parameters) {}
    }
}