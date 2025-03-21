using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FoodDelivery.Pages.Admin.Users
{
    public class UpdateModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UpdateModel(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [BindProperty]
        public ApplicationUser AppUser { get; set; }
        public List<string> UsersRoles { get; set; }
        public List<string> AllRoles { get; set; }
        public List<string> OldRoles { get; set; }
        public async Task OnGetAsync(string id)
        {
            AppUser = _unitOfWork.ApplicationUser.Get(u=>u.Id== id);
            var roles = await _userManager.GetRolesAsync(AppUser);
            UsersRoles = roles.ToList();
            OldRoles = roles.ToList();
            AllRoles = _roleManager.Roles.Select(r => r.Name).ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var newRoles = Request.Form["roles"];
            UsersRoles = newRoles.ToList();
            var oldRoles = await _userManager.GetRolesAsync(AppUser);
            OldRoles = oldRoles.ToList();
            var rolesToAdd = new List<String>();

            var user = _unitOfWork.ApplicationUser.Get(u => u.Id == AppUser.Id);
            user.FirstName = AppUser.FirstName;
            user.LastName = AppUser.LastName;
            user.Email = AppUser.Email;
            user.PhoneNumber
                = AppUser.PhoneNumber;

            _unitOfWork.ApplicationUser.Update(user);
            _unitOfWork.CommitAsync();

            foreach (var role in newRoles)
            {
                if (!OldRoles.Contains(role))
                {
                    rolesToAdd.Add(role);
                }

            }

            foreach (var role in OldRoles)
            {
                if (!UsersRoles.Contains(role))
                {
                    var result = await _userManager.RemoveFromRoleAsync(user,role);
                }

            }

            var result1 = await _userManager.AddToRolesAsync(user, rolesToAdd.AsEnumerable());
            return RedirectToPage("./Index", new { success = true, message = "Update Successful!" });
            
        }
    }
}
