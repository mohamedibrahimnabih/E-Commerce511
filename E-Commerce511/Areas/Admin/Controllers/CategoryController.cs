using E_Commerce511.DataAccess;
using E_Commerce511.Models;
using E_Commerce511.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce511.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        //ApplicationDbContext dbContext = new ApplicationDbContext();
        CategoryRepository categoryRepositry = new CategoryRepository();

        public IActionResult Index()
        {
            var categories = categoryRepositry.Get();

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
                categoryRepositry.Create(category);
                categoryRepositry.Commit();
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        [HttpGet]
        public IActionResult Edit(int catgeoryId)
        {
            var category = categoryRepositry.GetOne(e=>e.Id == catgeoryId);

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

            categoryRepositry.Edit(category);
            categoryRepositry.Commit();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int catgeoryId)
        {
            var category = categoryRepositry.GetOne(e => e.Id == catgeoryId);

            if (category != null)
            {
                categoryRepositry.Delete(category);
                categoryRepositry.Commit();

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("NotFoundPage", "Home");
        }
    }
}
