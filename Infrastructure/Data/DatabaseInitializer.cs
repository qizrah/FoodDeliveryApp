using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationCore.Models;
using ApplicationCore.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class DatabaseInitializer
    {
        private readonly ApplicationDbContext _context;

        public DatabaseInitializer(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            if (!_context.Category.Any())
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
                    _context.Category.Add(category);
                }
                _context.SaveChanges();
            }

            if (!_context.FoodType.Any())
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
                    _context.FoodType.Add(foodType);
                }
                _context.SaveChanges();
            }
        }
    }
}
