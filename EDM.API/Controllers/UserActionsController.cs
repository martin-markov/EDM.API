using EDM.Data;
using EDM.Data.Models;
using EDM.Models;
using EDM.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EDM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserActionsController : ControllerBase
    {
        private readonly ITokenProvider tokenProvider;
        private readonly IUserService userService;
        public UserActionsController(ITokenProvider tokenProvider, IUserService userService)
        {
            this.tokenProvider = tokenProvider;
            this.userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login([FromBody] UserLoginModel userModel)
        {
            var user = this.userService.GetByUsernameAndPassword(userModel.UserName, userModel.Password);
            if (user is null)
            {
                return BadRequest("Invalid credentials");
            }            
            
            return Ok(this.tokenProvider.GetToken(user));
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register([FromBody] UserDTO userModel)
        {
            var validationResult = this.userService.Validate(userModel);
            if (validationResult.IsValid)
            {
                var createdUser = this.userService.Create(userModel);
                return Ok(createdUser);
            }

            return BadRequest(validationResult.Errors);
        }
    }
}
