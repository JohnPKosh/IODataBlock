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
            _appStateLoader = new DynamicJsonFileLoader(new FileInfo(@"c:\junk\test.appState.json"));
            _appState = new ExpandoObject();
        }

        private readonly DynamicJsonFileLoader _appStateLoader;
        private dynamic _appState;

        [TestMethod]
        public void CanSaveStateWithJsonConfigLoader()
        {
            /* Set a property on the config */
            _appState.hello = @"hello world";
            /* Save the config */
            _appStateLoader.SaveState(_appState);
        }

        [TestMethod]
        public void CanLoadStateWithJsonConfigLoader()
        {
            /* Load the config */
            _appState = _appStateLoader.LoadState();
            Assert.IsTrue(_appState.hello == @"hello world");
        }

        [TestMethod]
        public void CanSaveState()
        {
            /* Set a property on the config */
            DynamicAppState.Instance.Value.hello = @"hello world!";
            /* Save the config */
            DynamicAppState.Instance.Save(_appStateLoader);
        }

        [TestMethod]
        public void CanLoadState()
        {
            /* Set a property on the config */
            DynamicAppState.Instance.Load(_appStateLoader);
            /* Save the config */
            Assert.IsTrue(DynamicAppState.Instance.Value.hello == @"hello world!");
        }
    }
}
