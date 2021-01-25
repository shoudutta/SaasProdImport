using SaasProdImport.Interfaces;
using SaasProdImport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaasProdImport.Services
{
    class ConfigurationService : IConfigurationService
    {
        private readonly IContext context;

        public ConfigurationService(IContext context)
        {
            this.context = context;
        }

        public int GetNextId()
        {
            return this.context.GetConfigurations().Count() + 1;
        }

        public List<Configuration> GetConfigurations()
        {
            return this.context.GetConfigurations();
        }

        public Configuration GetConfigurationForFormat(int formatId)
        {
            return this.context.GetConfigurations().FindAll(x => x.FormatId == formatId).FirstOrDefault();
        }

        public void AddConfiguration(Configuration configuration)
        {
            this.context.AddConfiguration(configuration);
        }
    }
}
