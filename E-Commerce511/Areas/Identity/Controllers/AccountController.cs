using E_Commerce511.Models;
using E_Commerce511.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce511.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if(ModelState.IsValid)
            {
                ApplicationUser applicationUser = new()
                {
                    UserName = registerVM.UserName,
                    Email = registerVM.Email,
                    Address = registerVM.Address
                };

                var result = await userManager.CreateAsync(applicationUser, registerVM.Password);

                if(result.Succeeded)
                {
                    // Success Register
                    return RedirectToAction("Index", "Home", new { area = "Customer" });
                }
                else
                {
                    // Error
                    ModelState.AddModelError("Password", "Don't match the constraints");
                }
            }

            return View(registerVM);
        }
    }
}
