using System.Dynamic;
using System.Collections.Generic;
using Business.Templates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RazorEngine.Templating;
using UnitTests_01;


namespace BasicTests.TemplateTests
{
    /// <summary>
    /// Summary description for NewTemplateTests
    /// </summary>
    [TestClass]
    public class NewTemplateTests
    {
        //public NewTemplateTests()
        //{
        //    //
        //    // TODO: Add constructor logic here
        //    //
        //}

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion



        #region Migrated Tests

        [TestMethod]
        public void ParseWithRazorRaw_ParseModel()
        {
            const string templateString = @"
@Model.Title
#############
@Model.Message
#############
";
            var data = new { Message = "Hello World!", Title = "Test Template Transform." };
            var output = data.ParseWithRazorRaw(templateString);

            StringAssert.Contains(output, "Hello World");
        }

        [TestMethod]
        public void ParseWithRazorRaw_ParseModelWithViewBag()
        {
            const string templateString = @"
@Model.Title
#############
@Model.Message
#############
@ViewBag.Comment
";

            var viewBag = new DynamicViewBag();
            viewBag.AddValue("Comment", "I have a comment");
            var data = new { Message = "Hello World!", Title = "Test Template Transform." };
            var output = data.ParseWithRazorRaw(templateString, viewBag: viewBag);

            StringAssert.Contains(output, "Hello World");
        }

        [TestMethod]
        public void ParseWithRazorRaw_BracketText()
        {
            const string templateString = @"
@Model.Title
#############
<text>{ hello } </text>
@Model.Message
#############
";
            var data = new { Message = "Hello World!", Title = "Test Template Transform." };
            var output = data.ParseWithRazorRaw(templateString);

            StringAssert.Contains(output, "Test Template Transform");
        }

        [TestMethod]
        public void ParseWithRazorRaw_IfTest()
        {
            const string templateString = @"
@if(Model.ShowTitle){
@Model.Title
}
#############
<text>{ hello } </text>
@Model.Message
#############
";
            var data = new { Message = "Hello World!", Title = "Test Template Transform.", ShowTitle = false };
            var output = data.ParseWithRazorRaw(templateString);

            Assert.IsTrue(!output.Contains("Test Template Transform"));
        }

        [TestMethod]
        public void ParseWithRazorRaw_UsingTest()
        {
            const string templateString = @"
@using System.IO;
@{
var filetext = File.ReadAllText(@""C:\junk\Test_DIDs.txt"");
}
@if(Model.ShowTitle){
@Model.Title
}
#############
<text>{ hello } </text>
@Model.Message
#############
@filetext
";
            var data = new { Message = "Hello World!", Title = "Test Template Transform.", ShowTitle = false };
            var output = data.ParseWithRazorRaw(templateString);

            Assert.IsTrue(!output.Contains("Test Template Transform"));
        }

        [TestMethod]
        public void ParseWithRazorRaw_TemplateBase()
        {
            const string templateString = @"@model UnitTests_01.TestMessage
@using System.IO;
@{
var filetext = File.ReadAllText(@""C:\junk\Test_DIDs.txt"");
}
@if(Model.ShowTitle){<text>@Model.Title</text>
<text>@Model.TitleUpper</text>
}
@GetHelloWorldText()
@GetSomeOutput(@Model)
#############
{ hello }
@Model.Message
#############
@filetext
#############
@GetDataFromFile()
";
            //var data = new { Message = "Hello World!", Title = "Test Template Transform.", ShowTitle = false };
            var data = new TestMessage();
            data.Message = "Hello Expando";
            data.Title = "Test Template Transform.";
            data.ShowTitle = true;

            var output = data.ParseWithRazorRaw(templateString, templateBase: typeof(GenericTemplateBase<>));

            Assert.IsTrue(output.Contains("Saying Hello World from a method."));
        }

        [TestMethod]
        public void ParseWithRazorRaw_TemplateBaseWithViewBag()
        {
            var TemplateString = @"@model UnitTests_01.TestMessage
@using System.IO;
@{
var filetext = File.ReadAllText(@""C:\junk\Test_DIDs.txt"");
}
@if(Model.ShowTitle){<text>@Model.Title</text>
<text>@Model.TitleUpper</text>
}
@GetHelloWorldText()
@GetSomeOutput(@Model)
#############
{ hello }
@Model.Message
#############
@filetext
#############
@GetDataFromFile()
#############
#############
@GetDataFromSpecificFile(@ViewBag.FilePath)

";
            var viewBag = new DynamicViewBag();
            viewBag.AddValue("FilePath", @"C:\junk\Test_DIDs.txt");

            var data = new TestMessage();
            data.Message = "Hello Expando";
            data.Title = "Test Template Transform.";
            data.ShowTitle = true;

            var output = data.ParseWithRazorRaw(TemplateString, templateBase: typeof(GenericTemplateBase<>), viewBag: viewBag);

            Assert.IsTrue(output.Contains("Saying Hello World from a method."));
        }

