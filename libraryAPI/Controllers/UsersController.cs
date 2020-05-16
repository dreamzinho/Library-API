using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using libraryAPI.Services;
using libraryAPI.Entities;
using System.Linq;

namespace libraryAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody]AuthenticateModel model)
        {
            var user = _userService.Authenticate(model.Username, model.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody]AuthenticateModel model)
        {
            _userService.AddUser(new User { Username = model.Username, Email = model.Email, Password = model.Password });

            if (model.Email == null || model.Username == null || model.Password == null)
                return BadRequest(new { message = "Username or password is incorrect" });
            return Ok();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }
    }
}