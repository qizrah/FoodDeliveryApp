using ApplicationCore.Models;
using System.Security.Claims;
using ApplicationCore.ViewModels;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Infrastructure.Utilities;
using Stripe;
using ApplicationCore.Interfaces;

namespace FoodDelivery.Pages.Customer.Cart
{
    public class SummaryModel : PageModel
    {
        private readonly IUnitOfWork unitOfWork;
        public SummaryModel(IUnitOfWork _unitofwork)
        {
            unitOfWork = _unitofwork;
        }
        [BindProperty]
        public OrderDetailsCartVM OrderDetailsCart { get; set; }
        public void OnGet()
        {
            OrderDetailsCart = new OrderDetailsCartVM
            {
                OrderHeader = new ApplicationCore.Models.OrderHeader(),
                ListCart = new List<ShoppingCart>()
            };
            OrderDetailsCart.OrderHeader.OrderTotal = 0;
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
                    cartList.MenuItem = unitOfWork.MenuItem.GetById(cartList.MenuItemId);
                    OrderDetailsCart.OrderHeader.OrderTotal += (cartList.MenuItem.price * cartList.Count);
                }
                OrderDetailsCart.OrderHeader.OrderTotal += OrderDetailsCart.OrderHeader.OrderTotal * SD.SalesTaxPercent;
                ApplicationUser applicationUser = unitOfWork.ApplicationUser.Get(c=>c.Id == claim.Value);
                OrderDetailsCart.OrderHeader.DeliveryName = applicationUser.FullName;
                OrderDetailsCart.OrderHeader.PhoneNumber = applicationUser.PhoneNumber;
                OrderDetailsCart.OrderHeader.DeliveryTime = DateTime.Now;
                OrderDetailsCart.OrderHeader.DeliveryDate = DateTime.Now;

            }
        }

        public IActionResult OnPost(string stripeToken)
        {
            var ClaimsIdentity = User.Identity as ClaimsIdentity;
            var claim = ClaimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                OrderDetailsCart.ListCart = unitOfWork.ShoppingCart.List(x=>x.ApplicationUserId == claim.Value).ToList();
                OrderDetailsCart.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
                OrderDetailsCart.OrderHeader.DeliveryDate = DateTime.Now;
                OrderDetailsCart.OrderHeader.ApplicationUserId = claim.Value;
                OrderDetailsCart.OrderHeader.Status = SD.StatusSubmitted;
                OrderDetailsCart.OrderHeader.DeliveryTime = Convert.ToDateTime(OrderDetailsCart.OrderHeader.DeliveryDate.ToShortDateString()+" "+ OrderDetailsCart.OrderHeader.DeliveryTime.ToShortTimeString());

                List<OrderDetails> orderDetailsList = new List<OrderDetails>();
                unitOfWork.OrderHeader.Add(OrderDetailsCart.OrderHeader);
                unitOfWork.Commit();

                foreach (var item in OrderDetailsCart.ListCart)
                {
                    item.MenuItem = unitOfWork.MenuItem.Get(x => x.Id == item.MenuItemId, includes: "Category,FoodType");
                    OrderDetails orderDetails = new OrderDetails
                    {
                        MenuItemId = item.MenuItemId,
                        OrderHeaderId = OrderDetailsCart.OrderHeader.Id,
                        Name = item.MenuItem.Name,
                        Price = item.MenuItem.price,
                        Count = item.Count
                    };

                    OrderDetailsCart.OrderHeader.OrderTotal += (orderDetails.Count * orderDetails.Price) * (1+ SD.SalesTaxPercent);
                    unitOfWork.OrderDetails.Add(orderDetails);
                }

                OrderDetailsCart.OrderHeader.OrderTotal = Convert.ToDouble(String.Format("{0:.##}", OrderDetailsCart.OrderHeader.OrderTotal));

                HttpContext.Session.SetInt32(SD.ShoppingCart,0);
                unitOfWork.Commit();

                //calling stripe payment func
                if (stripeToken != null)
                {
                    var options = new ChargeCreateOptions
                    {
                        Amount = Convert.ToInt32(OrderDetailsCart.OrderHeader.OrderTotal*100),
                        Currency = "usd",
                        Description = "order id: "+ OrderDetailsCart.OrderHeader.Id,
                        Source = stripeToken
                    };

                    var service = new ChargeService();
                    Charge charge = service.Create(options);
                    OrderDetailsCart.OrderHeader.TransactionId = charge.Id;
                    if (charge.Status.ToLower()== "succeeded")
                    {
                        OrderDetailsCart.OrderHeader.PaymentStatus = SD.PaymentStatusApproved;
                    }
                    else
                    {
                        OrderDetailsCart.OrderHeader.PaymentStatus = SD.PaymentStatusRejected;
                    }
                    unitOfWork.Commit();
                    return RedirectToPage("/Customer/Cart/OrderConfirmation",new { id=  OrderDetailsCart.OrderHeader.Id});
                }
                
            }
            return Page();
        }
    }
}
