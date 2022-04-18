using System.Collections.Generic;

namespace Utils.Dispatching
{
    public interface IDispatchRegistry
    {
        string Register(IDispatchee dispatchee);
        void Unregister(IDispatchee dispatchee);
        void Unregister(params string[] uniqueIds);

        IDispatchee GetDispatchee(string uniqueId);
        IReadOnlyList<IDispatchee> Dispatchees { get; }
    }
}