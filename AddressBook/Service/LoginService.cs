using AddressBook.Dao;
using AddressBook.Session;

namespace AddressBook.Service
{
    class LoginService
    {
        private readonly UserDao _userDao;

        public LoginService(UserDao userDao)
        {
            _userDao = userDao;
        }

        public bool Login(string username, string password)
        {
            var user = _userDao.FindByUsername(username);
            if (user == null || user.Password != password) return false;
            AppSession.UserId = user.Id;
            return true;
        }
    }
}
