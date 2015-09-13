using System.Linq;
using System.Windows;
using System.Windows.Input;
using AddressBook.Login;
using AddressBook.Model;
using AddressBook.Service;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using static System.Windows.MessageBoxButton;
using static System.Windows.MessageBoxImage;

namespace AddressBook.SignUp
{
    class SignUpViewModel : ObservableObject, IPageViewModel
    {
        public static readonly string Name = "SignUp";

        private ICommand _signUpCommand;
        private readonly INavigator _navigator;
        private string _username;
        private string _password;
        private bool _error;
        private string _confirmPassword;
        private ICommand _backToLoginCommand;
        private string _errorText;

        public SignUpViewModel(INavigator navigator)
        {
            _navigator = navigator;
        }

        public void Init()
        {
            Error = false;
        }

        public ICommand SignUpCommand
        {
            get
            {
                _signUpCommand = _signUpCommand ?? new RelayCommand(SignUp);
                return _signUpCommand;
            }
        }

        private void SignUp()
        {
            if (!Validate()) return;
            if (ServiceLocator.UserService.SignUp(new LoginModel(_username, _password)))
            {
                MessageBox.Show("You have been successfully signed up", "Information", OK, Information);
                return;
            }
            ShowError("User with given username already exists");
        }

        private bool Validate()
        {
            if(Username == null || Username.Length < 3 || Username.Any(char.IsWhiteSpace))
            {
                ShowError("Username must have at least 3 no-whitespace characters");
                return false;
            }
            if (Password == null || Password.Length < 6 || Password.Any(char.IsWhiteSpace))
            {
                ShowError("Password must have at least 6 no-whitespace characters");
                return false;
            }
            if (Password != ConfirmPassword)
            {
                ShowError("Passwords don't match");
                return false;
            }
            Error = false;
            return true;
        }

        private void ShowError(string errorText)
        {
            ErrorText = errorText;
            Error = true;
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

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                RaisePropertyChanged(() => Password);
            }
        }

        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set
            {
                _confirmPassword = value;
                RaisePropertyChanged(() => ConfirmPassword);
            }
        }

        public bool Error
        {
            get
            {
                return _error;
            }
            set
            {
                _error = value;
                RaisePropertyChanged(() => Error);
            }
        }

        public ICommand BackToLoginCommand
        {
            get
            {
                _backToLoginCommand = _backToLoginCommand ?? new RelayCommand(BackToLogin);
                return _backToLoginCommand;
            }
        }

        private void BackToLogin()
        {
            _navigator.Navigate(LoginViewModel.Name);
        }

        public string ErrorText
        {
            get { return _errorText; }
            set
            {
                _errorText = value;
                RaisePropertyChanged(() => ErrorText);
            }
        }
    }
}
