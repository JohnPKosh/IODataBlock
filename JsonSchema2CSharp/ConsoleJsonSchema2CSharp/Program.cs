using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJsonSchema;
using NJsonSchema.CodeGeneration.CSharp;

namespace ConsoleJsonSchema2CSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            var settings = new CSharpGeneratorSettings();
            settings.ClassStyle = CSharpClassStyle.Poco;
            settings.RequiredPropertiesMustBeDefined = true;
            settings.Namespace = "Myspace";

            var schema = JsonSchema4.FromFile(@"schema2.json");
            var generator = new CSharpGenerator(schema, settings);
            var file = generator.GenerateFile();
            Console.WriteLine(file);

        }
    }
}
