using SaasProdImport.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaasProdImport.Interfaces
{
    /// <summary>
    /// Provides an interface to interact with the different formats in the system
    /// </summary>
    public interface IFormatService
    {
        List<Format> GetFormats();

        /// <summary>
        /// Provides an end point for retrieving the Format object for a given format extension.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Format GetFormatByName(string name);

        int AddFormat(string name);

    }
}
