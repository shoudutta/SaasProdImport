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
        Format GetFormatByName(string name);
        int AddFormat(string name);

    }
}
