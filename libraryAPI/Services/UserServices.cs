using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using libraryAPI.Entities;
using libraryAPI.Helpers;
using Microsoft.AspNetCore.Identity;
using libraryAPI.DTOs;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace libraryAPI.Services
{
    public interface IUserServices
    {
        Task<User> Authenticate(string username, string password);
        void AddUser(RegisterModelDTO user);
        IEnumerable<User> GetAll();

    }

    public class UserServices : IUserServices
    {
        private readonly AppSettings _appSettings;
        private readonly UserManager<User> _userManager;
        private readonly LibraryDbContext _context;
        

        public UserServices(IOptions<AppSettings> appSettings, UserManager<User> userManager, LibraryDbContext context)
        {
            _context = context;
            _appSettings = appSettings.Value;
            _userManager = userManager;
        }

        public async Task<User> Authenticate(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            // return null if user not found
            if (user == null)
                return null;

            var passwd = await _userManager.CheckPasswordAsync(user, password);
            
            if(passwd == false)
                return null;
            
            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
            return user;
        }

        public IEnumerable<User> GetAll()
        {
            var _users = _userManager.Users.ToList();
            return _users;
        }

        public void AddUser(RegisterModelDTO registerModelDTO)
        {
            var user = new User
            {
                UserName = registerModelDTO.UserName,
                Email = registerModelDTO.Email,
            };

            IdentityResult result = _userManager.CreateAsync(user, registerModelDTO.Password).Result;

            if (result.Succeeded)
            {
                _userManager.AddToRoleAsync(user, "User").Wait();
            }
        }

        
    }
}