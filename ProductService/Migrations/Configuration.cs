using ProductService.Models;
using System;
using System.Data.Entity.Migrations;

namespace ProductService.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ProductServiceContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ProductServiceContext context)
        {
            var samsungGuid = new Guid("8f2e9176-35ee-4f0a-ae55-83023d2db1a3");
            var appleGuid = new Guid("de1287c0-4b15-4a7b-9d8a-dd21b3cafec3");

            context.Products.AddOrUpdate(x => x.Id,
                new Product() 
                { 
                    Id = samsungGuid,
                    Name = "Samsung Galaxy S7", 
                    Description = "Newest mobile product from Samsung.", 
                    Price = 1024.99M, 
                    DeliveryPrice = 16.99M 
                },
                new Product()
                { 
                    Id = appleGuid,
                    Name = "Apple iPhone 6", 
                    Description = "Newest mobile product from Apple.", 
                    Price = 1299.99M, 
                    DeliveryPrice = 15.99M 
                }
            );

            
            context.ProductOptions.AddOrUpdate(x => x.Id,
                new ProductOption()
                {
                    Id = new Guid("0643ccf0-ab00-4862-b3c5-40e2731abcc9"),
                    Name = "White", 
                    Description = "White Samsung Galaxy S7",
                    ProductId = samsungGuid, 
                },
                new ProductOption() {
                    Id = new Guid("a21d5777-a655-4020-b431-624bb331e9a2"),
                    Name = "Black", 
                    Description = "Black Samsung Galaxy S7",
                    ProductId = samsungGuid, 
                },
                new ProductOption() { 
                    Id = new Guid("5c2996ab-54ad-4999-92d2-89245682d534"),
                    Name = "Rose Gold", 
                    Description = "Gold Apple iPhone 6S", 
                    ProductId = appleGuid, 
                },
                new ProductOption() { 
                    Id = new Guid("9ae6f477-a010-4ec9-b6a8-92a85d6c5f03"),
                    Name = "White", 
                    Description = "White Apple iPhone 6S",
                    ProductId = appleGuid, 
                },
                new ProductOption() { 
                    Id = new Guid("4e2bc5f2-699a-4c42-802e-ce4b4d2ac0ef"),
                    Name = "Black",
                    Description = "Black Apple iPhone 6S",
                    ProductId = appleGuid,
                }
            );
            
        }
    }
}