using AddressBook.Entity;
using AddressBook.Model;
using AutoMapper;

namespace AddressBook
{
    public static class MapperConfiguration
    {
        public static void Configure()
        {
            Mapper.CreateMap<Entity.Address, AddressModel>();
            Mapper.CreateMap<AddressModel, Entity.Address>()
                .ForMember(d => d.Entry, o => o.Ignore());

            Mapper.CreateMap<Entry, EntryModel>();
            Mapper.CreateMap<EntryModel, Entry>();
        }
    }
}
