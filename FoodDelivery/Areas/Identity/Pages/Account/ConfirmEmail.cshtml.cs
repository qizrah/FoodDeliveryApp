// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Models;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace FoodDelivery.Areas.Identity.Pages.Account
{
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;

        public ConfirmEmailModel(UserManager<IdentityUser> userManager,IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }
        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";
            if (result.Succeeded)
            {
                await GenerateAndSendSignupCouponAsync(user);
            }
            return Page();
        }

        private async Task GenerateAndSendSignupCouponAsync(IdentityUser user)
        {
            var couponCode = $"WELCOME{Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper()}";
            var coupon = new Coupon
            {
                Code = couponCode,
                DiscountAmount = 5.00m,
                ApplicationUserId = user.Id,
                MaxUses = 1,
                Uses = 0,
                ExpirationDate = DateTime.UtcNow.AddDays(14),
                IsOneTimeUse = true
            };

            using (var scope = HttpContext.RequestServices.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Coupon.Add(coupon);
                await dbContext.SaveChangesAsync();
            }

            await _emailSender.SendEmailAsync(user.Email, "Welcome Coupon",
                $"Thank you for signing up! Here's your $5 welcome coupon code: <strong>{couponCode}</strong><br/>" +
                $"It expires in 14 days and is valid for one use only.");
        }

    }
}
