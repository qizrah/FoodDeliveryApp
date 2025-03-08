using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FoodDelivery.Pages.Admin.FoodTypes
{
    public class UpsertModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public FoodType FoodTypeObj { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public UpsertModel(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        public IActionResult OnGet(int? id)
        {
            FoodTypeObj = new FoodType();

            if (id != 0) // edit
            {
                FoodTypeObj = _unitOfWork.FoodType.Get(u => u.Id == id);

                if (FoodTypeObj == null)
                {
                    return NotFound();
                }
            }
            return Page(); //assume insert new mode
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            //If New
            if (FoodTypeObj.Id == 0)
            {
                _unitOfWork.FoodType.Add(FoodTypeObj);
            }
            //existing
            else
            {
                _unitOfWork.FoodType.Update(FoodTypeObj);
            }
            _unitOfWork.Commit();
            return RedirectToPage("./Index");
        }
    }
}
