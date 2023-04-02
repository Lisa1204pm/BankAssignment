using System.ComponentModel.DataAnnotations;

namespace BankAssignment.Models
{
    public class BankAccountType
    {
        [Key]
        public string AccountType { get; set; }
        public virtual ICollection<BankAccount> BankAccounts { get; set; }

    }
}
