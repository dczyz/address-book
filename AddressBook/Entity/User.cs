using System.Collections.Generic;

namespace AddressBook.Entity
{
    public class User : BaseEntity
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public virtual ICollection<Entry> Entries{ get; set; }
    }
}