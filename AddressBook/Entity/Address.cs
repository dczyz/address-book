namespace AddressBook.Entity
{
    public class Address : BaseEntity
    {
        public string City { get; set; }

        public string Postcode { get; set; }

        public string Street { get; set; }

        public string HomeNumber { get; set; }

        public string FlatNumber { get; set; }

        public string PhoneNumber1 { get; set; }

        public string PhoneNumber2 { get; set; }

        public string FaxNumber { get; set; }

        public virtual Entry Entry { get; set; }
    }
}
