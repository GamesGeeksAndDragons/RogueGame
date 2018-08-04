using System;
using System.Collections.Generic;
using System.Linq;
using Utils;
using Utils.Coordinates;

namespace Assets.Messaging
{
    public class DispatchRegistry
    {
        private readonly Dictionary<string, IDispatchee> _uniquelyNamedDispatchees = new Dictionary<string, IDispatchee>();
        private readonly Dictionary<string, uint> _dispatcheeCounts = new Dictionary<string, uint>();

        private string GenerateUniqueId(IDispatchee dispatchee)
        {
            uint count = 0;
            if (_dispatcheeCounts.ContainsKey(dispatchee.Name))
            {
                count = _dispatcheeCounts[dispatchee.Name];
            }

            _dispatcheeCounts[dispatchee.Name] = ++count;

            return dispatchee.Name + count;
        }

        internal string Register(IDispatchee dispatchee)
        {
            dispatchee.ThrowIfNull(nameof(dispatchee));

            var uniqueId = dispatchee.UniqueId.IsNullOrEmpty() ? GenerateUniqueId(dispatchee) : dispatchee.UniqueId;

            _uniquelyNamedDispatchees[uniqueId] = dispatchee;

            return uniqueId;
        }

        internal void Deregister(IDispatchee dispatchee)
        {
            dispatchee.ThrowIfNull(nameof(dispatchee));
            dispatchee.UniqueId.ThrowIfEmpty(nameof(dispatchee.UniqueId));

            _uniquelyNamedDispatchees.Remove(dispatchee.UniqueId);
        }

        public void Deregister(string uniqueId)
        {
            if (!_uniquelyNamedDispatchees.ContainsKey(uniqueId)) throw new ArgumentException($"Attempting to Deregister [{uniqueId}] which is not registered.");

            var dispatchee = GetDispatchee(uniqueId);

            Deregister(dispatchee);
        }

        public IDispatchee GetDispatchee(string id)
        {
            return _uniquelyNamedDispatchees[id];
        }

        public IReadOnlyList<IDispatchee> Dispatchees => _uniquelyNamedDispatchees.Values.ToList();
    }
}
