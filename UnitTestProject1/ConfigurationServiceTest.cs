using Microsoft.VisualStudio.TestTools.UnitTesting;
using SaasProdImport.Models;
using SaasProdImport.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaasProdImport.UnitTests
{
    [TestClass]
    public class ConfigurationServiceTest : TestBase
    {
        private static ConfigurationService configurationService;
        private static Configuration configuration;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            TestBase test = new TestBase();
            configurationService = new ConfigurationService(TestBase.context);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            configuration = new Configuration
            {
                ConfigurationId = 4,
                FormatId = 0,
                ConfigTag = "Config",
                NameTag = "Name",
                TwitterTag = "Twitter"
            };
        }

        [TestCleanup]
        public void TestCleanup()
        {
            configuration = null;
        }

        [TestMethod]
        public void GetConfigurations()
        {
            var dummyConfigs = configurationService.GetConfigurations();
            Assert.IsNotNull(dummyConfigs);
            Assert.AreEqual(2, dummyConfigs.Count);
            Assert.IsNull(dummyConfigs.Find(x => x.ConfigurationId == 3));
        }

        [TestMethod]
        public void AddConfig()
        {
            configuration.ConfigurationId = configurationService.GetNextId();
            Assert.AreEqual(3, configuration.ConfigurationId);

            configurationService.AddConfiguration(configuration);
            var dummyConfigs = configurationService.GetConfigurations();
            Assert.IsNotNull(dummyConfigs);
            Assert.AreEqual(3, dummyConfigs.Count);
            Assert.IsNotNull(dummyConfigs.Find(x => x.ConfigurationId == 3));
            Assert.IsTrue(dummyConfigs.Contains(configuration));
        }
    }
}
