using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppCore.Models
{
    public class Category
    {
        public int id { get; set; }
        public string name { get; set; }
        public string? imagePath { get; set; }
        [NotMapped]
        public IFormFile photo { get; set; }

    }
    public class Product
    {
        public int id { get; set; }
        public string name { get; set; }
        [ForeignKey("categ")]
        public int categID { get; set; }
        public Category ?categ { get; set; }
        public string ?imagePath { get; set; }
        [NotMapped]
        public IFormFile photo { get; set; }
        public float price { get; set; }
        public bool check { get; set; }
    }
    public class Cart
    {
        [Key]
        public int id { get; set; }
        public string userId{ get; set; }
        public string name { get; set; }
        public int pid { get; set; }
        public int qty { get; set; }
        public float price { get; set; }
        public bool check { get; set; }
    }
    public class Inventory
    {
        public int id { get; set; }
        [ForeignKey("product")]
        public int pid { get; set; }
        public Product? product { get; set; }
        [ForeignKey("cid")]
        public int categId { get; set; }
        public Category? cid { get; set; }
        public float qty { get; set; }

    }
    public class Ordered
    {
        [Key] 
        public int Oid { get; set; }
        public string name { get; set; }
        public int qty { get; set; }
        public float price { get; set; }
        public float totalPrice { get; set; }
        public string userid { get; set; }

    }
   
  public class ProductSold
   {
        public int id { get; set; }
        public string name { get; set; } 
        public float price { get; set; } 
        public string cateoryaname { get; set; } 
        public string userid { get; set; } 
    }








    public class CateringContext : DbContext
    {
        public CateringContext(DbContextOptions<CateringContext> option) : base(option) { }
        public DbSet<Category> categories { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<Cart> carts { get; set; }
        public DbSet<Inventory> inventories { get; set; }
        public DbSet<Ordered> orderes { get; set; }
        public DbSet<ProductSold> productSold { get; set; }
               
    }
}
