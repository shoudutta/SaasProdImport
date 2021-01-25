using SaasProdImport.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaasProdImport.Controllers
{
    class Controller : IController
    {
        private readonly IManifestService manifestService;
        private readonly IConfigurationService configurationService;

        public Controller(IManifestService manifestService, IConfigurationService configurationService)
        {
            this.manifestService = manifestService;
            this.configurationService = configurationService;
        }
        public void Main()
        {
            int userChoice = 3;
            do
            {
                Console.WriteLine("Please select your option - ");
                Console.WriteLine("1 - Read product manifests");
                Console.WriteLine("2 - Add configuration");
                Console.WriteLine("3 - View configurations");
                Console.WriteLine("4 - View imported products");
                Console.WriteLine("5 - Exit");
                userChoice = Convert.ToInt32(Console.ReadLine());
                switch (userChoice)
                {
                    case 1:
                        manifestService.ReadProductManifests();
                        break;
                    case 2:
                        manifestService.AddConfiguration();
                        break;
                    case 3:
                        manifestService.DisplayConfigurations();
                        break;
                    case 4:
                        manifestService.DisplayProducts();
                        break;
                    default:
                        break;
                }
                Console.WriteLine();
            } while (userChoice != 5);
        }
    }
}
