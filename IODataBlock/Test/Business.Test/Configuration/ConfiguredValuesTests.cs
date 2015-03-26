using System;
using System.Dynamic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Business.Common.Configuration;

namespace Business.Test.Configuration
{
    [TestClass]
    public class ConfiguredValuesTests
    {
        public ConfiguredValuesTests()
        {
            _configLoader = new JsonConfigLoader(new FileInfo(@"c:\junk\test.config.json"));
            _config = new ExpandoObject();
        }

        private readonly JsonConfigLoader _configLoader;
        private dynamic _config;

        [TestMethod]
        public void CanSaveConfigWithJsonConfigLoader()
        {
            /* Set a property on the config */
            _config.hello = @"hello world";
            /* Save the config */
            _configLoader.SaveConfiguration(_config);
        }

        [TestMethod]
        public void CanLoadConfigWithJsonConfigLoader()
        {
            /* Load the config */
            _config = _configLoader.LoadConfiguration();
            Assert.IsTrue(_config.hello == @"hello world");
        }

        [TestMethod]
        public void CanSaveConfig()
        {
            /* Set a property on the config */
            ConfiguredValues.Instance.Config.hello = @"hello world!";
            /* Save the config */
            ConfiguredValues.Instance.Save(_configLoader);
        }

        [TestMethod]
        public void CanLoadConfig()
        {
            /* Set a property on the config */
            ConfiguredValues.Instance.Load(_configLoader);
            /* Save the config */
            Assert.IsTrue(ConfiguredValues.Instance.Config.hello == @"hello world!");
        }

    }
}
