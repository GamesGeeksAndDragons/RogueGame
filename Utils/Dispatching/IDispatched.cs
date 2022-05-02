#nullable enable
namespace Utils.Dispatching
{
    public interface IDispatched
    {
        string Name { get; }
        string UniqueId { get; }

        string Actor { get; }
    }
}