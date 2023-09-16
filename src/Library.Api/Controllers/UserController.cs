using Libary.Infastructure.Services.Interfaces;
using Library.Domain.BaseClasses; 
using Library.Domain.DTOs.User;
using Library.Domain.Entities;
using Microsoft.AspNetCore.Mvc; 

namespace Library.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly ILogger<UserController> logger;
        private readonly IUserService userService;

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            this.logger = logger;
            this.userService = userService;
        }

        [HttpPost]
        [ActionName("CreateUser")]
        [ProducesResponseType(typeof(ResponseMessageNoContent), 200)]
        [ProducesResponseType(typeof(ResponseMessageNoContent), 500)]
        public async Task<ActionResult<ResponseMessage<User>>> CreateUser([FromBody] CreateUserRequest req, CancellationToken cancellationToken)
        {
            try
            { 
                return Custom(await userService.CreateUser(req,cancellationToken));
            }
            catch (Exception ex)
            { 
                logger.LogError(ex, ex.Message);
                return BadRequest(ResponseMessage<User>.Fail(ex));
            }
        }

        [HttpPost]
        [ActionName("Login")]
        [ProducesResponseType(typeof(ResponseMessage<LoginResponse>), 200)]
        [ProducesResponseType(typeof(ResponseMessage<LoginResponse>), 401)]
        [ProducesResponseType(typeof(ResponseMessage<LoginResponse>), 500)]
        public async Task<ActionResult<ResponseMessage<User>>> Login([FromBody] LoginRequest req, CancellationToken cancellationToken)
        {
            try
            {
                return Custom(await userService.Login(req, cancellationToken));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return BadRequest(ResponseMessage<User>.Fail(ex));
            }
        }
    }
}
