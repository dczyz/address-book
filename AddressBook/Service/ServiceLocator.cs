namespace AddressBook.Service
{
    static class ServiceLocator
    {
        public static readonly UserService UserService = new UserService();

        public static readonly AddressService AddressService = new AddressService();

        public static readonly PhotoService PhotoService = new PhotoService();
    }
}
