#nullable enable
using Assets.Deeds;
using Utils;
using Utils.Dispatching;

// ReSharper disable VirtualMemberCallInConstructor

namespace Assets.Messaging;

internal abstract class Dispatched<T> : IDispatched
        where T : class
{
    protected Dispatched(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string actor, string state)
    {
        actor.Length.ThrowIfEqual(0, nameof(actor));

        DispatchRegistry = dispatchRegistry;
        ActionRegistry = actionRegistry;
        Actor = actor.Intern();

        RegisterActions();

        UpdateState(state.ToParameters());
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
    public string UniqueId { get; protected internal set; } = null!;

    public virtual void UpdateState(Parameters state)
    {
        var uniqueId = state.GetUniqueId() ?? "";
        UniqueId = DispatchRegistry.Register(this, uniqueId);
    }

    public virtual Parameters CurrentState => new Parameters().AddUniqueId(UniqueId);
}
