using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TicketGo.Application.Interfaces;
using TicketGo.Application.DTOs;

namespace TicketGo.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class AccountsController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // GET: Admin/Accounts
        public async Task<IActionResult> Index()
        {
            var accounts = await _accountService.GetAllAccountsAsync();
            return View(accounts);
        }

        // GET: Admin/Accounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _accountService.GetAccountByIdAsync(id.Value);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // GET: Admin/Accounts/Create
        public async Task<IActionResult> Create()
        {
            var roles = await _accountService.GetAllRolesAsync();
            ViewData["IdRole"] = new SelectList(roles, "IdRole", "RoleName");
            return View();
        }

        // POST: Admin/Accounts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUpdateAccountDto accountDto)
        {
            if (ModelState.IsValid)
            {
                await _accountService.CreateAccountAsync(accountDto);
                return RedirectToAction(nameof(Index));
            }

            var roles = await _accountService.GetAllRolesAsync();
            ViewData["IdRole"] = new SelectList(roles, "IdRole", "RoleName", accountDto.IdRole);
            return View(accountDto);
        }

        // GET: Admin/Accounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _accountService.GetAccountByIdAsync(id.Value);
            if (account == null)
            {
                return NotFound();
            }

            var roles = await _accountService.GetAllRolesAsync();
            ViewData["IdRole"] = new SelectList(roles, "IdRole", "RoleName", account.IdRole);

            var accountDto = new CreateUpdateAccountDto
            {
                IdAccount = account.IdAccount,
                Phone = account.Phone,
                Email = account.Email,
                Password = account.Password,
                Sex = account.Sex,
                DateOfBirth = account.DateOfBirth,
                IdRole = account.IdRole
            };

            return View(accountDto);
        }

        // POST: Admin/Accounts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateUpdateAccountDto accountDto)
        {
            if (id != accountDto.IdAccount)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _accountService.UpdateAccountAsync(id, accountDto);
                }
                catch (Exception)
                {
                    if (!await _accountService.GetAccountByIdAsync(id) != null)
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            var roles = await _accountService.GetAllRolesAsync();
            ViewData["IdRole"] = new SelectList(roles, "IdRole", "RoleName", accountDto.IdRole);
            return View(accountDto);
        }

        // GET: Admin/Accounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _accountService.GetAccountByIdAsync(id.Value);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // POST: Admin/Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _accountService.DeleteAccountAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}