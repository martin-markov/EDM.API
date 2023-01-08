using EDM.Data;
using EDM.Data.Models;
using EDM.Models;
using EDM.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EDM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var users = this.userService.GetAll();
            return Ok(users);
        }

        [HttpGet("Get/{userIdentifier}")]
        public IActionResult GetById(Guid userIdentifier)
        {
            var user = this.userService.GetByGuid(userIdentifier);
            if (user is null)
            {
                return NotFound("User does not exists");
            }
            return Ok(user);
        }

        [HttpPost("Create")]
        public IActionResult Create([FromBody] UserDTO user)
        {
            var validationResult = this.userService.Validate(user);
            if (validationResult.IsValid)
            {
                var createdUser = this.userService.Create(user);
                return Ok(createdUser);
            }

            return BadRequest(validationResult.Errors);
        }

        [HttpPut("Update/{userIdentifier}")]
        public IActionResult Update(Guid userIdentifier, [FromBody] UserDTO user)
        {
            var dbUser = this.userService.GetByGuid(userIdentifier);
            if (dbUser is null)
            {
                return NotFound("User does not exists");
            }
            ValidationResult validationResult = this.userService.Validate(user);
            if (validationResult.IsValid)
            {
                var udpatedUser = this.userService.Update(dbUser.UserId, user);
                return Ok(udpatedUser);
            }

            return BadRequest(validationResult.Errors);
        }

        [HttpDelete("Delete/{userIdentifier}")]
        public IActionResult Delete(Guid userIdentifier)
        {
            var dbUser = this.userService.GetByGuid(userIdentifier);
            if (dbUser is null)
            {
                return NotFound("User does not exists");
            }

            this.userService.Delete(dbUser.UserId);
            return Ok();
        }
    }
}
