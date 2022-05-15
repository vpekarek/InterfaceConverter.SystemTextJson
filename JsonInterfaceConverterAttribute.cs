using System.Text.Json.Serialization;

namespace System.Text.Json;

[AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
public class JsonInterfaceConverterAttribute : JsonConverterAttribute
{
    public JsonInterfaceConverterAttribute(Type converterType)
        : base(converterType)
    {
    }
}