using Microsoft.AspNetCore.Mvc;

namespace DO_AN.Controllers
{
    public class TicketsController : Controller
    {
        public IActionResult ManagerTicket()
        {
            return View();
        }
    }
}
