using SupermarketPOS.Core.Enums;
using SupermarketPOS.Core.Models;
using BCrypt.Net;

namespace SupermarketPOS.Infrastructure.Data;

/// <summary>
/// چاندنی داتای نموونەیی - Database Seeder
/// </summary>
public static class DbSeeder
{
    public static async Task SeedAsync(SupermarketDbContext context)
    {
        // چاندنی بەکارهێنەری سەرەتایی - Seed default admin user
        if (!context.Users.Any())
        {
            var adminUser = new User
            {
                Username = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                FullName = "بەڕێوەبەری سیستەم",
                Role = UserRole.Admin,
                Phone = "07501234567",
                IsActive = true
            };

            var cashier = new User
            {
                Username = "cashier",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("cashier123"),
                FullName = "کاشێر یەکەم",
                Role = UserRole.Cashier,
                Phone = "07509876543",
                IsActive = true
            };

            context.Users.AddRange(adminUser, cashier);
            await context.SaveChangesAsync();
        }

        // چاندنی پۆلێنەکان - Seed categories
        if (!context.Categories.Any())
        {
            var categories = new List<Category>
            {
                new() { Name = "خواردەمەنی", Description = "خواردنەوە و خواردەمەنی" },
                new() { Name = "خواردنەوە", Description = "خواردنەوە و جووس" },
                new() { Name = "پاکیژەیی", Description = "کەلوپەلی پاکیژەیی" },
                new() { Name = "تەندروستی", Description = "کەلوپەلی تەندروستی" },
                new() { Name = "کەلوپەلی ماڵەوە", Description = "کەلوپەلی بۆ ماڵەوە" },
                new() { Name = "کەلوپەلی منداڵان", Description = "شتی تایبەت بە منداڵان" },
                new() { Name = "سەوزە و میوە", Description = "سەوزە و میوەی تازە" },
                new() { Name = "گۆشت و ماسی", Description = "گۆشت و ماسی" }
            };

            context.Categories.AddRange(categories);
            await context.SaveChangesAsync();
        }

        // چاندنی دابینکەران - Seed suppliers
        if (!context.Suppliers.Any())
        {
            var suppliers = new List<Supplier>
            {
                new() { Name = "کۆمپانیای نان و خواردەمەنی", Phone = "07701234567", Email = "info@food.krd", Address = "هەولێر" },
                new() { Name = "کۆمپانیای خواردنەوە", Phone = "07709876543", Email = "info@drinks.krd", Address = "سلێمانی" },
                new() { Name = "کۆمپانیای پاکیژەیی", Phone = "07751234567", Email = "info@clean.krd", Address = "دهۆک" }
            };

            context.Suppliers.AddRange(suppliers);
            await context.SaveChangesAsync();
        }

        // چاندنی بەرهەمەکان - Seed products
        if (!context.Products.Any())
        {
            var category1 = context.Categories.First();
            var supplier1 = context.Suppliers.First();

            var products = new List<Product>
            {
                new() { Barcode = "1001", Name = "نانی سپی", PurchasePrice = 500, SellingPrice = 750, StockQuantity = 100, CategoryId = category1.Id, SupplierId = supplier1.Id },
                new() { Barcode = "1002", Name = "شیر", PurchasePrice = 1000, SellingPrice = 1250, StockQuantity = 50, CategoryId = category1.Id, SupplierId = supplier1.Id },
                new() { Barcode = "1003", Name = "پەنیر", PurchasePrice = 2000, SellingPrice = 2500, StockQuantity = 30, CategoryId = category1.Id, SupplierId = supplier1.Id },
                new() { Barcode = "1004", Name = "هێلکە", PurchasePrice = 500, SellingPrice = 750, StockQuantity = 200, CategoryId = category1.Id, SupplierId = supplier1.Id },
                new() { Barcode = "1005", Name = "چای", PurchasePrice = 3000, SellingPrice = 3500, StockQuantity = 40, CategoryId = category1.Id, SupplierId = supplier1.Id },
                new() { Barcode = "1006", Name = "شەکر", PurchasePrice = 1500, SellingPrice = 2000, StockQuantity = 60, CategoryId = category1.Id, SupplierId = supplier1.Id },
                new() { Barcode = "1007", Name = "برنج", PurchasePrice = 4000, SellingPrice = 5000, StockQuantity = 80, CategoryId = category1.Id, SupplierId = supplier1.Id },
                new() { Barcode = "1008", Name = "زەیتی خواردن", PurchasePrice = 5000, SellingPrice = 6000, StockQuantity = 25, CategoryId = category1.Id, SupplierId = supplier1.Id }
            };

            context.Products.AddRange(products);
            await context.SaveChangesAsync();
        }

        // چاندنی کڕیارەکان - Seed customers
        if (!context.Customers.Any())
        {
            var customers = new List<Customer>
            {
                new() { Name = "کڕیاری گشتی", Phone = "0000000000", LoyaltyPoints = 0 },
                new() { Name = "ئاحمەد محەمەد", Phone = "07501111111", LoyaltyPoints = 150, TotalPurchases = 500000 },
                new() { Name = "فاتمە عەلی", Phone = "07502222222", LoyaltyPoints = 250, TotalPurchases = 800000 }
            };

            context.Customers.AddRange(customers);
            await context.SaveChangesAsync();
        }
    }
}
