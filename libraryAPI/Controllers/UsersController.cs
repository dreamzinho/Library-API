using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using libraryAPI.Services;
using libraryAPI.Entities;
using System.Linq;
using libraryAPI.DTOs;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace libraryAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserServices _userService;
        private readonly LibraryDbContext _context;

        public UsersController(IUserServices userService, LibraryDbContext context)
        {
            _userService = userService;
            _context = context;
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
            if (_context.Users.Any(u => u.UserName == model.UserName || u.Email == model.Email))
                return BadRequest(new { message = "Username with that email or username already exists" });

            if (model.UserName == null || model.Email == null || model.Password == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var isAdded = _userService.AddUser(model);

            if (!isAdded.Succeeded) return BadRequest(new { message = "Invalid Username or Password" });

            return Ok();
        }

        [HttpDelete]
        [AllowAnonymous]
        public IActionResult RemoveUser([FromQuery(Name = "userId")] string userId)
        {
            if (userId == null) return BadRequest();


            var result = _userService.RemoveUser(userId);
             
            if (result.Result.Item1 == null) return NotFound();

            return Ok(result.Result.Item2);
        }

        //[Authorize(Roles = "Admin")]
        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }
        [AllowAnonymous]
        [HttpGet("googleRegister")]
        public async Task<IActionResult> ValidateGoogleUser(string tokenId)
        {
            var response = await _userService.ValidateGoogleUser(tokenId);
            if (response == null) return BadRequest();
            return Ok(response);
        }
    }
}