using System.Net.Http;
using System.Text;
using System.Text.Json;


public class JsonContent : StringContent
{
    public JsonContent(string content)
        : base(content, Encoding.UTF8, "application/json")
    {
    }

    public JsonContent(string content, Encoding encoding)
        : base(content, encoding)
    {
    }

    public JsonContent(string content, Encoding encoding, string mediaType)
        : base(content, encoding, mediaType)
    {
    }

    public JsonContent(object instance)
        : base(
            instance != null ? JsonSerializer.Serialize(instance, JsonConfiguration.Options) : string.Empty,
            Encoding.UTF8, "application/json")
    {
        var request = JsonSerializer.Serialize<object>(instance, JsonConfiguration.Options);
    }
}