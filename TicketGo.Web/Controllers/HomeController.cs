using Microsoft.AspNetCore.Mvc;

namespace TicketGo.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult TrangChu()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                ViewBag.MySession = HttpContext.Session.GetString("UserSession").ToString();
            }
            return View();
        }
    }
}