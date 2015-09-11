using System;
using System.Collections.Generic;
using AddressBook.Entity;

namespace AddressBook.Dto
{
    class EntryDto
    {
        public EntryDto()
        {
            Addresses = new List<AddressDto>();
        }

        public int? Id { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public DateTime Birthday { get; set; }

        public string PhoneNumber1 { get; set; }

        public string PhoneNumber2 { get; set; }

        public string PhoneNumber3 { get; set; }

        public ICollection<AddressDto> Addresses { get; set; }

        public override string ToString()
        {
            return $"{SecondName} {FirstName}";
        }
    }
}
