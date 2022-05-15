using System.Text.Json.Serialization;

namespace System.Text.Json;

public class InterfaceWriteConverter<T> : JsonConverter<T>
    where T : class
{

    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var deserialized = JsonSerializer.Deserialize(ref reader, typeof(T), options);
        return (T)deserialized;
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case null:
                JsonSerializer.Serialize(writer, (T)null, options);
                break;
            default:
                {
                    var type = value.GetType();
                    using var jsonDocument = JsonDocument.Parse(JsonSerializer.Serialize(value, type, options));
                    writer.WriteStartObject();

                    foreach (var element in jsonDocument.RootElement.EnumerateObject())
                    {
                        element.WriteTo(writer);
                    }

                    writer.WriteEndObject();
                    break;
                }
        }
    }
}
