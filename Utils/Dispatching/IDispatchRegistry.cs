#nullable enable
namespace Utils.Dispatching
{
    public interface IDispatchRegistry
    {
        string Register(IDispatched dispatched);
        void Unregister(IDispatched dispatched);
        void Unregister(params string[] uniqueIds);

        IDispatched GetDispatched(string uniqueId);
        IReadOnlyList<IDispatched> Dispatched { get; }
    }
}