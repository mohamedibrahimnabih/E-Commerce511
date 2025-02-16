using E_Commerce511.DataAccess;
using E_Commerce511.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

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
        public IActionResult Create(Product product, IFormFile? file)
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

                // Save img name in db
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

        [HttpGet]
        public IActionResult Edit(int productId)
        {
            var product = dbContext.Products.FirstOrDefault(e => e.Id == productId);

            var categories = dbContext.Categories;
            //ViewBag.Categories = categories;
            ViewData["Categories"] = categories.ToList();

            if (product != null)
            {
                return View(product);
            }

            return RedirectToAction("NotFoundPage", "Home");
        }

        [HttpPost]
        public IActionResult Edit(Product product, IFormFile? file)
        {
            var productInDb = dbContext.Products.AsNoTracking().FirstOrDefault(e => e.Id == product.Id);

            if (productInDb != null && file != null && file.Length > 0)
            {
                // Save img in wwwroot
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    file.CopyTo(stream);
                }

                // Delete old img from wwwroot
                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", productInDb.Img);
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }

                // Save img name in db
                product.Img = fileName;
            }
            else
                product.Img = productInDb.Img;

            if (product != null)
            {
                dbContext.Products.Update(product);
                dbContext.SaveChanges();

                return RedirectToAction("Index");
            }

            return RedirectToAction("NotFoundPage", "Home");
        }

        public IActionResult DeleteImg(int productId)
        {
            var product = dbContext.Products.FirstOrDefault(e => e.Id == productId);

            if (product != null)
            {
                // Delete old img from wwwroot
                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", product.Img);
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }

                // Delete img name in db
                product.Img = null;
                dbContext.SaveChanges();

                return RedirectToAction("Edit", new { productId });
            }

            return RedirectToAction("NotFoundPage", "Home");
        }

        public IActionResult Delete(int productId)
        {
            var product = dbContext.Products.FirstOrDefault(e => e.Id == productId);

            if (product != null)
            {
                // Delete old img from wwwroot
                if(product.Img != null)
                {
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", product.Img);
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }

                // Delete img name in db
                dbContext.Products.Remove(product);
                dbContext.SaveChanges();

                return RedirectToAction("Index");
            }

            return RedirectToAction("NotFoundPage", "Home");
        }
    }
}
