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
           
            try {
                var user = db.Set<User>().FirstOrDefault(x => x.Email.Equals(email));

                if (CryptSharp.Crypter.CheckPassword(password, user.Password) == true)
                {
                    return user;
                }

                return null;
            }
            catch { return null; }
        }

        public static User SearchByEmailLogin(string email)
        {
            try { return db.Set<User>().FirstOrDefault(x => x.Email.Equals(email)); }
            catch { return null; }
        }
    }
}