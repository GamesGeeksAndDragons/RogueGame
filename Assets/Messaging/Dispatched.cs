#nullable enable
using Assets.Deeds;
using Utils;
using Utils.Dispatching;

// ReSharper disable VirtualMemberCallInConstructor

namespace Assets.Messaging;

internal abstract class Dispatched<T> : IDispatched
        where T : class
{
    protected Dispatched(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string actor, string uniqueId = "")
    {
        actor.Length.ThrowIfEqual(0, nameof(actor));

        DispatchRegistry = dispatchRegistry;
        ActionRegistry = actionRegistry;
        Actor = actor.Intern();

        UniqueId = DispatchRegistry.Register(this, uniqueId);

        RegisterActions();
    }

    public override string ToString()
    {
        return Actor;
    }

    protected internal virtual void RegisterActions() { }

    public IDispatchRegistry DispatchRegistry { get; protected set; }
    public IActionRegistry ActionRegistry { get; protected set; }
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
}
