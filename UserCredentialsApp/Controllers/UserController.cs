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

        public UserController(DatabaseContext _dbcontext)
        {
            this.dbContext = _dbcontext;
        }


        [HttpGet]

        public IActionResult GetUser(string username)
        {
            var traceId = HttpContext.Request.Headers["trace-id"];
            Console.WriteLine($"{traceId}" + "The request has been recieved");
            var user = dbContext.userRegisters.FirstOrDefault(u => u.username == username);
               
            if (user == null)
            {
                Error e = new Error($"{traceId}"+ "Username not found", 404);
                return NotFound();
            }

            var opt = new JsonSerializerOptions() { WriteIndented = true };
            string strJson = JsonSerializer.Serialize(user, opt);
            HttpContext.Response.Headers["Trace-id"] = traceId;
            return Ok(strJson);

        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserRegister userreg)
        {
            var traceId = HttpContext.Request.Headers["TraceId"];
            Console.WriteLine($"{traceId}" + "request arrived");
           
            if (userreg == null)
            {
                Console.WriteLine($"{traceId}" + "user not found");
                return BadRequest($"{traceId}" + "Invalid user registration data.");
            }
            userreg.id = new Guid();
            dbContext.userRegisters.Add(userreg);
            await dbContext.SaveChangesAsync();

            var opt = new JsonSerializerOptions() { WriteIndented = true };
            string strJson = JsonSerializer.Serialize(userreg, opt);
            Console.WriteLine($"{traceId}" + "data formatted");
            HttpContext.Response.Headers["Trace-id"] = traceId;
            return Ok(strJson);

        }
    }
}
