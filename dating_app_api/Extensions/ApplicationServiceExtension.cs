using dating_app_api.Data;
using dating_app_api.Interfaces;
using dating_app_api.Services;
using Microsoft.EntityFrameworkCore;

namespace dating_app_api.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("SqlServerConnection"));
            });
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}
