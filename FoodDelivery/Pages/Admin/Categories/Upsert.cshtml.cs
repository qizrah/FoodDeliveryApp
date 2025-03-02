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
        public UpsertModel(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public IActionResult OnGet(int? id)
        {
            CategoryObj = new Category();

            if (id != 0) // edit
            {
                CategoryObj = _unitOfWork.Category.GetById(id);

            }

            if (CategoryObj == null)
                return NotFound();

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (CategoryObj.Id == 0)
            {
                _unitOfWork.Category.Add(CategoryObj);
            }
            else
            {
                _unitOfWork.Category.Update(CategoryObj);
            }

            _unitOfWork.Commit();

            return RedirectToPage("./Index");
        }
    }
}
