using ContactsWebApi.Models.Context;
using ContactsWebApi.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactInfosController : ControllerBase
    {
        private readonly ContactDbContext _context;

        public ContactInfosController(ContactDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Post(ContactInfo contactInfo)
        {
            await _context.ContactInfos.AddAsync(contactInfo);
            await _context.SaveChangesAsync();

            Person person = await _context.Persons.Include(p=>p.ContactInfos).FirstOrDefaultAsync(p=> p.Id== contactInfo.Id);

            return Ok(person);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        { 
            ContactInfo contactInfo = await _context.ContactInfos.FindAsync(id);
            _context.ContactInfos.Remove(contactInfo);
            await _context.SaveChangesAsync();

            Person person = await _context.Persons.Include(p => p.ContactInfos).FirstOrDefaultAsync(p => p.Id == contactInfo.Id);

            return Ok(person);
        }
    }
}