        [TestMethod]
        public void ParseWithRazorRaw_AnonymousModel_TemplateBase()
        {
            var TemplateString = @"
@using System.IO;
@{
var filetext = File.ReadAllText(@""C:\junk\Test_DIDs.txt"");
}
@if(Model.ShowTitle){<text>@Model.Title</text>
}
@GetHelloWorldText()
@GetSomeOutput(@Model)
#############
{ hello }
@Model.Message
#############
@filetext
#############
@GetDataFromFile()
";
            var data = new { Message = "Hello World!", Title = "Test Template Transform.", ShowTitle = true };
            //TestMessage data = new TestMessage();
            //data.Message = "Hello Expando";
            //data.Title = "Test Template Transform.";
            //data.ShowTitle = true;

            var output = data.ParseWithRazorRaw(TemplateString, templateBase: typeof(GenericTemplateBase<>));

            Assert.IsTrue(output.Contains("Saying Hello World from a method."));
        }

        [TestMethod]
        public void ParseWithRazorRaw_DynamicModel_TemplateBase()
        {
            var TemplateString = @"
@using System.IO;
@{
var filetext = File.ReadAllText(@""C:\junk\Test_DIDs.txt"");
}
@if(Model.ShowTitle){<text>@Model.Title</text>
}
@GetHelloWorldText()
@GetSomeOutput(@Model)
#############
{ hello }
@Model.Message
#############
@filetext
#############
@GetDataFromFile()
";
            //var data = new { Message = "Hello World!", Title = "Test Template Transform.", ShowTitle = true };
            dynamic data = new ExpandoObject();
            data.Message = "Hello Expando";
            data.Title = "Test Template Transform.";
            data.ShowTitle = true;

            //var output = Business.Templates.deprecated.RazorTemplateExtensions.ParseWithRazorRaw(data, TemplateString, templateType: typeof(GenericTemplateBase<>));
            var output = RazorTemplateExtensions.ParseWithRazorRaw(data, TemplateString, templateBase: typeof(GenericTemplateBase<>));

            Assert.IsTrue(output.Contains("Saying Hello World from a method."));
        }

        [TestMethod]
        public void ParseWithRazorRaw_SimpleRenderBody()
        {
            var TemplateString = @"@{Layout = ""Parent"";}
@if(Model.ShowTitle){
@Model.Title
}
#############
<text>{ hello } </text>
@Model.Message
#############
";

            var parenttemplate = @"PARENT
@RenderBody()
PARENT";

            var data = new { Message = "Hello World!", Title = "Test Template Transform.", ShowTitle = false };

            var otherTemplates = new RazorTemplateSections();
            var parent = new RazorTemplateSection(parenttemplate, "Parent");
            otherTemplates.Add(parent);

            var output = data.ParseWithRazorRaw(TemplateString, sectionTemplates: otherTemplates);

            Assert.IsTrue(!output.Contains("Test Template Transform"));
        }

        [TestMethod]
        public void ParseWithRazorRaw_RenderBodyWithModel()
        {
            var TemplateString = @"@{Layout = ""Parent"";}
@if(Model.ShowTitle){
@Model.Title
}
#############
<text>{ hello } </text>
@Model.Message
#############
";

            var parenttemplate = @"@Model.Title PARENT
@RenderBody()
PARENT";

            //var data = new { Message = "Hello World!", Title = "Test Template Transform.", ShowTitle = false };
            var data = new TestMessage();
            data.Message = "Hello Expando";
            data.Title = "Test Template Transform.";
            data.ShowTitle = true;

            var otherTemplates = new RazorTemplateSections();
            otherTemplates.Add(parenttemplate, "Parent");

            var output = data.ParseWithRazorRaw(TemplateString, sectionTemplates: otherTemplates);

            Assert.IsTrue(output.Contains("Test Template Transform"));
        }

