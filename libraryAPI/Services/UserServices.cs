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
using Google.Apis.Auth;

namespace libraryAPI.Services
{
    public interface IUserServices
    {
        Task<User> Authenticate(string username, string password);
        IdentityResult AddUser(RegisterModelDTO user);
        IEnumerable<User> GetAll();
        Task<User> ValidateGoogleUser(string TokenId);
        Task<(IdentityUser,bool)> RemoveUser(string userId);

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

        public IdentityResult AddUser(RegisterModelDTO registerModelDTO)
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

            return result;
        }

        public async Task<(IdentityUser,bool)> RemoveUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) return (null, false);

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded) return (user, false);

            return (user, true);
        }

        public async Task<User> ValidateGoogleUser(string TokenId)
        {
            var CLIENT_ID = "393357815889-jl0jci0rs1ghjvq94lcbgl1rfms0cruv.apps.googleusercontent.com";
            GoogleJsonWebSignature.Payload googleToken = null;
            var settings = new GoogleJsonWebSignature.ValidationSettings() { Audience = new List<string>() { CLIENT_ID } };

            try { googleToken = await GoogleJsonWebSignature.ValidateAsync(TokenId, settings); }
            catch { return null; }
            
            var user = await _userManager.FindByNameAsync(googleToken.Email);
            if (user == null)
            {
                var newUser = new User
                {
                    UserName = googleToken.Email,
                    Email = googleToken.Email
                };
                await _userManager.CreateAsync(newUser);
                _userManager.AddToRoleAsync(newUser, "User").Wait();

                // authentication successful so generate jwt token
                var tokenHandler2 = new JwtSecurityTokenHandler();
                var key2 = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor2 = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, newUser.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key2), SecurityAlgorithms.HmacSha256Signature)
                };
                var token2 = tokenHandler2.CreateToken(tokenDescriptor2);
                newUser.Token = tokenHandler2.WriteToken(token2);
                return newUser;
            }

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
    }
}