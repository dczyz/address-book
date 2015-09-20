using AddressBook.Login;
using AddressBook.MainView;
using AddressBook.Service;
using AddressBook.SignUp;
using GalaSoft.MvvmLight.Ioc;
using ServiceLocator = Microsoft.Practices.ServiceLocation.ServiceLocator;

namespace AddressBook.Application
{
    class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            var applicationViewModel = new ApplicationViewModel();
            var userService = new UserService();
            var photoService = new PhotoService();
            var addressService = new AddressService();
            SimpleIoc.Default.Register<ApplicationViewModel>();
            SimpleIoc.Default.Register<IUserService>(() => userService);
            SimpleIoc.Default.Register<IAddressService>(() => addressService);
            SimpleIoc.Default.Register<IPhotoService>(() => photoService);
            SimpleIoc.Default.Register(() => new MainViewModel(applicationViewModel, addressService, photoService));
            SimpleIoc.Default.Register(() => new LoginViewModel(applicationViewModel, userService));
            SimpleIoc.Default.Register(() => new SignUpViewModel(applicationViewModel, userService));
        }


        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();

        public LoginViewModel Login => ServiceLocator.Current.GetInstance<LoginViewModel>();

        public SignUpViewModel SignUpViewModel => ServiceLocator.Current.GetInstance<SignUpViewModel>();
    }
}
