using AddressBook.Dto;
using AddressBook.Entity;
using AutoMapper;

namespace AddressBook
{
    public static class MapperConfiguration
    {
        public static void Configure()
        {
            Mapper.CreateMap<Entity.Address, AddressDto>();
            Mapper.CreateMap<AddressDto, Entity.Address>()
                .ForMember(d => d.Entry, o => o.Ignore());

            Mapper.CreateMap<Entry, EntryDto>();
            Mapper.CreateMap<EntryDto, Entry>();
        }
    }
}
