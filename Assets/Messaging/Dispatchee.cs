using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Deeds;
using Utils;
using Utils.Coordinates;
using Utils.Dispatching;
using Parameters = System.Collections.Generic.IReadOnlyList<(string Name, string Value)>;

// ReSharper disable VirtualMemberCallInConstructor

namespace Assets.Messaging
{
    internal abstract class Dispatchee<T> : IDispatchee, ICloner<T>
        where T : class
    {
        protected Dispatchee(Coordinate coordinates, DispatchRegistry dispatchRegistry, ActionRegistry actionRegistry)
        {
            Coordinates = coordinates;
            DispatchRegistry = dispatchRegistry;
            ActionRegistry = actionRegistry;

            UniqueId = DispatchRegistry.Register(this);

            RegisterActions();
        }

        public virtual void Dispatch(string parameters)
        {
            var parametersList = parameters.ToParameters().ToList();

            foreach (var parameter in parametersList)
            {
                var action = ActionRegistry.GetAction(Name, parameter.Name);
                action.Act(this, parameter.Value);
            }
        }

        protected internal virtual void RegisterActions() { }

        public Coordinate Coordinates { get; protected set; }
        public DispatchRegistry DispatchRegistry { get; }
        public ActionRegistry ActionRegistry { get; }

        public static readonly string DispatcheeName = typeof(T).Name;
        public string Name => DispatcheeName;
        public string UniqueId { get; protected internal set; }

        public virtual void UpdateState(T t, Parameters state)
        {
            var dispatchee = t as Dispatchee<T>;
            dispatchee.ThrowIfNull(nameof(t));

            if (state.HasValue(nameof(Coordinates))) dispatchee.Coordinates = state.ToValue<Coordinate>(nameof(Coordinates));
        }

        protected internal static string FormatState(Coordinate? coordinates = null, string uniqueId = null)
        {
            var state = string.Empty;

            if (coordinates.HasValue) state += coordinates.Value.FormatParameter();
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
