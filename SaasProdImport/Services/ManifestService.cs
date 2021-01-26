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
            Console.WriteLine("Enter directory?");
            string manifestLocation = Console.ReadLine();
            // Retrieve the files in the location and iterate over each and read the contents
            List<string> filesInDirectory = Directory.GetFiles(manifestLocation).ToList();
            filesInDirectory.ForEach(x =>
            {
                string formatExtension = GetFileFormat(x);
                var format = formatService.GetFormatByName(formatExtension);
                var configForFormat = configurationService.GetConfigurationForFormat(format.FormatId);
                try
                {
                    // Read the data from the manifest file
                    string fileData = ReadManifest(x);
                    // Parse the file into products
                    configurationService.ReadProductsFromFile(fileData, formatExtension, configForFormat, out List<Product> products);                    
                    products.ForEach(product =>
                    {
                        product.ProductId = productService.GetNextId();
                        productService.AddProduct(product);
                        // Display the newly imported product and its details - sample output
                        DisplayImportedProduct(product);
                    });
                }
                catch (NotImplementedException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            });
        }
        
        public void AddConfiguration()
        {
            // Accept the format extension and configuration details from the user
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
            // Iterate the set of products and display the details for each
            var products = productService.GetProducts();
            products.ForEach(product =>
            {
                Console.WriteLine($"Name: {product.ProductName}; Categories: {product.ProductConfiguration}; Twitter: {product.ProductTwitter}");
            });
        }

        private static string ReadManifest(string fileName)
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
            }

            return fileData;
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
