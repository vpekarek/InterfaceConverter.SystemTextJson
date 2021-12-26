# Interface converter for System.Text.Json

## Description
Use this converter when you need to serialize and deserialize interface types, or list of object with single interface.

Deserialized entity has the original type as serialized.

## Usage

Specify `JsonInterfaceConverter` on any interface you want to serialize using this converter.

```C#
[JsonInterfaceConverter(typeof(InterfaceConverter<IExample>))]
public interface IExample
{
    int Id { get; set; }
    string Name { get; set; }
}

```

