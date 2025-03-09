using E_Commerce511.DataAccess;
using E_Commerce511.Models;
using E_Commerce511.Repositories;
using E_Commerce511.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce511.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]

    public class CategoryController : Controller
    {
        private readonly ICategoryRepository categoryRepository;

        //ApplicationDbContext dbContext = new ApplicationDbContext();
        public CategoryController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            var categories = categoryRepository.Get();

            return View(categories.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Category());
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            // Validation
            //ModelState.Remove("Products");

            if(ModelState.IsValid)
            {
                categoryRepository.Create(category);
                categoryRepository.Commit();

                //TempData["notifaction"] = "Created Category Successfuly";
                CookieOptions cookieOptions = new CookieOptions()
                {
                    Expires = DateTime.Now.AddSeconds(10),
                    Secure = true
                };
                Response.Cookies.Append("notifaction", "Created Category Successfuly", cookieOptions);

                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        [HttpGet]
        public IActionResult Edit(int catgeoryId)
        {
            var category = categoryRepository.GetOne(e=>e.Id == catgeoryId);

            if(category != null)
            {
                return View(category);
            }

            return RedirectToAction("NotFoundPage", "Home");
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            // Validation

            categoryRepository.Edit(category);
            categoryRepository.Commit();

            TempData["notifaction"] = "Update Category Successfuly";


            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int catgeoryId)
        {
            var category = categoryRepository.GetOne(e => e.Id == catgeoryId);

            if (category != null)
            {
                categoryRepository.Delete(category);
                categoryRepository.Commit();

                TempData["notifaction"] = "Delete Category Successfuly";


                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("NotFoundPage", "Home");
        }
    }
}
