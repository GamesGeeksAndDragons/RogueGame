using System.Collections.Generic;

namespace Utils.Dispatching
{
    public interface IDispatchRegistry
    {
        IDispatchee GetDispatchee(string uniqueId);
        IReadOnlyList<IDispatchee> Dispatchees { get; }
    }
}