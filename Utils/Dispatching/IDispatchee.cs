namespace Utils.Dispatching
{
    public interface IDispatchee
    {
        void Dispatch(string parameters);

        string Name { get; }
        string UniqueId { get; }
    }
}