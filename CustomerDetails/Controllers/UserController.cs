using CustomerDetails.Context;
using CustomerDetails.Handler;
using CustomerDetails.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CustomerDetails.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly CustomerDbContext _context;
        private readonly JWTSettings _jwtSettings;
        public UserController(CustomerDbContext context,IOptions<JWTSettings> options)
        {
            _context = context;
            _jwtSettings = options.Value;
        }


        [AllowAnonymous]
        [HttpPost("AdminLogin")]
        public IActionResult Authenticate([FromBody] UserJWT userJWT) 
        {
            var _user = _context.Customers.FirstOrDefault(o => "Admin@123.com" == userJWT.Email && "admin123" == userJWT.password);
            if (_user == null) 
                return Unauthorized();
            
            var tokenhandler = new JwtSecurityTokenHandler(); 
            var tokenkey = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.Name,_user.LastName),
                    }
                ),
                Expires = DateTime.Now.AddMinutes(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenkey),SecurityAlgorithms.HmacSha256)
            };
            var token = tokenhandler.CreateToken(tokenDescriptor);
            string finaltoken = tokenhandler.WriteToken(token);

            return Ok(finaltoken);
        }

        [AllowAnonymous]
        [HttpPost("CustomerLogin")]
        public IActionResult EmpAuthenticate([FromBody] UserJWT userJWT)
        {
            var _user = _context.Customers.FirstOrDefault(o => o.Email == userJWT.Email && o.LastName == userJWT.password);
            if (_user == null)
                return Unauthorized();

            var tokenhandler = new JwtSecurityTokenHandler();
            var tokenkey = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.Name,_user.Contact),
                    }
                ),
                Expires = DateTime.Now.AddMinutes(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenkey), SecurityAlgorithms.HmacSha256)
            };
            var token = tokenhandler.CreateToken(tokenDescriptor);
            string finaltoken = tokenhandler.WriteToken(token);

            return Ok(finaltoken);
        }
    }
}
