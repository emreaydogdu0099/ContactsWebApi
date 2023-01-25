using ContactsWebApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContactsWebApi.Models.Context
{
    public class ContactDbContext : DbContext
    {
        public ContactDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Person> Persons { get; set; }
        public DbSet<ContactInfo> ContactInfos { get; set; }
    }
}
