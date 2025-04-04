using Microsoft.AspNetCore.Mvc;
using TicketGo.Application.Interfaces;

namespace TicketGo.Web.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IVnPayService _vnPayService;

        public PaymentController(IVnPayService vnPayService)
        {
            _vnPayService = vnPayService;
        }
    }
}