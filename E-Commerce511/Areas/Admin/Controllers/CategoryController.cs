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

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Edit(int catgeoryId)
        {
            var category = dbContext.Categories.FirstOrDefault(e => e.Id == catgeoryId);

            if(category != null)
            {
                return View(category);
            }

            return RedirectToAction("NotFoundPage", "Home");
        }

        public IActionResult SaveEdit(int catgeoryId, string categoryName)
        {
            // Validation
            dbContext.Categories.Update(new Category
            {
                Id = catgeoryId,
                Name = categoryName
            });
            dbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
