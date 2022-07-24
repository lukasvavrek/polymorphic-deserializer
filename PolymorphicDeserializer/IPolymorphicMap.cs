using System;

namespace PolymorphicDeserializer
{
    public interface IPolymorphicMap
    {
        Type GetPolymorphicType(string type);
    }
}