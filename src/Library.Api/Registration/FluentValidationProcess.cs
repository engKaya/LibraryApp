using FluentValidation.AspNetCore;
using FluentValidation;
using Libary.Infastructure.Validations;
using Library.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Registration
{
    public static class FluentValidationProcess
    {
        public static IServiceCollection ConfigureValidation(this IServiceCollection services)
        {
            services.AddScoped<IValidator<CreateUserRequest>, CreateUserRequestValidation>();
            services.AddFluentValidation(s =>
            {
                s.RegisterValidatorsFromAssemblyContaining<CreateUserRequestValidation>();
            });
            services.Configure<ApiBehaviorOptions>(opt =>
            {
                opt.SuppressModelStateInvalidFilter = true;
            });

            return services;
        }
    }
}
