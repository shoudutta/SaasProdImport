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
        void AddConfiguration(Configuration configuration);
    }
}
