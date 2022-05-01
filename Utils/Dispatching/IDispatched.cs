#nullable enable
namespace Utils.Dispatching
{
    public interface IDispatched
    {
        void Dispatch(string parameters);

        string Name { get; }
        string UniqueId { get; }

        string Actor { get; }
    }
}