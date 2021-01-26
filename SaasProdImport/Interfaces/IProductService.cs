using SaasProdImport.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaasProdImport.Interfaces
{
    /// <summary>
    /// Provides an interface to interact with the products imported into the system
    /// </summary>
    public interface IProductService
    {
        List<Product> GetProducts();

        /// <summary>
        /// Returns the next id for products collection - needed to streamline mock behaviour of the db
        /// </summary>
        /// <returns></returns>
        int GetNextId();

        void AddProduct(Product product);

    }
}
