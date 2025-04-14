using System.Security.Claims;
using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Infrastructure.Data;
using Infrastructure.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FoodDelivery.Pages.Customer.Home
{
    public class DetailsModel : PageModel
    {
        private readonly IUnitOfWork unitOfWork;
        [BindProperty]
        public int txtCount { get; set; } = 1;
        public MenuItem objMenuItem { get; set; }
        public ShoppingCart objCart { get; set; }
        public DetailsModel(IUnitOfWork _unitofwork)
        {
            unitOfWork = _unitofwork;
        }

        public IActionResult OnGet(int? id)
        {
            if (id != null)
            {
                var ClaimsIdentity = User.Identity as ClaimsIdentity;
                var claim = ClaimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                HttpContext.Session.SetString("UserLoggedIn", claim?.Value ?? string.Empty);
                objMenuItem = unitOfWork.MenuItem.Get(m => m.Id == id, includes: "Category,FoodType");
                return Page();
            }
            else
            {
                return NotFound();
            }
        }

        public IActionResult OnPost(MenuItem objMenuItem)
        {
            var ClaimsIdentity = User.Identity as ClaimsIdentity;
            var userId = ClaimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var existingCart = unitOfWork.ShoppingCart.Get(u=>u.ApplicationUserId == userId && u.MenuItemId == objMenuItem.Id);

            if (existingCart == null)
            {
                var newCart = new ShoppingCart {
                
                ApplicationUserId=userId,
                MenuItemId = objMenuItem.Id,
                Count= txtCount
                };

                unitOfWork.ShoppingCart.Add(newCart);

                HttpContext.Session.SetInt32(SD.ShoppingCart, txtCount);
            }
            else
            {
                existingCart.Count += txtCount;
                unitOfWork.ShoppingCart.Update(existingCart);

                HttpContext.Session.SetInt32(SD.ShoppingCart, existingCart.Count);
            }
            unitOfWork.CommitAsync();
            return RedirectToPage("Index");
        }
    }
}
