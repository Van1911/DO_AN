using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TicketGo.Application.Interfaces;
using TicketGo.Application.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace TicketGo.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("admin")]
    public class DashboardController : Controller
    {
        public DashboardController()
        { }

        public IActionResult Dashboard()
        {
            return View();
        }
    }
}