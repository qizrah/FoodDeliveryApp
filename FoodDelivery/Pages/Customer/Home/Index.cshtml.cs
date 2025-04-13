using ApplicationCore.Models;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FoodDelivery.Pages.Customer.Home
{
    public class IndexModel : PageModel
    {
        private readonly UnitOfWork unitOfWork;

        public IndexModel(UnitOfWork _unitofwork)
        {
            unitOfWork = _unitofwork;
        }

        public List<MenuItem> MenuItemList { get; set; }
        public List<Category > CategoryList { get; set; }

        public void OnGet()
        {
            MenuItemList = (List<MenuItem>)unitOfWork.MenuItem.List();
            CategoryList = (List<Category>)unitOfWork.Category.List();

        }
    }
}
