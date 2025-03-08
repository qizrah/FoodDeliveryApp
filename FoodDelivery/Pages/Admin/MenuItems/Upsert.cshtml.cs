using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FoodDelivery.Pages.Admin.MenuItems
{
    public class UpsertModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        [BindProperty]
        public MenuItem MenuItem { get; set; }

        public IEnumerable<SelectListItem> CategoryList { get; set; }
        public IEnumerable<SelectListItem> FoodTypeList { get; set; }

        public UpsertModel(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        
        public void OnGet(int? id)
        {
            MenuItem = new MenuItem();

            if (id != null) // edit
            {
                MenuItem = _unitOfWork.MenuItem.Get(u => u.Id == id,true);
                var categories = _unitOfWork.Category.List();
                var foodtypes = _unitOfWork.FoodType.List();

                CategoryList = categories.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
                FoodTypeList = foodtypes.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
                
            }

            if (MenuItem == null)
            {
                MenuItem = new();
            }
        }

        public IActionResult OnPost(int? id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                string webRootPath = _webHostEnvironment.WebRootPath;

                // If a file is uploaded
                if (MenuItem.UploadedFile != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"images\menuitems\");
                    var extension = Path.GetExtension(MenuItem.UploadedFile.FileName);

                    var fullPath = Path.Combine(uploads, fileName + extension);

                    // Save the uploaded file to the specified path
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        MenuItem.UploadedFile.CopyTo(fileStream);
                    }

                    // Update the Image property with the relative path
                    MenuItem.Image = $"/images/menuitems/{fileName}{extension}";
                }

                // Handle insert or update logic
                if (MenuItem.Id == 0)
                {
                    _unitOfWork.MenuItem.Add(MenuItem);
                }
                else
                {
                    var objFromDb = _unitOfWork.MenuItem.Get(u => u.Id == MenuItem.Id, true);

                    if (MenuItem.UploadedFile == null)
                    {
                        // Retain the existing image if no new file is uploaded
                        MenuItem.Image = objFromDb.Image;
                    }
                    else if (!string.IsNullOrEmpty(objFromDb.Image))
                    {
                        // Delete the old file if it exists
                        var oldImagePath = Path.Combine(webRootPath, objFromDb.Image.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    _unitOfWork.MenuItem.Update(MenuItem);
                }

                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                // Log the error (optional)
                // Handle exceptions as needed
            }

            return RedirectToPage("./Index");
        }

    }
}
