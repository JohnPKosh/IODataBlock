using Business.Common.System.App;
using Business.Common.System.States;
using Business.Test.TestUtility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Business.Test.Common.System.App
{
    [TestClass]
    public class AppStateTests
    {
        public AppStateTests()
        {
            _appStateLoader = new JsonFileLoader(new FileInfo(@"c:\junk\test.appState.json"));
            _appState = FakePet.CreateBela();
        }

        private readonly JsonFileLoader _appStateLoader;
        private FakePet _appState;

        [TestMethod]
        public void CanSaveStateWithJsonConfigLoader()
        {
            /* Set a property on the config */
            _appState.Age = 7;
            /* Save the config */
            _appStateLoader.SaveState(_appState);
        }

        [TestMethod]
        public void CanLoadStateWithJsonConfigLoader()
        {
            /* Load the config */
            _appState = _appStateLoader.LoadState<FakePet>();
            Assert.IsTrue(_appState.Age == 7);
        }

        [TestMethod]
        public void CanSaveState()
        {
            /* Set a property on the config */
            AppState.Instance.Value = _appState;
            /* Save the config */
            AppState.Instance.Save(_appStateLoader);
        }

        [TestMethod]
        public void CanLoadState()
        {
            /* Set a property on the config */
            _appState.Age = 7;
            /* Load the config */
            AppState.Instance.Load<FakePet>(_appStateLoader);
            /* Save the config */
            var fakePet = AppState.Instance.Value as FakePet;
            Assert.IsTrue(fakePet != null && fakePet.Name == _appState.Name);
        }
    }
}