using E_Commerce511.DataAccess;
using E_Commerce511.Models;
using E_Commerce511.Repositories;
using E_Commerce511.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace E_Commerce511.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]

    public class ProductController : Controller
    {
        //ApplicationDbContext dbContext = new ApplicationDbContext();
        //ProductRepository productRepository = new ProductRepository();  
        //CategoryRepository categoryRepository = new CategoryRepository();
        private readonly IProductRepository productRepository;
        private readonly ICategoryRepository categoryRepository;

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            var products = productRepository.Get(includes: [e => e.Category]);

            return View(products.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            var categories = categoryRepository.Get();
            //ViewBag.Categories = categories;
            ViewData["Categories"] = categories.ToList();

            return View(new Product());
        }

        [HttpPost]
        public IActionResult Create(Product product, IFormFile? file)
        {
            // Validation

            if(ModelState.IsValid)
            {
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

                productRepository.Create(product);
                productRepository.Commit();

                return RedirectToAction("Index");
            }

            var categories = categoryRepository.Get();
            //ViewBag.Categories = categories;
            ViewData["Categories"] = categories.ToList();
            return View(product);
        }

        [HttpGet]
        public IActionResult Edit(int productId)
        {
            var product = productRepository.GetOne(e=>e.Id == productId);

            var categories = categoryRepository.Get();
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
            var productInDb = productRepository.GetOne(e => e.Id == product.Id, tracked: false);

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
                productRepository.Edit(product);
                productRepository.Commit();

                return RedirectToAction("Index");
            }

            return RedirectToAction("NotFoundPage", "Home");
        }

        public IActionResult DeleteImg(int productId)
        {
            var product = productRepository.GetOne(e => e.Id == productId);

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
                productRepository.Commit();

                return RedirectToAction("Edit", new { productId });
            }

            return RedirectToAction("NotFoundPage", "Home");
        }

        public IActionResult Delete(int productId)
        {
            var product = productRepository.GetOne(e => e.Id == productId);

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
                productRepository.Delete(product);
                productRepository.Commit();

                return RedirectToAction("Index");
            }

            return RedirectToAction("NotFoundPage", "Home");
        }
    }
}
