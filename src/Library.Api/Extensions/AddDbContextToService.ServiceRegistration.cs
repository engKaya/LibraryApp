using Library.Infastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Library.Api.Extensions
{
    public static class AddDbContextToService
    {
        public static IServiceCollection AddDbContextToServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<LibraryDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("UserConnectionString"));
                options.EnableSensitiveDataLogging();
            });

            return services;
        }
    }
}
