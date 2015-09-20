using AddressBook.Entity;
using AddressBook.Model;

namespace AddressBook.Mapper
{
    public static class MapperConfiguration
    {
        public static void Configure()
        {
            AutoMapper.Mapper.CreateMap<Entity.Address, AddressModel>();
            AutoMapper.Mapper.CreateMap<AddressModel, Entity.Address>()
                .ForMember(d => d.Entry, o => o.Ignore());

            AutoMapper.Mapper.CreateMap<Entry, EntryModel>();
            AutoMapper.Mapper.CreateMap<EntryModel, Entry>();
            AutoMapper.Mapper.CreateMap<LoginModel, User>();
        }
    }
}
