using System.Diagnostics;
using E_Commerce511.DataAccess;
using E_Commerce511.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce511.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        ApplicationDbContext dbContext = new ApplicationDbContext();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(string? categoryName, int? rating, string? productName, double? minRange, double? maxRange)
        {
            IQueryable<Product> products = dbContext.Products.Include(e => e.Category);

            if (categoryName != null)
            {
                products = products.Where(e => e.Category.Name == categoryName);
            }

            if (productName != null)
            {
                products = products.Where(e => e.Name.Contains(productName));
            }

            #region List of categories
            var categories = dbContext.Categories.ToList();

            //var productsWithCategories = new
            //{
            //    Products = products.ToList(),
            //    Categories = categories.ToList()
            //};

            //ViewData["categories"] = categories;
            ViewBag.categories = categories;
            #endregion

            return View(products.ToList());
        }

        public IActionResult Details(int productId)
        {
            var product = dbContext.Products.Include(e => e.Category).FirstOrDefault(e => e.Id == productId);

            if (product != null)
            {
                // Anno. Type => Product, ProductsWithCategories
                var productWithRelated = new
                {
                    Product = product,
                    Related = dbContext.Products.Where(e => e.Name.Contains(product.Name.Substring(0, 5)) && e.Id != product.Id).Skip(0).Take(4).ToList()
                };

                return View(productWithRelated);
            }

            return RedirectToAction(nameof(NotFoundPage));
        }

        public IActionResult NotFoundPage()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
