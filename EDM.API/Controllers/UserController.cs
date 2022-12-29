using EDM.Data;
using EDM.Data.Models;
using EDM.Models;
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
        private readonly ApiDbContext dbContext;

        public UserController(ApiDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(dbContext.Users.ToList());
        }

        [HttpGet("Get/{userId}")]
        public IActionResult GetById(int userId)
        {
            var user = this.dbContext.Users.FirstOrDefault(x => x.UserId == userId);
            if (user is null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost("Create")]
        public IActionResult Create([FromBody] UserDataModel user)
        {
            var dbUser = new User()
            {
                UserName = user.UserName,
                Password = user.Password,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = 1
            };
            this.dbContext.Users.Add(dbUser);
            this.dbContext.SaveChanges();

            return Ok(dbUser);
        }

        [HttpPut("Update/{userId}")]
        public IActionResult Update(int userId, [FromBody] UserDataModel user)
        {
            var dbUser = this.dbContext.Users.First(x => x.UserId == userId);
            dbUser.FirstName = user.FirstName;
            dbUser.LastName = user.LastName;
            dbUser.Email = user.Email;
            dbUser.Phone = user.Phone;
            dbUser.LastModifiedDate = DateTime.UtcNow;

            this.dbContext.SaveChanges();

            return Ok();
        }

        [HttpDelete("Delete/{userId}")]
        public IActionResult Delete(int userId)
        {
            var dbUser = this.dbContext.Users.FirstOrDefault(x => x.UserId == userId);
            if (dbUser is null)
            {
                return NotFound();
            }

            this.dbContext.Remove(dbUser);
            return Ok();
        }
    }
}
