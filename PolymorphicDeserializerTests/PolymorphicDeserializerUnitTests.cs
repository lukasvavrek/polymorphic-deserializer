using System.Text.Json;
using Xunit;
using PolymorphicDeserializer.Domain;
using PolymorphicDeserializer.Domain.Service;

namespace PolymorphicDeserializerTests
{
    public class PolymorphicDeserializerUnitTests
    {
        [Fact]
        public void Deserialize_ShouldUseCorrectType_Student()
        {
            const string json = @"{
  ""Type"" : ""Student"",
  ""Name"" : ""test name"",
  ""Correctness"" : 97.65
}";

            var content = JsonSerializer.Deserialize<RegistryContent>(json);

            Assert.IsType<StudentRegistryContent>(content);
            Assert.Equal(RegistryType.Student, content.Type);

            Assert.Equal("test name", ((StudentRegistryContent) content).Name);
            Assert.Equal(97.65, ((StudentRegistryContent) content).Correctness, 2);
        }

        [Fact]
        public void Deserialize_ShouldUseCorrectType_Book()
        {
            const string json = @"{
  ""Type"" : ""Book"",
  ""Title"" : ""eragon"",
  ""Author"" : ""christopher paolini""
}";

            var content = JsonSerializer.Deserialize<RegistryContent>(json);

            Assert.IsType<BookRegistryContent>(content);
            Assert.Equal(RegistryType.Book, content.Type);

            Assert.Equal("eragon", ((BookRegistryContent) content).Title);
            Assert.Equal("christopher paolini", ((BookRegistryContent) content).Author);
        }

        [Fact]
        public void Service_GetRegistriesOfType_ShouldReturnCorrectType()
        {
            var registry = new Service()
                .GetRegistryOfType<StudentRegistryContent>();

            Assert.IsType<Registry<StudentRegistryContent>>(registry);
            Assert.Equal("test name", registry.Content.Name);
            Assert.Equal(97.65, registry.Content.Correctness, 2);
        }
    }
}