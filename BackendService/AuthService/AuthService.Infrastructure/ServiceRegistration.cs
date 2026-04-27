using AuthService.Application.Mapper;
using AuthService.Application.Repositories;
using AuthService.Application.Services.Abstractions;
using AuthService.Application.Services.Implmentations;
using AuthService.Infrastructure.Persistent;
using AuthService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthService.Infrastructure
{
    public class ServiceRegistration
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            // Register DbContext
            services.AddDbContext<AuthDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DbConnection")));

            // Register Repositories
            services.AddScoped<IUserRepository, UserRepository>();

            // Register Services
            services.AddScoped<IUserAppService, UserAppService>();

            //Register AutoMapper

           services.AddAutoMapper(cfg => cfg.AddProfile<UserMapper>());
        }
    }
}
