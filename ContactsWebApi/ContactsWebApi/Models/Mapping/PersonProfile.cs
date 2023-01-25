using AutoMapper;
using ContactsWebApi.Models.Dtos;
using ContactsWebApi.Models.Entities;

namespace ContactsWebApi.Models.Mapping
{
    public class PersonProfile : Profile
    {
        public PersonProfile()
        {
            CreateMap<PersonDto, Person>();
        }
    }
}
