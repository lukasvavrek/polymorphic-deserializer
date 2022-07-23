using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PolymorphicDeserializer;

public enum RegistryType 
{
    Student,
    Book
}


[JsonConverter(typeof(RegistryJsonConverter))]
public abstract class RegistryContent
{
    public abstract RegistryType Type { get; }
}

public class StudentRegistryContent : RegistryContent
{
    public override RegistryType Type => RegistryType.Student;
    
    public string Name { get; set; }
    public float Correctness { get; set; }
}

public class BookRegistryContent : RegistryContent
{
    public override RegistryType Type => RegistryType.Book;
    
    public string Title { get; set; }
    public string Author { get; set; }
}

public class Registry<TContent>
    where TContent : RegistryContent
{
    public Registry(TContent content)
    {
        Content = content;
    }

    public RegistryType Type => Content.Type;
    public TContent Content { get; }
}

public class RegistryJsonConverter : JsonConverter<RegistryContent>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsAssignableFrom(typeof(RegistryContent));
    }

    public override RegistryContent Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (JsonDocument.TryParseValue(ref reader, out var doc))
        {
            if (doc.RootElement.TryGetProperty(nameof(RegistryContent.Type), out var type))
            {
                var typeValue = type.GetString();
                var rootElement = doc.RootElement.GetRawText();

                return typeValue switch
                {
                    nameof(RegistryType.Student) => JsonSerializer.Deserialize<StudentRegistryContent>(rootElement, options),
                    nameof(RegistryType.Book) => JsonSerializer.Deserialize<BookRegistryContent>(rootElement, options),
                    _ => throw new JsonException($"Unsupported type {typeValue}")
                };
            }
        }

        throw new JsonException("Unexpected error during deserialization.");
    }

    public override void Write(Utf8JsonWriter writer, RegistryContent value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}



public class Service
{
    public Registry<TContent> GetRegistryOfType<TContent>()
        where TContent : RegistryContent, new()
    {
        var db = new Dictionary<RegistryType, string>
        {
            {
                RegistryType.Book,
                @"{
  ""Type"" : ""Book"",
  ""Title"" : ""eragon"",
  ""Author"" : ""christopher paolini""
}"
            },
            {
                RegistryType.Student,
                @"{
  ""Type"" : ""Student"",
  ""Name"" : ""test name"",
  ""Correctness"" : 97.65
}"
            }
        };

        var json = db[new TContent().Type];

        var content = JsonSerializer.Deserialize<TContent>(json);

        return new Registry<TContent>(content);
    }
}