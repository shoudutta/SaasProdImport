using SaasProdImport.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaasProdImport.Interfaces
{
    /// <summary>
    /// Provides an interface to interact with the configurations of the various formats
    /// </summary>
    public interface IConfigurationService
    {
        List<Configuration> GetConfigurations();

        Configuration GetConfigurationForFormat(int formatId);

        /// <summary>
        /// Returns the next id for configurations collection - needed to streamline mock behaviour of the db
        /// </summary>
        /// <returns></returns>
        int GetNextId();

        /// <summary>
        /// Adds the configuration object to the set of configurations
        /// </summary>
        /// <param name="configuration"></param>
        void AddConfiguration(Configuration configuration);

        /// <summary>
        /// Reads the products from the data passed based on the format of the file
        /// </summary>
        /// <param name="fileData"></param>
        /// <param name="fileFormat"></param>
        /// <param name="configuration"></param>
        /// <param name="products"></param>
        void ReadProductsFromFile(string fileData, string fileFormat, Configuration configuration, out List<Product> products);
    }
}
