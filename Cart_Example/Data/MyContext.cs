using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cart_Example.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cart_Example.Data
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options):base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Seed User

            modelBuilder.Entity<User>().HasData(
                new User()
                {
                    UserId = 1,
                    Username = "SadraBoromand",
                    Email = "sadrabroo@gmail.com",
                    Password = "123"
                }
                );

            #endregion

            #region Seed Product

            modelBuilder.Entity<Product>().HasData(
                new Product()
                {
                    ProductId = 1,
                    Title = "Asp.net Core",
                    Text = "Asp.net Core",
                    Price = 35000,
                    ImageName = "0.jpg"
                },
                new Product()
                {
                    ProductId = 2,
                    Title = "Cource PHP",
                    Text = "Cource PHP",
                    Price = 25000,
                    ImageName = "1.jpg"
                },
                new Product()
                {
                    ProductId = 3,
                    Title = "Android",
                    Text = "Android",
                    Price = 15000,
                    ImageName = "3.jpg"
                },
                new Product()
                {
                    ProductId = 4,
                    Title = "PWA",
                    Text = "PWA",
                    Price = 12000,
                    ImageName = "4.jpg"
                }
            );

            #endregion
        }
    }
}
