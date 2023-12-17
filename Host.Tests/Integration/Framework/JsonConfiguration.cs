using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

public static class JsonConfiguration
{
    static JsonConfiguration()
    {
        Options.Converters.Add(new JsonStringEnumConverter(new GraphQlEnumNamingPolicy()));
        Options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        Options.Converters.Add(new DictionaryToKeyValuePairOfStringConverter());
    }

    public static JsonSerializerOptions Options { get; } = new();
}

public class DictionaryToKeyValuePairOfStringConverter : JsonConverter<Dictionary<string, string>>
{
    public override Dictionary<string, string>? Read(ref Utf8JsonReader reader, Type typeToConvert,
        JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, Dictionary<string, string> value,
        JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        foreach (var dictItem in value)
        {
            writer.WriteStartObject();
            writer.WriteString("key", dictItem.Key);
            writer.WriteString("value", dictItem.Value);
            writer.WriteEndObject();
        }

        writer.WriteEndArray();
    }
}

public class GraphQlEnumNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        var sb = new StringBuilder();
        var gapBetweenBigLetters = 0;
        for (var itemIndex = 0; itemIndex < name.Length; itemIndex++)
        {
            var currentItem = name[itemIndex];
            if (char.IsUpper(currentItem) && itemIndex != 0)
            {
                if (gapBetweenBigLetters > 1) sb.Append("_");
                sb.Append(char.ToUpper(currentItem));

                gapBetweenBigLetters = 0;
            }
            else
            {
                gapBetweenBigLetters++;
                sb.Append(char.ToUpper(currentItem));
            }
        }

        return sb.ToString();
    }
}