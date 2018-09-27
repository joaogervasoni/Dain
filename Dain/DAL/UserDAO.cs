using Dain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dain.DAL
{
    public class UserDAO : BaseDAO<User>
    {
        public static User SearchByEmailPassword(string email, string password)
        {
            try { return db.Set<User>().FirstOrDefault(x => x.Email.Equals(email) && x.Password.Equals(password)); }
            catch { return null; }
        }

        public static User SearchByEmailLogin(string email, string login)
        {
            try { return db.Set<User>().FirstOrDefault(x => x.Email.Equals(email) && x.Login.Equals(login)); }
            catch { return null; }
        }
    }
}