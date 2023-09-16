using FluentValidation;
using Libary.Infastructure.Uof;
using Library.Domain.DTOs.User;

namespace Libary.Infastructure.Validations
{
    public class CreateUserRequestValidation : AbstractValidator<CreateUserRequest>
    {
        private readonly IUnitOfWork uof;

        public CreateUserRequestValidation(IUnitOfWork _uof)
        {
            this.uof = _uof;    
            RuleFor(x => x.FullName).NotEmpty().WithMessage("FullName is required");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
            RuleFor(x => x.Email).Custom((email, context) =>
            {
                var user = uof.UserRepository.FindFirst(x => x.Email == email).GetAwaiter().GetResult();

                if (user is not null)
                {
                    context.AddFailure("Email", "Email Already Exists");
                }
            });
        } 
    }
}
