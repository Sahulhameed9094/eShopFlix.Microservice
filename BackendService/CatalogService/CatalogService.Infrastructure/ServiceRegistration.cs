using CatalogService.Application.Mappers;
using CatalogService.Application.Repositories;
using CatalogService.Application.Services.Abstractions;
using CatalogService.Application.Services.Implementations;
using CatalogService.Infrastructure.Persistence;
using CatalogService.Infrastructure.Persistence.Repositoires;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace CatalogService.Infrastructure
{
    public class ServiceRegistration
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            // Register the DbContext
            services.AddDbContext<CatalogServiceContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DbConnection")));

            // Register repositories
            services.AddScoped<IProductRepository, ProductRepository>();

            // Register other services as needed
            services.AddScoped<IProductAppService, ProductAppService>();

            // Register AutoMapper
            services.AddAutoMapper(cfg => cfg.AddProfile<ProductMapper>());
        }
    }
}
