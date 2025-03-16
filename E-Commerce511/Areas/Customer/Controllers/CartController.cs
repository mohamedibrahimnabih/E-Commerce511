using E_Commerce511.Models;
using E_Commerce511.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Threading.Tasks;

namespace E_Commerce511.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOrderRepository _orderRepo;
        private readonly IOrderItemRepository _orderItemRepository;

        public CartController(ICartRepository orderRepository, UserManager<ApplicationUser> userManager, IOrderItemRepository orderItemRepository, IOrderRepository orderRepo)
        {
            _cartRepository = orderRepository;
            _userManager = userManager;
            _orderItemRepository = orderItemRepository;
            _orderRepo = orderRepo;
        }

        public async Task<IActionResult> AddToCart(int productId, int count)
        {
            var user = _userManager.GetUserId(User);

            var cart = new Cart()
            {
                ProductId = productId,
                Count = count,
                ApplicationUserId = user
            };

            var cartInDb = _cartRepository.GetOne(e => e.ProductId == productId && e.ApplicationUserId == user);

            if (cartInDb != null)
                cartInDb.Count += count;
            else
                _cartRepository.Create(cart);

            _cartRepository.Commit();

            //TempData["notifaction"] = "Add product to cart successfuly";

            return RedirectToAction("Index", "Home", new
            {
                area = "Customer"
            });
        }

        public IActionResult Index()
        {
            var cart = _cartRepository.Get(e=>e.ApplicationUserId == _userManager.GetUserId(User), includes: [e => e.Product, e => e.ApplicationUser]);

            var totalPrice = cart.Sum(e => e.Product.Price * e.Count);

            ViewBag.TotalPrice = totalPrice;
            return View(cart);
        }

        public IActionResult Increment(int productId)
        {
            var cart = _cartRepository.GetOne(e => e.ApplicationUserId == _userManager.GetUserId(User) && e.ProductId == productId);

            if (cart != null)
            {
                cart.Count++;
                _cartRepository.Commit();
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Decrement(int productId)
        {
            var cart = _cartRepository.GetOne(e => e.ApplicationUserId == _userManager.GetUserId(User) && e.ProductId == productId);

            if (cart != null && cart.Count > 1)
            {
                cart.Count--;
                _cartRepository.Commit();
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Pay()
        {
            var userId = _userManager.GetUserId(User);
            var cart = _cartRepository.Get(e => e.ApplicationUserId == userId, includes: [e => e.Product, e => e.ApplicationUser]);

            var order = new Order();
            order.ApplicationUserId = userId;
            order.OrderDate = DateTime.Now;
            order.OrderTotal = (double)cart.Sum(e => e.Product.Price * e.Count);

            _orderRepo.Create(order);
            _orderRepo.Commit();

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = $"{Request.Scheme}://{Request.Host}/Customer/Checkout/Success?orderId={order.Id}",
                CancelUrl = $"{Request.Scheme}://{Request.Host}/Customer/Checkout/Cancel",
            };

            foreach (var item in cart)
            {
                options.LineItems.Add(
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "egp",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Product.Name,
                                Description = item.Product.Description,
                            },
                            UnitAmount = (long)item.Product.Price * 100,
                        },
                        Quantity = item.Count,
                    }
                );
            }

            var service = new SessionService();
            var session = service.Create(options);
            order.SessionId = session.Id;
            _orderRepo.Commit();

            List<OrderItem> orderItems = [];
            foreach (var item in cart)
            {
                var orderItem = new OrderItem()
                {
                    OrderId = order.Id,
                    Count = item.Count,
                    Price = (double)item.Product.Price,
                    ProductId = item.ProductId,
                };

                orderItems.Add(orderItem);
            }
            _orderItemRepository.CreateRange(orderItems);
            _orderRepo.Commit();

            return Redirect(session.Url);

        }

    }
}
