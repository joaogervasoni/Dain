using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Dain.Models;

namespace Dain.DAL
{
    public class ProductDAO : BaseDAO<Product>
    {
        public static List<Product> ReturnList(int pubId)
        {
            try { return db.Products.Where(x => x.PubId == pubId).ToList(); }
            catch { return null; }
        }

        internal static Product Search(int? id)
        {
            try { return db.Set<Product>().Find(id); }
            catch { return null; }
        }
    }
}