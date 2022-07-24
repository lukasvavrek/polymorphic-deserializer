using System.Text.Json.Serialization;

namespace PolymorphicDeserializer.Domain
{
    [PolymorphicType(nameof(Type))]
    [JsonConverter(typeof(PolymorphicJsonConverter<RegistryContent, RegistryContentPolymorphicMap>))]
    public abstract class RegistryContent : IHasType
    {
        public abstract RegistryType Type { get; }


        public string TypePropertyName => nameof(Type);
    }
}
