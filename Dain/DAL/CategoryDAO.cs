using Dain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dain.DAL
{
    public class CategoryDAO
    {
        private static Context db = SingletonContext.GetInstance();

        public static Category Search(int id)
        {
            try
            {
                return db.Categories.Find(id);
            }
            catch { return null; }
        }

        public static List<Category> ReturnList()
        {
            try
            {
                return db.Categories.ToList();
            }
            catch { return null; }
        }

        public static Category Search(int? Id)
        {
            try
            {
                return db.Categories.Find(Id);
            }
            catch { return null; }
        }
    }
}