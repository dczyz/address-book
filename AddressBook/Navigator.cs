using System.Security.Cryptography.X509Certificates;

namespace AddressBook
{
    interface INavigator
    {
        void Navigate(string viewModelName);
    }
}
