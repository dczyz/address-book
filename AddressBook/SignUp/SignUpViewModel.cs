using System;
using System.Data.SqlServerCe;
using System.Linq;
using System.Windows.Input;
using AddressBook.Service;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace AddressBook.SignUp
{
    class SignUpViewModel : ObservableObject, IPageViewModel
    {
        private ICommand _signUpCommand;
        private readonly INavigator _navigator;
        private string _username;
        private string _password;
        private bool _showError;
        private string _confirmPassword;
        private ICommand _backToLoginCommand;
        private string _errorText;

        public SignUpViewModel(INavigator navigator)
        {
            _navigator = navigator;
        }

        public string Name => "SignUp";

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
            if (ServiceLocator.UserService.SignUp(_username, _password))
            {
                //TODO pokaż coś
                return;
            }
            ShowError("User with given username already exists");
        }

        private bool Validate()
        {
            if(Username == null || Username.Length < 6 || Username.Any(char.IsWhiteSpace))
            {
                ShowError("Username must have at least 6 no-whitespace characters");
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
                return _showError;
            }
            set
            {
                _showError = value;
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
            _navigator.Navigate("Login");
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
