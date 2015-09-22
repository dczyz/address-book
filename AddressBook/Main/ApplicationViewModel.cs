using System.Collections.Generic;
using AddressBook.Login;
using AddressBook.MainView;
using AddressBook.SignUp;
using GalaSoft.MvvmLight;

namespace AddressBook.Main
{
    public class ApplicationViewModel : ObservableObject, INavigator, IPageViewModel
    {
        private IPageViewModel _currentPageViewModel;

        private readonly Dictionary<string, IPageViewModel> _pageViewModels = new Dictionary<string, IPageViewModel>();

        public void Init()
        {
            _pageViewModels.Add(LoginViewModel.Name, ViewModelLocator.LoginViewModel);
            _pageViewModels.Add(SignUpViewModel.Name, ViewModelLocator.SignUpViewModel);
            _pageViewModels.Add(MainViewModel.Name, ViewModelLocator.MainViewModel);
            CurrentPageViewModel = _pageViewModels[LoginViewModel.Name];
        }

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
            var currentPageViewModel = _pageViewModels[viewModelName];
            currentPageViewModel.Init();
            CurrentPageViewModel = currentPageViewModel;
        }
    }
}
