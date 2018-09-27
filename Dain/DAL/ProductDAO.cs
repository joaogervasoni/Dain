using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Dain.Models;

namespace Dain.DAL
{
    public class ProductDAO
    {
        private static Context db = SingletonContext.GetInstance();

        public static bool Insert(Product product)
        {
            try
            {
                db.Products.Add(product);
                db.SaveChanges();
                return true;
            }
            catch { return false; }
        }

        public static bool Delete(int id)
        {
            try
            {
                var product = Search(id);
                db.Entry(product).State = EntityState.Deleted;
                db.Products.Remove(product);

                db.SaveChanges();
                return true;
            }
            catch { return false; }
        }

        public static bool Update(Product product)
        {
            try
            {
                var local = db.Products.Local.FirstOrDefault(f => f.Id == product.Id);
                db.Entry(local).State = EntityState.Detached;
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch
            { return false; }
        }

        public static Product Search(int? id)
        {
            try
            {
                return db.Products.Find(id);
            }
            catch { return null; }
        }

        public static List<Product> ReturnList()
        {
            try
            {
                return db.Products.ToList();
            }
            catch { return null; }
        }

        public static List<Product> ReturnList(int PubId)
        {
            try
            {
                return db.Products.Where(x => x.PubId == PubId).ToList();
            }
            catch { return null; }
        }
    }
}