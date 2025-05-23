using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FoodDelivery.Pages.Admin.Coupons
{
    public class UpsertModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpsertModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
        public Coupon Coupon { get; set; }

        public IActionResult OnGet(int? id)
        {
            if (id == null || id == 0)
            {
                Coupon = new Coupon();
            }
            else
            {
                Coupon = _unitOfWork.Coupon.Get(x => x.Id == id);
                if (Coupon == null)
                {
                    return NotFound();
                }
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Coupon.Id == 0)
            {
                _unitOfWork.Coupon.Add(Coupon);
            }
            else
            {
                _unitOfWork.Coupon.Update(Coupon);
            }

            _unitOfWork.Commit();
            return RedirectToPage("./Index");
        }
    }
}
