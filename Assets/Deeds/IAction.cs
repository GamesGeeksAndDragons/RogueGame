#nullable enable
using Utils.Dispatching;
using Parameters = System.Collections.Generic.IReadOnlyList<(string Name, string Value)>;

namespace Assets.Deeds
{
    public interface IAction
    {
        void Act(IDispatchRegistry dispatchRegistry, IDispatched dispatched, string actionValue);
    }
}