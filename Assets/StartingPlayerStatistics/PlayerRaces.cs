using System.Collections.Immutable;
using System.Text.Json;

namespace Assets.StartingPlayerStatistics;

public static class PlayerRaces
{
    private static readonly Lazy<ImmutableDictionary<string, IPlayerRace>> Races = new(() =>
        {
            var directory = FileAndDirectoryHelpers.GetLoadDirectory(FileAndDirectoryHelpers.LoadFolder);
            var path = Path.Combine(directory, nameof(PlayerRaces)).ChangeExtension("json");
            var json = File.ReadAllText(path);

            var races = JsonSerializer.Deserialize<Dictionary<string, PlayerRace>>(json);
            if (races == null) return ImmutableDictionary<string, IPlayerRace>.Empty;

            return races.Select(race => new KeyValuePair<string, IPlayerRace>(race.Key, race.Value))
                .ToImmutableDictionary();
        }
    );

    public static ImmutableDictionary<string, IPlayerRace> Get()
    {
        return Races.Value;
    }
}