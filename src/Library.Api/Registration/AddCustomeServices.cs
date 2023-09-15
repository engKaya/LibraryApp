using Libary.Application.Interfaces;
using Libary.Infastructure.Repos;
using Libary.Infastructure.Uof;
using Library.Application.Interfaces.Repos;
using Library.Domain.Interfaces;
using System.Reflection;

namespace Library.Api.Registration
{
    public static class AddCustomeServices
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddExternalLibraries();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            //services.AddScoped<IBookRepository, BookRepository>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddLogging(conf => conf.AddConsole()).Configure<LoggerFilterOptions>(cfg => cfg.MinLevel = LogLevel.Debug);
            services.ConfigureValidation();
            return services;
        }
        public static void AddExternalLibraries(this IServiceCollection services)
        {
            var ass = Assembly.GetExecutingAssembly();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(ass));
            services.AddAutoMapper(ass);
        }
    }
}
