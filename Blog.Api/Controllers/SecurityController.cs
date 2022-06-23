using Blog.Service;
using Blog.Service.Security;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SecurityController : ControllerBase
    {

        protected readonly UserService UserService;

        public  SecurityController(UserService userService)
        {
            UserService = userService;
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromBody] UserDTO user)
        {
            try
            {
                ServiceResponse<TokenDTO> response = UserService.Login(user);
                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ServiceResponse<object>.Error(ex));
            }
        }

        [HttpPost]
        [Route("Signup")]
        public IActionResult Signup([FromBody] UserDTO user)
        {
            try
            {
                ServiceResponse<UserDTO> response = UserService.Signup(user);
                return StatusCode(StatusCodes.Status201Created, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ServiceResponse<object>.Error(ex));
            }
        }
    }
}