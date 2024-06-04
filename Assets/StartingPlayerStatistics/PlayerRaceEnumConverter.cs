using System.Text.Json;
using System.Text.Json.Serialization;

namespace Assets.StartingPlayerStatistics;

internal class PlayerRaceEnumConverter : JsonConverter<PlayerRaceEnum>
{
    public override PlayerRaceEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, PlayerRaceEnum value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}