using E_Commerce511.Models;
using E_Commerce511.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace E_Commerce511.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
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
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = new()
                {
                    UserName = registerVM.UserName,
                    Email = registerVM.Email,
                    Address = registerVM.Address
                };

                var result = await userManager.CreateAsync(applicationUser, registerVM.Password);

                if (result.Succeeded)
                {
                    // Success Register
                    await signInManager.SignInAsync(applicationUser, false);

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

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if(ModelState.IsValid)
            {
                var appUser = await userManager.FindByEmailAsync(loginVM.Email);

                if(appUser != null)
                {
                    var result = await userManager.CheckPasswordAsync(appUser, loginVM.Password);

                    if (result)
                    {
                        // Login
                        await signInManager.SignInAsync(appUser, loginVM.RememberMe);

                        return RedirectToAction("Index", "Home", new { area = "Customer" });
                    }
                    else
                    {
                        ModelState.AddModelError("Password", "Don't match the password");
                    }
                }
                else
                {
                    ModelState.AddModelError("Email", "Can not found the email");
                }
            }

            return View(loginVM);
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account", new { area = "Identity" });
        }
    }
}
