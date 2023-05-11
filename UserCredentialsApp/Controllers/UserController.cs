using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserCredentialsApp.Models;

namespace UserCredentialsApp.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {

        private DatabaseContext  dbContext;

        public UserController(DatabaseContext _dbcontext)
        {
            this.dbContext = _dbcontext;
        }


        [HttpGet]
        public IActionResult GetUser(string username)
        {
            var user = dbContext.userRegisters.FirstOrDefault(u => u.username == username);
           

            if (user == null)
            {
                return NotFound();
            }
            return Ok(user.Password);

        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserRegister userreg)
        {
            dbContext.userRegisters.Add(userreg);
            await dbContext.SaveChangesAsync();
            return Ok();

        }
    }
}
