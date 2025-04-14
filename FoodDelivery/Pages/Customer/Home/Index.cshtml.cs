using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FoodDelivery.Pages.Customer.Home
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork unitOfWork;

        public IndexModel(IUnitOfWork _unitofwork)
        {
            unitOfWork = _unitofwork;
        }

        public List<MenuItem> MenuItemList { get; set; }
        public List<Category > CategoryList { get; set; }

        public void OnGet()
        {
            MenuItemList = (List<MenuItem>)unitOfWork.MenuItem.List(predicate: null, includes: "Category,FoodType")
        .ToList();
            CategoryList = (List<Category>)unitOfWork.Category.List();

        }
    }
}