        [TestMethod]
        public void ParseWithRazorRaw_MultipleRenderBodyWithModel()
        {
            var TemplateString = @"@{Layout = ""Parent"";}
@if(Model.ShowTitle){
@Model.Title
}
#############
<text>{ hello } </text>
@Model.Message
#############
";

            var parenttemplate = @"@{Layout = ""GrandParent"";}@Model.Title PARENT
@RenderBody()
PARENT";

            var grandparenttemplate = @"@Model.Title GRANDPARENT
@RenderBody()
GRANDPARENT";

            //var data = new { Message = "Hello World!", Title = "Test Template Transform.", ShowTitle = false };
            var data = new TestMessage();
            data.Message = "Hello Expando";
            data.Title = "Test Template Transform.";
            data.ShowTitle = false;

            var otherTemplates = new RazorTemplateSections();
            otherTemplates.Add(grandparenttemplate, "GrandParent");
            otherTemplates.Add(parenttemplate, "Parent");

            var output = data.ParseWithRazorRaw(TemplateString, sectionTemplates: otherTemplates);

            Assert.IsTrue(output.Contains("Test Template Transform"));
        }

        [TestMethod]
        public void ParseWithRazorRaw_MultipleSectionsWithModel()
        {
            var TemplateString = @"@{Layout = ""Parent"";}
@if(Model.ShowTitle){
@Model.Title
}
#############
<text>{ hello } </text>
@Model.Message
#############
@section TestSection {TestSection:-)}
";

            var parenttemplate = @"@{Layout = ""GrandParent"";}@Model.Title PARENT
@RenderBody()
@section ParentSection {ParentSection:-)}
PARENT";

            var grandparenttemplate = @"@Model.Title / @if(Model.ShowTitle){@RenderSection(""TestSection"") <text>/</text> }@RenderSection(""ParentSection"") GRANDPARENT
@RenderBody()
GRANDPARENT";

            //var TestSection = @"@section TestSection {<span>Hello</span>}";

            //var data = new { Message = "Hello World!", Title = "Test Template Transform.", ShowTitle = false };
            var data = new TestMessage();
            data.Message = "Hello Expando";
            data.Title = "Test Template Transform.";
            data.ShowTitle = true;

            var otherTemplates = new RazorTemplateSections();
            otherTemplates.Add(grandparenttemplate, "GrandParent");
            otherTemplates.Add(parenttemplate, "Parent");
            //otherTemplates.Add(TestSection, data, "TestSection");

            var output = data.ParseWithRazorRaw(TemplateString, sectionTemplates: otherTemplates);

            Assert.IsTrue(output.Contains("Test Template Transform"));
        }

        [TestMethod]
        public void ParseWithRazorRaw_Include()
        {
            var TemplateString = @"@{Layout = ""Parent"";}
@if(Model.ShowTitle){
@Model.Title
}
#############
<text>{ hello } </text>
@Model.Message
#############
@section TestSection {TestSection:-)}

@Include(""TestInclude"")

";

            var parenttemplate = @"@{Layout = ""GrandParent"";}@Model.Title PARENT
@RenderBody()
@section ParentSection {ParentSection:-)}
PARENT";
            var grandparenttemplate = @"@Model.Title / @if(Model.ShowTitle){@RenderSection(""TestSection"") <text>/</text> @Include(""TestInclude"") }@RenderSection(""ParentSection"") GRANDPARENT
@RenderBody()
GRANDPARENT";

            var TestInclude = @" :-) TestInclude :-) ";

            //var data = new { Message = "Hello World!", Title = "Test Template Transform.", ShowTitle = false };
            var data = new TestMessage();
            data.Message = "Hello Expando";
            data.Title = "Test Template Transform.";
            data.ShowTitle = true;

            var otherTemplates = new RazorTemplateSections();
            otherTemplates.Add(grandparenttemplate, "GrandParent");
            otherTemplates.Add(parenttemplate, "Parent");
            otherTemplates.Add(TestInclude, "TestInclude");

            var output = data.ParseWithRazorRaw(TemplateString, sectionTemplates: otherTemplates);

            Assert.IsTrue(output.Contains("Test Template Transform"));
        }

        [TestMethod]
        public void ParseWithRazorRaw_Include_WithCustomModel()
        {
            var TemplateString = @"@{Layout = ""Parent"";}
@if(Model.ShowTitle){
@Model.Title
}
#############
<text>{ hello } </text>
@Model.Message
#############
@section TestSection {TestSection:-)}

