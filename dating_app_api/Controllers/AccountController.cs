using dating_app_api.Data;
using dating_app_api.DTOs;
using dating_app_api.Entities;
using dating_app_api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace dating_app_api.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext context;
        private readonly ITokenService tokenService;

        public AccountController(DataContext context, ITokenService tokenService)
        {
            this.context = context;
            this.tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            if(await this.UserExists(registerDTO.UserName)) return BadRequest("UserName is already in used.");

            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                UserName = registerDTO.UserName,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password)),
                PasswordSalt = hmac.Key
            };

            this.context.Users.Add(user);
            await this.context.SaveChangesAsync();

            return new UserDTO
            {
                UserName = user.UserName,
                Token = this.tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var user = await this.context.Users.SingleOrDefaultAsync(users => users.UserName == loginDTO.UserName);

            if (user == null) return Unauthorized("UserName is invalid.");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));

            for(int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Password is invalid");
            }

            return new UserDTO
            {
                UserName = user.UserName,
                Token = this.tokenService.CreateToken(user)
            };
        }

        private async Task<bool> UserExists(string userName)
        {
            return await this.context.Users.AnyAsync(users => users.UserName == userName);
        }
    }
}
