using E_Commerce511.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;

namespace E_Commerce511.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public IActionResult Refund(int orderId)
        {
            var order = _orderRepository.GetOne(e => e.Id == orderId);

            if(order != null)
            {
                if(order.PaymentStatus == true && order.PaymentStripeId != null)
                {
                    var service = new SessionService();
                    var session = service.Get(order.SessionId);

                    var refundOptions = new RefundCreateOptions
                    {
                        PaymentIntent = order.PaymentStripeId,
                        Amount = (long)order.OrderTotal,
                        Reason = RefundReasons.RequestedByCustomer
                    };

                    var refundService = new RefundService();
                    var refundSession = refundService.Create(refundOptions);

                    order.PaymentStatus = false;
                    order.PaymentStripeId = null;
                    _orderRepository.Commit();
                }
            }

            return View();
        }
    }
}