body:
@Include(""TestInclude"")
@Include(""TestInclude"", new UnitTests_01.TestMessage{ Title = ""Test"" })
body:
";

            var grandparenttemplate = @"@model UnitTests_01.TestMessage
@Model.Title /
@if(Model.ShowTitle){
@RenderSection(""TestSection"") <text>/</text>
@Include(""TestInclude"", new UnitTests_01.TestMessage{ Title = ""Test"" })
}
@RenderSection(""ParentSection"") GRANDPARENT
@RenderBody()
GRANDPARENT";

            var parenttemplate = @"@{Layout = ""GrandParent"";}@Model.Title PARENT
@RenderBody()
@section ParentSection {ParentSection:-)}
PARENT";

            var TestInclude = @" :-) TestInclude :-)###### @Model.Title ########";

            //var data = new { Message = "Hello World!", Title = "Test Template Transform.", ShowTitle = false };
            var data = new TestMessage();
            data.Message = "Hello Expando";
            data.Title = "Test Template Transform.";
            data.ShowTitle = true;

            var otherTemplates = new RazorTemplateSections();
            otherTemplates.Add(grandparenttemplate, "GrandParent");
            otherTemplates.Add(parenttemplate, "Parent");
            otherTemplates.Add(TestInclude, "TestInclude");

            var output = data.ParseWithRazorRaw(TemplateString, sectionTemplates: otherTemplates);

            Assert.IsTrue(output.Contains("Test Template Transform"));
        }

        [TestMethod]
        public void ParseWithRazorRaw_Include_WithCustomAnonymousModel()
        {
            var TemplateString = @"@{Layout = ""Parent"";}
@if(Model.ShowTitle){
@Model.Title
}
#############
<text>{ hello } </text>
@Model.Message
#############
@section TestSection {TestSection:-)}

@Include(""TestInclude"")

";

            var grandparenttemplate = @"
@Model.Title /
@if(Model.ShowTitle){
@RenderSection(""TestSection"") <text>/</text>
@Include(""TestInclude"", new { Title = @Model.Message })
}
@RenderSection(""ParentSection"") GRANDPARENT
@RenderBody()
GRANDPARENT";

            var parenttemplate = @"@{Layout = ""GrandParent"";}@Model.Title PARENT
@RenderBody()
@section ParentSection {ParentSection:-)}
PARENT";

            var TestInclude = @" :-) TestInclude :-)###### @Model.Title ########";

            var data = new { Message = "Hello World!", Title = "Test Template Transform.", ShowTitle = true };
            //TestMessage data = new TestMessage();
            //data.Message = "Hello Expando";
            //data.Title = "Test Template Transform.";
            //data.ShowTitle = true;

            var otherTemplates = new RazorTemplateSections();
            otherTemplates.Add(grandparenttemplate, "GrandParent");
            otherTemplates.Add(parenttemplate, "Parent");
            otherTemplates.Add(TestInclude, "TestInclude");

            var output = data.ParseWithRazorRaw(TemplateString, sectionTemplates: otherTemplates);

            Assert.IsTrue(output.Contains("Test Template Transform"));
        }

        [TestMethod]
        public void ParseWithRazorRaw_Include_WithCustomDynamicModel()
        {
            var TemplateString = @"@{Layout = ""Parent"";}
@if(Model.ShowTitle){
@Model.Title
}
#############
<text>{ hello } </text>
@Model.Message
#############
@section TestSection {TestSection:-)}

@Include(""TestInclude"")

";

            var grandparenttemplate = @"@using System.Dynamic;
@{
dynamic newobj = new ExpandoObject();
newobj.Title = Model.Message;
}
@Model.Title /
@if(Model.ShowTitle){
@RenderSection(""TestSection"") <text>/</text>

@Include(""TestInclude"", @newobj)
}
@RenderSection(""ParentSection"") GRANDPARENT
@RenderBody()
GRANDPARENT";

            var parenttemplate = @"@{Layout = ""GrandParent"";}@Model.Title PARENT
@RenderBody()
@section ParentSection {ParentSection:-)}
PARENT";

            var TestInclude = @" :-) TestInclude :-)
