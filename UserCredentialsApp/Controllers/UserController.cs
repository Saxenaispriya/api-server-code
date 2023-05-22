using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml;
using UserCredentialsApp.Models;

namespace UserCredentialsApp.Controllers
{
    class Error
    {
        public string message;
        public int statusCode;

        public Error(string message, int statusCode)
        {
            this.message = message;
            this.statusCode = statusCode;
        }

        public string getErrorString(Error e)
        {
            var opt = new JsonSerializerOptions() { WriteIndented = true };
            string strJson = JsonSerializer.Serialize(e, opt);
            Console.WriteLine(strJson);
            return strJson;
        }
    }

    [Route("api/[controller]")]
    public class UserController : Controller
    {

        private DatabaseContext  dbContext;
        //Guid newGuid = Guid.NewGuid();
        
     

        public UserController(DatabaseContext _dbcontext)
        {
            this.dbContext = _dbcontext;
        }

        
        
        //this method for user's get information

        [HttpGet]

        public IActionResult GetUser(string username)
        {
            Trace.Write("here user is :"+username);
            var user = dbContext.userRegisters.FirstOrDefault(u => u.username == username);
               
            if (user == null)
            {
                Error e = new Error("Username not found", 404);
                return NotFound();
            }

            var opt = new JsonSerializerOptions() { WriteIndented = true };
            string strJson = JsonSerializer.Serialize(user, opt);
            return Ok(strJson);

        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserRegister userreg)
        {
            Console.WriteLine("request arrived");
           
            if (userreg == null)
            {
                Console.WriteLine("user not found");
                return BadRequest("Invalid user registration data.");
            }
            userreg.id = new Guid();
            dbContext.userRegisters.Add(userreg);
            await dbContext.SaveChangesAsync();

            var opt = new JsonSerializerOptions() { WriteIndented = true };
            string strJson = JsonSerializer.Serialize(userreg, opt);
            Console.WriteLine("data formatted");
            return Ok(strJson);

        }
    }
}
