using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using BookStore.Models;

namespace BookStore.DAL
{
    public class BookContext : DbContext
    {
        public BookContext() : base("BookContext")
        {

        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //删除复数表名约定
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }
    }
}