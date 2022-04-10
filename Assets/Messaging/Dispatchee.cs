using System;
using System.Linq;
using Assets.Actors;
using Assets.Deeds;
using Utils;
using Utils.Coordinates;
using Utils.Dispatching;
using Parameters = System.Collections.Generic.List<(string Name, string Value)>;

// ReSharper disable VirtualMemberCallInConstructor

namespace Assets.Messaging
{
    internal abstract class Dispatchee<T> : IDispatchee
        where T : class
    {
        protected Dispatchee(Coordinate coordinates, IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry)
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

        public override string ToString()
        {
            return this.ToDisplayChar();
        }

        protected internal virtual void RegisterActions() { }

        public Coordinate Coordinates { get; protected set; }
        public IDispatchRegistry DispatchRegistry { get; }
        public IActionRegistry ActionRegistry { get; }

        public static readonly string DispatcheeName = typeof(T).Name;
        public string Name => DispatcheeName;
        public string UniqueId { get; protected internal set; }

        public virtual void UpdateState(Parameters state)
        {
            if (state.HasValue(nameof(Coordinates)))
            {
                Coordinates = state.ToValue<Coordinate>(nameof(Coordinates));
            }
        }

        public virtual Parameters CurrentState()
        {
            return new Parameters
            {
                (Name: nameof(UniqueId), Value: UniqueId),
                (Name: nameof(Coordinates), Value: Coordinates.ToString()),
            };
        }

        protected bool IsZero(double num)
        {
            const double floatingTolerance = 0.00001;

            return Math.Abs(num) > floatingTolerance;
        }


        public abstract T Create();
    }
}
