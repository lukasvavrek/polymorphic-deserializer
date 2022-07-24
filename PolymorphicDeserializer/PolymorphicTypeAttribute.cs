using System;

namespace PolymorphicDeserializer
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PolymorphicTypeAttribute : Attribute
    {
        public PolymorphicTypeAttribute(string typePropertyName)
        {
            TypePropertyName = typePropertyName;
        }
        
        public string TypePropertyName { get; }
    }
}