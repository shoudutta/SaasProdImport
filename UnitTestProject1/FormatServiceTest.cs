using Microsoft.VisualStudio.TestTools.UnitTesting;
using SaasProdImport.Models;
using SaasProdImport.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaasProdImport.UnitTests
{
    [TestClass]
    public class FormatServiceTest : TestBase
    {
        private static FormatService formatService;
        private static Format format;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            TestBase testBase = new TestBase();
            formatService = new FormatService(TestBase.context);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            format = new Format
            {
                FormatId = 0,
                FormatName = "csv"
            };
        }

        [TestCleanup]
        public void TestCleanup()
        {
            format = null;
        }

        [TestMethod]
        public void GetFormats()
        {
            var dummyFormats = formatService.GetFormats();
            Assert.IsNotNull(dummyFormats);
            Assert.AreEqual(2, dummyFormats.Count);
        }

        [TestMethod]
        public void GetFormatByName()
        {
            var jsonFormat = formatService.GetFormatByName("json");
            Assert.IsNotNull(jsonFormat);

            var csvFormat = formatService.GetFormatByName("csv");
            Assert.IsNull(csvFormat);
        }

        [TestMethod]
        public void AddFormat()
        {
            formatService.AddFormat(format.FormatName);
            var dummyFormats = formatService.GetFormats();
            Assert.IsNotNull(dummyFormats);
            Assert.AreEqual(3, dummyFormats.Count);
            Assert.IsFalse(dummyFormats.Contains(format));
            Assert.IsTrue(dummyFormats.Exists(x => x.FormatName == format.FormatName));
        }
    }
}
