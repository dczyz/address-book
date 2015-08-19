using System;
using System.Linq;
using AddressBook.Database;
using AddressBook.Entity;

namespace AddressBook.Dao
{
    class UserDao
    {
        public User FindByUsername(string username)
        {
            return new DatabaseContext().Users.FirstOrDefault(u => u.Username == username);
        }
    }
}
