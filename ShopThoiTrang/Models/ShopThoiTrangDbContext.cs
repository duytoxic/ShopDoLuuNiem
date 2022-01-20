using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ShopThoiTrang.Models
{
    public class ShopThoiTrangDbContext:DbContext
    {
        public ShopThoiTrangDbContext():base("name=ChuoiKN"){}
        public DbSet<Category> Categorys { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Orderdetail> Orderdetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}