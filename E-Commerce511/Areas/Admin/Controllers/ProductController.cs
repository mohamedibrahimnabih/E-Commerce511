using E_Commerce511.DataAccess;
using E_Commerce511.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce511.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        ApplicationDbContext dbContext = new ApplicationDbContext();

        public IActionResult Index()
        {
            var products = dbContext.Products.Include(e => e.Category);

            return View(products.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            var categories = dbContext.Categories;
            //ViewBag.Categories = categories;
            ViewData["Categories"] = categories.ToList();

            return View();
        }

        [HttpPost]
        public IActionResult Create(Product product, IFormFile file)
        {
            // Validation

            if (file != null && file.Length > 0)
            {
                // Save img in wwwroot
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    file.CopyTo(stream);
                }

                product.Img = fileName;
            }

            if (product != null)
            {
                dbContext.Products.Add(product);
                dbContext.SaveChanges();

                return RedirectToAction("Index");
            }

            return RedirectToAction("NotFoundPage", "Home");
        }
    }
}
