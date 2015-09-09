namespace AddressBook.Service
{
    static class ServiceLocator
    {
        public static UserService UserService = new UserService();

        public static AddressService AddressService = new AddressService();

        public static PhotoService PhotoService = new PhotoService();
    }
}
