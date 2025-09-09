using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApp.Application.Interfaces;
using WebApp.Application.services;
using WebApp.Application.Services;
using WebApp.Core.interfaces;
using WebApp.Infrastructure.Identity;
using WebApp.Infrastructure.persistence;
using WebApp.Infrastructure.repositories;

namespace WebApp.Infrastructure.Persistence
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure( this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepo<>), typeof(GenericRepo<>));
            services.AddScoped<IAuthService,AuthService>();
            services.AddTransient<IEmailService, EmailService>();


            return services;
        }
    }
}
