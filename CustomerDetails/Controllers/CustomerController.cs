using CustomerDetails.Context;
using CustomerDetails.Filter;
using CustomerDetails.Helpers;
using CustomerDetails.Models;
using CustomerDetails.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomerDetails.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerDbContext _context;
        public CustomerController (CustomerDbContext context)
        {
            _context = context;
        }

        [HttpPost("Create")]
        public ActionResult<Customer> Create([FromBody] Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
            return Ok(customer);
        }

        [HttpPut]
        public ActionResult<Customer> Update([FromBody] Customer customer)
        {
            _context.Customers.Update(customer);
            _context.SaveChanges();
            return Ok(customer);
        }
        [HttpDelete]
        public ActionResult DeleteById(int id)
        {
            var customer = _context.Customers.Find(id);
            _context.Customers.Remove(customer);
            _context.SaveChanges();
            return Ok(customer);
        }
        [HttpGet("GetById{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var customer = await _context.Customers.Where(a => a.Id == id).FirstOrDefaultAsync();
            return Ok(new Response<Customer>(customer));
        }


        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _context.Customers.ToListAsync();
            return Ok(response);
        }

        [HttpGet("Pagination")]
        public async Task<IActionResult> GetAll([FromQuery] PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.Customers
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToListAsync();
            var totalRecords = await _context.Customers.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<Customer>(pagedData, validFilter, totalRecords);
            return Ok(pagedReponse);
        }
    }
}