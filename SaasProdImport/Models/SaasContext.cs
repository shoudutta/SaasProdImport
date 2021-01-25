using System;
using System.Collections.Generic;
using System.Text;

namespace SaasProdImport.Models
{
    using Microsoft.EntityFrameworkCore;
    using SaasProdImport.Interfaces;

    public class SaasContext : IContext
    {
        private List<Format> Formats { get; set; }
        private List<Product> Products { get; set; }
        private List<Configuration> Configurations { get; set; }

        public SaasContext()
        {
            this.Formats = new List<Format>()
            {
                new Format
                {
                    FormatId = 1,
                    FormatName = "yaml"
                },
                new Format
                {
                    FormatId = 2,
                    FormatName = "json"
                }
            };

            this.Products = new List<Product>();

            this.Configurations = new List<Configuration>()
            {
                new Configuration
                {
                    ConfigurationId = 1,
                    FormatId = 1,
                    ConfigTag = "tags",
                    NameTag = "name",
                    TwitterTag = "twitter"
                },
                new Configuration
                {
                    ConfigurationId = 2,
                    FormatId = 2,
                    ConfigTag = "categories",
                    NameTag = "title",
                    TwitterTag = "twitter"
                }
            };
        }

        public List<Product> GetProducts()
        {
            return this.Products;
        }

        public void AddProduct(Product product)
        {
            this.Products.Add(product);
        }

        public List<Configuration> GetConfigurations()
        {
            return this.Configurations;
        }

        public void AddConfiguration(Configuration configuration)
        {
            this.Configurations.Add(configuration);
        }

        public List<Format> GetFormats()
        {
            return this.Formats;
        }

        public Format GetFormatByName(string name)
        {
            return this.Formats.Find(x => x.FormatName.ToLower().Equals(name.ToLower()));
        }

        public void AddFormat(Format format)
        {
            this.Formats.Add(format);
        }
    }
}
