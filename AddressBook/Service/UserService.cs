using System.Linq;
using AddressBook.Database;
using AddressBook.Entity;
using AddressBook.Session;

namespace AddressBook.Service
{
    class UserService
    {
        public bool Login(string username, string password)
        {
            using (var db = new DatabaseContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Username == username);
                if (user == null || user.Password != password) return false;
                AppSession.UserId = user.Id;
                return true;
            }
            
        }

        public bool SignUp(string username, string password)
        {
            using (var db = new DatabaseContext())
            {
                if (db.Users.FirstOrDefault(u => u.Username == username) != null)
                {
                    return false;
                }
                db.Users.Add(new User { Username = username, Password = password});
                db.SaveChanges();
                return true;
            }
        }
    }
}
