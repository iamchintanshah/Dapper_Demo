using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LearnDapper.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public AuthController(IConfiguration config)
        {
            _configuration = config;
        }

        [HttpPost]
        public IActionResult SignIn(string email, string password, string role)
        {
            if (email == "admin@gmail.com" && password == "123")
            {
                return Ok(GenerateJWT(email,role));
            }
            return BadRequest("User not found!");
        }

        /// <summary>
        /// create new JWT token
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        private string GenerateJWT(string email, string role)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Issuer = _configuration.GetValue<string>("Jwt:Issuer"),
                Audience = _configuration.GetValue<string>("Jwt:Audience"),
                Subject = new ClaimsIdentity(new List<Claim>() {
                    new Claim(JwtRegisteredClaimNames.Email, email),
                    new Claim(ClaimTypes.Role, role),
                }),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwt = jwtTokenHandler.WriteToken(token);
            return jwt;
        }
    }
}
