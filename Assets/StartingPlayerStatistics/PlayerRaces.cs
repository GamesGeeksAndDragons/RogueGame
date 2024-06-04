using System.Collections.Immutable;
using System.Text.Json;

namespace Assets.StartingPlayerStatistics;

public static class PlayerRaces
{
    private static readonly Lazy<ImmutableDictionary<string, PlayerRace>> Races = new(() =>
        {
            var directory = FileAndDirectoryHelpers.GetLoadDirectory(FileAndDirectoryHelpers.LoadFolder);
            var path = Path.Combine(directory, nameof(PlayerRaces)).ChangeExtension("json");
            var json = File.ReadAllText(path);

            var races = JsonSerializer.Deserialize<Dictionary<string, PlayerRace>>(json);
            if (races == null) return ImmutableDictionary<string, PlayerRace>.Empty;

            foreach (var (name, race) in races)
            {
                race.Name = name;
            }

            return races.ToImmutableDictionary();
        }
    );

    public static ImmutableDictionary<string, PlayerRace> Get()
    {
        return Races.Value;
    }
}