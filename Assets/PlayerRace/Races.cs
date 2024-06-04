using System.Collections.Immutable;
using System.Text.Json;

namespace Assets.PlayerRace;

public static class Races
{
    private static Lazy<ImmutableDictionary<string, Race>> _races = new Lazy<ImmutableDictionary<string, Race>>(() =>
        {
            var directory = FileAndDirectoryHelpers.GetLoadDirectory(FileAndDirectoryHelpers.LoadFolder);
            var path = Path.Combine(directory, "Races.json");
            var json = File.ReadAllText(path);

            var races = JsonSerializer.Deserialize<Dictionary<string, Race>>(json);
            if (races == null) return ImmutableDictionary<string, Race>.Empty;

            foreach (var (name, race) in races)
            {
                race.Name = name;
            }

            return races.ToImmutableDictionary();
        }
    );

    public static ImmutableDictionary<string, Race> Get()
    {
        return _races.Value;
    }
}