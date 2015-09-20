using System.Collections.Generic;
using AddressBook.Login;
using AddressBook.MainView;
using AddressBook.Service;
using AddressBook.SignUp;
using GalaSoft.MvvmLight;
using ServiceLocator = Microsoft.Practices.ServiceLocation.ServiceLocator;

namespace AddressBook.Application
{
    public class ApplicationViewModel : ObservableObject, INavigator
    {
        private IPageViewModel _currentPageViewModel;

        private Dictionary<string, IPageViewModel> _pageViewModels;

        public ApplicationViewModel()
        {
            var serviceLocator = ServiceLocator.Current;
            var userService = serviceLocator.GetInstance<IUserService>();
            var login = new LoginViewModel(this, userService);
            PageViewModels.Add(LoginViewModel.Name, login);
            var signUp = new SignUpViewModel(this, userService);
            PageViewModels.Add(SignUpViewModel.Name, signUp);
            var main = new MainViewModel(this, serviceLocator.GetInstance<IAddressService>(), serviceLocator.GetInstance<IPhotoService>());
            PageViewModels.Add(MainViewModel.Name, main);

            CurrentPageViewModel = login;
        }

        public Dictionary<string, IPageViewModel> PageViewModels => _pageViewModels ?? (_pageViewModels = new Dictionary<string, IPageViewModel>());

        public IPageViewModel CurrentPageViewModel
        {
            get
            {
                return _currentPageViewModel;
            }
            set
            {
                if (_currentPageViewModel == value) return;
                _currentPageViewModel = value;
                RaisePropertyChanged(() => CurrentPageViewModel);
            }
        }

        public void Navigate(string viewModelName)
        {
            var currentPageViewModel = PageViewModels[viewModelName];
            currentPageViewModel.Init();
            CurrentPageViewModel = currentPageViewModel;
        }
    }
}
