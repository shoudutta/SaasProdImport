using SaasProdImport.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaasProdImport.Interfaces
{
    /// <summary>
    /// Dummy interface to mock the behaviour of a database provider
    /// </summary>
    public interface IContext
    {
        List<Product> GetProducts();
        void AddProduct(Product product);

        List<Configuration> GetConfigurations();
        void AddConfiguration(Configuration configuration);

        List<Format> GetFormats();
        Format GetFormatByName(string name);
        void AddFormat(Format format);
    }
}
