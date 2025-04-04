using System;
using System.Collections.Generic;

namespace DO_AN.Models
{
    public partial class Role
    {
        public Role()
        {
            Accounts = new HashSet<Account>();
        }

        public int IdRole { get; set; }
        public string NameRole { get; set; } = null!;

        public virtual ICollection<Account> Accounts { get; set; }
    }
}
