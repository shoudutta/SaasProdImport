using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SaasProdImport.Interfaces;
using SaasProdImport.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using YamlDotNet.Serialization;

namespace SaasProdImport.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IContext context;

        public ConfigurationService(IContext context)
        {
            this.context = context;
        }

        public int GetNextId()
        {
            // Return the count of the current list incremented by 1
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

        public void ReadProductsFromFile(string fileData, string fileFormat, Configuration configuration, out List<Product> products)
        {
            // Reading the file data differs for each format
            switch (fileFormat)
            {
                case "json":
                    ReadProductFromJson(fileData, configuration, out products);
                    break;
                case "yaml":
                    ReadProductFromYaml(fileData, configuration, out products);
                    break;
                default:
                    throw new NotImplementedException("Sorry, this format is not yet implemented");
            }
        }


        private static void ReadProductFromJson(string fileData, Configuration configuration, out List<Product> products)
        {
            products = new List<Product>();
            // Convert the string into dynamic JSON key-value pairs
            dynamic fileConfigs = JsonConvert.DeserializeObject<ExpandoObject>(fileData, new ExpandoObjectConverter());
            foreach (var prod in (IEnumerable<dynamic>)fileConfigs.products)
            {
                Product product = new Product();
                foreach (var entry in prod)
                {
                    // Compare the key value with the different configuration tags for the product and assign where they match
                    if (entry.Key.ToString().ToLower().Equals(configuration.ConfigTag.ToLower()))
                    {
                        string prodConfig = string.Empty;
                        foreach (string value in entry.Value)
                        {
                            prodConfig = prodConfig == string.Empty ? value : prodConfig + "," + value;
                        }
                        product.ProductConfiguration = prodConfig;
                    }
                    else if (entry.Key.ToString().ToLower().Equals(configuration.NameTag.ToLower()))
                    {
                        product.ProductName = entry.Value.ToString();
                    }
                    else if (entry.Key.ToString().ToLower().Equals(configuration.TwitterTag.ToLower()))
                    {
                        product.ProductTwitter = entry.Value.ToString();
                    }
                }
                products.Add(product);
            }
        }

        private static void ReadProductFromYaml(string fileData, Configuration configuration, out List<Product> products)
        {
            products = new List<Product>();
            Product product;
            var deserializer = new Deserializer();
            // Convert the string into a list of hashtables
            var result = deserializer.Deserialize<List<Hashtable>>(new StringReader(fileData));
            foreach (var item in result)
            {
                product = new Product();

                foreach (DictionaryEntry entry in item)
                {
                    // Compare the key value with the different configuration tags for the product and assign where they match
                    if (entry.Key.ToString().ToLower().Equals(configuration.ConfigTag.ToLower()))
                    {
                        product.ProductConfiguration = entry.Value.ToString();
                    }
                    else if (entry.Key.ToString().ToLower().Equals(configuration.NameTag.ToLower()))
                    {
                        product.ProductName = entry.Value.ToString();
                    }
                    else if (entry.Key.ToString().ToLower().Equals(configuration.TwitterTag.ToLower()))
                    {
                        product.ProductTwitter = entry.Value.ToString();
                    }
                }
                products.Add(product);
            }
        }

    }
}
