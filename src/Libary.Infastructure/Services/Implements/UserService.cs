using Libary.Infastructure.Services.Interfaces;
using Libary.Infastructure.Uof;
using Library.Domain.BaseClasses;
using Library.Domain.DTOs.User;
using Library.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Libary.Infastructure.Services.Implements
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration configuration;

        public UserService(IUnitOfWork unitOfWork, IConfiguration _configuration)
        {
            _unitOfWork = unitOfWork;
            configuration = _configuration;
        }

        public async Task<ResponseMessage<User>> CreateUser(CreateUserRequest req, CancellationToken cancellationToken)
        {
            var newUser = new User(req);
            var result = await _unitOfWork.UserRepository.Add(newUser, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            var response = ResponseMessage<User>.Success(result);
            return response;
        }

        public async Task<ResponseMessage<LoginResponse>> Login(LoginRequest req, CancellationToken cancellationToken)
        {
            User user = await _unitOfWork.UserRepository.FindFirst(x => x.Email == req.Email);
            if (user is null || !user.VerifyPassword(req.Password)) 
                return ResponseMessage<LoginResponse>.Fail("Email or Password is wrong",401);

            var secretKey = configuration["CustomSettings:Key"];

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.UserData, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email), 
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creeds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expire = DateTime.Now.AddHours(24);
            var token = new JwtSecurityToken(claims: claims, expires: expire, signingCredentials: creeds, notBefore: DateTime.Now);
            var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);

            var response = ResponseMessage<LoginResponse>.Success(new LoginResponse(user.Email, encodedToken, expire));

            return response;
        }
    }
}
