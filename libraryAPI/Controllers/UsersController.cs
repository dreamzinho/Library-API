using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using libraryAPI.Services;
using libraryAPI.Entities;
using System.Linq;
using libraryAPI.DTOs;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace libraryAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserServices _userService;

        public UsersController(IUserServices userService)
        {
            _userService = userService;
        }


        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginModelDTO model)
        {
            var user = await _userService.Authenticate(model.UserName, model.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody]RegisterModelDTO model)
        {
            if (model.UserName == null || model.Email == null || model.Password == null)
                return BadRequest(new { message = "Username or password is incorrect" });
            _userService.AddUser(model);
            
            return Ok();
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }
    }
}