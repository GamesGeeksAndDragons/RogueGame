using Assets.PlayerClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Assets.PlayerRace
{
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
}
