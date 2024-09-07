using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
namespace Practice
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                List<Product> products = new List<Product>
                {
                        new Product { Id = 1, Name = "Moloko", Price = 45, Category = "Molochka", Description = "Moloko korov`e" },
                        new Product { Id = 2, Name = "Pivo", Price = 55, Category = "Slaboalkohol", Description = "Pivo Svetloe" },
                        new Product { Id = 3, Name = "Kvas", Price = 60, Category = "Napitki", Description = "Kvas Beliy" },
                        new Product { Id = 4, Name = "Hleb", Price = 23, Category = "Hlebobulochye", Description = "Baton Obedenniy" },
                        new Product { Id = 5, Name = "Arbuz", Price = 15, Category = "Frukty/Yagody", Description = "Arbuz Khersonskiy" },
                        new Product { Id = 6, Name = "Luk", Price = 45, Category = "Ovoschi", Description = "Luk Krymskiy" },
                };

                db.Products.AddRange(products);
                db.SaveChanges();
            }
        }
    }

    public class Product
    {
        public Product() : this("name", 0, "category", "description") { }
        public Product(string name, decimal price, string category, string description)
        {
            //Id = id;
            Name = name;
            Price = price;
            Category = category;
            Description = description;
        }
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public string? Category { get; set; }
        public string? Description { get; set; }
    }

    public class ApplicationContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-M496S5I;Database=TESTDBPractice;Trusted_Connection=True; TrustServerCertificate=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /////////////////////////////////////////////////////      PRACTICE           //////////////////////////////////////////////////////////////////////////
            //1
            //modelBuilder.Entity<User>(e =>
            //{
            //    e.HasKey(p => p.Id).HasName("UserId");

            //    e.HasKey(p => new { p.PhoneNumber, p.Passport });

            //    e.Property(p => p.FirstName)
            //          .HasMaxLength(100)
            //          .IsRequired();

            //    e.Property(p => p.FIO).IsRequired(false);

            //    e.ToTable(e => e.HasCheckConstraint("Age", " Age > 0 AND Age < 150"));

            //    e.ToTable(e => e.HasCheckConstraint("JobPosition", " Position IN (0, 1, 2, 3, 4)"));
            //});
            //2
            modelBuilder.Entity<Product>(e =>
            {
                e.HasKey(p => new
                {
                    p.Name,
                    p.Category
                });

                e.Property(p => p.Name).IsRequired().HasMaxLength(100);


                e.ToTable(e => e.HasCheckConstraint("Price", " Price > 0"));

                e.Property(p => p.Category).IsRequired();

                e.Property(p => p.Description).IsRequired();
            });

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            base.OnModelCreating(modelBuilder);
        }
    }
}
