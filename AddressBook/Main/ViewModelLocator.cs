using AddressBook.Login;
using AddressBook.MainView;
using AddressBook.Service;
using AddressBook.SignUp;
using GalaSoft.MvvmLight.Ioc;
using ServiceLocator = Microsoft.Practices.ServiceLocation.ServiceLocator;

namespace AddressBook.Main
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
            SimpleIoc.Default.Register(() => applicationViewModel);
            SimpleIoc.Default.Register<IUserService>(() => userService);
            SimpleIoc.Default.Register<IAddressService>(() => addressService);
            SimpleIoc.Default.Register<IPhotoService>(() => photoService);
            SimpleIoc.Default.Register(() => new MainViewModel(applicationViewModel, addressService, photoService));
            SimpleIoc.Default.Register(() => new LoginViewModel(applicationViewModel, userService));
            SimpleIoc.Default.Register(() => new SignUpViewModel(applicationViewModel, userService));
        }


        public static ApplicationViewModel ApplicationViewModel => ServiceLocator.Current.GetInstance<ApplicationViewModel>();

        public static MainViewModel MainViewModel => ServiceLocator.Current.GetInstance<MainViewModel>();

        public static LoginViewModel LoginViewModel => ServiceLocator.Current.GetInstance<LoginViewModel>();

        public static SignUpViewModel SignUpViewModel => ServiceLocator.Current.GetInstance<SignUpViewModel>();
    }
}
