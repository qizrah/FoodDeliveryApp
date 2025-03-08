using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FoodDelivery.Pages.Admin.Categories
{
    public class UpsertModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public Category CategoryObj { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public UpsertModel(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        public IActionResult OnGet(int ? id)
        {
            CategoryObj = new Category();

            if (id != 0) // edit
            {
                CategoryObj = _unitOfWork.Category.Get(u=> u.Id == id);
            
                if (CategoryObj == null)
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
            if (CategoryObj.Id == 0)
            {
                _unitOfWork.Category.Add(CategoryObj);
            }
            //existing
            else
            {
                _unitOfWork.Category.Update(CategoryObj);
            }
            _unitOfWork.Commit();
            return RedirectToPage("./Index");
        }
    }
}
