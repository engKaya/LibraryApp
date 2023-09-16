using Library.Domain.BaseClasses; 
using Library.Domain.DTOs.User;
using Library.Domain.Entities;

namespace Libary.Infastructure.Services.Interfaces
{
    public interface IUserService
    {
        Task<ResponseMessage<User>> CreateUser(CreateUserRequest req, CancellationToken cancellationToken);
        Task<ResponseMessage<LoginResponse>> Login(LoginRequest req, CancellationToken cancellationToken);

    }
}
