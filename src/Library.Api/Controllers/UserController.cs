using Library.Domain.BaseClasses;
using Library.Domain.DTOs;
using Library.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly ILogger<UserController> logger;

        public UserController(ILogger<UserController> _logger)
        {
            logger = _logger;
        }

        [HttpPost]
        [ActionName("CreateUser")]
        [ProducesResponseType(typeof(ResponseMessageNoContent), 200)]
        [ProducesResponseType(typeof(ResponseMessageNoContent), 500)]
        public async Task<ActionResult<ResponseMessage<User>>> CreateUser([FromBody] CreateUserRequest req, CancellationToken cancellationToken)
        {
            try
            { 
                var newUser = new User(req);
                var result = await uof.UserRepository.Add(newUser, cancellationToken);
                await uof.SaveChangesAsync(cancellationToken);
                var response = ResponseMessage<User>.Success(result);
                return Custom(response);
            }
            catch (Exception ex)
            { 
                logger.LogError(ex, ex.Message);
                return BadRequest(ResponseMessage<User>.Fail(ex));
            }
        }
    }
}
