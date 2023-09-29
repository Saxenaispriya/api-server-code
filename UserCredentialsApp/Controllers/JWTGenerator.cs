using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using UserCredentialsApp.Models;

namespace UserCredentialsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        IConfiguration configuration;
        public AuthController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Auth([FromBody] User user)
        {
            IActionResult response = Unauthorized();

            if (user != null)
            {
                if ((user.UserName.Equals("ravi@gmail.com") || user.UserName.Equals("priya@gmail.com")) && user.Password.Equals("a"))
                {
                    var issuer = configuration["Jwt:Issuer"];
                    var audience = configuration["Jwt:Audience"];
                    var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]);
                    var signingCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha512Signature
                    );

                    List<Claim> subjects = new List<Claim>();
                    subjects.Add(new Claim(JwtRegisteredClaimNames.Sub, user.UserName));
                    subjects.Add(new Claim(JwtRegisteredClaimNames.Email, user.UserName));

                    if (user.UserName.Equals("ravi@gmail.com"))
                    {
                        subjects.Add(new Claim(ClaimTypes.Role,"Administrator"));
                    } else
                    {
                        subjects.Add(new Claim(ClaimTypes.Role, "User"));
                    }

                    var subject = new ClaimsIdentity(subjects);

                    var expires = DateTime.UtcNow.AddMinutes(10);

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = subject,
                        Expires = expires,
                        Issuer = issuer,
                        Audience = audience,
                        SigningCredentials = signingCredentials
                    };

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var jwtToken = tokenHandler.WriteToken(token);

                    return Ok(jwtToken);
                }
            }
           
            

            return response;
        }
    }
}
