#nullable enable
using Utils;
using Utils.Dispatching;

namespace Assets.Messaging
{
    internal class DispatchRegistry : IDispatchRegistry
    {
        private readonly Dictionary<string, IDispatched> _uniquelyNamedDispatched = new Dictionary<string, IDispatched>();
        private readonly Dictionary<string, uint> _dispatchedCounts = new Dictionary<string, uint>();

        private string GenerateUniqueId(IDispatched dispatched)
        {
            uint count = 0;
            if (_dispatchedCounts.ContainsKey(dispatched.Name))
            {
                count = _dispatchedCounts[dispatched.Name];
            }

            _dispatchedCounts[dispatched.Name] = ++count;

            return dispatched.Name + count;
        }

        public string Register(IDispatched dispatched)
        {
            dispatched.ThrowIfNull(nameof(dispatched));

            var uniqueId = dispatched.UniqueId.IsNullOrEmpty() ? GenerateUniqueId(dispatched) : dispatched.UniqueId;

            EnsureToUnregisterExistingDispatched(dispatched, uniqueId);

            _uniquelyNamedDispatched[uniqueId] = dispatched;

            return uniqueId;
        }

        private bool DoesDispatchedWithSameIdExists(string uniqueId)
        {
            return _uniquelyNamedDispatched.ContainsKey(uniqueId);
        }

        private void EnsureToUnregisterExistingDispatched(IDispatched dispatched, string uniqueId)
        {
            if (DoesDispatchedWithSameIdExists(uniqueId))
            {
                var existing = _uniquelyNamedDispatched[uniqueId];
                if (!existing.IsSameInstance(dispatched))
                {
                    Unregister(existing);
                }
            }
        }

        public void Unregister(IDispatched dispatched)
        {
            dispatched.ThrowIfNull(nameof(dispatched));
            dispatched.UniqueId.ThrowIfEmpty(nameof(dispatched.UniqueId));

            _uniquelyNamedDispatched.Remove(dispatched.UniqueId);
        }

        public void Unregister(params string[] uniqueIds)
        {
            uniqueIds.Length.ThrowIfBelow(0, nameof(uniqueIds));

            foreach (var uniqueId in uniqueIds)
            {
                if (!DoesDispatchedWithSameIdExists(uniqueId)) throw new ArgumentException($"Attempting to Unregister [{uniqueId}] which is not registered.");

                var dispatched = GetDispatched(uniqueId);

                Unregister(dispatched);
            }
        }

        public IDispatched GetDispatched(string uniqueId)
        {
            uniqueId.ThrowIfEmpty(nameof(uniqueId));

            return _uniquelyNamedDispatched[uniqueId];
        }

        public IReadOnlyList<IDispatched> Dispatched => _uniquelyNamedDispatched.Values.ToList();
    }
}
