using System.Windows.Input;
using AddressBook.Main;
using AddressBook.MainView;
using AddressBook.Model;
using AddressBook.Service;
using AddressBook.SignUp;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace AddressBook.Login
{
    public class LoginViewModel : ObservableObject, IPageViewModel
    {
        public static readonly string Name = "LoginViewModel";

        private ICommand _signUpCommand;
        private string _password;
        private ICommand _loginCommand;
        private string _username;
        private bool _error;

        private readonly INavigator _navigator;
        private readonly IUserService _userService;

        public LoginViewModel(INavigator navigator, IUserService userService)
        {
            _navigator = navigator;
            _userService = userService;
        }

        public void Init() { }

        public ICommand SignUpCommand
        {
            get
            {
                _signUpCommand = _signUpCommand ?? new RelayCommand(() => _navigator.Navigate(SignUpViewModel.Name));
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
            if (_userService.Login(new LoginModel(_username, _password)))
            {
                _navigator.Navigate(MainViewModel.Name);
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
