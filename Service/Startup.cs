using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Service.Interfaces;
using Service.Repositories;
using Service.Services;

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
        }
    }
}
