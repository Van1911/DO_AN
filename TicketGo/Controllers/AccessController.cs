using DO_AN.Models;
using Microsoft.AspNetCore.Mvc;
using DO_AN.ViewModel;
using Microsoft.EntityFrameworkCore;
using DO_AN.Services;

namespace DO_AN.Controllers
{
    public class AccessController : Controller
    {
        
        private readonly DOANContext _context;
       
        public AccessController(DOANContext context)
        {
            _context = context;
        }
    
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (ModelState.IsValid)
            {
                // Tạo tài khoản mới
                var newAccount = new Account()
                {
                    Email = registerVM.Email,
                    Password = registerVM.Password,
                    Phone = registerVM.Phone,
                    Sex = registerVM.Sex,
                    DateOfBirth = registerVM.DateOfBirth,
                    IdRole = 2 // Giả sử ID vai trò cho người dùng thường
                };

                // Lưu tài khoản mới vào cơ sở dữ liệu
                _context.Accounts.Add(newAccount);
                await _context.SaveChangesAsync();

                // Tạo khách hàng mới liên kết với tài khoản
                var newCustomer = new Customer
                {
                    IdCus = newAccount.IdAccount,
                    IdAccount = newAccount.IdAccount,
                    FullName = registerVM.FullName
                };

                // Lưu khách hàng mới vào cơ sở dữ liệu
                _context.Customers.Add(newCustomer);
                await _context.SaveChangesAsync();




                // Gửi email xác thực




                //await SendVerificationEmail(registerVM.Email);





                ViewBag.Status = 1; // Đăng ký thành công
                return RedirectToAction("Login");
            }

            // Nếu đăng ký thất bại, quay lại view Register với thông báo lỗi
            ViewBag.Status = 0; // Đăng ký thất bại
            return View(registerVM);
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(Account user)
        {
            var myUser = _context.Accounts.Where(x => x.Email == user.Email && x.Password == user.Password).FirstOrDefault();
            if(myUser != null)
            {
                if(user.IdRole == 1)
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
            else
            {
                ViewBag.Message = "Login Failed...";
            }
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("UserSession");

            return RedirectToAction("Login","Access");
        }
        public IActionResult AccountInfo()
        {
            return View();
        }

        // Action để gửi email xác thực
        //public async Task<IActionResult> SendVerificationEmail(string email)
        //{
        //    // Sinh mã xác thực
        //    string verificationCode = GenerateVerificationCode();

        //    // Lưu mã xác thực vào Session
        //    HttpContext.Session.SetString("VerificationCode", verificationCode);
        //    HttpContext.Session.SetInt32("VerificationCodeExpiration", DateTime.UtcNow.AddMinutes(10).Minute); // Thiết lập thời gian hết hạn

        //    // Gửi email
        //    string subject = "Xác thực email";
        //    string htmlMessage = $"Mã xác thực của bạn là: <strong>{verificationCode}</strong>";
        //    await _emailService.SendEmailAsync(email, subject, htmlMessage);

        //    // Chuyển hướng đến hành động VerifyEmail
        //    return RedirectToAction("VerifyEmail", new { email });
        //}

        // Action để xác thực mã từ email
        public IActionResult VerifyEmail(string email)
        {
            var model = new VerifyEmailViewModel { Email = email };
            return View(model);
        }

        // Action để xử lý xác thực mã
        [HttpPost]
        public IActionResult VerifyEmail(VerifyEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Lấy mã xác thực từ Session và kiểm tra thời gian hết hạn
                    var savedVerificationCode = HttpContext.Session.GetString("VerificationCode");
                    var expirationTime = HttpContext.Session.GetInt32("VerificationCodeExpiration");

                    if (savedVerificationCode != null && DateTime.UtcNow.Minute < expirationTime)
                    {
                        if (model.VerificationCode == savedVerificationCode)
                        {
                            // Tìm tài khoản dựa trên email từ cơ sở dữ liệu
                            var account = _context.Accounts.FirstOrDefault(x => x.Email == model.Email);

                            if (account != null)
                            {
                                // Đánh dấu tài khoản là đã xác thực
                                // Ví dụ: account.IsEmailVerified = true;
                                // _context.SaveChanges();

                                // Xóa mã xác thực khỏi Session sau khi xác thực thành công
                                HttpContext.Session.Remove("VerificationCode");
                                HttpContext.Session.Remove("VerificationCodeExpiration");

                                // Chuyển hướng đến trang chủ hoặc trang nào đó
                                return RedirectToAction("TrangChu", "Home");
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "Không tìm thấy tài khoản với email này.");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Mã xác thực không chính xác hoặc đã hết hạn.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Mã xác thực không tồn tại hoặc đã hết hạn.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Đã xảy ra lỗi trong quá trình xác thực email: {ex.Message}");
                }
            }

            // Nếu ModelState không hợp lệ hoặc xác thực không thành công, quay lại view VerifyEmail với model và thông báo lỗi
            return View(model);
        }



        // Đoạn mã để tạo mã xác thực ngẫu nhiên
        private string GenerateVerificationCode()
        {
            // Ví dụ đơn giản, tạo một mã ngẫu nhiên
            return Guid.NewGuid().ToString().Substring(0, 6); // Lấy 6 ký tự đầu tiên
        }










        //[HttpGet]
        //public IActionResult ForgotPassword()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    var account = await context.Accounts.SingleOrDefaultAsync(a => a.Email == model.Email);
        //    if (account == null)
        //    {
        //        ModelState.AddModelError(string.Empty, "No account found with that email address.");
        //        return View(model);
        //    }

        //    var token = Guid.NewGuid().ToString(); // Generate a reset token
        //    var resetLink = Url.Action("ResetPassword", "Account", new { token }, Request.Scheme);

        //    var message = $"Reset your password by clicking <a href='{resetLink}'>here</a>";
        //    await _emailSender.SendEmailAsync(model.Email, "Reset Your Password", message);

        //    TempData["Message"] = "Check your email for the password reset link.";
        //    return RedirectToAction("ForgotPassword");
        //}

        //[HttpGet]
        //public IActionResult ResetPassword(string token)
        //{
        //    return View(new ResetPasswordViewModel { Token = token });
        //}

        //[HttpPost]
        //public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    // Here you should verify the token and reset the password
        //    var account = await context.Accounts.SingleOrDefaultAsync(a => a.Email == model.Email);
        //    if (account == null)
        //    {
        //        ModelState.AddModelError(string.Empty, "Invalid email address.");
        //        return View(model);
        //    }

        //    account.Password = HashPassword(model.NewPassword); // Ensure you hash the password
        //    await context.SaveChangesAsync();

        //    TempData["Message"] = "Your password has been reset.";
        //    return RedirectToAction("Login");
        //}

        //private string HashPassword(string password)
        //{
        //    // Implement secure hashing algorithm
        //    return BCrypt.Net.BCrypt.HashPassword(password); // Example using BCrypt
        //}

    }
}
