using Microsoft.AspNetCore.Mvc;
using TicketGo.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace TicketGo.Web.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IVNPayService _vnPayService;

        public PaymentController(IVNPayService vnPayService)
        {
            _vnPayService = vnPayService;
        }
    }
}