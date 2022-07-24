using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using PolymorphicDeserializer.Domain;

namespace PolymorphicDeserializer
{
    public class PolymorphicJsonConverter<TBaseType, TPolymorphicMap> : JsonConverter<TBaseType>
        where TBaseType : IHasType
        where TPolymorphicMap : IPolymorphicMap, new()
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsAssignableFrom(typeof(RegistryContent));
        }

        public override TBaseType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (!JsonDocument.TryParseValue(ref reader, out var doc))
                throw new JsonException("Failed to parse JSON.");

            var typePropertyName = DetermineTypePropertyName();

            if (!doc.RootElement.TryGetProperty(typePropertyName, out var type))
                throw new InvalidOperationException("Couldn't determine the type property.");
            
            var typeValue = type.GetString();
            var rootElement = doc.RootElement.GetRawText();

            var polymorphicType = new TPolymorphicMap().GetPolymorphicType(typeValue);

            return (TBaseType) JsonSerializer.Deserialize(rootElement, polymorphicType, options);
        }

        private string DetermineTypePropertyName()
        {
            const string defaultPropertyName = "Type";

            var typeAttribute = (PolymorphicTypeAttribute?)Attribute.GetCustomAttribute(typeof(TBaseType), typeof(PolymorphicTypeAttribute));
            
            if (typeAttribute != null)
            {
                return typeAttribute.TypePropertyName;
            }

            return defaultPropertyName;
        }

        public override void Write(Utf8JsonWriter writer, TBaseType value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}