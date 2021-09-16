using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Service.Interfaces;
using Service.Models;
using Service.Repositories;
using Service.Services;
using System.Collections.Generic;
using System.IO;

namespace Service
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddScoped<ICalculateProductPrice, CalculateProductPrice>();
            services.AddScoped<IReceiptService, ReceiptService>();
            services.AddScoped<IRegisterService, RegisterService>();
            services.AddDbContext<ProductContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ProductContext")));
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ISupplyService, SupplyService>();
            services.AddScoped<IMapperService, MapperService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            var scope = app.ApplicationServices.CreateScope();
            var productContext = scope.ServiceProvider.GetService<ProductContext>();
            PopulateProductsTable(productContext);
        }

        private void PopulateProductsTable(ProductContext context)
        {
            var data = File.ReadAllText(Directory.GetCurrentDirectory() + "\\Repositories\\ProductData.json");
            var products = JsonConvert.DeserializeObject<List<Product>>(data);
            context.AddRange(products);
            context.SaveChanges();
        }
    }
}
