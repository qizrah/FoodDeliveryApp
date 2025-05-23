using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FoodDelivery.Pages.Admin.Coupons
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Coupon> Coupons { get; set; }

        public void OnGet()
        {
            Coupons = _unitOfWork.Coupon.List();
        }

        public IActionResult OnPostDelete(int id)
        {
            var coupon = _unitOfWork.Coupon.Get(x => x.Id == id);
            if (coupon != null)
            {
                _unitOfWork.Coupon.Delete(coupon);
                _unitOfWork.Commit();
            }
            return RedirectToPage();
        }
    }
}
