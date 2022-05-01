#nullable enable
using Assets.Actors;
using Assets.Deeds;
using Utils;
using Utils.Dispatching;
using Parameters = System.Collections.Generic.List<(string Name, string Value)>;

// ReSharper disable VirtualMemberCallInConstructor

namespace Assets.Messaging
{
    internal abstract class Dispatched<T> : IDispatched
        where T : class
    {
        protected Dispatched(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string actor)
        {
            dispatchRegistry.ThrowIfNull(nameof(dispatchRegistry));
            actionRegistry.ThrowIfNull(nameof(actionRegistry));
            actor.Length.ThrowIfEqual(0, nameof(actor));

            DispatchRegistry = dispatchRegistry;
            ActionRegistry = actionRegistry;
            Actor = actor;

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
            return Actor;
        }

        protected internal virtual void RegisterActions() { }

        public IDispatchRegistry DispatchRegistry { get; }
        public IActionRegistry ActionRegistry { get; }
        public string Actor { get; }

        public static readonly string DispatchedName = typeof(T).Name;
        public string Name => DispatchedName;
        public string UniqueId { get; protected internal set; }

        public virtual void UpdateState(Parameters state)
        {
        }

        public virtual Parameters CurrentState()
        {
            return new Parameters
            {
                (Name: nameof(UniqueId), Value: UniqueId),
            };
        }

        protected bool IsZero(double num)
        {
            const double floatingTolerance = 0.00001;

            return Math.Abs(num) > floatingTolerance;
        }
    }
}
