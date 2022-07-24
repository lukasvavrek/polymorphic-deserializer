namespace PolymorphicDeserializer.Domain
{
    public class StudentRegistryContent : RegistryContent
    {
        public override RegistryType Type => RegistryType.Student;
    
        public string Name { get; set; }
        public float Correctness { get; set; }
    }
}