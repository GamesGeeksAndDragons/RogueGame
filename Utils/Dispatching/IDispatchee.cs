using Utils.Coordinates;

namespace Utils.Dispatching
{
    public interface IDispatchee
    {
        void Dispatch(string parameters);
        IDispatchee CloneDispatchee(string stateChange = null);

        Coordinate Coordinates { get; }

        string Name { get; }
        string UniqueId { get; }
    }
}