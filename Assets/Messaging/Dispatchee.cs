using System;
using System.Collections.Generic;
using Utils;
using Utils.Coordinates;
using ExtractedParameters = System.Collections.Generic.IReadOnlyList<(string name, string value)>;

// ReSharper disable VirtualMemberCallInConstructor

namespace Assets.Messaging
{
    public interface ICloner<T>
    {
        T Clone(string stateChange = null);
        T Create();
        void UpdateState(T dispatchee, ExtractedParameters state);
    }

    public interface IDispatchee
    {
        void Dispatch(string dispatchee, string parameters);
        IDispatchee CloneDispatchee(string stateChange = null);

        Coordinate Coordinates { get; }

        string Name { get; }
        string UniqueId { get; }
    }

    internal abstract class Dispatchee<T> : IDispatchee, ICloner<T>
        where T : class
    {
        protected internal DispatchRegistry Registry;

        protected Dispatchee(Coordinate coordinates, DispatchRegistry registry)
        {
            Coordinates = coordinates;
            Registry = registry;

            UniqueId = Registry.Register(this);

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

        public virtual void UpdateState(T t, ExtractedParameters state)
        {
            var dispatchee = t as Dispatchee<T>;
            dispatchee.ThrowIfNull(nameof(t));

            if (state.HasValue(nameof(Coordinates))) dispatchee.Coordinates = state.ToValue<Coordinate>(nameof(Coordinates));
        }

        protected internal static string FormatState(Coordinate? coordinates = null, string uniqueId = null)
        {
            var state = string.Empty;

            if (coordinates.HasValue) state += nameof(Coordinates).FormatParameter(coordinates.Value);
            if (! uniqueId.IsNullOrEmpty()) state += nameof(UniqueId).FormatParameter(uniqueId);

            return state;
        }

        public IDispatchee CloneDispatchee(string stateChange = null)
        {
            return (IDispatchee)Clone(stateChange);
        }

        public T Clone(string stateChange = null)
        {
            var clone = Create();
            if (stateChange.IsNullOrEmpty()) return clone;

            var stateParameters = stateChange.ToParameters();
            UpdateState(clone, stateParameters);

            return clone;
        }

        public abstract T Create();
    }
}
