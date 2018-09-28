using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dain.Models;

namespace Dain.DAL
{
    public class RatingDAO : BaseDAO<Rating>
    {
        internal static Rating SearchByPersonAndPubId(int personId, int pubId)
        {
            try { return db.Set<Rating>().FirstOrDefault(x => x.PersonId == personId && x.PubId == pubId); }
            catch { return null; }
        }
    }
}