using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FoodDelivery.Pages.Admin.MenuItems
{
    public class ManageFoodTypesModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public ManageFoodTypesModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
        public MenuItem MenuItem { get; set; }

        [BindProperty]
        public List<int> SelectedFoodTypeIds { get; set; } = new();

        public List<FoodType> FoodTypes { get; set; } = new();
        public List<int> AssignedFoodTypeIds { get; set; } = new();

        public void OnGet(int? id)
        {
            MenuItem = _unitOfWork.MenuItem.Get(m => m.Id == id);
            FoodTypes = _unitOfWork.FoodType.List().ToList();
            AssignedFoodTypeIds = _unitOfWork.MenuItemFoodType
                .List(mf => mf.MenuItemId == id)
                .Select(mf => mf.FoodTypeId)
                .ToList();
        }

        public IActionResult OnPost()
        {
            // Clear all existing relations
            var existing = _unitOfWork.MenuItemFoodType.List(m => m.MenuItemId == MenuItem.Id).ToList();
            foreach (var rel in existing)
                _unitOfWork.MenuItemFoodType.Delete(rel);

            // Add selected food types
            foreach (var foodTypeId in SelectedFoodTypeIds)
            {
                _unitOfWork.MenuItemFoodType.Add(new MenuItemFoodType
                {
                    MenuItemId = MenuItem.Id,
                    FoodTypeId = foodTypeId
                });
            }

            _unitOfWork.Commit();
            return RedirectToPage("./Index");
        }
    }
}
