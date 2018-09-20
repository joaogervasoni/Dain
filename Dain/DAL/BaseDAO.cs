using Dain.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Dain.DAL
{
    public abstract class BaseDAO<T> where T : User
    {
        private static Context db = SingletonContext.GetInstance();
        public static bool Insert(T entidade)
        {
            try
            {
                db.Set<T>().Add(entidade);
                db.SaveChanges();
                return true;
            }
            catch { return false; }

        }

        public static bool Update(T entidade)
        {
            try
            {
                var local = db.Set<T>().Local.FirstOrDefault(f => f.Id == entidade.Id);
                db.Entry(local).State = EntityState.Detached;
                db.Entry(entidade).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch
            { return false; }
        }

        public static void Delete(T entidade)
        {
            db.Entry<T>(entidade).State = EntityState.Deleted;
            db.Set<T>().Remove(entidade);

            db.SaveChanges();
        }

        public static T Search(int? id)
        {
            int Id = int.Parse(id.ToString());
            return db.Set<T>().Find(id);
        }

        public static T Search(string Email, string Password)
        {
            try
            {
                return db.Set<T>().FirstOrDefault(x => x.Email.Equals(Email) && x.Password.Equals(Password));
            }
            catch
            { return null; }
        }

    }
}