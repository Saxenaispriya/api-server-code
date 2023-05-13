using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml;
using UserCredentialsApp.Models;

namespace UserCredentialsApp.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {

        private DatabaseContext  dbContext;
        //Guid newGuid = Guid.NewGuid();
        
     

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

            var opt = new JsonSerializerOptions() { WriteIndented = true };
            string strJson = JsonSerializer.Serialize(user, opt);

           
            return Ok(strJson);

        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserRegister userreg)
        {
            if (userreg == null)
            {
                return BadRequest("Invalid user registration data.");
            }
            // userreg.id = new Guid();

            //  var usersavedata = dbContext.userRegisters.Add(userreg);

            // var opt = new JsonSerializerOptions() { WriteIndented = true };
            // string strJson = JsonSerializer.Serialize(usersavedata, opt);
            //// dbContext.userRegisters.Add(userreg);
            // await dbContext.SaveChangesAsync();
            // return Ok(strJson);

            userreg.id = new Guid();
            dbContext.userRegisters.Add(userreg);
            await dbContext.SaveChangesAsync();

            var opt = new JsonSerializerOptions() { WriteIndented = true };
            string strJson = JsonSerializer.Serialize(userreg, opt);
            return Ok(strJson);

        }
    }
}
