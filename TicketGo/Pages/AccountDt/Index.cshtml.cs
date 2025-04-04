using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DO_AN.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
namespace DO_AN.Pages.AccountDt
{
    public class IndexModel : PageModel
    {
        public Account? Account { get; set; }
        public Customer? Customer { get; set; }
        private DOANContext context { get; set; }
        public IndexModel(DOANContext _context)
        {
            context = _context;
        }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Account = await context.Accounts.FindAsync(id);
            var customer = await context.Customers
                .Include(c => c.IdAccountNavigation)
                .FirstOrDefaultAsync(c => c.IdAccount == id);
            Customer = customer;
            if (Account == null)
            {
                return NotFound();
            }
            if(customer == null)
            {
                return NotFound();
            }
            
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int id ,string FullName,string email,string Phone , bool Sex,DateTime DateOfBirth)
        {
            var customer = await context.Customers
                .Include(c => c.IdAccountNavigation) 
                .FirstOrDefaultAsync(c => c.IdAccount == id);
            Account? account = await context.Accounts.FindAsync(id);
            if(account != null)
            {
                email = account.Email;
                account.DateOfBirth = DateOfBirth;
                account.Phone = Phone;
                account.Sex = Sex;
            }
            if(customer != null)
            {
                customer.FullName= FullName;
            }
            await context.SaveChangesAsync();
            return RedirectToPage();
        }
        
    }
}

