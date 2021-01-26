using SaasProdImport.Interfaces;
using SaasProdImport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaasProdImport.Services
{
    public class ProductService : IProductService
    {
        private readonly IContext context;

        public ProductService(IContext context)
        {
            this.context = context;
        }

        public int GetNextId()
        {
            //Return the count of the current list incremented by 1
            return this.context.GetProducts().Count() + 1;
        }

        public List<Product> GetProducts()
        {
            return this.context.GetProducts();
        }

        public void AddProduct(Product product)
        {
            this.context.AddProduct(product);
        }
    }
}
