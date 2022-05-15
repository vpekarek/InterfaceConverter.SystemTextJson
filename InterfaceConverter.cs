using System.Reflection;
using System.Text.Json.Serialization;

namespace System.Text.Json;
public class InterfaceConverter<T> : JsonConverter<T>
    where T : class
{
    private static Dictionary<string, Type> _sources = new Dictionary<string, Type>();

    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        Utf8JsonReader readerClone = reader;
        if (readerClone.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException();
        }

        readerClone.Read();
        if (readerClone.TokenType != JsonTokenType.PropertyName)
        {
            throw new JsonException();
        }

        string propertyName = readerClone.GetString() ?? string.Empty;
        if (propertyName != "$type")
        {
            throw new JsonException();
        }

        readerClone.Read();
        if (readerClone.TokenType != JsonTokenType.String)
        {
            throw new JsonException();
        }

        string typeValue = readerClone.GetString() ?? string.Empty;
        Type? entityType = GetCustomType(typeValue);

        var deserialized = JsonSerializer.Deserialize(ref reader, entityType!, options);
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
                    writer.WriteString("$type", type.FullName);

                    foreach (var element in jsonDocument.RootElement.EnumerateObject())
                    {
                        element.WriteTo(writer);
                    }

                    writer.WriteEndObject();
                    break;
                }
        }
    }

    private static Type GetCustomType(string typeName)
    {
        if (_sources.ContainsKey(typeName))
        {
            return _sources[typeName];
        }

        List<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();

        foreach (var assembly in assemblies)
        {
            Type t = assembly.GetType(typeName, false);
            if (t != null)
            {
                _sources.Add(typeName, t);
                return t;
            }
        }

        throw new ArgumentException("Type " + typeName + " doesn't exist in the current app domain");
    }
}
