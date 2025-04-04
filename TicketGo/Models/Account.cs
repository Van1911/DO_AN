using System;
using System.Collections.Generic;

namespace DO_AN.Models
{
    public partial class Account
    {
        public Account()
        {
            Customers = new HashSet<Customer>();
        }

        public int IdAccount { get; set; }
        public string? Phone { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool? Sex { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int IdRole { get; set; }

        public virtual Role IdRoleNavigation { get; set; } = null!;
        public virtual ICollection<Customer> Customers { get; set; }
    }
}
