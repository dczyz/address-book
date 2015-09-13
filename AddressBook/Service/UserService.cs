using System.Linq;
using AddressBook.Database;
using AddressBook.Entity;
using AddressBook.Model;
using AddressBook.Session;
using static AutoMapper.Mapper;

namespace AddressBook.Service
{
    class UserService
    {
        public bool Login(LoginModel loginModel)
        {
            using (var db = new DatabaseContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Username == loginModel.Username);
                if (user == null || user.Password != loginModel.Password) return false;
                AppSession.UserId = user.Id;
                return true;
            }
            
        }

        public bool SignUp(LoginModel loginModel)
        {
            using (var db = new DatabaseContext())
            {
                if (db.Users.FirstOrDefault(u => u.Username == loginModel.Username) != null)
                {
                    return false;
                }
                db.Users.Add(Map<User>(loginModel));
                db.SaveChanges();
                return true;
            }
        }
    }
}
