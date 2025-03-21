using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Infrastructure.Data;
using Infrastructure.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext db, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            _db.Database.EnsureCreated();

            try
            {
                if (_db.Database.GetPendingMigrations().Any())
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception) { }

            if (_db.Users.Any()) return;

            _roleManager.CreateAsync(new IdentityRole(SD.AdminRole)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.DriverRole)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.KitchenRole)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.CustomerRole)).GetAwaiter().GetResult();

            var adminUser = new ApplicationUser
            {
                UserName = "admin@example.com",
                Email = "admin@example.com",
                FirstName = "Admin",
                LastName = "User",
                PhoneNumber = "1234567890"
            };

            _userManager.CreateAsync(adminUser, "Admin123*").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(adminUser, SD.AdminRole).GetAwaiter().GetResult();

            if (!_db.Category.Any())
            {
                var categories = new List<Category>
                {
                    new Category { Name = "Soup", DisplayOrder = 1 },
                    new Category { Name = "Salad", DisplayOrder = 2 },
                    new Category { Name = "Entrees", DisplayOrder = 3 },
                    new Category { Name = "Dessert", DisplayOrder = 4 },
                    new Category { Name = "Beverages", DisplayOrder = 5 }
                };

                foreach (var category in categories)
                {
                    _db.Category.Add(category);
                }
                _db.SaveChanges();
            }

            if (!_db.FoodType.Any())
            {
                var foodTypes = new List<FoodType>
                {
                    new FoodType { Name = "Beef" },
                    new FoodType { Name = "Chicken" },
                    new FoodType { Name = "Veggie" },
                    new FoodType { Name = "Sugar Free" },
                    new FoodType { Name = "Seafood" }
                };

                foreach (var foodType in foodTypes)
                {
                    _db.FoodType.Add(foodType);
                }
                _db.SaveChanges();
            }
        }
    }

}
