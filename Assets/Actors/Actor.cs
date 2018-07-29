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
        IActor Move(Coordinate coordinates);
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

        public string Name => typeof(T).Name;

        public string UniqueId { get; }

        public abstract IActor Clone();

        public Coordinate Coordinates { get; private set; }

        public IActor Move(Coordinate coordinates)
        {
            var clone = (Actor<T>)Clone();
            clone.Coordinates = coordinates;
            return (IActor)clone;
        }
    }
}