using E_Commerce511.Models;
using E_Commerce511.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce511.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class UserController : Controller
    {
        private readonly IApplicationUserRepository _userRepository;

        public UserController(IApplicationUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Index(string? query, int page = 1) 
        {

            IEnumerable<ApplicationUser> users = _userRepository.Get();
            
            if (query != null)
            {
                users = _userRepository.Get(e => e.UserName.Contains(query) || e.Email.Contains(query)); // 13
            }

            var totalPages = Math.Ceiling((decimal)(users.ToList().Count / 2));

            if (totalPages < page - 1)
                return RedirectToAction("NotFoundPage", "Home", new { area = "Customer" });

            users = users.Skip((page-1) * 2).Take(2); // 3

            ViewBag.totalPages = totalPages;
            return View(users.ToList());

        } 
    }
}
