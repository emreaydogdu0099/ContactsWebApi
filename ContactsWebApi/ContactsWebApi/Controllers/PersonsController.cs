using AutoMapper;
using ContactsWebApi.Models.Context;
using ContactsWebApi.Models.Dtos;
using ContactsWebApi.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using System;

namespace ContactsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly ContactDbContext _context;
        private readonly IMapper _mapper;

        public PersonsController(ContactDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpPost]
        public async Task<IActionResult> Post(PersonDto personDto)
        {
            Person person = _mapper.Map<Person>(personDto);
                
            await _context.Persons.AddAsync(person);
            await _context.SaveChangesAsync();

            RemoveCache<List<Person>>("people");
            return Ok("İşlem Başarılı");
        }

        [HttpPut("{id}")] // localhost:4200/Persons/1
        public async Task<IActionResult> Put(int id, PersonDto personDto)
        {
            Person person = await _context.Persons.FindAsync(id);
            person.Company = personDto.Company;
            person.FirstName = personDto.FirstName;
            person.LastName = personDto.LastName;

            await _context.SaveChangesAsync();

            RemoveCache<List<Person>>("people");
            return Ok("İşlem Başarılı");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Person person = await _context.Persons.FindAsync(id);
            _context.Persons.Remove(person);
            await _context.SaveChangesAsync();

            RemoveCache<List<Person>>("people");
            return Ok("İşlem Başarılı");
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<Person> people = GetCache<List<Person>>("people");
            if (people == null)
            {
                people = await _context.Persons.Include(p => p.ContactInfos).ToListAsync();
                SetCache<List<Person>>("people", people);
            }

            return Ok(people);
        }

        T GetCache<T>(string key)
        {
            var redisclient = new RedisClient("localhost", 6379);
            IRedisTypedClient<T> persons = redisclient.As<T>();

            return redisclient.Get<T>(key);
        }
        void SetCache<T>(string key, T value)
        {
            var redisclient = new RedisClient("localhost", 6379);
            IRedisTypedClient<List<Person>> persons = redisclient.As<List<Person>>();

            redisclient.Set<T>(key, value);
        }
        void RemoveCache<T>(string key)
        {
            var redisclient = new RedisClient("localhost", 6379);
            IRedisTypedClient<T> persons = redisclient.As<T>();

            redisclient.Remove(key);
        }
    }
}
