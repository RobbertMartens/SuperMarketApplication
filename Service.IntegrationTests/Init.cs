using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;
using Service.Interfaces;
using Service.Repositories;
using Service.Services;
using System.IO;

namespace Service.IntegrationTests
{
    public class Init
    {
        protected ICalculateProductPrice CalculateProductPrice;
        protected ICalculateCartPrice CalculateCartPrice;
        protected IRegisterService RegisterService;
        protected IProductService ProductService;
        protected ILijpeVoorraadServerService LijpeVoorraadServerService;
        private AppSettings Config;

        [SetUp]
        public void SetUp()
        {
            var filePath = Directory.GetCurrentDirectory().Replace(@"bin\Debug\netcoreapp3.1", "") + "appsettings.json";
            using (var reader = new StreamReader(filePath))
            {
                var file = reader.ReadToEnd();
                Config = JsonConvert.DeserializeObject<AppSettings>(file);
            }

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDbContext<ProductContext>(options => options.UseSqlServer(Config.ProductContext));
            serviceCollection.AddScoped<ICalculateProductPrice, CalculateProductPrice>();
            serviceCollection.AddScoped<ICalculateCartPrice, CalculateCartPrice>();
            serviceCollection.AddScoped<IRegisterService, RegisterService>();
            serviceCollection.AddScoped<IProductService, ProductService>();
            serviceCollection.AddScoped<ILijpeVoorraadServerService, LijpeVoorraadServerService>();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            CalculateProductPrice = serviceProvider.GetService<ICalculateProductPrice>();
            CalculateCartPrice = serviceProvider.GetService<ICalculateCartPrice>();
            RegisterService = serviceProvider.GetService<IRegisterService>();
            ProductService = serviceProvider.GetService<IProductService>();
            LijpeVoorraadServerService = serviceProvider.GetService<ILijpeVoorraadServerService>();
        }
    }
}
