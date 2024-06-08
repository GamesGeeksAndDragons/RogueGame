using System.Collections.Immutable;
using System.Text.Json;

namespace Assets.StartingPlayerStatistics;

internal static class PlayerRacesLoader
{
    private static readonly Lazy<ImmutableDictionary<string, PlayerRace>> Races = new(() =>
        {
            var directory = FileAndDirectoryHelpers.GetLoadDirectory(FileAndDirectoryHelpers.LoadFolder);
            var path = Path.Combine(directory, "PlayerRaces.json");
            var json = File.ReadAllText(path);

            var races = JsonSerializer.Deserialize<Dictionary<string, PlayerRace>>(json);
            
            return races == null ? 
                ImmutableDictionary<string, PlayerRace>.Empty : 
                races.ToImmutableDictionary();
        }
    );

    internal static ImmutableDictionary<string, PlayerRace> Load()
    {
        return Races.Value;
    }
}