using HostingService.Domain.Services;
using HostingService.Infra.Data;
using HostingService.Infra.Repositories;
using HostingService.Infra.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HostingService.Infra.Configuration
{
    public static class DependenceInjection
    {
        public static IServiceCollection AddDataDependencyInjection(this IServiceCollection services, IConfiguration configuration)
        {
            if (services is null) throw new ArgumentNullException(nameof(services));
            services.AddTransient<IAuthServices, AuthServices>();
            services.AddDbContext<ApplicationDbContext>(options => { options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")); });
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            return services;
        }
    }
}