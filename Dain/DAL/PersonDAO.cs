using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dain.Models;

namespace Dain.DAL
{
    public class PersonDAO : BaseDAO<Person>
    {
        public static Person SearchByUserId(int userId)
        {
            try { return db.Set<Person>().FirstOrDefault(x => x.UserId == userId); }
            catch { return null; }
        }
    }
}