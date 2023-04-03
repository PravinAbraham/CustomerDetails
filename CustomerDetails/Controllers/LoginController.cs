using CustomerDetails.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomerDetails.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly CustomerDbContext _context;
        public LoginController(CustomerDbContext context)
        {
            _context = context;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _context.login.ToListAsync();
            return Ok(response);
        }
    }
}
