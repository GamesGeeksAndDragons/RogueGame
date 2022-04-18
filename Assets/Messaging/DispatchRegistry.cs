using System;
using System.Collections.Generic;
using System.Linq;
using Utils;
using Utils.Dispatching;

namespace Assets.Messaging
{
    internal class DispatchRegistry : IDispatchRegistry
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

        public string Register(IDispatchee dispatchee)
        {
            dispatchee.ThrowIfNull(nameof(dispatchee));

            var uniqueId = dispatchee.UniqueId.IsNullOrEmpty() ? GenerateUniqueId(dispatchee) : dispatchee.UniqueId;

            EnsureToUnregisterExistingDispatchee(dispatchee, uniqueId);

            _uniquelyNamedDispatchees[uniqueId] = dispatchee;

            return uniqueId;
        }

        private bool DispatcheeWithSameIdExists(string uniqueId)
        {
            return _uniquelyNamedDispatchees.ContainsKey(uniqueId);
        }

        private void EnsureToUnregisterExistingDispatchee(IDispatchee dispatchee, string uniqueId)
        {
            if (DispatcheeWithSameIdExists(uniqueId))
            {
                var existing = _uniquelyNamedDispatchees[uniqueId];
                if (!existing.IsSameInstance(dispatchee))
                {
                    Unregister(existing);
                }
            }
        }

        public void Unregister(IDispatchee dispatchee)
        {
            dispatchee.ThrowIfNull(nameof(dispatchee));
            dispatchee.UniqueId.ThrowIfEmpty(nameof(dispatchee.UniqueId));

            _uniquelyNamedDispatchees.Remove(dispatchee.UniqueId);
        }

        public void Unregister(params string[] uniqueIds)
        {
            uniqueIds.Length.ThrowIfBelow(0, nameof(uniqueIds));

            foreach (var uniqueId in uniqueIds)
            {
                if (!DispatcheeWithSameIdExists(uniqueId)) throw new ArgumentException($"Attempting to Unregister [{uniqueId}] which is not registered.");

                var dispatchee = GetDispatchee(uniqueId);

                Unregister(dispatchee);
            }
        }

        public IDispatchee GetDispatchee(string uniqueId)
        {
            uniqueId.ThrowIfEmpty(nameof(uniqueId));

            return _uniquelyNamedDispatchees[uniqueId];
        }

        public IReadOnlyList<IDispatchee> Dispatchees => _uniquelyNamedDispatchees.Values.ToList();

        public IDispatchee[][] Tiles { get; private set; }
    }
}
