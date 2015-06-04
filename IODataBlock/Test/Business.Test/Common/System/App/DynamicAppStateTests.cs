using System;
using System.Dynamic;
using System.IO;
using Business.Common.System;
using Business.Common.System.App;
using Business.Common.System.States;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Business.Test.Common.System.App
{
    [TestClass]
    public class DynamicAppStateTests
    {
        public DynamicAppStateTests()
        {
            _dynamicJsonLoader = new DynamicJsonFileLoader(new FileInfo(@"c:\junk\test.appState2.json"));
            _dynamicBsonLoader = new DynamicBsonFileLoader(new FileInfo(@"c:\junk\test.appState2.bson"));
            _appState = new ExpandoObject();
        }

        private readonly IDynamicLoader _dynamicJsonLoader;
        private readonly IDynamicLoader _dynamicBsonLoader;
        private dynamic _appState;

        [TestMethod]
        public void CanSaveStateWithJsonConfigLoader()
        {
            /* Set a property on the config */
            _appState.hello = @"hello world";
            /* Save the config */
            _dynamicJsonLoader.Save(_appState);
        }

        [TestMethod]
        public void CanLoadStateWithJsonConfigLoader()
        {
            /* Load the config */
            _appState = _dynamicJsonLoader.Load();
            Assert.IsTrue(_appState.hello == @"hello world");
        }


        [TestMethod]
        public void CanSaveStateWithBsonConfigLoader()
        {
            /* Set a property on the config */
            _appState.hello = @"hello world";
            /* Save the config */
            _dynamicBsonLoader.Save(_appState);
        }

        [TestMethod]
        public void CanLoadStateWithBsonConfigLoader()
        {
            /* Load the config */
            _appState = _dynamicBsonLoader.Load();
            Assert.IsTrue(_appState.hello == @"hello world");
        }




        [TestMethod]
        public void CanSaveState()
        {
            /* Set a property on the config */
            DynamicAppState.Instance.Value.hello = @"hello world!";
            /* Save the config */
            DynamicAppState.Instance.Save(_dynamicJsonLoader);
        }

        [TestMethod]
        public void CanLoadState()
        {
            /* Set a property on the config */
            DynamicAppState.Instance.Load(_dynamicJsonLoader);
            /* Save the config */
            Assert.IsTrue(DynamicAppState.Instance.Value.hello == @"hello world!");
        }

        [TestMethod]
        public void CanSaveWithSystemCommand()
        {
            /* Set a property on the config */
            //DynamicAppState.Instance.Load(_appStateLoader);

            DynamicAppState.Instance.Value.hello = @"hello world from system command!";

            /* Save the config */
            //Assert.IsTrue(DynamicAppState.Instance.Value.hello == @"hello world!");

            var correllationId = NewGuid();

            // Execute the command that does the work ALL IN ONE STEP :-).
            var responseObject = SystemCommandParser.ExecuteCommand("System.App.DynamicAppState", "SaveDynamicAppState", _dynamicJsonLoader, correllationId);

            var responseString = responseObject.ToJson(true);
            Assert.IsTrue(!String.IsNullOrWhiteSpace(responseString));

            #region Json result

            /*
                {
                  "RequestData": {
                    "CommandName": "Save",
                    "RequestData": {},
                    "CorrelationId": "52e1e641-3ffa-422b-8017-31061e5cd839",
                    "DateCreatedUtc": "0001-01-01T00:00:00"
                  },
                  "CorrelationId": "52e1e641-3ffa-422b-8017-31061e5cd839",
                  "ResponseData": true,
                  "ResponseCode": "200",
                  "HasExceptions": false,
                  "ExceptionCount": 0
                }
            */

            #endregion Json result
        }

        [TestMethod]
        public void CanSaveWithSystemCommandWithoutLoader()
        {
            /* Set a property on the config */
            //DynamicAppState.Instance.Load(_appStateLoader);
            var loadResponse = SystemCommandParser.ExecuteCommand("System.App.DynamicAppState", "LoadDynamicAppState", null, NewGuid());
            Assert.IsFalse(loadResponse.HasExceptions);

            //DynamicAppState.Instance.Value.hello = @"hello world from system command!";

            /* Save the config */
            //Assert.IsTrue(DynamicAppState.Instance.Value.hello == @"hello world!");

            var correllationId = NewGuid();

            // Execute the command that does the work ALL IN ONE STEP :-).
            var responseObject = SystemCommandParser.ExecuteCommand("System.App.DynamicAppState", "SaveDynamicAppState", null, correllationId);

            var responseString = responseObject.ToJson(true);
            Assert.IsTrue(!String.IsNullOrWhiteSpace(responseString));

            #region Json result

            /*
                {
                  "RequestData": {
                    "CommandName": "Save",
                    "RequestData": {},
                    "CorrelationId": "52e1e641-3ffa-422b-8017-31061e5cd839",
                    "DateCreatedUtc": "0001-01-01T00:00:00"
                  },
                  "CorrelationId": "52e1e641-3ffa-422b-8017-31061e5cd839",
                  "ResponseData": true,
                  "ResponseCode": "200",
                  "HasExceptions": false,
                  "ExceptionCount": 0
                }
            */

            #endregion Json result
        }

        [TestMethod]
        public void CanGetSetLoadSave()
        {
            // Can we load what is persisted?
            var loadResponse = SystemCommandParser.ExecuteCommand("System.App.DynamicAppState", "LoadDynamicAppState", null, NewGuid());
            Assert.IsFalse(loadResponse.HasExceptions);

            // Can we get the app state data?
            var getResponse = SystemCommandParser.ExecuteCommand("System.App.DynamicAppState", "GetDynamicAppState", null, NewGuid());
            var workingdata = getResponse.ResponseData as dynamic;
            Assert.IsFalse(getResponse.HasExceptions);
            Assert.IsNotNull(workingdata);

            // Can we set the app state data?
            workingdata.LastUpdated = DateTime.Now;
            workingdata.StringValues = new[] {"hello", "world"};
            var setResponse = SystemCommandParser.ExecuteCommand("System.App.DynamicAppState", "SetDynamicAppState", workingdata, NewGuid());
            Assert.IsFalse(setResponse.HasExceptions);

            // Can we save what should be persisted?
            var saveResponse = SystemCommandParser.ExecuteCommand("System.App.DynamicAppState", "SaveDynamicAppState", null, NewGuid());
            Assert.IsFalse(saveResponse.HasExceptions);
        }

        private string NewGuid()
        {
            return Guid.NewGuid().ToString();
        }
    }
}