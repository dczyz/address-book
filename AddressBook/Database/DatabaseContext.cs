using System.Data.Entity;
using AddressBook.Entity;

namespace AddressBook.Database
{
    public class DatabaseContext : DbContext
    {
         public DbSet<User> Users { get; set; }
    }
}