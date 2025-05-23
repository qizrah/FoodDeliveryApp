using System.Security.Claims;
using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using ApplicationCore.ViewModels;
using Infrastructure.Data;
using Infrastructure.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FoodDelivery.Pages.Customer.Cart
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork unitOfWork;

        public IndexModel(IUnitOfWork _unitofwork)
        {
            unitOfWork = _unitofwork;
        }

        public OrderDetailsCartVM OrderDetailsCart { get; set; }

        [BindProperty]
        public string CouponCode { get; set; }

        public void OnGet()
        {
            LoadCartData();
        }

        public IActionResult OnPostApplyCoupon()
        {
            LoadCartData();

            if (!string.IsNullOrWhiteSpace(CouponCode))
            {
                var coupon = unitOfWork.Coupon.Get(c => c.Code.ToLower() == CouponCode.ToLower());
                if (coupon != null && (coupon.MaxUses > 0) && (!coupon.ExpirationDate.HasValue || coupon.ExpirationDate > DateTime.Now))
                {
                    if (OrderDetailsCart.OrderHeader.OrderTotal >= Convert.ToDouble(coupon.MinimumOrderAmount))
                    {
                        OrderDetailsCart.OrderHeader.OrderTotal -= Convert.ToDouble(coupon.DiscountAmount);
                        ViewData["CouponMessage"] = $"Coupon '{coupon.Code}' applied. ${coupon.DiscountAmount} off.";
                        HttpContext.Session.SetString("CouponCode", coupon.Code);
                        OrderDetailsCart.OrderHeader.CouponCode = coupon.Code;
                        OrderDetailsCart.OrderHeader.DiscountAmount = Convert.ToDouble(coupon.DiscountAmount);
                    }
                    else
                    {
                        ViewData["CouponMessage"] = $"Minimum order amount is ${coupon.MinimumOrderAmount}.";
                    }
                }
                else
                {
                    ViewData["CouponMessage"] = "Invalid or expired coupon.";
                }
            }

            return Page();
        }

        private void LoadCartData()
        {
            OrderDetailsCart = new OrderDetailsCartVM
            {
                OrderHeader = new ApplicationCore.Models.OrderHeader(),
                ListCart = new List<ShoppingCart>()
            };

            var ClaimsIdentity = User.Identity as ClaimsIdentity;
            var claim = ClaimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                IEnumerable<ShoppingCart> cart = unitOfWork.ShoppingCart.List(c => c.ApplicationUserId == claim.Value);
                if (cart != null)
                {
                    OrderDetailsCart.ListCart = cart.ToList();
                }

                foreach (var cartList in OrderDetailsCart.ListCart)
                {
                    cartList.MenuItem = unitOfWork.MenuItem.Get(m => m.Id == cartList.MenuItemId, includes: "Category,MenuItemFoodTypes.FoodType");
                    OrderDetailsCart.OrderHeader.OrderTotal += (cartList.MenuItem.price * cartList.Count);
                }
            }
        }

        public IActionResult OnPostMinus(int CartId)
        {
            var cart = unitOfWork.ShoppingCart.GetById(CartId);
            if (cart.Count == 1)
            {
                unitOfWork.ShoppingCart.Delete(cart);
            }
            else
            {
                cart.Count -= 1;
                unitOfWork.ShoppingCart.Update(cart);
            }
            unitOfWork.Commit();
            var cnt = unitOfWork.ShoppingCart.List(x => x.ApplicationUserId == cart.ApplicationUserId).Count();
            HttpContext.Session.SetInt32(SD.ShoppingCart, cnt);
            return RedirectToPage("/Customer/Cart/Index");
        }

        public IActionResult OnPostPlus(int CartId)
        {
            var cart = unitOfWork.ShoppingCart.GetById(CartId);
            cart.Count += 1;
            unitOfWork.ShoppingCart.Update(cart);
            unitOfWork.Commit();
            return RedirectToPage("/Customer/Cart/Index");
        }

        public IActionResult OnPostRemove(int CartId)
        {
            var cart = unitOfWork.ShoppingCart.GetById(CartId);
            unitOfWork.ShoppingCart.Delete(cart);
            unitOfWork.Commit();
            var cnt = unitOfWork.ShoppingCart.List(x => x.ApplicationUserId == cart.ApplicationUserId).Count();
            HttpContext.Session.SetInt32(SD.ShoppingCart, cnt);
            return RedirectToPage("/Customer/Cart/Index");
        }
    }
}
