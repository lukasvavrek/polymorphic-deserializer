namespace PolymorphicDeserializer.Domain
{
    public class BookRegistryContent : RegistryContent
    {
        public override RegistryType Type => RegistryType.Book;
    
        public string Title { get; set; }
        public string Author { get; set; }
    }
}