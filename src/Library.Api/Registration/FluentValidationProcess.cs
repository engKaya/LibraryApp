using FluentValidation.AspNetCore;
using FluentValidation;
using Libary.Infastructure.Validations;
using Microsoft.AspNetCore.Mvc;  
using Library.Domain.DTOs.User;
using Library.Domain.Validations;

namespace Library.Api.Registration
{
    public static class FluentValidationProcess
    {
        public static IServiceCollection ConfigureValidation(this IServiceCollection services)
        {
           //services.RegisterValidationCouples();
            services.AddFluentValidation(s =>{
                s.RegisterValidatorsFromAssemblyContaining<CreateUserRequestValidation>();
            });
            services.Configure<ApiBehaviorOptions>(opt =>
            {
                opt.SuppressModelStateInvalidFilter = true;
            });

            return services;
        }

        public static void RegisterValidationCouples(this IServiceCollection services)
        {
            services.AddScoped<IValidator<CreateUserRequest>, CreateUserRequestValidation>();
            services.AddScoped<IValidator<LoginRequest>, LoginRequestValidation>();

        }
    }
}
