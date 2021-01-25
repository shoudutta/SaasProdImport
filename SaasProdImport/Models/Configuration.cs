using System;
using System.Collections.Generic;
using System.Text;

namespace SaasProdImport.Models
{
    public class Configuration
    {
        public int ConfigurationId { get; set; }
        public int FormatId { get; set; }
        public string NameTag { get; set; }
        public string ConfigTag { get; set; }
        public string TwitterTag { get; set; }
    }
}
