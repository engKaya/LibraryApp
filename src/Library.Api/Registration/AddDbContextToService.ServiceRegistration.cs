using Library.Infastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Library.Api.Registration
{
    public static class AddDbContextToService
    {
        public static IServiceCollection AddDbContextToServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<LibraryDbContext>(options =>
            {
                var dbco = configuration.GetConnectionString("AppConnectionString");
                options.UseSqlServer(dbco);
                options.EnableSensitiveDataLogging();
            });

            return services;
        }
    }
}
