using E_Commerce511.Models;
using E_Commerce511.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace E_Commerce511.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(IOrderRepository orderRepository, UserManager<ApplicationUser> userManager)
        {
            _orderRepository = orderRepository;
            _userManager = userManager;
        }

        public async Task<IActionResult> AddToCart(int productId, int count)
        {
            var user = _userManager.GetUserId(User);

            var cart = new Order()
            {
                ProductId = productId,
                Count = count,
                ApplicationUserId = user
            };

            var cartInDb = _orderRepository.GetOne(e => e.ProductId == productId && e.ApplicationUserId == user);

            if (cartInDb != null)
                cartInDb.Count+=1;
            else
                _orderRepository.Create(cart);

            _orderRepository.Commit();

            TempData["notifaction"] = "Add product to cart successfuly";

            return RedirectToAction("Index", "Home", new
            {
                area = "Customer"
            });
        }

        public IActionResult Index()
        {
            var cart = _orderRepository.Get(e=>e.ApplicationUserId == _userManager.GetUserId(User), includes: [e => e.Product, e => e.ApplicationUser]);

            return View(cart);
        }
    }
}
