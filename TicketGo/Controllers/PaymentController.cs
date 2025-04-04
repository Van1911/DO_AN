using Microsoft.AspNetCore.Mvc;
using DO_AN.Models;
using DO_AN.Services;
using Microsoft.AspNetCore.Authorization;

namespace DO_AN.Controllers
{
    public class PaymentController : Controller
    {
        private readonly DOANContext _context;
        private readonly IVNPayService _vnPayService;

        public PaymentController(DOANContext context, IVNPayService vnPayService)
        {
            _context = context;
            _vnPayService = vnPayService;
        }
    }
}
