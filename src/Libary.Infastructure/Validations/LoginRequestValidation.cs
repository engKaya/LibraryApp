using FluentValidation;
using Library.Domain.DTOs.User;

namespace Library.Domain.Validations
{
    public class LoginRequestValidation : AbstractValidator<LoginRequest>
    { 
        public LoginRequestValidation()
        { 
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required"); 
        } 
    }
}
