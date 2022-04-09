using Utils.Dispatching;
using Parameters = System.Collections.Generic.IReadOnlyList<(string Name, string Value)>;

namespace Assets.Deeds
{
    class MoveAction : IAction
    {
        public void Act(IDispatchee dispatchee, string actionValue)
        {
            throw new System.NotImplementedException();
        }
    }
}
