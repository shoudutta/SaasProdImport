using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SaasProdImport.Controllers;
using SaasProdImport.Interfaces;
using SaasProdImport.Models;
using SaasProdImport.Services;
using System;

namespace SaasProdImport
{
    class Program
    {
        static void Main(string[] args)
        {
            using (IHost host = CreateHostBuilder(args).Build())
            {
                Run(host.Services);

                host.Run();
            }
        }

        private static void Run(IServiceProvider services)
        {
            using (IServiceScope serviceScope = services.CreateScope())
            {
                IServiceProvider provider = serviceScope.ServiceProvider;

                var controller = provider.GetRequiredService<IController>();
                controller.Main();
            }
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureServices((_, services) =>
            services.AddSingleton<IFormatService, FormatService>().
            AddSingleton<IController, Controller>().
            AddSingleton<IManifestService, ManifestService>().
            AddSingleton<IConfigurationService, ConfigurationService>().
            AddSingleton<IProductService,ProductService>().
            AddSingleton<IContext,SaasContext>());
    }
}
