using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dain.Models;

namespace Dain.DAL
{
    public class PubDAO : BaseDAO<Pub>
    {
        public static Pub SearchByUserId(int userId)
        {
            try { return db.Set<Pub>().FirstOrDefault(x => x.UserId == userId); }
            catch { return null; }
        }
    }
}