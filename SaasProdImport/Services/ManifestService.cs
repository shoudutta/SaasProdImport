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
    class ManifestService : IManifestService
    {
        private readonly IConfigurationService configurationService;
        private readonly IProductService productService;
        private readonly IFormatService formatService;

        public ManifestService(IConfigurationService configurationService, IProductService productService, IFormatService formatService)
        {
            this.productService = productService;
            this.configurationService = configurationService;
            this.formatService = formatService;
        }

        public void ReadProductManifests()
        {
            //Console.WriteLine("Enter format?");
            //string fileFormat = Console.ReadLine();
            //var format = formatService.GetFormatByName(fileFormat);
            //var configForFormat = configurationService.GetConfigurationForFormat(format.FormatId);
            //Console.WriteLine($"\n Name - {configForFormat.NameTag}\n Configuration - {configForFormat.ConfigTag} \n Twitter - {configForFormat.TwitterTag}");
            Console.WriteLine("Enter directory?");
            string manifestLocation = Console.ReadLine();
            List<string> filesInDirectory = Directory.GetFiles(manifestLocation).ToList();
            filesInDirectory.ForEach(x =>
            {
                //ReadManifest(x.Substring(x.LastIndexOf('\\') + 1));
                string formatExtension = GetFileFormat(x);
                var format = formatService.GetFormatByName(formatExtension);
                var configForFormat = configurationService.GetConfigurationForFormat(format.FormatId);
                try
                {
                    ReadManifest(x, formatExtension, configForFormat, out List<Product> products);
                    products.ForEach(product =>
                    {
                        product.ProductId = productService.GetNextId();
                        productService.AddProduct(product);
                        DisplayImportedProduct(product);
                    });
                }
                catch (NotImplementedException ex)
                {

                }
            });
        }

        public void AddConfiguration()
        {
            Console.WriteLine("Enter format extension?");
            string formatExtension = Console.ReadLine();
            int formatId = formatService.AddFormat(formatExtension);
            Configuration toBeAdded = new Configuration();
            toBeAdded.FormatId = formatId;
            toBeAdded.ConfigurationId = configurationService.GetNextId();
            Console.WriteLine("Enter tag for \"Configurations\"?");
            toBeAdded.ConfigTag = Console.ReadLine();
            Console.WriteLine("Enter tag for \"Name\"?");
            toBeAdded.NameTag = Console.ReadLine();
            Console.WriteLine("Enter tag for \"Twitter\"?");
            toBeAdded.TwitterTag = Console.ReadLine();
            configurationService.AddConfiguration(toBeAdded);
        }

        public void DisplayConfigurations()
        {
            var configs = configurationService.GetConfigurations();
            var formats = formatService.GetFormats();
            configs.ForEach(x =>
            {
                var format = formats.Find(y => y.FormatId == x.FormatId);
                Console.WriteLine($"\n Format - {format.FormatName}\n Name - {x.NameTag} \n Configuration - {x.ConfigTag} \n Twitter - {x.TwitterTag} \n");
            });
        }

        public void DisplayProducts()
        {
            var products = productService.GetProducts();
            products.ForEach(product =>
            {
                Console.WriteLine($"Name: {product.ProductName}; Categories: {product.ProductConfiguration}; Twitter: {product.ProductTwitter}");
            });
        }

        private static void ReadManifest(string fileName, string fileFormat, Configuration configuration, out List<Product> products)
        {
            Console.WriteLine("Importing - " + fileName);
            string fileData = string.Empty;
            using (FileStream fileStream = new FileStream(fileName, FileMode.Open))
            {
                fileData = string.Empty;
                using (StreamReader streamReader = new StreamReader(fileStream))
                {
                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        fileData += line;
                        fileData += "\n";
                    }
                }
                //Console.WriteLine(fileData);
            }
            try
            {
                ReadProductFromFileData(fileData, fileFormat, configuration, out products);
            }
            catch (NotImplementedException ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }

        }

        private static void ReadProductFromFileData(string fileData, string fileFormat, Configuration configuration, out List<Product> products)
        {
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
            dynamic fileConfigs = JsonConvert.DeserializeObject<ExpandoObject>(fileData, new ExpandoObjectConverter());
            foreach(var prod in (IEnumerable<dynamic>)fileConfigs.products)
            {
                Product product = new Product();
                foreach(var entry in prod)
                {
                    if (entry.Key.ToString().ToLower().Equals(configuration.ConfigTag.ToLower()))
                    {
                        string prodConfig = string.Empty;
                        foreach(string value in entry.Value)
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
            var result = deserializer.Deserialize<List<Hashtable>>(new StringReader(fileData));
            foreach (var item in result)
            {
                product = new Product();

                foreach (DictionaryEntry entry in item)
                {
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

        private static string GetFileFormat(string v)
        {
            return v.Substring(v.LastIndexOf('.') + 1);
        }


        private void DisplayImportedProduct(Product product)
        {
            Console.WriteLine($"Importing - Name: {product.ProductName}; Categories: {product.ProductConfiguration}; Twitter: {product.ProductTwitter}");
        }

    }
}
