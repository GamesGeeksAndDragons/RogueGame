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

        public void Unregister(string uniqueId)
        {
            uniqueId.ThrowIfEmpty(nameof(uniqueId));

            if (!DispatcheeWithSameIdExists(uniqueId)) throw new ArgumentException($"Attempting to Unregister [{uniqueId}] which is not registered.");

            var dispatchee = GetDispatchee(uniqueId);

            Unregister(dispatchee);
        }

        public IDispatchee GetDispatchee(string uniqueId)
        {
            uniqueId.ThrowIfEmpty(nameof(uniqueId));

            return _uniquelyNamedDispatchees[uniqueId];
        }

        public IReadOnlyList<IDispatchee> Dispatchees => _uniquelyNamedDispatchees.Values.ToList();

        public IDispatchee[][] Tiles { get; private set; }

        internal void NewLevel()
        {
            var tiles = Dispatchees.Where(dispatchee => dispatchee.IsTile());

            var tilesRegistry = new List<List<IDispatchee>>();

            foreach (var tile in tiles)
            {
                if (!tile.IsTile()) continue;

                var row = GetRow(tile);
                AddTile(tile, row);
            }

            Tiles = tilesRegistry.Select(list => list.ToArray()).ToArray();

            void Grow<T>(List<T> list, Func<T> create, int newSize)
            {
                var growBy = newSize - list.Count + 1;

                for (int i = 0; i < growBy; i++)
                {
                    list.Add(create());
                }
            }

            List<IDispatchee> GetRow(IDispatchee tile)
            {
                var row = tile.Coordinates.Row;

                if (tilesRegistry.Count < row - 1)
                {
                    Grow(tilesRegistry, () => new List<IDispatchee>(), row);
                    var growBy = row - tilesRegistry.Count + 1;

                    for (int i = 0; i < growBy; i++)
                    {
                        tilesRegistry.Add(new List<IDispatchee>());
                    }
                }

                return tilesRegistry[row];
            }

            void AddTile(IDispatchee tile, List<IDispatchee> row)
            {
                var column = tile.Coordinates.Column;

                if (row.Count < column - 1)
                {
                    Grow(row, () => null, column);
                }

                row[column] = tile;
            }
        }
    }
}
