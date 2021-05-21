using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        protected IConfiguration Configuration;
        protected ICalculateProductPrice CalculateProductPrice;
        protected IReceiptService ReceiptService;
        protected IRegisterService RegisterService;
        protected IProductService ProductService;
        protected ILijpeVoorraadServerService LijpeVoorraadServerService;
        protected IMapperService MapperService;

        [SetUp]
        public void SetUp()
        {
            var builder = new ConfigurationBuilder().
                SetBasePath(Directory.GetCurrentDirectory()).
                AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDbContext<ProductContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ProductContext")));
            serviceCollection.AddScoped<ICalculateProductPrice, CalculateProductPrice>();
            serviceCollection.AddScoped<IReceiptService, ReceiptService>();
            serviceCollection.AddScoped<IRegisterService, RegisterService>();
            serviceCollection.AddScoped<IProductService, ProductService>();
            serviceCollection.AddScoped<ILijpeVoorraadServerService, LijpeVoorraadServerService>();
            serviceCollection.AddScoped<IMapperService, MapperService>();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            CalculateProductPrice = serviceProvider.GetService<ICalculateProductPrice>();
            ReceiptService = serviceProvider.GetService<IReceiptService>();
            RegisterService = serviceProvider.GetService<IRegisterService>();
            ProductService = serviceProvider.GetService<IProductService>();
            LijpeVoorraadServerService = serviceProvider.GetService<ILijpeVoorraadServerService>();
        }
    }
}