@if(1==1){
<text>############ </text>@Model.Title<text> ############</text>
}
###### @Model.Title ########
";

            //var data = new { Message = "Hello World!", Title = "Test Template Transform.", ShowTitle = true };
            dynamic data = new ExpandoObject();
            data.Message = "Hello Expando";
            data.Title = "Test Template Transform.";
            data.ShowTitle = true;

            var otherTemplates = new RazorTemplateSections();
            otherTemplates.Add(grandparenttemplate,  "GrandParent");
            otherTemplates.Add(parenttemplate, "Parent");
            otherTemplates.Add(TestInclude, "TestInclude");

            var output = RazorTemplateExtensions.ParseWithRazorRaw(data, TemplateString, sectionTemplates: otherTemplates, templateBase: typeof(GenericTemplateBase<>));

            Assert.IsTrue(output.Contains("Test Template Transform"));
        }

        #endregion

        [TestMethod]
        public void NewTemplateTest()
        {
            var parser = new TemplateParser();

            var TemplateString = @"
@using System.IO;
@{
var filetext = File.ReadAllText(@""C:\junk\Test_DIDs.txt"");
}
@if(Model.ShowTitle){<text>@Model.Title</text>
}

@GetHelloWorldText()
@GetSomeOutput(@Model)
#############
{ hello }
@Model.Message
#############
@filetext
#############
@GetDataFromFile()

";
            //var data = new { Message = "Hello World!", Title = "Test Template Transform.", ShowTitle = true };
            dynamic data = new ExpandoObject();
            data.Message = "Hello Expando";
            data.Title = "Test Template Transform.";
            data.ShowTitle = true;

            var output = parser.RenderTemplate(data, null, TemplateString, templateBase: typeof(GenericTemplateBase<>));

            Assert.IsTrue(output.Contains("Test Template Transform."));


        }


        [TestMethod]
        public void NewTemplateTest2()
        {
            var parser = new TemplateParser();

            var TemplateString = @"@{Layout = ""Parent"";}
@if(Model.ShowTitle){
@Model.Title
}
#############
<text>{ hello } </text>
@Model.Message
#############
";

            var parenttemplate = @"@{Layout = ""GrandParent"";}@Model.Title PARENT
@RenderBody()
PARENT";

            var grandparenttemplate = @"@Model.Title GRANDPARENT
@RenderBody()
GRANDPARENT";

            //var data = new { Message = "Hello World!", Title = "Test Template Transform.", ShowTitle = false };
            var data = new TestMessage();
            data.Message = "Hello Expando";
            data.Title = "Test Template Transform.";
            data.ShowTitle = false;

            var otherTemplates = new RazorTemplateSections
            {
                {grandparenttemplate, "GrandParent"},
                {parenttemplate, "Parent"}
            };

            //var output = data.ParseWithRazorRaw(TemplateString, sectionTemplates: otherTemplates);

            //Assert.IsTrue(output.Contains("Test Template Transform"));
            var output = parser.RenderTemplate(data, typeof(TestMessage), TemplateString, sectionTemplates: otherTemplates, templateBase: typeof(GenericTemplateBase<>));

            Assert.IsTrue(output.Contains("Test Template Transform."));


        }

        [TestMethod]
        public void NewTemplateTest3()
        {
            var parser = new TemplateParser();

            const string templateString = @"@{Layout = ""Parent"";}
@if(Model.ShowTitle){
@Model.Title
}
#############
<text>{ hello } </text>
@Model.Message
#############
";

            var parenttemplate = @"@{Layout = ""GrandParent"";}@Model.Title PARENT
@RenderBody()
PARENT";

            var grandparenttemplate = @"@Model.Title GRANDPARENT
@RenderBody()
GRANDPARENT";

            var messages = new List<TestMessage>
            {
                new TestMessage
                {
                    Message = "Hello Message",
                    Title = "Test Template Transform.",
                    ShowTitle = false
                },
                new TestMessage
                {
                    Message = "Hello Message with title",
                    Title = "Test Template Transform.",
                    ShowTitle = true
                }
            };

            var otherTemplates = new RazorTemplateSections
            {
                {grandparenttemplate, "GrandParent"},
                {parenttemplate, "Parent"}
            };

            //var output = data.ParseWithRazorRaw(TemplateString, sectionTemplates: otherTemplates);

            //Assert.IsTrue(output.Contains("Test Template Transform"));
            var output = parser.RenderTemplates(messages, typeof(TestMessage), templateString, sectionTemplates: otherTemplates, templateBase: typeof(GenericTemplateBase<>));
            foreach (var s in output)
            {
                Assert.IsTrue(s.Contains("Test Template Transform."));
            }

        }
    }
}
