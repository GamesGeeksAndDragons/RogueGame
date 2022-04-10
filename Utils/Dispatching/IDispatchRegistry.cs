using System.Collections.Generic;

namespace Utils.Dispatching
{
    public interface IDispatchRegistry
    {
        string Register(IDispatchee dispatchee);
        void Unregister(IDispatchee dispatchee);
        void Unregister(string uniqueId);

        IDispatchee GetDispatchee(string uniqueId);
        IReadOnlyList<IDispatchee> Dispatchees { get; }
    }
}