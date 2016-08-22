using RazorEngine.Templating;
using System;
using System.IO;

namespace BasicTests.TemplateTests
{
    public class TestMessage
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public bool ShowTitle { get; set; }

        public string TitleUpper => Title.ToUpper();
    }

    public abstract class GenericTemplateBase<T> : TemplateBase<T>
    {
        public string GetHelloWorldText()
        {
            return "Saying Hello World from a method.";
        }

        public string GetSomeOutput(T value)
        {
            return value.ToString();
        }

        public string GetDataFromFile()
        {
            return File.ReadAllText(@"C:\junk\Test_DIDs.txt");
        }

        public string GetDataFromSpecificFile(string filePath)
        {
            return File.ReadAllText(filePath);
        }
    }

    public abstract class NonGenericTemplateBase : TemplateBase
    {
        public string GetHelloWorldText()
        {
            return "Saying Hello World from a method.";
        }
    }
}