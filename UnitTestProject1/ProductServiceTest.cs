using Microsoft.VisualStudio.TestTools.UnitTesting;
using SaasProdImport.Models;
using SaasProdImport.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaasProdImport.UnitTests
{
    [TestClass]
    public class ProductServiceTest : TestBase
    {
        private static ProductService productService;
        private static Product product;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            TestBase testBase = new TestBase();
            productService = new ProductService(TestBase.context);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            product = new Product
            {
                ProductId = 0,
                ProductConfiguration = "domain 1, domain 2",
                ProductName = "Dummy product",
                ProductTwitter = "Dummy twitter"
            };
        }

        [TestCleanup]
        public void TestCleanup()
        {
            product = null;
        }

        [TestMethod]
        public void GetProducts()
        {
            var dummyProducts = productService.GetProducts();
            Assert.IsNotNull(dummyProducts);
            Assert.AreEqual(0, dummyProducts.Count);
        }

        [TestMethod]
        public void AddProduct()
        {
            product.ProductId = productService.GetNextId();
            Assert.AreEqual(1, product.ProductId);

            productService.AddProduct(product);
            var dummyProducts = productService.GetProducts();
            Assert.IsNotNull(dummyProducts);
            Assert.AreEqual(1, dummyProducts.Count);
            Assert.IsTrue(dummyProducts.Contains(product));
        }
    }
}
