using Microsoft.AspNetCore.Mvc;
using TicketGo.Application.Interfaces;
using TicketGo.Application.DTOs;
using TicketGo.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace TicketGo.Web.Controllers
{
    public class AccessController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IResendService _resendService;

        public AccessController(IAccountService accountService, IResendService resendService)
        {
            _accountService = accountService;
            _resendService = resendService;
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
            // Kiểm tra email đã tồn tại
            var existingAccount = await _accountService.GetAllAccountsAsync();
            if (existingAccount.Any(a => a.Email == registerDto.Email) && existingAccount.Any(a => a.IsEmailConfirmed == true))
            {
                ModelState.AddModelError("Email", "Email đã tồn tại.");
                return View(registerDto);
            }

            // Kiểm tra model hợp lệ
            if (!ModelState.IsValid)
            {
                var allErrors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);
                return View(registerDto);
            }

            // Đăng ký tài khoản
            var success = await _accountService.RegisterAsync(registerDto);
            if (success)
            {
                // Lấy tài khoản vừa tạo để tạo token
                var account = await _accountService.GetByEmailAsync(registerDto.Email);
                if (account != null)
                {
                    // Tạo và gửi email xác thực
                    var token = await _accountService.GenerateTokenAsync(account);
                    var verifyUrl = Url.Action("VerifyEmail", "Access", new { email = account.Email, token }, protocol: Request.Scheme);
                    await _resendService.SendVerificationEmailAsync(registerDto.Email, verifyUrl);
                    TempData["SuccessMessage"] = "Đăng ký thành công! Vui lòng kiểm tra email để xác minh tài khoản.";
                    return RedirectToAction("Login");
                }
            }

            ViewBag.Message = "Đăng ký thất bại. Vui lòng thử lại.";
            return View(registerDto);
        }

        // [Đăng nhập]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // [Đăng nhập]
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var myUser = await _accountService.LoginAsync(loginDto.Email, loginDto.Password);
            if (myUser != null)
            {
                // Kiểm tra xem email đã được xác thực chưa
                if (!myUser.IsEmailConfirmed)
                {
                    ViewBag.Message = "Vui lòng xác thực email trước khi đăng nhập.";
                    return View();
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, myUser.Email),
                    new Claim(ClaimTypes.Role, myUser.IdRole == 1 ? "Admin" : "Customer"),
                    new Claim("UserID", myUser.IdAccount.ToString())
                };

                var identity = new ClaimsIdentity(claims, "MyCookieAuth");
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync("MyCookieAuth", principal);

                // Thiết lập session
                HttpContext.Session.SetString("UserSession", myUser.Email);
                HttpContext.Session.SetInt32("AccountID", myUser.IdAccount);

                if (myUser.IdRole == 1)
                {
                    return RedirectToAction("Dashboard", "Dashboard", new { area = "Admin" });
                }
                else
                {
                    return RedirectToAction("TrangChu", "Home");
                }
            }

            ViewBag.Message = "Email hoặc mật khẩu không đúng.";
            return View();
        }

        // [Xác thực email]
        [HttpGet]
        public async Task<IActionResult> VerifyEmail(string email, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                TempData["ErrorMessage"] = "Liên kết xác thực không hợp lệ.";
                return RedirectToAction("Login");
            }

            var success = await _accountService.VerifyEmailAsync(email, token);
            if (success)
            {
                TempData["SuccessMessage"] = "Xác thực email thành công! Bạn đã có thể đăng nhập.";
            }
            else
            {
                TempData["ErrorMessage"] = "Xác thực email thất bại. Liên kết không hợp lệ hoặc hết hạn.";
            }

            return RedirectToAction("Login");
        }

        // [Đăng xuất]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("MyCookieAuth");
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
        
    }
}