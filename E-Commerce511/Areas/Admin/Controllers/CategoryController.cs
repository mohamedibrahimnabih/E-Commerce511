using E_Commerce511.DataAccess;
using E_Commerce511.Models;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce511.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        ApplicationDbContext dbContext = new ApplicationDbContext();

        public IActionResult Index()
        {
            var categories = dbContext.Categories;

            return View(categories.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            // Validation

            dbContext.Categories.Add(new Category
            {
                Name = category.Name
            });
            dbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int catgeoryId)
        {
            var category = dbContext.Categories.FirstOrDefault(e => e.Id == catgeoryId);

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

            dbContext.Categories.Update(new Category
            {
                Id = category.Id,
                Name = category.Name
            });
            dbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int catgeoryId)
        {
            var category = dbContext.Categories.FirstOrDefault(e => e.Id == catgeoryId);

            if (category != null)
            {
                dbContext.Categories.Remove(category);
                dbContext.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("NotFoundPage", "Home");
        }
    }
}
