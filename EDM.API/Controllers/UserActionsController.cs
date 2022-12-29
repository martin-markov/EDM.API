using EDM.Data;
using EDM.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using EDM.Data.Models;

namespace EDM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserActionsController : ControllerBase
    {
        private readonly ApiDbContext dbContext;
        private readonly IConfiguration config;
        public UserActionsController(ApiDbContext dbContext, IConfiguration config)
        {
            this.dbContext = dbContext;
            this.config = config;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login([FromBody] UserLoginModel userModel)
        {
            var user = Authenticate(userModel);
            if (user != null)
            {
                var token = GenerateToken(user);
                return Ok(token);
            }
            return NotFound();
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register([FromBody] UserRegistrationModel userModel)
        {
            var dbUser = new User()
            {
                UserName = userModel.UserName,
                Password = userModel.Password,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                Email = userModel.Email,
                Phone = userModel.Phone,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = 1
            };
            this.dbContext.Users.Add(dbUser);
            this.dbContext.SaveChanges();
            //mail validation and etc
            return Ok(dbUser);
        }

        private string GenerateToken(UserDataModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:key"]));
            var creadentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserName)
            };

            var token = new JwtSecurityToken(
                config["Jwt:Issuer"],
                config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(20),
                signingCredentials: creadentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UserDataModel Authenticate(UserLoginModel userLogin)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return this.dbContext.Users.Where(x => x.UserName.ToLower() == userLogin.UserName.ToLower() && x.Password == userLogin.Password)
                .Select(x => new UserDataModel()
                {
                    UserName = x.UserName,
                    Password = x.Password
                }).FirstOrDefault();
#pragma warning restore CS8603 // Possible null reference return.
        }
    }
}
