using System.Collections.Immutable;
using System.Text.Json;
using System.Text.Json.Serialization;

using SpellType = System.Collections.Immutable.ImmutableDictionary<int, Assets.StartingPlayerStatistics.SpellNames>;
using SpellNamesType = System.Collections.Immutable.ImmutableDictionary<string, System.Collections.Immutable.ImmutableDictionary<int, Assets.StartingPlayerStatistics.SpellNames>>;

namespace Assets.StartingPlayerStatistics;

internal static class SpellNamesLoader
{
    private static readonly Lazy<SpellNamesType> SpellNames = new(() =>
    {
        var directory = FileAndDirectoryHelpers.GetLoadDirectory(FileAndDirectoryHelpers.LoadFolder);
        var path = Path.Combine(directory, nameof(StartingPlayerStatistics.SpellNames)).ChangeExtension("json");
        var json = File.ReadAllText(path);

        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters =
            {
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
            }
        };

        var spellNames = JsonSerializer.Deserialize<Dictionary<string, Dictionary<int, SpellNames>>>(json, options);

        if (spellNames == null) return SpellNamesType.Empty;

        return spellNames.Select(ToSpellNamesType).ToImmutableDictionary();

        KeyValuePair<string, SpellType> ToSpellNamesType(KeyValuePair<string, Dictionary<int, SpellNames>> loaded)
        {
            var immutableSpellNames = loaded.Value.ToImmutableDictionary();
            return new KeyValuePair<string, SpellType>(loaded.Key, immutableSpellNames);
        }
    });


    internal static SpellNamesType Load()
    {
        return SpellNames.Value;
    }
}