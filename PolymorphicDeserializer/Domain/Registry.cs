namespace PolymorphicDeserializer.Domain
{
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
}