namespace Assets.PlayerRace;

public record StartingStatistic(int Min, string Die);

//public class StartingStatistic
//{
//    public int Min { get; set; }
//    public string Die { get; set; } = null!;
//}

//public class StartingStatisticConverter : JsonConverter<StartingStatistic>
//{
//    public override StartingStatistic Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
//    {
//        reader.Read();
//        var min = reader.GetInt32();

//        reader.Read();
//        var die = reader.GetString();

//        while (reader.Read())
//        {
//        }

//        return new StartingStatistic(min, die!);
//    }

//    public override void Write(Utf8JsonWriter writer, StartingStatistic value, JsonSerializerOptions options)
//    {
//        throw new NotImplementedException();
//    }
//}
