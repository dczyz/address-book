using System;
using System.Collections.Generic;

namespace AddressBook.Model
{
    public class EntryModel
    {
        public EntryModel()
        {
            Addresses = new List<AddressModel>();
        }

        public int? Id { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public DateTime Birthday { get; set; }

        public string PhoneNumber1 { get; set; }

        public string PhoneNumber2 { get; set; }

        public ICollection<AddressModel> Addresses { get; set; }

        public override string ToString()
        {
            return $"{SecondName} {FirstName}";
        }
    }
}
