using ContactsWebApi.Models.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactsWebApi.Controllers
{   
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly ContactDbContext _context;

        public ReportsController(ContactDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var locations = _context.ContactInfos.GroupBy(c => c.Location).Select(s=>s.Key).ToList();
            var result = from location in locations
                         select new { Location = location,
                         PersonsCount = _context.ContactInfos.Where(c=>c.Location == location).Count(),
                         PhoneNumbersCount = _context.ContactInfos.Where(c => c.Location == location && c.PhoneNumber != "").Count()
                         };

            return Ok(result.OrderBy(p=>p.PersonsCount).ToList());
        }
    }
}
