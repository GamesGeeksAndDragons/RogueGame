using System;
using System.Collections.Generic;
using Assets.Messaging;
using Utils.Coordinates;
using ExtractedParameters = System.Collections.Generic.IReadOnlyList<(string name, string value)>;

namespace Assets.Actors
{
    public interface IActor
    {
        string Name { get; }

        string UniqueId { get; }

        Coordinate Coordinates { get; }

        IActor Clone(string parameters = null);
        void Dispatch(string actor, string parameters);
    }

    internal abstract class Actor<T> where T : Actor<T>, IActor
    {
        protected Actor(Coordinate coordinate, ActorRegistry actorRegistry)
        {
            Coordinates = coordinate;
            ActorRegistry = actorRegistry;
            UniqueId = actorRegistry.Register((IActor)this);

            RegisterActions();
        }

        protected Actor(Actor<T> rhs)
        {
            Coordinates = rhs.Coordinates;
            ActorRegistry = rhs.ActorRegistry;
            UniqueId = rhs.UniqueId;

            RegisterActions();
;        }

        protected internal readonly ActorRegistry ActorRegistry;

        public string Name => ActorName;
        public static string ActorName => typeof(T).Name;

        public string UniqueId { get; }

        public abstract IActor Clone(string parameters=null);

        public Coordinate Coordinates { get; protected set; }

        public virtual void Dispatch(string actor, string parameters)
        {
            if (ActionRegistry.TryGetValue(actor, out var actionImpl))
            {
                var extracted = parameters.ToParameters();
                actionImpl(extracted);
            }
        }

        protected internal Dictionary<string, Action<ExtractedParameters>> ActionRegistry = new Dictionary<string, Action<ExtractedParameters>>();

        protected internal void RegisterAction(string action, Action<ExtractedParameters> impl)
        {
            if(ActionRegistry.ContainsKey(action)) throw new ArgumentException($"Action [{action}] already registered", nameof(action));

            ActionRegistry[action] = impl;
        }

        protected internal virtual void RegisterActions() { }
    }
}