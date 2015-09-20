using AddressBook.Model;

namespace AddressBook.Service
{
    public interface IUserService
    {
        bool Login(LoginModel loginModel);

        bool SignUp(LoginModel loginModel);
    }
}