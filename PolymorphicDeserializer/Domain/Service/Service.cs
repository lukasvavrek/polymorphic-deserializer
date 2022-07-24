using System.Collections.Generic;
using System.Text.Json;

namespace PolymorphicDeserializer.Domain.Service
{
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
}