using System.Collections.Immutable;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Assets.StartingPlayerStatistics;

public class PlayerClasses
{
    private static readonly Lazy<ImmutableDictionary<string, PlayerClass>> Classes = new(() =>
        {
            var directory = FileAndDirectoryHelpers.GetLoadDirectory(FileAndDirectoryHelpers.LoadFolder);
            var path = Path.Combine(directory, nameof(PlayerClasses)).ChangeExtension("json");
            var json = File.ReadAllText(path);

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters =
                {
                    new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                }
            };

            var classes = JsonSerializer.Deserialize<Dictionary<string, PlayerClass>>(json, options);
            if (classes == null) return ImmutableDictionary<string, PlayerClass>.Empty;

            return classes.ToImmutableDictionary();
        }
    );

    public static ImmutableDictionary<string, PlayerClass> Get()
    {
        return Classes.Value;
    }

}