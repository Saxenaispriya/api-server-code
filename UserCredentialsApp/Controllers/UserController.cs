using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml;
using UserCredentialsApp.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
        [Authorize(Roles = "User")]
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
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AddUser([FromBody] UserRegister userreg)
        {
            var traceId = HttpContext.Request.Headers["TraceId"];
            Console.WriteLine($"{traceId}" + "request arrived");
           
            if (userreg == null)
            {
                Console.WriteLine($"{traceId}" + "user not found");
                return BadRequest($"{traceId}" + "Invalid user registration data.");
            }
            userreg.Id = new Guid();
            dbContext.userRegisters.Add(userreg);
            await dbContext.SaveChangesAsync();

            var opt = new JsonSerializerOptions() { WriteIndented = true };
            string strJson = JsonSerializer.Serialize(userreg, opt);
            Console.WriteLine($"{traceId}" + "data formatted");
            HttpContext.Response.Headers["Trace-id"] = traceId;
            return Ok(strJson);
        }


        [HttpPut]
        public async Task<IActionResult> updateUser(Guid id, [FromBody] UserRegister updatedUser)
        {
            var traceId = HttpContext.Request.Headers["TraceId"];
            Console.WriteLine($"{traceId}" + " PUT request arrived");

            if (updatedUser == null)
            {
                Console.WriteLine($"{traceId}" + " User data not provided");
                return BadRequest($"{traceId}" + " Invalid user data.");
            }
            var existingUser = await dbContext.userRegisters.FindAsync(id);

            if (existingUser == null)
            {
                Console.WriteLine($"{traceId}" + " User not found");
                return NotFound($"{traceId}" + " User not found.");
            }

            existingUser.username = updatedUser.username;
            existingUser.Password = updatedUser.Password;

            dbContext.userRegisters.Update(existingUser);
            await dbContext.SaveChangesAsync();

            var res= new JsonSerializerOptions() { WriteIndented = true };
            string strjson = JsonSerializer.Serialize(existingUser,res);
            Console.WriteLine($"{traceId}" + " User data updated");

            HttpContext.Response.Headers["Trace-id"] = traceId;

            return Ok(strjson);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UserUpdatedpatch(Guid id, [FromBody] UserRegister userreg)
        {
            

            var traceId = HttpContext.Request.Headers["TraceId"];

            if (userreg == null)
            {
                Console.WriteLine($"{traceId}" + " Patch request arrived");
                return BadRequest("Patch Request body is empty");
            }

            var patchvar = await dbContext.userRegisters.FindAsync(id);

            if (patchvar == null)
            {
                Console.WriteLine($"{traceId}" + " User not found");
                return NotFound($"{traceId}" + " User not found.");
            }

            if(userreg.username !=null)
            {
                patchvar.username = userreg.username;
            }
            if(userreg.Password != null)
            {
                patchvar.Password = userreg.Password;
            } 
           

            //if(!ModelState.IsValid)
            //{
            //    Console.WriteLine($"{traceId} JSON Patch Errors:");
            //    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            //    {
            //        Console.WriteLine($"{traceId} - {error.ErrorMessage}");
            //    }
            //    return BadRequest("Invalid JSON Patch operations.");

            //}
            // Save changes to the database
            await dbContext.SaveChangesAsync();

            return Ok(patchvar);
        }

    }
}
