using System.Windows.Input;
using AddressBook.Service;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace AddressBook.Login
{
    class LoginViewModel : ObservableObject, IPageViewModel
    {
        private readonly INavigator _navigator;
        private ICommand _signUpCommand;
        private string _password;
        private ICommand _loginCommand;
        private string _username;
        private bool _error;

        public LoginViewModel(INavigator navigator)
        {
            _navigator = navigator;
        }

        public string Name => "Login";

        public void Init()
        {
            
        }

        public ICommand SignUpCommand
        {
            get
            {
                _signUpCommand = _signUpCommand ?? new RelayCommand(() => _navigator.Navigate("SignUp"));
                return _signUpCommand;
            }
        }

        public ICommand LoginCommand
        {
            get
            {
                _loginCommand = _loginCommand ?? new RelayCommand(Login);
                return _loginCommand;
            }
        }

        private void Login()
        {
            if (ServiceLocator.UserService.Login(_username, _password))
            {
                _navigator.Navigate("Main");
                return;
            }
            Error = true;
        }

        public bool Error
        {
            get { return _error; }
            set
            {
                _error = value;
                RaisePropertyChanged(() => Error);
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                RaisePropertyChanged(() => Password);
            }
        }

        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                RaisePropertyChanged(() => Username);
            }
        }
    }
}
