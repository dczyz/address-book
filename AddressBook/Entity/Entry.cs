using System;
using System.Collections.Generic;

namespace AddressBook.Entity
{
    public class Entry : BaseEntity
    {
        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public DateTime Birthday { get; set; }

        public string PhoneNumber1 { get; set; }

        public string PhoneNumber2 { get; set; }

        public int UserId { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }

        public virtual User User { get; set; }
    }
}
