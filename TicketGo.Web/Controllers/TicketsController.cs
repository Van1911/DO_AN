using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace TicketGo.Web.Controllers
{
    public class TicketsController : Controller
    {
        public IActionResult ManagerTicket()
        {
            return View();
        }
    }
}
