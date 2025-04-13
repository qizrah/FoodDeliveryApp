using System.Security.Claims;
using ApplicationCore.Models;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FoodDelivery.Pages.Customer.Home
{
    public class DetailsModel : PageModel
    {
        private readonly UnitOfWork unitOfWork;
        [BindProperty]
        public int txtCount { get; set; } = 1;
        public MenuItem objMenuItem { get; set; }
        public ShoppingCart objCart { get; set; }
        public DetailsModel(UnitOfWork _unitofwork)
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
                objMenuItem = unitOfWork.MenuItem.GetById((int)id);
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
            }
            else
            {
                existingCart.Count += txtCount;
                unitOfWork.ShoppingCart.Update(existingCart);
            }

            unitOfWork.CommitAsync();
            return RedirectToPage("Index");
        }
    }
}
