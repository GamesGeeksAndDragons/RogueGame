using System;
using System.Collections.Generic;
using Assets.Actors;
using Utils.Coordinates;
using ExtractedParameters = System.Collections.Generic.IReadOnlyList<(string name, string value)>;

// ReSharper disable VirtualMemberCallInConstructor

namespace Assets.Messaging
{
    public interface IDispatchee
    {
        void Dispatch(string dispatchee, string parameters);

        Coordinate Coordinates { get; }

        string Name { get; }
        string UniqueId { get; }

        IDispatchee Clone(string parameters = null);
    }

    internal abstract class Dispatchee<T> : IDispatchee
        where T : IDispatchee
    {
        protected internal readonly DispatchRegistry Registry;

        protected Dispatchee(Coordinate coordinates, DispatchRegistry registry)
        {
            Coordinates = coordinates;
            Registry = registry;

            UniqueId = Registry.Register(this);

            RegisterActions();
        }

        protected Dispatchee(Dispatchee<T> rhs)
        {
            Coordinates = rhs.Coordinates;
            Registry = rhs.Registry;
            UniqueId = rhs.UniqueId;

            Registry.Register(this);

            RegisterActions();
        }

        public virtual void Dispatch(string dispatchee, string parameters)
        {
            if (ActionRegistry.TryGetValue(dispatchee, out var actionImpl))
            {
                var extracted = parameters.ToParameters();
                actionImpl(extracted);
            }
        }

        protected internal Dictionary<string, Action<ExtractedParameters>> ActionRegistry = new Dictionary<string, Action<ExtractedParameters>>();

        protected internal void RegisterAction(string action, Action<ExtractedParameters> impl)
        {
            if (ActionRegistry.ContainsKey(action)) throw new ArgumentException($"Action [{action}] already registered", nameof(action));

            ActionRegistry[action] = impl;
        }

        protected internal virtual void RegisterActions() { }

        public Coordinate Coordinates { get; protected set; }

        public static string DispatcheeName => typeof(T).Name;
        public string Name => DispatcheeName;
        public string UniqueId { get; protected internal set; }

        public abstract IDispatchee Clone(string parameters = null);
    }
}
