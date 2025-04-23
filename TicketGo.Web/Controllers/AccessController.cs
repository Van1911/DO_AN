using Microsoft.AspNetCore.Mvc;
using TicketGo.Application.Interfaces;
using TicketGo.Application.DTOs;
using TicketGo.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace TicketGo.Web.Controllers
{
    public class AccessController : Controller
    {
        private readonly IAccountService _accountService;

        public AccessController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        // [Đăng ký tài khoản]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        
        // [Đăng ký tài khoản]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            // Kiểm tra email đã tồn tại trước khi validate model
            var existingAccount = await _accountService.GetAllAccountsAsync();
            if (existingAccount.Any(a => a.Email == registerDto.Email))
            {
                ModelState.AddModelError("Email", "Email đã tồn tại.");
                return View(registerDto);
            }

            // Nếu model không hợp lệ
            if (!ModelState.IsValid)
            {
                var allErrors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);
                return View(registerDto);
            }

            // Đăng ký nếu mọi thứ hợp lệ
            var success = await _accountService.RegisterAsync(registerDto);
            if (success)
            {
                TempData["SuccessMessage"] = "Đăng ký thành công! Vui lòng đăng nhập.";
                return RedirectToAction("Login");
            }

            ViewBag.Message = "Đăng ký thất bại. Vui lòng thử lại.";
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