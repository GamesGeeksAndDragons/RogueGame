using System.Collections.Immutable;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Assets.StartingPlayerStatistics;

public class PlayerClassesLoader
{
    private static readonly Lazy<ImmutableDictionary<string, PlayerClass>> Classes = new(() =>
        {
            var directory = FileAndDirectoryHelpers.GetLoadDirectory(FileAndDirectoryHelpers.LoadFolder);
            var path = Path.Combine(directory, "PlayerClasses.json");
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
            
            return classes == null ? 
                ImmutableDictionary<string, PlayerClass>.Empty : 
                classes.ToImmutableDictionary();
        }
    );

    internal static ImmutableDictionary<string, PlayerClass> Load()
    {
        return Classes.Value;
    }

}