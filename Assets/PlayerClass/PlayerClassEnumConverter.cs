using System.Text.Json;
using System.Text.Json.Serialization;
using Utils;

namespace Assets.PlayerClass
{
    internal class PlayerClassEnumConverter : JsonConverter<PlayerClassEnum>
    {
        public override PlayerClassEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            PlayerClassEnum deserializedEnum = PlayerClassEnum.None;

            while (reader.Read() && reader.TokenType == JsonTokenType.String)
            {
                var playerClass = reader.GetString();
                if (playerClass.IsNullOrEmptyOrWhiteSpace()) continue;

                Enum.TryParse<PlayerClassEnum>(playerClass!, out var playerClassEnum);

                deserializedEnum |= playerClassEnum;
            }

            return deserializedEnum;
        }

        public override void Write(Utf8JsonWriter writer, PlayerClassEnum value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
