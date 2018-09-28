using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Dain.Models
{
    public class Context : DbContext
    {
        public Context() : base("DbDain") { }

        public DbSet<Pub> Pubs { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Rating> Ratings { get; set; }
    }
}