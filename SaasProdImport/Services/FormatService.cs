using SaasProdImport.Interfaces;
using SaasProdImport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaasProdImport.Services
{
    public class FormatService : IFormatService
    {
        private readonly IContext context;

        public FormatService(IContext context)
        {
            this.context = context;
        }

        public List<Format> GetFormats()
        {
            return this.context.GetFormats();
        }

        public Format GetFormatByName(string name)
        {
            return this.context.GetFormats().FindAll(x => x.FormatName.ToLower().Equals(name.ToLower())).FirstOrDefault();
        }

        public int AddFormat(string name)
        {
            Format toBeAdded = new Format();
            toBeAdded.FormatName = name;
            toBeAdded.FormatId = GetFormats().Count() + 1;

            this.context.AddFormat(toBeAdded);

            return toBeAdded.FormatId;
        }
    }
}
