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

        public static T Insert(T entity)
        {
            if (Search(entity.Email, entity.Password) == null)
            {
                var returnEntity = db.Set<T>().Add(entity);
                db.SaveChanges();
                return returnEntity;
            }

            return null;
        }

        public static bool Update(T entity)
        {
            try
            {
                var local = db.Set<T>().Local.FirstOrDefault(f => f.Id == entity.Id);
                db.Entry(local).State = EntityState.Detached;
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch
            { return false; }
        }

        public static void Delete(T entity)
        {
            db.Entry<T>(entity).State = EntityState.Deleted;
            db.Set<T>().Remove(entity);

            db.SaveChanges();
        }

        public static T Search(int id) => db.Set<T>().Find(id);

        public static T Search(string Email, string Password)
        {
            try
            {
                return db.Set<T>().FirstOrDefault(x => x.Email.Equals(Email) && x.Password.Equals(Password));
            }
            catch
            { return null; }
        }

        public static List<T> ReturnList()
        {
            try
            {
                return db.Set<T>().ToList();
            }
            catch
            { return null; }
        }

    }
}