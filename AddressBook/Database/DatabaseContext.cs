using System.Data.Entity;
using AddressBook.Entity;

namespace AddressBook.Database
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Entry> Entries { get; set; }

        public DbSet<Entity.Address> Addresses{ get; set; }
    }
}