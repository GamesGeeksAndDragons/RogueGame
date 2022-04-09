using ExtractedParameters = System.Collections.Generic.IReadOnlyList<(string Name, string Value)>;

// ReSharper disable VirtualMemberCallInConstructor

namespace Utils.Dispatching
{
    public interface ICloner<T>
    {
        T Clone(string stateChange = null);
        T Create();
        void UpdateState(T dispatchee, ExtractedParameters state);
    }
}
