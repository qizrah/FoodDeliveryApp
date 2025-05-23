using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public UnitOfWork(ApplicationDbContext dbContext)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        {
            _dbContext = dbContext;
        }

        private IGenericRepository<Category> _Category;
        private IGenericRepository<FoodType> _FoodType;
        private IGenericRepository<MenuItem> _MenuItem;
        private IGenericRepository<ApplicationUser> _ApplicationUser;
        private IGenericRepository<OrderHeader> _OrderHeader;
        private IGenericRepository<OrderDetails> _OrderDetails;
        private IGenericRepository<ShoppingCart> _ShoppingCart;
        private IGenericRepository<MenuItemFoodType> _MenuItemFoodType;
        private IGenericRepository<Coupon> _Coupon;

        public IGenericRepository<Category> Category
        {
            get {
                if (_Category == null)
                {
                    _Category = new GenericRepository<Category>(_dbContext);
                }
                return _Category;
            }
        }

        public IGenericRepository<FoodType> FoodType
        {
            get
            {
                if (_FoodType == null)
                {
                    _FoodType = new GenericRepository<FoodType>(_dbContext);
                }
                return _FoodType;
            }
        }
        public IGenericRepository<MenuItem> MenuItem
        {
            get
            {
                if (_MenuItem == null)
                {
                    _MenuItem = new GenericRepository<MenuItem>(_dbContext);
                }
                return _MenuItem;
            }
        }
        
        public IGenericRepository<ApplicationUser> ApplicationUser
        {
            get
            {
                if (_ApplicationUser == null)
                {
                    _ApplicationUser = new GenericRepository<ApplicationUser>(_dbContext);
                }
                return _ApplicationUser;
            }
        }
        public IGenericRepository<OrderDetails> OrderDetails
        {
            get
            {
                if (_OrderDetails == null)
                {
                    _OrderDetails = new GenericRepository<OrderDetails>(_dbContext);
                }
                return _OrderDetails;
            }
        }
        public IGenericRepository<OrderHeader> OrderHeader
        {
            get
            {
                if (_OrderHeader == null)
                {
                    _OrderHeader = new GenericRepository<OrderHeader>(_dbContext);
                }
                return _OrderHeader;
            }
        }
        public IGenericRepository<ShoppingCart> ShoppingCart
        {
            get
            {
                if (_ShoppingCart == null)
                {
                    _ShoppingCart = new GenericRepository<ShoppingCart>(_dbContext);
                }
                return _ShoppingCart;
            }
        }
        
        public IGenericRepository<MenuItemFoodType> MenuItemFoodType
        {
            get
            {
                if (_MenuItemFoodType == null)
                {
                    _MenuItemFoodType = new GenericRepository<MenuItemFoodType>(_dbContext);
                }
                return _MenuItemFoodType;
            }
        }
        
        public IGenericRepository<Coupon> Coupon
        {
            get
            {
                if (_Coupon == null)
                {
                    _Coupon = new GenericRepository<Coupon>(_dbContext);
                }
                return _Coupon;
            }
        }

        public int Commit()
        {
            return _dbContext.SaveChanges();
        }

        public async Task<int> CommitAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
