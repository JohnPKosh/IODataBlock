using System;
using Business.Common.System;
using Business.Test.TestUtility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Business.Test.Common
{
    [TestClass]
    public class CommandCollectionTests
    {
        public CommandCollectionTests()
        {
            //var commandObjectDictionary = new Dictionary<string, IEnumerable<ICommandObject>>
            //{
            //    {"FileCommands", new List<ICommandObject> {new WriteToFileCommand(), new ReadFromFileCommand()}},
            //    {"JsonFileCommands",new List<ICommandObject> {new WriteToJsonFileCommand(), new ReadFromJsonFileCommand()}}
            //};
            //_parser = new CommandCollectionParser(commandObjectDictionary);
            _parser = new CommandCollectionParser(FileCommandDictionary.Instance.CommandDictionary);
        }

        private readonly ICommandCollectionParser _parser;

        [TestMethod]
        public void ReallyBasicSuccessfullReadFromFileTest2()
        {
            var correllationId = NewGuid();

            // Execute the command that does the work ALL IN ONE STEP :-).
            var responseObject = _parser.Execute("JsonFileCommands", "ReadFromJsonFile", "Monkey?", correllationId);

            // TODO: Add some sort of TryParse logic for the command above!

            var responseString = responseObject.ToJson(true);
            Assert.IsTrue(!String.IsNullOrWhiteSpace(responseString));

            #region Json result

            /*
                {
                  "RequestData": {
                    "CommandName": "ReadFromJsonFile",
                    "RequestData": "Monkey?",
                    "CorrelationId": "559f4cee-f314-4c70-ba87-75c34bdc0abb",
                    "DateCreatedUtc": "0001-01-01T00:00:00"
                  },
                  "CorrelationId": "559f4cee-f314-4c70-ba87-75c34bdc0abb",
                  "ResponseData": "hello Monkey? from ReadFromJsonFile!",
                  "ResponseCode": "200",
                  "HasExceptions": false,
                  "ExceptionCount": 0
                }
            */

            #endregion Json result
        }

        private string NewGuid()
        {
            return Guid.NewGuid().ToString();
        }
    }
}