using System;

namespace PolymorphicDeserializer.Domain
{
    public class RegistryContentPolymorphicMap : IPolymorphicMap
    {
        public Type GetPolymorphicType(string type)
        {
            return type switch
            {
                nameof(RegistryType.Student) => typeof(StudentRegistryContent),
                nameof(RegistryType.Book) => typeof(BookRegistryContent),
                _ => throw new ArgumentException($"Unsupported type {type}")
            };
        }
    }
}