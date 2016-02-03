using System;
using System.IO;
using RazorEngine.Templating;

namespace BasicTests.TemplateTests
{
    public class TestMessage
    {

        public String Title { get; set; }
        public String Message { get; set; }
        public bool ShowTitle { get; set; }

        public String TitleUpper { get { return Title.ToUpper(); } }


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
