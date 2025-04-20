using Microsoft.AspNetCore.Mvc;
using TicketGo.Application.Interfaces;
using TicketGo.Application.DTOs;
using TicketGo.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace TicketGo.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(AccountDto registerDto)
        {
            if (ModelState.IsValid)
            {
                var success = await _accountService.RegisterAsync(registerDto);
                if (success)
                {
                    ViewBag.Status = 1; // Đăng ký thành công
                    return RedirectToAction("Login");
                }
            }

            ViewBag.Status = 0; // Đăng ký thất bại
            return View(registerDto);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Account user)
        {
            var myUser = await _accountService.LoginAsync(user.Email, user.Password);
            if (myUser != null)
            {
                if (myUser.IdRole == 1)
                {
                    HttpContext.Session.SetString("UserSession", myUser.Email);
                    return RedirectToAction("Register", "Access");
                }
                else
                {
                    HttpContext.Session.SetString("UserSession", myUser.Email);
                    HttpContext.Session.SetInt32("UserID", myUser.IdAccount);
                    return RedirectToAction("TrangChu", "Home");
                }
            }

            ViewBag.Message = "Login Failed...";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("UserSession");
            return RedirectToAction("Login", "Access");
        }

        public IActionResult AccountInfo()
        {
            return View();
        }

        [HttpGet]
        public IActionResult VerifyEmail(string email)
        {
            var model = new VerifyEmailDto { Email = email };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> VerifyEmail(VerifyEmailDto model)
        {
            if (ModelState.IsValid)
            {
                var success = await _accountService.VerifyEmailAsync(model, HttpContext);
                if (success)
                {
                    return RedirectToAction("TrangChu", "Home");
                }

                ModelState.AddModelError(string.Empty, "Mã xác thực không chính xác hoặc đã hết hạn.");
            }

            return View(model);
        }
    }
}