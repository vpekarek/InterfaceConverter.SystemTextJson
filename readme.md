# Interface converter for System.Text.Json

## Description
Use this converter when you need to serialize and deserialize interface types, or list of object with single interface.

Deserialized entity has the original type as serialized.

## Usage

Specify `JsonInterfaceConverter` on any interface you want to serialize using this converter.

```C#
#:package InterfaceConverter.SystemTextJson@1.3.2

using System.Text.Json;

Console.WriteLine(JsonSerializer.Deserialize<Foo.IExample>("""
{
    "$type": "Foo.Example",
    "Id": 123,
    "Name": "Bar"
}
""")); // output: Example { Id = 123, Name = Bar }

namespace Foo
{
    [JsonInterfaceConverter(typeof(InterfaceConverter<IExample>))]
    interface IExample
    {
        int Id { get; }
        string Name { get; }
    }
    record Example(int Id, string Name) : IExample;
}
```

