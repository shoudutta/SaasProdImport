using System;
using System.Collections.Generic;
using System.Text;

namespace SaasProdImport.Interfaces
{
    /// <summary>
    /// Provides an entry point for the business logic for the system
    /// </summary>
    public interface IManifestService
    {
        void ReadProductManifests();

        void AddConfiguration();

        void DisplayConfigurations();

        void DisplayProducts();
    }
}
